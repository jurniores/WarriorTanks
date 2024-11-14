using System.Collections.Generic;
using Omni.Core;
using Omni.Core.Modules.Matchmaking;
using UnityEngine;

public class ServerLogin : ServerBehaviour
{
    [SerializeField]
    private Transform spawn1, spawn2;
    private Dictionary<int, EntityList> EntityListDic = new();
    NetworkMatchmaking Matchmaking;
    NetworkGroup group;
    protected override void OnServerStart()
    {
        Matchmaking = NetworkManager.Matchmaking;
        group = Matchmaking.Server.AddGroup("groupInitial");
    }

    protected override void OnServerPeerDisconnected(NetworkPeer peer, Phase phase)
    {
        if (phase == Phase.Begin)
        {
            int identityId = EntityListDic[peer.Id].identityId;
            var identity = NetworkManager.Server.GetIdentity(identityId);
            identity.Destroy();
        }
    }

    [Server(ConstantsGame.TANK_LOGIN)]
    void LoginRPCServer(DataBuffer buffer, NetworkPeer peer)
    {
        Matchmaking.Server.JoinGroup(group, peer);
        var identity = NetworkManager.GetPrefab(0).SpawnOnServer(peer);
        var playerServer = identity.Get<PropertyServer>();
        playerServer.transform.position = EntityListDic.Count % 2 == 0 ? spawn1.position : spawn2.position;
        string name = buffer.ReadString();
        playerServer.SetInfo(name);

        buffer.SeekToBegin();
        buffer.WriteIdentity(identity);

        Remote.Invoke(ConstantsGame.TANK_LOGIN, peer, buffer, Target.GroupMembers);

        buffer.SeekToBegin();

        buffer.WriteAsBinary(EntityListDic);
        Remote.Invoke(ConstantsGame.TANK_LOGIN_ALL, peer, buffer, Target.Self);

        EntityListDic.Add(peer.Id, new EntityList
        {
            peerId = peer.Id,
            identityId = identity.IdentityId,
            nameTank = name,
            team = EntityListDic.Count
        });
    }

}
