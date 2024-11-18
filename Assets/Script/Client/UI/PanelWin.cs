using Omni.Core;
using UnityEngine;
using UnityEngine.UI;

public class PanelWin : ServiceBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI txtWinLosePro;
    [SerializeField]
    private GameObject obj;
    private Image image;

    public override void Start()
    {
        base.Start();
        image = GetComponent<Image>();
        image.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetWinLose(bool winLose)
    {
        image.enabled = true;
        obj.SetActive(true);
        if (winLose)
        {
            txtWinLosePro.text = "WIN";
            txtWinLosePro.color = Color.green;
        }
        else
        {
            txtWinLosePro.text = "LOSE";
            txtWinLosePro.color = Color.red;
        }
    }
    public void ClosePanelDeath()
    {
        image.enabled = false;
        obj.SetActive(false);
    }
}
