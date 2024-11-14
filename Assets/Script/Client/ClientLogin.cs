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
    private GameObject panel;
    public Texture2D cursorTexture;

    protected override void OnStart()
    {
        SetCurtor(false);
        button.onClick.AddListener(Login);
    }

    private void Login()
    {
        if (!string.IsNullOrEmpty(inputField.text) || inputField.text.Length > 4)
        {
            using var buffer = NetworkManager.Pool.Rent();
            buffer.WriteString(inputField.text);
            Local.Invoke(ConstantsGame.TANK_LOGIN, buffer);
        }
    }

    [Client(ConstantsGame.TANK_LOGIN)]
    void LoginRpcClient(DataBuffer buffer)
    {
        buffer.ReadIdentity(out var peerId, out var identityId);
        SpawnOnClient(peerId, identityId);
        panel.SetActive(false);
        SetCurtor(true);
    }

    [Client(ConstantsGame.TANK_LOGIN_ALL)]
    void LoginRpcAllClient(DataBuffer buffer)
    {
        var players = buffer.ReadAsBinary<Dictionary<int, EntityList>>();

        foreach (var player in players.Values)
        {
            SpawnOnClient(player.peerId, player.identityId);
        }
    }


    void SpawnOnClient(int peerId, int identityId)
    {
        NetworkManager.GetPrefab(1).SpawnOnClient(peerId, identityId);
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
