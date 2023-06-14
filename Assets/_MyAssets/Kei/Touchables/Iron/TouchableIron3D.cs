using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchableIron3D : Touchable
{
    [SerializeField] private Light _light;
    [SerializeField][ColorUsage(false, true)] private Color _color;

    private Color _defaultColor;
    private MeshRenderer _render;
    private Material _material;

    const float _lightIntensity = 8;

    void Start()
    {
        _render = GetComponent<MeshRenderer>();
        _material = _render.material;
        _material.EnableKeyword("_EMISSION");
        _defaultColor = Color.black;
    }

    protected override void ThermalEvent(float diff)
    {
        float t;
        t = (_thermalEnergy - MinEnergy) / (MaxEnergy - MinEnergy);
        _material.SetColor("_EmissionColor", Color.Lerp(_defaultColor, _color, t));
        _light.intensity = _lightIntensity * t;
    }
}
