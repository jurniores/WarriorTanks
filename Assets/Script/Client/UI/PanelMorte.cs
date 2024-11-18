using Omni.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelMorte : ServiceBehaviour
{
    [SerializeField]
    private TextMeshProUGUI txtTimeMorte;
    [SerializeField]
    private GameObject obj;
    private float time;
    private Image image;

    public override void Start()
    {
        base.Start();
        image = GetComponent<Image>();
    }
    void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            txtTimeMorte.text = Mathf.Round(time).ToString();
        }
    }

    public void SetTimeDeath(float time)
    {
        obj.SetActive(true);
        image.enabled = true;
        this.time = time;
    }
    public void ClosePanelDeath()
    {
        image.enabled = false;
        obj.SetActive(false);
    }
}
