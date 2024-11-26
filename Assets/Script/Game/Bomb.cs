using DG.Tweening;
using Omni.Core;
using Omni.Threading.Tasks.Triggers;
using UnityEngine;

public partial class Bomb : NetworkBehaviour
{
    [SerializeField]
    private TankServer tankServer, tankServerAtual;
    [SerializeField]
    private TankClient tankClient;
    [SerializeField]
    private float distance = 0, vel;
    [SerializeField]
    [NetworkVariable]
    private int m_Team;
    [NetworkVariable]
    private Vector3 m_InitialPosition;
    public GroupManager groupManager;
    public float detectionRadius = 0.5f; // Raio de detecção ao redor da bala
    public LayerMask targetLayer; // Camada dos objetos que queremos detectar
    private NetworkGroup group;
    Collider2D hitPlayer = null;
    public float followSpeed = 5f; // Velocidade de seguir
    public float maxDistance = 2f; // Distância máxima entre o caminhão e a caçamba
    public float dragFactor = 0.9f; // Fator
    private Vector2 velocity; // Para armazenar a velocidade da caçamba
    private Spawn spawn;

    public NetworkGroup Group
    {
        get => group; set
        {
            group = value;

            DefaultNetworkVariableOptions = new()
            {
                GroupId = group.Id
            };
        }
    }

    protected override void OnStart()
    {
        if (IsServer)
        {
            GetComponent<Animator>().enabled = false;
            int rand = Random.Range(1, 3);
            m_Team = rand;
            spawn = NetworkService.Get<Spawn>("Spawn" + m_Team);
            transform.position = spawn.transform.position;
            m_InitialPosition = transform.position;
        }

    }
    partial void OnInitialPositionChanged(Vector3 prevInitialPosition, Vector3 nextInitialPosition, bool isWriting)
    {
        if (isWriting) return;
        transform.position = nextInitialPosition;
    }
    private void Update()
    {

        Vector2 currentPosition = transform.position;
        Vector2 truckPosition = Vector2.zero;
        if (IsServer && tankServer != null) truckPosition = tankServer.transform.position;
        if (!IsServer && tankClient != null) truckPosition = tankClient.transform.position;
        // Calcula a direção e distância
        Vector2 direction = truckPosition - currentPosition;
        float distance = direction.magnitude;

        // Normaliza a direção
        direction.Normalize();

        // Aplica força na direção do caminhão
        if (distance > maxDistance)
        {
            velocity += direction * followSpeed * Time.deltaTime;
        }

        // Aplica o efeito de arrasto para suavizar o movimento
        velocity *= dragFactor;

        // Atualiza a posição da caçamba
        if (IsServer && Team != 0)
        {
            Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRadius, targetLayer);
            if (hit != null && hitPlayer != hit)
            {
                hitPlayer = hit;
                // Detecção de colisão com um objeto na camada especificada
                OnHitObject(hit);
            }

            if (tankServer != null)
            {
                transform.position = currentPosition + velocity * Time.deltaTime;
            }
        }
        else
        {
            if (tankClient != null)
            {
                transform.position = currentPosition + velocity * Time.deltaTime;
            }
        }
    }

    private void OnHitObject(Collider2D hit)
    {
        if (hit.CompareTag("Player"))
        {
            using var buffer = NetworkManager.Pool.Rent();
            tankServer = hit.GetComponent<TankServer>();
            print(tankServer);
            buffer.Write(tankServer.IdentityId);
            Remote.Invoke(ConstantsGame.BOMB_PLAYER_TRIGGER, buffer, groupId: Group.Id);
        }
        else if (hit.CompareTag("Spawn"))
        {
            var spawn = hit.GetComponent<Spawn>();
            if (spawn.team != Team)
            {
                //Lógica de vitória e derrota
                //Servidor
                groupManager.EndGame(true);
                Remote.Invoke(ConstantsGame.END_GAME);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Desenha o raio de detecção no editor para depuração
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
    [Client(ConstantsGame.END_GAME)]
    void EndGameBombRPC(DataBuffer buffer)
    {
        //Client
        print("End game no cliente");
        groupManager.EndGame(true);
    }

    [Client(ConstantsGame.BOMB_PLAYER_TRIGGER)]
    void ClientRpcPlayerClient(DataBuffer buffer)
    {
        tankClient = NetworkManager.Client.GetIdentity(buffer.Read<int>()).Get<TankClient>();
    }

}
