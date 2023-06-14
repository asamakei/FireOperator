using System;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
[VolumeComponentMenu("Example Effect")]
public class GlitchVolume : VolumeComponent // VolumeComponentを継承する
{
    public bool IsActive() => tintColor != Color.white;

    // Volumeコンポーネントで設定できる値にはXxxParameterクラスを使う
    public ColorParameter tintColor = new ColorParameter(Color.white);
}
