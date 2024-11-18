using System;
using System.Collections.Generic;
using Omni.Threading.Tasks;
using UnityEngine;

public partial class PropertyServer : PropertiesBase
{
    [SerializeField]
    private List<Levels> levels;
    public void Demage(int dmg, PropertyServer propertyQueMeatacou)
    {
        if (m_Death || m_Team == propertyQueMeatacou.m_Team) return;
        Hp -= dmg;
        if (Hp <= 0)
        {
            DeathTank();
            propertyQueMeatacou.MoreExp(5);
        }
    }

    public void MoreExp(int moreExp)
    {
        Exp += moreExp;
        ValidateUp();
    }
    public bool Shot()
    {
        if (refresh) return false;
        m_BulletPent -= 1;
        if (m_BulletPent == 0) Refresh();
        CowntDownBulletReddy = false;
        WaitBullet();
        return true;
    }
    async void WaitBullet()
    {
        await UniTask.WaitForSeconds(m_CountDown);
        CowntDownBulletReddy = true;
    }

    protected override void OnBaseLevelChanged(int prevLevel, int nextLevel, bool isWriting)
    {
        if (isWriting)
        {
            SetLevel(nextLevel);
        }
    }
    protected override void OnBaseDeathChanged(bool prevDeath, bool nextDeath, bool isWriting)
    {
        if (isWriting && !nextDeath && nextDeath != prevDeath)
        {
            print("Setei level novamente");
            SetLevel(m_Level);
        }
    }

    void SetLevel(int nextLevel)
    {
        var level = levels[nextLevel - 1];

        HpTotal = level.hpTotal;
        Hp = level.hpTotal;

        Dano = level.dano;
        BulletTotal = level.bulletTotal;
        BulletPent = level.bulletPent;
        CountDown = level.cowntDown;
    }

    void ValidateUp()
    {
        if (m_Exp >= m_ExpUp && Level <= levels.Count)
        {
            Level += 1;
            Exp = 0;
            ExpUp += 2;
        }
    }
    protected override void OnBaseExpChanged(float prevExp, float nextExp, bool isWriting)
    {
        print(nextExp);
        if (isWriting)
        {

        }
    }

}
