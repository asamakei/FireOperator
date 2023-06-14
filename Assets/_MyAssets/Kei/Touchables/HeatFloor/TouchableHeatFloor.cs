using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TouchableHeatFloor : Touchable{
    [SerializeField] ParticleSystem _particle;
    [SerializeField] float _burningEmissionRate = 1;

    private BoxCollider2D _collider;
    private SpriteRenderer _render;

    const float _burningAppropriateValue = 200 / 1.8f;

    void Start() {
        _collider = GetComponent<BoxCollider2D>();
        _render = GetComponent<SpriteRenderer>();
        _particle = GetComponent<ParticleSystem>();
    }
    public override void AutoMake() {
        if (_collider == null) _collider = GetComponent<BoxCollider2D>();
        if (_render == null) _render = GetComponent<SpriteRenderer>();
        if (_particle == null) _particle = GetComponent<ParticleSystem>();

        Vector3 shapeScale = _render.size;
        Vector3 scale = transform.localScale;
        ParticleSystem.ShapeModule particleShape = _particle.shape;
        Vector3 particleScale = new Vector3(shapeScale.x * scale.x, shapeScale.y * scale.y / 2f, shapeScale.z * scale.z);
        Vector3 colliderScale = new Vector3(shapeScale.x, shapeScale.y+0.1f, shapeScale.z);
        particleShape.scale = particleScale;
        particleShape.position = Vector3.up * particleScale.y / 2;
        _collider.size = colliderScale;

        float area = scale.x * scale.y;
        ParticleSystem.EmissionModule particleEmission = _particle.emission;
        particleEmission.rateOverTime = _burningEmissionRate * area * _burningAppropriateValue;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(TouchableRope))]
    [CanEditMultipleObjects]
    public class TouchableRopeEditor : TouchableEditor { }
#endif
}
