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
    private TankClient tankClient;
    private UI uI;
    protected override void OnStart()
    {
        if (IsLocalPlayer) uI = NetworkService.Get<UI>();
        tankClient = Identity.Get<TankClient>();
    }
    protected override void OnBaseNameTankChanged(string prevNameTank, string nextNameTank, bool isWriting)
    {
        uIPlayer.SetName(nextNameTank);
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
            tankClient.enabled = false;
            //Implementar outras coisas quando acabar
        }

        if (!IsLocalPlayer) return;
        uI.SetHp(nextHp);
    }

    protected override void OnBaseBulletPentChanged(int prevBulletPent, int nextBulletPent, bool isWriting)
    {
        if (!IsLocalPlayer) return;
        uI.MinusBullet(nextBulletPent);
    }

    protected override void OnBaseBulletTotalChanged(int prevBulletTotal, int nextBulletTotal, bool isWriting)
    {
        if (!IsLocalPlayer) return;
        uI.SetPent(nextBulletTotal);
    }
   
}
