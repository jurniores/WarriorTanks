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
    private float vel = 1;
    [SerializeField]
    private TextMeshProUGUI nameTankText;
    private float hpComplete, hpAtual;
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
    }

    public void SetHp(float hp)
    {
        if(hpComplete == hp) return;
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
        await UniTask.WaitForSeconds(0.3f);
        demage = true;
    }
    public void SetName(string nameTank)
    {
        nameTankText.text = nameTank;
    }
}
