using System.Collections.Generic;
using Omni.Core;
using UnityEngine;

public class GroupManager : NetworkBehaviour
{
    public List<TankBase> team1, 
    team2;

    public void SendClientStartGame()
    {
        Remote.Invoke(ConstantsGame.START_GAME);
    }
    public void EndGame(bool teamCounter)
    {
        // foreach (var tkClient in listTankClient.Values)
        // {
        //     tkClient.FinishGame(bomb.Team, teamCounter);
        // }
    }
}
