using System.Collections.Generic;
using Omni.Core;
using Omni.Core.Modules.Matchmaking;
using Omni.Threading.Tasks;
using UnityEngine;

public class ServerLogin : ServerBehaviour
{
    [SerializeField]
    private Transform spawn1, spawn2;
    [SerializeField]
    private float timeBattle = 60;
    [SerializeField]
    private List<NetworkPeer> PlayerWaitForGame = new();
    private float time = 10;
    GroupManagerServer groupAtual;
    [SerializeField]
    private int qtdPlayers;

    // protected override void OnServerPeerDisconnected(NetworkPeer peer, Phase phase)
    // {
    //     if (phase == Phase.Begin)
    //     {
    //         int identityId = EntityListDic[peer.Id].identityId;
    //         var identity = NetworkManager.Server.GetIdentity(identityId);
    //         EntityListDic.Remove(peer.Id);
    //         identity.Destroy();
    //     }
    // }

    [Server(ConstantsGame.LOGIN)]
    void LoginRPCServer(DataBuffer buffer, NetworkPeer peer)
    {
        if(peer.Data.TryGet<GroupManagerServer>("group", out var group)) return;
        if (PlayerWaitForGame.Count == 0)
        {
            groupAtual = NetworkManager.GetPrefab(4).SpawnOnServer(NetworkManager.Server.ServerPeer).Get<GroupManagerServer>();
        }
        peer.Data["name"] = buffer.ReadString();
        
        peer.Data["group"] = groupAtual;

        buffer.SeekToBegin();
        buffer.WriteIdentity(groupAtual.Identity);
        PlayerWaitForGame.Add(peer);

        if (PlayerWaitForGame.Count >= qtdPlayers)
        {
            print("Zerei os players");
            PlayerWaitForGame = new();
        }
        
        Remote.Invoke(ConstantsGame.LOGIN, peer, buffer, Target.Self);
        groupAtual.SetPeerInGroup(peer);
    }
}
