using System;
using DG.Tweening;
using Omni.Core;
using UnityEngine;

public class TankServer : TankBase
{
    public float moveSpeed = 5f;
    public float smoothTime = 1.5f;
    private Vector2 currentVelocity = Vector2.zero;
    private void Update()
    {
        HeadRotateServer(angleTank);
        m_MoveNow = transform.position;
        Vector2 targetVelocity = Move * moveSpeed;

        Vector2 smoothVelocity = Vector2.SmoothDamp(currentVelocity, targetVelocity, ref currentVelocity, smoothTime);

        if (smoothVelocity != Vector2.zero)
        {
            TankRotate(smoothVelocity);
        }

        transform.Translate(smoothVelocity * Time.deltaTime);
    }

    [Server(ConstantsGame.TANK_MOVE)]
    void RecieveMoveRpcServer(DataBuffer buffer, NetworkPeer peer)
    {
        Move = buffer.Read<HalfVector2>();
        Vector2 positionClient = buffer.Read<HalfVector2>();
        
        if (Vector2.Distance(transform.position, positionClient) > 1.5f)
        {
            buffer.SeekToBegin();
            buffer.Write((HalfVector2)(Vector2)transform.position);
            Remote.Invoke(ConstantsGame.TANK_CORRECT_MOVIMENT, buffer, Target.GroupMembers);
        }


        // Usa DOTween para rotacionar suavemente o objeto na direção do movimento
    }

    [Server(ConstantsGame.TANK_MOVE_ROT)]
    void RecieveMoveRotRpcServer(DataBuffer buffer, NetworkPeer peer)
    {
        angleTank = buffer.Read<Half>();
    }

    [Server(ConstantsGame.TANK_BULLET)]
    void RecieveBulletRpcServer(DataBuffer buffer, NetworkPeer peer)
    {
        if(!propertiesBase.CowntDownBulletReddy || propertiesBase.BulletTotal < 0) return;
        if(!((PropertyServer)propertiesBase).Shot()) return;
        Vector2 dir = buffer.Read<HalfVector2>();

        var bulletBase = NetworkManager.GetPrefab(2).SpawnOnServer(peer).Get<BulletBase>();
        bulletBase.transform.position = mira.position;
        bulletBase.SetDirection(dir.normalized, propertiesBase);
        
        buffer.SeekToBegin();
        buffer.WriteIdentity(bulletBase.Identity);
        buffer.Write((HalfVector2)dir);
        Remote.Invoke(ConstantsGame.TANK_BULLET, buffer, Target.GroupMembers);
    }

    public override void OnTick(ITickInfo data)
    {
        using var buffer = NetworkManager.Pool.Rent();
        buffer.Write((Half)rotateHeadTank.eulerAngles.z);
        Remote.Invoke(ConstantsGame.TANK_MOVE_ROT, buffer, Target.GroupMembersExceptSelf);
    }

}
