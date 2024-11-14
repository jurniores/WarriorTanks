using System.Collections.Generic;
using DG.Tweening;
using Omni.Core;
using UnityEngine;

public class TankClient : TankBase
{
    [SerializeField]
    private Animator Anim;
    private Vector2 move, lastMove;
    public float moveSpeed = 5f;
    public float smoothTime = 1.5f;
    private Vector2 currentVelocity = Vector2.zero;
    private Vector2 directionToMouse;

    protected override void OnStart()
    {
        if (IsLocalPlayer) Camera.main.transform.SetParent(transform);
    }

    void Update()
    {
        Vector2 targetVelocity;
        if (IsLocalPlayer)
        {
            if (Input.GetMouseButtonDown(0))
            {
                BulletSendServer();
            }
            GetInputs();
            targetVelocity = move * moveSpeed;
            RotateFromMouse();
        }
        else
        {
            targetVelocity = Move * moveSpeed;

        }

        Vector2 smoothVelocity = Vector2.SmoothDamp(currentVelocity, targetVelocity, ref currentVelocity, smoothTime);

        if (smoothVelocity != Vector2.zero)
        {
            TankRotate(smoothVelocity);
        }
        HeadRotate(angleTank);
        transform.Translate(smoothVelocity * Time.deltaTime);
    }

    void RotateFromMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        directionToMouse = (mousePosition - transform.position).normalized;
        angleTank = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;

    }
    void GetInputs()
    {
        if (Input.GetKey(KeyCode.W))
        {
            move.y = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            move.y = -1;
        }
        else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            move.y = 0;
        }


        if (Input.GetKey(KeyCode.D))
        {
            move.x = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            move.x = -1;
        }
        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            move.x = 0;
        }


        if (move != lastMove)
        {
            lastMove = move;

            Local.Invoke(ConstantsGame.TANK_MOVE, (HalfVector2)move, (HalfVector2)(Vector2)transform.position);
        }
    }
    void BulletSendServer()
    {
        if (!propertiesBase.CowntDownBulletReddy) return;
        using var buffer = NetworkManager.Pool.Rent();
        buffer.Write((HalfVector2)directionToMouse);
        Local.Invoke(ConstantsGame.TANK_BULLET, buffer);
    }

    [Client(ConstantsGame.TANK_BULLET)]
    void RecieveBulletRpcClient(DataBuffer buffer)
    {
        buffer.ReadIdentity(out var peerId, out var identityId);
        Vector2 dir = buffer.Read<HalfVector2>();

        var bulletBase = NetworkManager.GetPrefab(2).SpawnOnClient(peerId, identityId).Get<BulletBase>();
        bulletBase.transform.position = mira.position;

        if (IsLocalPlayer)
        {
            bulletBase.SetDirection(directionToMouse.normalized, propertiesBase);
            ((PropertyClient)propertiesBase).uI.MinusBullet();
        }
        else bulletBase.SetDirection(dir.normalized, propertiesBase);

        Anim.Play("null");
        Anim.Play("smoke");
    }


    void RotateIsClient(Vector3 dir)
    {
        directionToMouse = (dir - transform.position).normalized;
        float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;

        rotateHeadTank.DORotate(Vector3.forward * angle, rotationDuration, RotateMode.Fast);
    }

    [Client(ConstantsGame.TANK_MOVE_ROT)]
    void RecieveMoveRotRpcServer(DataBuffer buffer)
    {
        angleTank = buffer.Read<Half>();
    }

    public override void OnTick(ITickInfo data)
    {
        if (!IsLocalPlayer) return;
        using var buffer = NetworkManager.Pool.Rent();
        buffer.Write((Half)rotateHeadTank.eulerAngles.z);
        Local.Invoke(ConstantsGame.TANK_MOVE_ROT, buffer);
    }

    [Client(ConstantsGame.TANK_CORRECT_MOVIMENT)]
    void RPCCorrectMovimentClient(DataBuffer buffer)
    {
        Vector2 correctMoviment = buffer.Read<HalfVector2>();
        transform.position = correctMoviment;
    }
}
