using Omni.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public partial class PropertiesBase : NetworkBehaviour
{
    [SerializeField]
    [NetworkVariable]
    private string m_NameTank;
    [SerializeField]
    [NetworkVariable]
    private int m_Hp = 100;
    [SerializeField]
    [NetworkVariable]
    private int m_HpTotal = 100;
    [SerializeField]
    [NetworkVariable]
    private int m_Dano = 7;
    [SerializeField]
    [NetworkVariable]
    private int m_BulletPent = 10;
    [SerializeField]
    [NetworkVariable]
    private int m_BulletTotal = 50;

    private void Start()
    {
        DefaultNetworkVariableOptions = new()
        {
            Target = Target.GroupMembers
        };
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space)){
            Hp--;
        }
    }
    public void SetInfo(string name)
    {
        m_NameTank = name;
    }

}
