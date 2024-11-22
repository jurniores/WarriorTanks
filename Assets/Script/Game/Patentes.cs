using System.Collections.Generic;
using Omni.Core;
using UnityEngine;

public class Patentes : ServiceBehaviour
{
    [SerializeField]
    private List<Sprite> patentes;


    public Sprite SetPatentes(int patente)
    {
        return patentes[patente];
    }
}
