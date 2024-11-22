using Omni.Core;
using Omni.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayer : MonoBehaviour
{
    [SerializeField]
    private Image imgHpFilled, imgFilledOrange;
    [SerializeField]
    private Image imgMpFilled, imagePatente;
    [SerializeField]
    private float vel = 1;
    [SerializeField]
    private TextMeshProUGUI nameTankText, levelTxtPlayer;
    private float hpComplete, hpAtual;
    private float mpComplete, mpAtual;
    private float hpOrange;
    private bool demage;
    // Update is called once per frame
    void Update()
    {
        if (demage && hpOrange >= hpAtual)
        {
            hpOrange -= Time.deltaTime * vel;
            imgFilledOrange.fillAmount = hpOrange / hpComplete;
            if (hpOrange <= hpAtual) demage = false;
        }

        if (mpAtual <= mpComplete)
        {
            mpAtual += Time.deltaTime;
            imgMpFilled.fillAmount = mpAtual / mpComplete;
        }
    }
    public void SetLvl(int lvl)
    {
        levelTxtPlayer.text = lvl.ToString();
    }
    public void SetHp(float hp)
    {
        if (hpComplete == hp) return;
        hpComplete = hp;
        hpAtual = hp;
        hpOrange = hp;

        imgHpFilled.fillAmount = 1;
        imgFilledOrange.fillAmount = 1;
    }
    public async void Demage(float hp)
    {
        hpAtual = hp;
        imgHpFilled.fillAmount = hpAtual / hpComplete;
        await UniTask.WaitForSeconds(0.5f);
        demage = true;
    }
    public void MpAction(float cowntDownMp)
    {
        mpComplete = cowntDownMp;
        mpAtual = 0;
        imgMpFilled.fillAmount = mpAtual;
    }
    public void SetName(string nameTank)
    {
        nameTankText.text = nameTank;
    }
    public void SetPatente(Sprite sprite){
        imagePatente.sprite = sprite;
    }
}
