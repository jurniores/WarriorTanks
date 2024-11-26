using System.Collections.Generic;
using Omni.Core;
using Omni.Core.Modules.Matchmaking;
using Omni.Threading.Tasks;
using Unity.VisualScripting;
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
    [SerializeField]
    private Dictionary<string, GroupManagerServer> dicGroups = new();
    GroupManagerServer groupAtual;
    [SerializeField]
    private int qtdPlayers;

    protected override void OnServerPeerDisconnected(NetworkPeer peer, Phase phase)
    {
        if (phase == Phase.Begin)
        {
            
            if (peer.Data.TryGet<GroupManagerServer>("group", out var group))
            {
                if (group.VerifyPlayerOn(peer))
                {
                    dicGroups.Remove(group.nameGroup);
                    group.Identity.Destroy();
                }
            }
        }
    }

    [Server(ConstantsGame.LOGIN)]
    void LoginRPCServer(DataBuffer buffer, NetworkPeer peer)
    {
        if (peer.Data.TryGet<GroupManagerServer>("group", out var _)) return;
        if (PlayerWaitForGame.Count == 0)
        {
            groupAtual = NetworkManager.GetPrefab(4).SpawnOnServer(NetworkManager.Server.ServerPeer).Get<GroupManagerServer>();
        }
        peer.Data["name"] = buffer.ReadString();
        groupAtual.nameGroup = SystemInfo.deviceUniqueIdentifier;
        peer.Data["group"] = groupAtual;

        buffer.SeekToBegin();
        buffer.WriteIdentity(groupAtual.Identity);
        PlayerWaitForGame.Add(peer);

        if (PlayerWaitForGame.Count >= qtdPlayers)
        {
            print("Zerei os players");
            dicGroups.Add(groupAtual.nameGroup, groupAtual);
            PlayerWaitForGame = new();
        }

        Remote.Invoke(ConstantsGame.LOGIN, peer, buffer, Target.Self);
        groupAtual.SetPeerInGroup(peer);
    }
}
