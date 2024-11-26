using System.Linq;
using Omni.Core;
using Omni.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelLogin : ServiceBehaviour
{
    [SerializeField]
    private ToggleGroup toggleGroup;
    [SerializeField]
    private TextMeshProUGUI txtProWait;
    [SerializeField]
    private Button btnSend;
    public byte typeRoom = 0;
    private bool send = true;

    bool buttonSend = false;
    public override void Start()
    {
        base.Start();
        typeRoom = 3;
        GetSelectedToggle(typeRoom);
        btnSend.onClick.AddListener(ButtonSend);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void GetSelectedToggle(byte nToggle)
    {
        typeRoom = nToggle;
    }

    public void ButtonSend()
    {
        if (send)
        {
            NetworkService.Get<ClientLogin>().Login();
        }
        else
        {
            RecieveLoginSend();
        }

    }

    public void RecieveLoginSend()
    {
        var txtBtn = btnSend.GetComponentInChildren<TextMeshProUGUI>();
        var imgBtn = btnSend.GetComponent<Image>();
        if (send)
        {
            txtBtn.text = "Cancel";
            buttonSend = true;
            txtProWait.gameObject.SetActive(true);
            imgBtn.color = Color.red;
            AnimPleaseWait();
        }
        else
        {
            txtProWait.gameObject.SetActive(false);
            imgBtn.color = Color.green;
            txtBtn.text = "Send";
            buttonSend = false;
        }
        send = !send;

    }


    async void AnimPleaseWait()
    {
        
        if (buttonSend)
        {
            txtProWait.text = "Plase Wait.";
            await UniTask.WaitForSeconds(1);
            txtProWait.text = "Plase Wait..";
            await UniTask.WaitForSeconds(1);
            txtProWait.text = "Plase Wait...";
            await UniTask.WaitForSeconds(1);
            AnimPleaseWait();
        }
    }

}
