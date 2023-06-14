using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchableIronNoLight : Touchable
{
    [SerializeField][ColorUsage(false, true)] private Color _color;

    private Color _defaultColor;
    private MeshRenderer _render;
    private Material _material;


    void Start()
    {
        _render = GetComponent<MeshRenderer>();
        if (_render != null)
        {
            _material = _render.material;
            _defaultColor = Color.black;
        } 
    }

    protected override void ThermalEvent(float diff)
    {
        float t;
        t = (_thermalEnergy - MinEnergy) / (MaxEnergy - MinEnergy);
        _material.SetColor("_Emission", Color.Lerp(_defaultColor, _color, t));
    }
}
