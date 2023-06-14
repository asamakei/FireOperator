using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitFace : MonoBehaviour
{
    [SerializeField] StraightFireShooter _straightFireShooter;
    Transform _transform;
    public Transform _Transform
    {
        get { return _transform; }
    }
    public SpriteRenderer spriteRenderer
    {
        get { return _spriteRenderer; }
    }
    public StraightFireShooter straightFireShooter
    {
        get { return _straightFireShooter; }
    }
    SpriteRenderer _spriteRenderer;
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _transform = transform;
    }
}
