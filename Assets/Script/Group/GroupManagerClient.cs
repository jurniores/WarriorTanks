using System.Collections.Generic;

using Omni.Core;
using UnityEngine;

public class GroupManagerClient : GroupManager
{
    [SerializeField]
    private PanelLogin panel;
    private GameInfo gameInfo;
    public Texture2D cursorTexture;

    protected override void OnStart()
    {
        gameInfo = NetworkService.Get<GameInfo>();
        panel = panel = NetworkService.Get<PanelLogin>();

    }

    [Client(ConstantsGame.START_GAME)]
    public void RecieveStartGame(DataBuffer buffer)
    {

    }
    [Client(ConstantsGame.START_GAME)]
    void StartGameClientRPC(DataBuffer buffer)
    {
        float time = buffer.Read<float>();
        gameInfo.SetInfoTime(time);
    }

    [Client(ConstantsGame.TANK_SPAWN_BOMB)]
    void LoginRpcAllClient(DataBuffer buffer)
    {
        buffer.ReadIdentity(out var peerId, out var identityId);

        //Instanciando a bomba
        bomb = NetworkManager.GetPrefab(3).SpawnOnClient(peerId, identityId).Get<Bomb>();
        bomb.groupManager = this;
    }
    void SpawnOnClient(int peerId, int identityId, int team)
    {
        var tankClient = NetworkManager.GetPrefab(1).SpawnOnClient(peerId, identityId).Get<TankClient>();
        tankClient.groupManager = this;
         if (team == 1)
        {
            team1.Add(tankClient);
        }
        else
        {
            team2.Add(tankClient);
        }
    }

    [Client(ConstantsGame.TANK_SPAWN)]
    void LoginRpcClient(DataBuffer buffer)
    {
        buffer.ReadIdentity(out var peerId, out var identityId);
        int team = buffer.Read<int>();
        print("peer vindo para mim " + peerId);
        SpawnOnClient(peerId, identityId, team);
        panel.transform.parent.gameObject.SetActive(false);

    }


    [Client(ConstantsGame.END_GAME)]
    void EndGameClientRPC(DataBuffer buffer)
    {
        EndGame(false);
    }

    


}
