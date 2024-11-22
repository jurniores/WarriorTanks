using System.Collections.Generic;
using Omni.Core;
using Omni.Threading.Tasks;
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
    private Dictionary<int, EntityList> EntityListDic = new();
    private float time = 10;
    private Bomb bomb;

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
            string nameGroup = $"GroupServer {GetHashCode()}";
            group = Matchmaking.Server.AddGroup(nameGroup);
            name = nameGroup;
        }

        Matchmaking.Server.JoinGroup(group, peer);
        playerInGroup.Add(peer);
        if(playerInGroup.Count >= qtdPlayers){
            bomb = GetPrefab(3).SpawnOnServer(Identity.Owner).Get<Bomb>();
            bomb.Group = group;
            playerInGroup.ForEach(player=>LoginRPCServer(player));
        }
    }

    void LoginRPCServer(NetworkPeer peer)
    {
        using var buffer = Pool.Rent();
        var identity = GetPrefab(0).SpawnOnServer(peer);
        var playerServer = identity.Get<PropertyServer>();
        string name = peer.Data.Get<string>("name");
        int team = EntityListDic.Count % 2 == 0 ? 1 : 2;

        playerServer.SetInfo(name, team == 1 ? spawn1.transform : spawn2.transform, team);

        buffer.SeekToBegin();
        buffer.WriteIdentity(identity);

        Remote.Invoke(ConstantsGame.TANK_SPAWN, buffer, groupId:group.Id);

        buffer.SeekToBegin();

        buffer.WriteAsBinary(EntityListDic);
        buffer.WriteIdentity(bomb.Identity);
        Remote.InvokeByPeer(ConstantsGame.TANK_LOGIN_ALL, peer, buffer, Target.Self);

        EntityListDic.Add(peer.Id, new EntityList
        {
            peerId = peer.Id,
            identityId = identity.IdentityId,
            nameTank = name,
            team = team
        });

        if (EntityListDic.Count >= qtdPlayers)
        {
            StartGame();
        }
    }
    async void StartGame()
    {
        var buffer = Pool.Rent();
        buffer.Write(timeBattle);
        Remote.Invoke(ConstantsGame.START_GAME, buffer, groupId: group.Id);
        buffer.Dispose();
        await UniTask.WaitForSeconds(timeBattle);
        Remote.Invoke(ConstantsGame.END_GAME, groupId: group.Id);
    }
}
