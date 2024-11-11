using Omni.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public partial class PropertiesBase : NetworkBehaviour
{
    [SerializeField]
    [NetworkVariable]
    private string m_NameTank;
    [SerializeField]
    [NetworkVariable]
    private int m_Hp = 100;
    [SerializeField]
    [NetworkVariable]
    private int m_HpTotal = 100;
    [SerializeField]
    [NetworkVariable]
    private int m_Dano = 7;
    [SerializeField]
    [NetworkVariable]
    private int m_BulletPent = 10;
    [SerializeField]
    [NetworkVariable]
    private int m_BulletTotal = 50;

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
    
    partial void OnNameTankChanged(string prevNameTank, string nextNameTank, bool isWriting)
    {
        if(isWriting) return;
        AttClientName(nextNameTank);
        //print("Chamei Name");
    }

    partial void OnHpChanged(int prevHp, int nextHp, bool isWriting)
    {
        if(isWriting) return;
        AttClientHp(nextHp);
        //print("Chamei hp");
    }

    partial void OnHpTotalChanged(int prevJorge, int nextJorge, bool isWriting)
    {
        if(isWriting) return;
        AttClientHpTotal(nextJorge);
        //print("Chamei Hp total");
    }
    partial void OnBulletPentChanged(int prevBulletPent, int nextBulletPent, bool isWriting)
    {
        if(isWriting) return;
        AttClientBulletPent(nextBulletPent);
        //print("Chamei Pent");
    }
    
    partial void OnBulletTotalChanged(int prevBulletToal, int nextBulletToal, bool isWriting)
    {
        if(isWriting) return;
        AttClientBulletTotal(nextBulletToal);
        //print("Chamei Pent Total");
    }

    partial void OnDanoChanged(int prevDano, int nextDano, bool isWriting)
    {
        if(isWriting) return;
        AttClientDano(nextDano);
        //print("Chamei Dano");
    }
    protected virtual void AttClientName(string nameTank)
    {

    }
    protected virtual void AttClientHp(float hp)
    {

    }
    protected virtual void AttClientHpTotal(float hpTotal)
    {

    }

    protected virtual void AttClientBulletPent(int bulletPent)
    {

    }
    protected virtual void AttClientBulletTotal(int bulletTotal)
    {

    }
    protected virtual void AttClientDano(float dmg)
    {

    }


}
