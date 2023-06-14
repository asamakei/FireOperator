using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchableBox3D : TouchableRope {
    protected override Vector3 GetSize() {
        return Vector3.one;
    }
}
