using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anonymou8s : Touchable
{
    protected override void ThermalEvent(float diff)
    {
        if (_thermalEnergy >= MaxEnergy)
        {
        }
    }
}
