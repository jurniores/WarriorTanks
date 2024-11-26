using System;
using System.Collections.Generic;
using Omni.Core;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ClientLogin : ClientBehaviour
{
    [SerializeField]
    private TMP_InputField inputField;
    [SerializeField]
    private Button button;
    [SerializeField]
    private PanelLogin panel;
    private GroupManagerClient groupManagerClient;
    public Texture2D cursorTexture;


    protected override void OnStart()
    {
        SetCurtor(false);
        panel = NetworkService.Get<PanelLogin>();
       
    }

    public void Login()
    {
        if (!string.IsNullOrEmpty(inputField.text) || inputField.text.Length > 4)
        {
            using var buffer = NetworkManager.Pool.Rent();
            buffer.WriteString(inputField.text);
            buffer.Write(panel.typeRoom);
            Local.Invoke(ConstantsGame.LOGIN, buffer);
        }
    }
    [Client(ConstantsGame.LOGIN)]
    private void LoginRecieveRPC(DataBuffer buffer)
    {
        buffer.ReadIdentity(out var peerId, out var identityId);
        groupManagerClient = NetworkManager.GetPrefab(5).SpawnOnClient(peerId, identityId).Get<GroupManagerClient>();
        panel.RecieveLoginSend();
    }

    public void SetCurtor(bool active)
    {
        if (active)
        {
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }
}
