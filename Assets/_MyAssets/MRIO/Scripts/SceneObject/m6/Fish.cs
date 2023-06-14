using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using KanKikuchi.AudioManager;
public enum FishState
{
    Idle,
    Baked,
    Overburned
}
public class Fish : Touchable
{

    static string BAKE = "Bake";
    static int NOTCOLMAP = 6;
    [SerializeField] float bakedEnergy = 200f;
    [SerializeField] Sprite burnedSprite;
    [SerializeField] Color firedColor = Color.black;
    [SerializeField] ParticleSystem burnedParticle;
    [SerializeField] ParticleSystem overBurnedParticle;
    Rigidbody2D _rigidbody;
    Sprite defaultSprite;
    SpriteRenderer spriteRenderer;
    Transform _transform;
    Rigidbody2D _rigidbody2d;
    AudioData bakeData;
    FishState fishState;
    StraightMover straightMover;
    Vector3 defaultScale;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        bakeData = AudioDataManager.Instance.GetAudioData(BAKE);
        _transform = transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2d = GetComponent<Rigidbody2D>();
        straightMover = GetComponent<StraightMover>();
        defaultSprite = spriteRenderer.sprite;
        defaultScale = _transform.localScale;
    }

    private void OnEnable()
    {
        if (spriteRenderer != null) spriteRenderer.sprite = defaultSprite;
        _thermalEnergy = 0;
        ChangeState(FishState.Idle);
        gameObject.layer = NOTCOLMAP;
        _rigidbody.mass *= 5;
        _rigidbody2d.gravityScale = 0;
        spriteRenderer.color = Color.white;
        _transform.localScale = defaultScale;
        if (straightMover != null) straightMover.ChangeState(MoverState.Walking);
    }
    protected override void ThermalEvent(float diff)
    {
        if (_thermalEnergy >= bakedEnergy && fishState == FishState.Idle)
        {
            _rigidbody.mass /= 5;
            if (bakeData != null) SEManager.Instance.Play(bakeData.audioClip, bakeData.volume);
            _transform.DOKill();
            if (straightMover != null) straightMover.ChangeState(MoverState.Idle);
            spriteRenderer.sprite = burnedSprite;
            _rigidbody2d.gravityScale = 1;
            gameObject.layer = 0;
            ChangeState(FishState.Baked);

        }
        else if (_thermalEnergy >= bakedEnergy && fishState == FishState.Baked)
        {
            spriteRenderer.color = Color.Lerp(Color.white, firedColor, Mathf.Clamp01((_thermalEnergy - bakedEnergy) / (MaxEnergy - bakedEnergy)));
        }

        if (_thermalEnergy >= MaxEnergy * 0.9f && fishState == FishState.Baked)
        {
            ChangeState(FishState.Overburned);
        }

    }

    public void ChangeState(FishState fishState)
    {
        this.fishState = fishState;
        switch (this.fishState)
        {
            case FishState.Idle:
                break;
            case FishState.Baked:
                if (burnedParticle != null) burnedParticle.Play();
                break;
            case FishState.Overburned:
                _transform.localScale = Vector3.zero;
                if (overBurnedParticle != null) overBurnedParticle.Play();
                _thermalEnergy = 0;
                OnTouchedExit();
                break;
        }
    }
}
