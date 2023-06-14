using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TouchableIcicle : TouchableIce {
    bool _isFall = false;
    protected override void SubThermalEvent(float diff) {
        if (_thermalEnergy >= MaxEnergy) {
            if (!_isFall) {
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                _isFall = true;
            }
        }
    }
}
