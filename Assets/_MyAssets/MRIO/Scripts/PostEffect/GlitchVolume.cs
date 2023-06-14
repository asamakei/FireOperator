using System;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
[VolumeComponentMenu("Example Effect")]
public class GlitchVolume : VolumeComponent // VolumeComponent���p������
{
    public bool IsActive() => tintColor != Color.white;

    // Volume�R���|�[�l���g�Őݒ�ł���l�ɂ�XxxParameter�N���X���g��
    public ColorParameter tintColor = new ColorParameter(Color.white);
}
