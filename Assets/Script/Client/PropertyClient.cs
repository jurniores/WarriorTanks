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

    protected override void AttClientName(string nameTank)
    {
        uIPlayer.SetName(nameTank);
    }

    protected override void AttClientHp(float hpChange)
    {
        uIPlayer.Demage(hpChange);

        if (hpChange <= 0)
        {
            anim.Play("explosion");
            tankClient.enabled = false;
            //Implementar outras coisas quando acabar
        }
        
        if (!IsLocalPlayer) return;
        uI.SetHp(hpChange);

    }
    protected override void AttClientHpTotal(float hpTotal)
    {
        uIPlayer.SetHp(hpTotal);
        if (!IsLocalPlayer) return;
        uI.setHpTotal(hpTotal);
    }
    protected override void AttClientBulletPent(int variableChange)
    {
        if (!IsLocalPlayer) return;
        uI.MinusBullet(variableChange);
    }
    protected override void AttClientBulletTotal(int variableChange)
    {
        if (!IsLocalPlayer) return;
        uI.SetPent(variableChange);
    }
    protected override void AttClientDano(float variableChange)
    {
        //Implementar
    }
}
