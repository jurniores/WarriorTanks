using System;
using Omni.Core;
using Omni.Threading.Tasks;
using UnityEngine;


public partial class PropertiesBase : NetworkBehaviour
{
    [NetworkVariable]
    protected string m_NameTank;
    [NetworkVariable]
    protected int m_HpTotal = 100;
    [NetworkVariable]
    protected int m_Hp = 100;
    [NetworkVariable]
    protected int m_Dano = 7;
    [NetworkVariable]
    protected int m_BulletPent = 10;
    [NetworkVariable]
    protected int m_BulletTotal = 50;
    [NetworkVariable]
    protected bool m_CowntDownBulletReddy = true;
    [NetworkVariable]
    protected float m_CountDown = 10;
    [NetworkVariable]
    protected int m_Level = 1;
    [NetworkVariable]
    protected float m_Exp = 0;
    [NetworkVariable]
    protected int m_ExpUp = 10;
    [SerializeField]
    protected TankBase tankBase;
    [NetworkVariable]
    public bool m_Death = false;
    [NetworkVariable]
    protected int m_Patente = 26;
    protected bool refresh = false;
    protected Transform spawnInitial;
    [NetworkVariable]
    public int m_Team = 0;

    private void Start()
    {
        DefaultNetworkVariableOptions = new()
        {
            Target = Target.GroupMembers,
        };
    }
    public void SetInfo(string name, Transform positionSpawn, int team)
    {
        m_NameTank = name;
        spawnInitial = positionSpawn;
        transform.position = positionSpawn.position;
        m_Team = team;
    }

    public async void Refresh()
    {
        if (BulletPent < 10 && BulletPent >= 0 && BulletTotal > 0)
        {
            print("REFReSH");
            refresh = true;
            //aplicar som de refresh
            await UniTask.WaitForSeconds(2);
            refresh = false;
            int bulletTotalPrev = BulletTotal;
            bulletTotalPrev -= 10;
            if (m_BulletTotal < 0)
            {
                BulletPent = 10 - bulletTotalPrev;
                BulletTotal = 0;
            }
            else
            {
                BulletPent = 10;
                BulletTotal = bulletTotalPrev;
            }
        }
    }

    protected async void DeathTank()
    {
        tankBase.enabled = false;
        enabled = false;
        Death = true;
        await UniTask.WaitForSeconds(5);
        Ressurexit();
    }
    protected void Ressurexit()
    {
        transform.position = spawnInitial.position;
        tankBase.Reposition(spawnInitial);
        tankBase.enabled = true;
        enabled = true;
        Death = false;
    }

}
