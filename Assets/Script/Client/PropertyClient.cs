using Omni.Core;
using TMPro;
using UnityEngine;

public partial class PropertyClient : PropertiesBase
{
    [SerializeField]
    private TextMeshProUGUI nameTextPro;
    [SerializeField]
    private UIPlayer uIPlayer;
    [SerializeField] Animator anim;
    [SerializeField] private PanelMorte panelMorte;
    public UI uI;
    protected override void OnStart()
    {
        if (IsLocalPlayer)
        {
            panelMorte = NetworkService.Get<PanelMorte>();
            uI = NetworkService.Get<UI>();
        }
    }
    protected override void OnBaseNameTankChanged(string prevNameTank, string nextNameTank, bool isWriting)
    {
        uIPlayer.SetName(nextNameTank);
    }
    protected override void OnBaseDeathChanged(bool prevDeath, bool nextDeath, bool isWriting)
    {
        if (nextDeath == true)
        {
            if (IsLocalPlayer) panelMorte.SetTimeDeath(5);
        }
        else
        {
            if (IsLocalPlayer) panelMorte.ClosePanelDeath();
            print("Ressugiu");
            anim.Play("normal");
            tankBase.enabled = true;
        }
    }
    protected override void OnBaseHpTotalChanged(int prevHpTotal, int nextHpTotal, bool isWriting)
    {
        uIPlayer.SetHp(nextHpTotal);
        if (!IsLocalPlayer) return;
        uI.setHpTotal(nextHpTotal);
    }
    protected override void OnBaseHpChanged(int prevHp, int nextHp, bool isWriting)
    {
        uIPlayer.Demage(nextHp);

        if (nextHp <= 0)
        {
            anim.Play("explosion");
            tankBase.enabled = false;
        }

        if (!IsLocalPlayer) return;
        uI.SetHp(nextHp);
    }

    protected override void OnBaseBulletPentChanged(int prevBulletPent, int nextBulletPent, bool isWriting)
    {
        if (!IsLocalPlayer) return;
        uI.BulletPent(nextBulletPent);
    }
    protected override void OnBaseExpChanged(float prevExp, float nextExp, bool isWriting)
    {
        print(nextExp);
        if (IsLocalPlayer) uI.SetExp(nextExp);
    }
    protected override void OnBaseLevelChanged(int prevLevel, int nextLevel, bool isWriting)
    {
        if (IsLocalPlayer) uI.SetLvl(nextLevel);
        uIPlayer.SetLvl(nextLevel);
    }
    protected override void OnBaseBulletTotalChanged(int prevBulletTotal, int nextBulletTotal, bool isWriting)
    {
        if (!IsLocalPlayer) return;
        uI.SetPent(nextBulletTotal);
    }
    protected override void OnBaseExpUpChanged(int prevExpUp, int nextExpUp, bool isWriting)
    {
        if (IsLocalPlayer) uI.SetExpTotal(nextExpUp);
    }
    protected override void OnBaseCowntDownBulletReddyChanged(bool prevCowntDownBulletReddy, bool nextCowntDownBulletReddy, bool isWriting)
    {
        if (!nextCowntDownBulletReddy)
        {
            if (IsLocalPlayer) uI.cowntDown(m_CountDown);
            uIPlayer.MpAction(m_CountDown);
        }
    }
}
