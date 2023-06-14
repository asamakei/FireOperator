using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraSetting", menuName = "ScriptableObjects/CameraSetting")]
public class CameraSetting : ScriptableObject {
    // 追従する速度の設定(0～1)
    public Vector2 Speed = Vector2.one;

    // 追従する方向の設定
    public bool IsMoveX = true;
    public bool IsMoveY = false;

    // プレイヤーとの相対座標
    public Vector2 Pivot = new Vector2(0,0);

}
