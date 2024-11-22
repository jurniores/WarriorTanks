using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Omni.Core;
using UnityEngine;

public class GroupManagerClient : GroupManager
{
    [SerializeField]
    private Dictionary<int, TankClient> listTankClient = new();
    [SerializeField]
    private GameObject panel;
    private GameInfo gameInfo;
    public Texture2D cursorTexture;
    private Bomb bomb;

    protected override void OnStart()
    {
        gameInfo = NetworkService.Get<GameInfo>();
        panel = GameObject.Find("Panel");

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

    [Client(ConstantsGame.TANK_LOGIN_ALL)]
    void LoginRpcAllClient(DataBuffer buffer)
    {
        var players = buffer.ReadAsBinary<Dictionary<int, EntityList>>();
        buffer.ReadIdentity(out var peerId, out var identityId);

        //Instanciando a bomba
        bomb = NetworkManager.GetPrefab(3).SpawnOnClient(peerId, identityId).Get<Bomb>();

        foreach (var player in players.Values)
        {
            SpawnOnClient(player.peerId, player.identityId);
        }
    }
    void SpawnOnClient(int peerId, int identityId)
    {
        var tankClient = NetworkManager.GetPrefab(1).SpawnOnClient(peerId, identityId).Get<TankClient>();
        listTankClient.Add(peerId, tankClient);
    }
    
    [Client(ConstantsGame.TANK_SPAWN)]
    void LoginRpcClient(DataBuffer buffer)
    {
        buffer.ReadIdentity(out var peerId, out var identityId);
        SpawnOnClient(peerId, identityId);
        panel.SetActive(false);
        
    }


    [Client(ConstantsGame.END_GAME)]
    void EndGameClientRPC(DataBuffer buffer)
    {
        EndGame(false);
    }


}
