using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchableIce3D : Touchable
{
    static float EPSILON = 0.1f;
    [SerializeField] float _meltingSpeed = 0.01f;
    [SerializeField] ParticleSystem _particle;
    IceState iceState;
    Transform _transform;
    float defaultmass;
    Rigidbody _rigidbody;
    bool _isParticlePlaying;
    private void Awake()
    {
        _transform = transform;
        _particle.Stop();
        _rigidbody = GetComponent<Rigidbody>();
        defaultmass = _rigidbody.mass;
    }
    public void ChangeIceState(IceState iceState)
    {
        this.iceState = iceState;
    }
    protected override void ThermalEvent(float diff)
    {
        if (_thermalEnergy >= MaxEnergy)
        {
            _transform.localScale -= Vector3.one * _meltingSpeed * Time.deltaTime;
            _rigidbody.mass = defaultmass * _transform.localScale.x;
            if (_particle == null) return;
            _isParticlePlaying = _particle.isEmitting;
            if (_isTouchable && !_isParticlePlaying)
            {
                _particle.Play();
            }
        }
        else if (_isParticlePlaying)
        {
            if (_particle == null) return;
            _particle.Stop();
        }
        if (_transform.localScale.x <= EPSILON) gameObject.SetActive(false);
    }
    protected override void TouchEnterEvent()
    {
        ChangeIceState(IceState.Melting);
    }
    protected override void TouchExitEvent()
    {
        ChangeIceState(IceState.Idle);
    }
}
