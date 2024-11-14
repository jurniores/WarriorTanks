using DG.Tweening;
using Omni.Core;
using Unity.VisualScripting;
using UnityEngine;

public partial class TankBase : NetworkBehaviour
{
    [NetworkVariable]
    private Vector2 m_Move;

    [NetworkVariable]
    protected Vector2 m_MoveNow;
    [SerializeField]
    protected PropertiesBase propertiesBase;
    protected float angleTank;
    [SerializeField]
    protected float velocity;
    [SerializeField]
    protected Transform rotateHeadTank, mira, tankRotate;
    protected float velLerp;
    protected JoysTick currentJoysTick;


    public float rotationDuration = 0.2f;
    private void Start()
    {
        DefaultNetworkVariableOptions = new()
        {
            Target = Target.GroupMembers
        };
    }


    public void HeadRotate(float angle)
    {
        if (!IsLocalPlayer) rotationDuration = 0.1f;
        rotateHeadTank.DORotate(Vector3.forward * angle, rotationDuration, RotateMode.Fast);
    }

    public void TankRotate(Vector2 dir)
    {
        if (dir == Vector2.zero) return;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        tankRotate.DORotate(Vector3.forward * angle, rotationDuration, RotateMode.Fast);
    }

    partial void OnMoveNowChanged(Vector2 prevMoveNow, Vector2 nextMoveNow, bool isWriting)
    {
        if (!isWriting)
        {
            transform.position = nextMoveNow;
        }
    }
}
