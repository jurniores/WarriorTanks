using Omni.Core;
using UnityEngine;


public partial class PropertiesBase : NetworkBehaviour
{
    [NetworkVariable]
    private string m_NameTank;
    [NetworkVariable]
    private int m_Hp = 100;
    [NetworkVariable]
    private int m_HpTotal = 100;
    [NetworkVariable]
    private int m_Dano = 7;
    [NetworkVariable]
    private int m_BulletPent = 10;
    [NetworkVariable]
    private int m_BulletTotal = 50;

    private void Start()
    {
        DefaultNetworkVariableOptions = new()
        {
            Target = Target.GroupMembers
        };
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsServer)
        {
            Hp--;
        }
    }

    public void SetInfo(string name)
    {
        m_NameTank = name;
    }

}
