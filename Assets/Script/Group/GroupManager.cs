using System.Collections.Generic;
using Omni.Core;
using UnityEngine;

public class GroupManager : NetworkBehaviour
{
    public List<TankBase> team1, team2;
    public Bomb bomb;

    public void SendClientStartGame()
    {
        Remote.Invoke(ConstantsGame.START_GAME);
    }
    public virtual void EndGame(bool teamCounter)
    {
        team1.ForEach(tank =>
         {
            tank.FinishGame(bomb.Team, teamCounter);
         });
        team2.ForEach(tank =>
        {
            tank.FinishGame(bomb.Team, teamCounter);
        });
    }
}
