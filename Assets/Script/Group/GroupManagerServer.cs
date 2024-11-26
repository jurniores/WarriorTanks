using System.Collections.Generic;
using Omni.Core;
using Omni.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using static Omni.Core.NetworkManager;

public class GroupManagerServer : GroupManager
{

    private Spawn spawn1;
    private Spawn spawn2;
    [SerializeField]
    private float timeBattle = 60;
    [SerializeField]
    private List<NetworkPeer> playerInGroup = new();
    private float time = 10;
    public string nameGroup;

    NetworkGroup group;
    [SerializeField]
    private int qtdPlayers;

    protected override void OnAwake()
    {
        spawn1 = NetworkService.Get<Spawn>("Spawn1");
        spawn2 = NetworkService.Get<Spawn>("Spawn2");

    }

    public void SetPeerInGroup(NetworkPeer peer)
    {
        if (group == null)
        {
            string nameGroup2 = $"GroupServer {nameGroup}";
            group = Matchmaking.Server.AddGroup(nameGroup2);
            name = nameGroup2;
        }

        Matchmaking.Server.JoinGroup(group, peer);
        playerInGroup.Add(peer);

        if (playerInGroup.Count >= qtdPlayers)
        {
            bomb = GetPrefab(3).SpawnOnServer(Identity.Owner).Get<Bomb>();
            bomb.Group = group;
            bomb.groupManager = this;
            int count = 0;
            playerInGroup.ForEach(player =>
            {
                LoginRPCServer(player, count);
                count++;
            });
        }
        StartGame();
    }

    void LoginRPCServer(NetworkPeer peer, int count)
    {
        using var buffer = Pool.Rent();
        var identity = GetPrefab(0).SpawnOnServer(peer);
        var playerServer = identity.Get<PropertyServer>();

        string name = peer.Data.Get<string>("name");
        var tankServer = identity.Get<TankServer>();
        var team = 0;
        peer.Data["tank"] = tankServer;
        if (count % 2 == 0)
        {
            team1.Add(tankServer);
            team = 1;
        }
        else
        {
            team = 2;
            team2.Add(tankServer);
        }

        tankServer.groupManager = this;
        playerServer.SetInfo(name, team == 1 ? spawn1.transform : spawn2.transform, team);

        buffer.SeekToBegin();
        buffer.WriteIdentity(identity);
        buffer.Write(team);

        Remote.Invoke(ConstantsGame.TANK_SPAWN, buffer, groupId: group.Id);

        using var bufferbombo = Pool.Rent();
        bufferbombo.WriteIdentity(bomb.Identity);
        Remote.InvokeByPeer(ConstantsGame.TANK_SPAWN_BOMB, peer, bufferbombo, Target.Self);
    }
    async void StartGame()
    {
        var buffer = Pool.Rent();
        buffer.Write(timeBattle);
        Remote.Invoke(ConstantsGame.START_GAME, buffer, groupId: group.Id);
        buffer.Dispose();
        await UniTask.WaitForSeconds(timeBattle);
        Remote.Invoke(ConstantsGame.END_GAME, groupId: group.Id);
        EndGame(false);
    }


    public bool VerifyPlayerOn(NetworkPeer peer)
    {

        //Apaga o Tank e o destroi
        if (peer.Data.TryGet<TankServer>("tank", out var tankServer))
        {
            team1.Remove(tankServer);
            team2.Remove(tankServer);
            tankServer.Identity.Destroy();
        }
        //Remove o Peer
        playerInGroup.Remove(peer);
        //Remove do grupo
        Matchmaking.Server.LeaveGroup(group, peer);
        //Verifica se o time vazio para retirar do game
        if (team1.Count >= 0 || team2.Count >= 0) return false;
        return true;
    }
}
