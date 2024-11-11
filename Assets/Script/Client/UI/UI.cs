using UnityEngine.UI;
using TMPro;
using UnityEngine;
using Omni.Core;

public class UI : ServiceBehaviour
{
    [SerializeField]
    private TextMeshProUGUI bulletText, hpText, timeBulletCowntDownText;
    [SerializeField]
    private Image imageCowntDown, imageCowntdownSlide;
    [SerializeField]
    private float vel = 1;

    private float time, timeInitial;
    private int bullet, bulletTotal;
    private float hpTotal, hp;
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

    public void MinusBullet(int bullet)
    {
        this.bullet = bullet;
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

}
