using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KanKikuchi.AudioManager;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TouchableWater : Touchable
{
    static float BOILDURATION = 3.8f;
    static string BOIL = "Boil";
    [SerializeField] private float _boilingSpeed = 1;
    [SerializeField] private float _bubbleStartEnergy = 0;
    [SerializeField] private float _bubbleEmissionRate = 1;
    [SerializeField] private BoxCollider _bubbleRect;

    private Transform _trans;
    private SpriteRenderer _render;
    private BoxCollider2D _collider;
    private ParticleSystem _particle;
    private BuoyancyEffector2D _effector;
    private ParticleSystem.EmissionModule _emission;
    private float _bubbleRate;
    private bool _isParticlePlaying = false;

    private float _remain = 1;
    AudioData boiledAudioData;
    float time;
    const float _bubbleAppropriateValue = 200 / 8f;

    void Awake()
    {
        boiledAudioData = AudioDataManager.Instance.GetAudioData(BOIL);
        _trans = transform;
        _render = GetComponent<SpriteRenderer>();
        _particle = GetComponent<ParticleSystem>();
        _emission = _particle.emission;
        _bubbleRate = _emission.rateOverTime.constantMax;
        ThermalEvent(0);
        time = BOILDURATION;
    }

    protected override void ThermalEvent(float diff)
    {
        if (_thermalEnergy >= MaxEnergy)
        {
            SetHeight(_remain - Time.deltaTime * _boilingSpeed);
        }
        _isParticlePlaying = _particle.isEmitting;
        if (_thermalEnergy >= _bubbleStartEnergy)
        {
            
            float bubbleSpeed;
            bubbleSpeed = _thermalEnergy - _bubbleStartEnergy;
            bubbleSpeed /= MaxEnergy - _bubbleStartEnergy;
            _emission.rateOverTime = _bubbleRate * bubbleSpeed;
            if (_isTouchable && !_isParticlePlaying)
            {
                _particle.Play();
            }
            time += Time.deltaTime;
            if(time > BOILDURATION && boiledAudioData != null)
            {
                time = 0;
                SEManager.Instance.Play(boiledAudioData.audioClip, boiledAudioData.volume);
            }
        }
        else
        {
            if (_isParticlePlaying && _thermalEnergy <= 0)
            {
                _particle.Stop();
            }
            if (time < BOILDURATION) time = BOILDURATION;
        }
    }

    private void SetHeight(float rate)
    {
        rate = Mathf.Clamp(rate, 0, 1);
        _remain = Mathf.Min(_remain, rate);
        _trans.localScale = new Vector3(1, _remain, 1);
        if (rate == 0)
        {
            _isTouchable = false;
            _particle.Stop();
            gameObject.SetActive(false);
        }
    }

    public override void AutoMake()
    {
        if (_render == null) _render = GetComponent<SpriteRenderer>();
        if (_collider == null) _collider = GetComponent<BoxCollider2D>();
        if (_effector == null) _effector = GetComponent<BuoyancyEffector2D>();
        if (_particle == null) _particle = GetComponent<ParticleSystem>();

        Vector3 shapeScale = _render.size;
        ParticleSystem.ShapeModule bubbleShape = _particle.shape;
        BuoyancyEffector2D effector = GetComponent<BuoyancyEffector2D>();
        bubbleShape.scale = new Vector3(shapeScale.x, 0, shapeScale.y);
        bubbleShape.position = new Vector3(0, shapeScale.y / 2, 0);
        _collider.size = shapeScale;
        _collider.offset = new Vector2(0, shapeScale.y / 2);
        _bubbleRect.size = new Vector3(shapeScale.x, shapeScale.y, 1);
        _bubbleRect.center = new Vector3(0, shapeScale.y / 2, 0);
        effector.surfaceLevel = shapeScale.y;

        float area = shapeScale.x * shapeScale.y;
        ParticleSystem.EmissionModule bubbleEmission = _particle.emission;
        bubbleEmission.rateOverTime = _bubbleEmissionRate * area * _bubbleAppropriateValue;

    }

#if UNITY_EDITOR
    [CustomEditor(typeof(TouchableWater))]
    [CanEditMultipleObjects]
    public class TouchableWaterEditor : TouchableEditor { }
#endif
}
