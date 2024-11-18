using Omni.Core;
using TMPro;
using UnityEngine;

public class GameInfo : ServiceBehaviour
{
    [SerializeField]
    private TextMeshProUGUI team1Txt, team12Txt, timeTxt;
    private float time = 0;
    void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            timeTxt.text = $"Time: {Mathf.Round(time)}";
        }
    }
    public void SetInfoTime(float time)
    {
        this.time = time;
    }
}
