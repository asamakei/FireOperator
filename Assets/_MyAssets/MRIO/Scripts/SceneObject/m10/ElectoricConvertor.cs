using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectoricConvertor : Touchable
{
    [SerializeField] private Color _color;
    Material _material;
    private Color _defaultColor = Color.black;
    private void Awake()
    {
        if (TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
        {
            _material = spriteRenderer.material;
            _defaultColor = _material.color;
        }

    }

    protected override void ThermalEvent(float diff)
    {
        float t;
        t = (_thermalEnergy - MinEnergy) / (MaxEnergy - MinEnergy);
        if (_material != null) _material.color = Color.Lerp(_defaultColor, _color, t);
    }
}
