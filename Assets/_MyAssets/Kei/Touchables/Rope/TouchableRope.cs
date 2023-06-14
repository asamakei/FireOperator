using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TouchableRope : Touchable {

    [SerializeField] float _particleStartEnergy = 0;
    [SerializeField] Renderer _render;
    [SerializeField] Color _burningColor = Color.red;
    [SerializeField] Color _burnedColor = Color.black;
    [SerializeField] ParticleSystem _burningParticle;
    [SerializeField] ParticleSystem _vanishParticle;
    [SerializeField] float _burningEmissionRate = 1;
    [SerializeField] float _vanishEmissionRate = 1;
    [SerializeField] bool _isCustomizableRect = false;    

    Rigidbody2D _rigid;
    BoxCollider2D _collider;
    ParticleSystem.EmissionModule _emission;
    Color _initColor;

    bool _isParticlePlaying = false;
    bool _isBurned = false;
    float _emissionRate;
    float _startBlackEnergy;

    const float _burningAppropriateValue = 100 / 1.8f;
    const float _vanishAppropriateValue = 3000 / 1.8f;

    void Start() {
        _rigid = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        _initColor = _render.material.color;

        if (!_isCustomizableRect) {
            //AutoSetRect(GetSize());
        }

        _emission = _burningParticle.emission;
        _emissionRate = _emission.rateOverTime.constantMax;

        _burningParticle.Stop();
        _isParticlePlaying = false;

        _startBlackEnergy = Mathf.Lerp(_particleStartEnergy, MaxEnergy, 0.6f);

    }
    protected virtual Vector3 GetSize() {
        return ((SpriteRenderer)_render).size;
    }

    protected override void ThermalEvent(float diff) {
        if (_isBurned)return;

        if (_thermalEnergy >= MaxEnergy) {// burned
            _isBurned = true;
            _burningParticle.Stop();
            _vanishParticle.Play();

            _rigid.simulated = false;
            transform.localScale = Vector3.zero;
            StartCoroutine(Vanish(_vanishParticle.main.startLifetime.constantMax));
        } else if (_thermalEnergy >= _startBlackEnergy) {// change color
            _render.material.color = Color.Lerp(_burningColor, _burnedColor, (_thermalEnergy- _startBlackEnergy) / (MaxEnergy - _startBlackEnergy));

        } else if (_thermalEnergy >= _particleStartEnergy) {// start burning
            if (!_isParticlePlaying) {
                _burningParticle.Play();
                _isParticlePlaying = true;
            }
            _emission.rateOverTime = _emissionRate * (_thermalEnergy - _particleStartEnergy) / (_startBlackEnergy - _particleStartEnergy);

        }else if (_thermalEnergy>0) {// change color
            if (_isParticlePlaying) {
                _burningParticle.Stop();
                _isParticlePlaying = false;
            }
            _render.material.color = Color.Lerp(_initColor,_burningColor, _thermalEnergy / _particleStartEnergy);
        }
    }
    IEnumerator Vanish(float time) {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }

    public override void AutoMake() {
        if (_collider == null) _collider = GetComponent<BoxCollider2D>();
        Vector3 shapeScale = GetSize();
        Vector3 scale = transform.localScale;
        ParticleSystem.ShapeModule burningShape = _burningParticle.shape;
        ParticleSystem.ShapeModule vanishShape = _vanishParticle.shape;
        scale = new Vector3(shapeScale.x * scale.x, shapeScale.y * scale.y, shapeScale.z * scale.z);
        vanishShape.scale = scale;
        burningShape.scale = scale;
        _collider.size = shapeScale;

        float area = scale.x * scale.y;
        ParticleSystem.EmissionModule burningEmission = _burningParticle.emission;
        ParticleSystem.EmissionModule vanishEmission = _vanishParticle.emission;
        burningEmission.rateOverTime = _burningEmissionRate * area * _burningAppropriateValue;
        vanishEmission.rateOverTime = _vanishEmissionRate * area * _vanishAppropriateValue;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(TouchableRope))]
    [CanEditMultipleObjects]
    public class TouchableRopeEditor : TouchableEditor {}
#endif
}
