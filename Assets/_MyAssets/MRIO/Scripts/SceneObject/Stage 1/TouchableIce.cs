using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KanKikuchi.AudioManager;
public enum IceState
{
    Idle,
    Melting
}
[RequireComponent(typeof(Rigidbody2D))]
public class TouchableIce : Touchable
{
    static string ICEMELT = "Melt";
    static float EFFECTINTERVAL = 0.1f;
    static float EPSILON = 0.1f;
    [SerializeField] float _meltingSpeed = 0.01f;
    [SerializeField] ParticleSystem _particle;
    IceState iceState;
    Transform _transform;
    float defaultmass;
    float defaultScaleX;
    Vector3 normalizedDefaultScale;
    Rigidbody2D _rigidbody2d;
    bool _isParticlePlaying;
    AudioData meltData = null;
    float time;
    private void Awake()
    {
        if (AudioDataManager.Instance != null) meltData = AudioDataManager.Instance.GetAudioData(ICEMELT);

        _transform = transform;
        _particle.Stop();
        _rigidbody2d = GetComponent<Rigidbody2D>();
        defaultmass = _rigidbody2d.mass;
        defaultScaleX = _transform.localScale.x;
        normalizedDefaultScale = _transform.localScale;
        normalizedDefaultScale /= Mathf.Max(_transform.localScale.x,
                                            _transform.localScale.y,
                                            _transform.localScale.z);
    }
    public void ChangeIceState(IceState iceState)
    {
        this.iceState = iceState;
    }
    protected override void ThermalEvent(float diff)
    {
        if (_thermalEnergy >= MaxEnergy)
        {
            _transform.localScale -= normalizedDefaultScale * _meltingSpeed * Time.deltaTime;
            _rigidbody2d.mass = defaultmass * _transform.localScale.x / defaultScaleX;
            if (_particle == null) return;
            _isParticlePlaying = _particle.isEmitting;
            time += Time.deltaTime;
            if (time < EFFECTINTERVAL) return;
            time = 0;
            if (_isTouchable && !_isParticlePlaying)
            {
                _particle.Play();
            }
            if (meltData != null) SEManager.Instance.Play(meltData.audioClip, meltData.volume);
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
