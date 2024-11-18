using UnityEngine.UI;
using TMPro;
using UnityEngine;
using Omni.Core;

public class UI : ServiceBehaviour
{
    [SerializeField]
    private TextMeshProUGUI bulletText, hpText, timeBulletCowntDownText, lvlTxt, expTxt;
    [SerializeField]
    private Image imageCowntDown, imageCowntdownSlide;
    [SerializeField]
    private float vel = 1;

    private float time, timeInitial, timeCd, timeCdInitial;
    private int bullet, bulletTotal;
    private float hpTotal, hp;
    private float exp, expTotal;
    // Update is called once per frame
    void Update()
    {
        if (time <= timeInitial)
        {
            time += Time.deltaTime * vel;
            imageCowntdownSlide.fillAmount = time / timeInitial;
            timeBulletCowntDownText.text = time.ToString("F1");
            if (time >= timeInitial)
            {
                imageCowntDown.gameObject.SetActive(false);
            }
        }

        if (timeCd <= timeCdInitial)
        {
            timeCd += Time.deltaTime;
            timeBulletCowntDownText.text = timeCd.ToString("F1");
        }
    }
    public void SetExp(float exp){
        this.exp = exp;
        expTxt.text = $"Exp: {exp}/{expTotal}";
    }
    public void SetExpTotal(int expTotal)
    {
        this.expTotal = expTotal;
        expTxt.text = $"Exp: {exp}/{expTotal}";
    }
    public void SetLvl(int lvl)
    {
        lvlTxt.text = $"Level: {lvl}";
    }
    
    public void SetTimeSlide(float time)
    {
        this.time = time;
        timeInitial = time;
        imageCowntDown.gameObject.SetActive(false);
    }

    public void SetPent(int bulletPent)
    {
        bulletTotal = bulletPent;
        bulletText.text = $"{bullet}/{bulletTotal}";
    }
    public void BulletPent(int bullet)
    {
        this.bullet = bullet;
        bulletText.text = $"{bullet}/{bulletTotal}";
    }
    public void MinusBullet()
    {
        bullet -= 1;
        bulletText.text = $"{bullet}/{bulletTotal}";
    }
    public void SetHp(float hp)
    {
        this.hp = hp;
        hpText.text = $"{hp}/{hpTotal}";
    }
    public void setHpTotal(float hpTotal)
    {
        this.hpTotal = hpTotal;
        hpText.text = $"{hp}/{hpTotal}";
    }
    public void cowntDown(float cowntDownMp)
    {
        timeCdInitial = cowntDownMp;
        timeCd = 0;
        timeBulletCowntDownText.text = "0";
    }

}
