using System.Collections;
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
    private Coroutine coroutineDestroyBomb;

    protected override void OnStart()
    {
        if (this != null && IsServer)
        {
            StartCoroutine(DestroyBomb(5));
        }
    }
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
    void OnHitObject(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            PropertyServer pEnemy = (PropertyServer)collider.GetComponent<PropertiesBase>();
            pEnemy.Demage(propertyServer.Dano, (PropertyServer)propertyServer);
        }
        speed = 0;
        Remote.Invoke(ConstantsGame.TANK_BULLET_DEMAGE, DataBuffer.Empty, Target.GroupMembers);
        StartCoroutine(DestroyBomb(2));
    }
    public void DisableBullet()
    {
        gameObject.SetActive(false);
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
    protected override void OnDestroy()
    {
        if (coroutineDestroyBomb != null) StopCoroutine(coroutineDestroyBomb);
    }
    [Client(ConstantsGame.TANK_BULLET_DEMAGE)]
    void RPCDemageClient()
    {
        anim.Play("bomb_explosion");
        speed = 0;
    }
    IEnumerator DestroyBomb(float time)
    {
        yield return new WaitForSeconds(time);
        Identity.Destroy(Target.GroupMembers);
    }
}
