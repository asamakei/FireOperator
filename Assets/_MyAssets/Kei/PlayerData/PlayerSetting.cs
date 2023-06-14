using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSetting", menuName = "ScriptableObjects/PlayerSetting")]
public class PlayerSetting : ScriptableObject{
    // 横移動の速度
    public float WalkSpeed = 1f;
    
    // ジャンプ力
    public float JumpForce = 1f;

    // 落下速度と終端速度
    public float GravityScale = 1f;
    public float TerminalVelocity = 10f;

    // 横移動の慣性 地面と空中と氷上
    public float InertiaScaleGround = 0f;
    public float InertiaScaleAir = 0f;
    public float InertiaScaleIce = 0f;

}
