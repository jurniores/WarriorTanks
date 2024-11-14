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
    protected float m_CountDown = 2;
    [NetworkVariable]
    protected int m_Level = 1;
    [NetworkVariable]
    protected float m_Exp = 0;
    [SerializeField]
    protected TankBase tankBase;
    protected int expUp = 10;




    private void Start()
    {
        DefaultNetworkVariableOptions = new()
        {
            Target = Target.GroupMembers
        };
    }
    public void SetInfo(string name)
    {
        m_NameTank = name;
    }

    public async void Refresh()
    {
        if (BulletPent < 10 && BulletPent >= 0 && BulletTotal > 0)
        {
            await UniTask.WaitForSeconds(2);
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
   

  

}
