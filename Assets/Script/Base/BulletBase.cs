using Omni.Core;
using Omni.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public partial class BulletBase : NetworkBehaviour
{
    [SerializeField]
    private float speed = 2f;
    [SerializeField]
    private Animator anim;
    PropertiesBase propertyServer;
    private Vector3 moveDirection;

    public float detectionRadius = 0.5f; // Raio de detecção ao redor da bala
    public LayerMask targetLayer; // Camada dos objetos que queremos detectar

    private bool collide;

    void Update()
    {
        // Verifica se algum objeto colidiu com a bala
        if (IsServer && !collide)
        {
            Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRadius, targetLayer);
            if (hit != null)
            {
                // Detecção de colisão com um objeto na camada especificada
                OnHitObject(hit);
                collide = true;
            }
        }

        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }

    async void OnHitObject(Collider2D collider)
    {
        print("Colidi com " + collider.name);
        if (collider.CompareTag("Player"))
        {
            PropertiesBase pEnemy = collider.GetComponent<PropertiesBase>();
            print(pEnemy.NameTank);
            pEnemy.Hp -= propertyServer.Dano;
            Remote.Invoke(ConstantsGame.TANK_BULLET_DEMAGE, DataBuffer.Empty, Target.GroupMembers);
            speed = 0;
            await UniTask.WaitForSeconds(2);
            Identity.Destroy(Target.GroupMembers);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Desenha o raio de detecção no editor para depuração
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    public void SetDirection(Vector3 direction, PropertiesBase propertyServer)
    {
        this.propertyServer = propertyServer;
        moveDirection = direction;

        float bulletAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, bulletAngle);
    }
    void OnBecameInvisible()
    {
        // Destroi a bala quando sai da tela para evitar sobrecarga de memória
        Destroy(gameObject);
    }

    [Client(ConstantsGame.TANK_BULLET_DEMAGE)]
    void RPCDemageClient()
    {
        anim.Play("bomb_explosion");
        speed = 0;
    }

}
