using System;
using System.Collections.Generic;
using Omni.Threading.Tasks;
using UnityEngine;

public partial class PropertyServer : PropertiesBase
{
    [SerializeField]
    private List<Levels> levels;
    public void Demage(int dmg, PropertiesBase propertyQueMeatacou)
    {
        Hp -= dmg;
        if (Hp <= 0)
        {
            tankBase.enabled = false;
            propertyQueMeatacou.Exp += 5f;
        }
    }
    public bool Shot()
    {
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
            var level = levels[nextLevel - 1];

            HpTotal = level.hpTotal;
            Hp = level.hpTotal;

            Dano = level.dano;
            BulletTotal = level.bulletTotal;
            BulletPent = level.bulletPent;
            CountDown = level.cowntDown;
        }
    }
    protected override void OnBaseExpChanged(float prevExp, float nextExp, bool isWriting)
    {
        if (isWriting)
        {
            if (nextExp > expUp && Level <= levels.Count)
            {
                Level += 1;
                expUp += 2;
            }
        }
    }

}
