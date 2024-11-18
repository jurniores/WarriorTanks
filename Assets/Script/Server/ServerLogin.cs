using System.Collections.Generic;
using Omni.Core;
using Omni.Core.Modules.Matchmaking;
using Omni.Threading.Tasks;
using UnityEngine;

public class ServerLogin : ServerBehaviour
{
    [SerializeField]
    private Transform spawn1, spawn2;
    private Dictionary<int, EntityList> EntityListDic = new();
    private float time = 10;
    private Bomb bomb;
    NetworkMatchmaking Matchmaking;
    NetworkGroup group;
    protected override void OnServerStart()
    {
        Matchmaking = NetworkManager.Matchmaking;
        bomb = NetworkManager.GetPrefab(3).SpawnOnServer(NetworkManager.Server.ServerPeer).Get<Bomb>();

        group = Matchmaking.Server.AddGroup("groupInitial");
        bomb.Group = group;
    }
    protected override void OnServerPeerDisconnected(NetworkPeer peer, Phase phase)
    {
        if (phase == Phase.Begin)
        {
            int identityId = EntityListDic[peer.Id].identityId;
            var identity = NetworkManager.Server.GetIdentity(identityId);
            EntityListDic.Remove(peer.Id);
            identity.Destroy();
        }
    }
    [Server(ConstantsGame.TANK_LOGIN)]
    void LoginRPCServer(DataBuffer buffer, NetworkPeer peer)
    {
        Matchmaking.Server.JoinGroup(group, peer);
        var identity = NetworkManager.GetPrefab(0).SpawnOnServer(peer);
        var playerServer = identity.Get<PropertyServer>();
        string name = buffer.ReadString();
        int team = EntityListDic.Count % 2 == 0 ? 1 : 2;

        playerServer.SetInfo(name, team == 1 ? spawn1 : spawn2, team);

        buffer.SeekToBegin();
        buffer.WriteIdentity(identity);

        Remote.Invoke(ConstantsGame.TANK_LOGIN, peer, buffer, Target.GroupMembers);

        buffer.SeekToBegin();

        buffer.WriteAsBinary(EntityListDic);
        buffer.WriteIdentity(bomb.Identity);
        Remote.Invoke(ConstantsGame.TANK_LOGIN_ALL, peer, buffer, Target.Self);

        EntityListDic.Add(peer.Id, new EntityList
        {
            peerId = peer.Id,
            identityId = identity.IdentityId,
            nameTank = name,
            team = team
        });

        if (EntityListDic.Count >= 2)
        {
            StartGame();
        }
    }
    async void StartGame()
    {
        var buffer = NetworkManager.Pool.Rent();
        buffer.Write(10);
        Remote.Invoke(ConstantsGame.START_GAME, NetworkManager.Server.ServerPeer, buffer, groupId: group.Id);
        buffer.Dispose();
        await UniTask.WaitForSeconds(10);
        Remote.Invoke(ConstantsGame.END_GAME, NetworkManager.Server.ServerPeer, groupId: group.Id);
    }

}
