using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraSetting", menuName = "ScriptableObjects/CameraSetting")]
public class CameraSetting : ScriptableObject {
    // �Ǐ]���鑬�x�̐ݒ�(0�`1)
    public Vector2 Speed = Vector2.one;

    // �Ǐ]��������̐ݒ�
    public bool IsMoveX = true;
    public bool IsMoveY = false;

    // �v���C���[�Ƃ̑��΍��W
    public Vector2 Pivot = new Vector2(0,0);

}
