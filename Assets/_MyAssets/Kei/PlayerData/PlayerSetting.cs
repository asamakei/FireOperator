using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSetting", menuName = "ScriptableObjects/PlayerSetting")]
public class PlayerSetting : ScriptableObject{
    // ���ړ��̑��x
    public float WalkSpeed = 1f;
    
    // �W�����v��
    public float JumpForce = 1f;

    // �������x�ƏI�[���x
    public float GravityScale = 1f;
    public float TerminalVelocity = 10f;

    // ���ړ��̊��� �n�ʂƋ󒆂ƕX��
    public float InertiaScaleGround = 0f;
    public float InertiaScaleAir = 0f;
    public float InertiaScaleIce = 0f;

}
