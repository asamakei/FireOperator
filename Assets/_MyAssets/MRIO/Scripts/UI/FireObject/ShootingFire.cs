using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[RequireComponent(typeof(SpriteRenderer))]

public class ShootingFire : FireObject
{
    [SerializeField] ParticleSystem fireParticle;
    [SerializeField] ParticleSystem explodeParticle;
    Transform _transform;
    Color defaultColor;
    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        _transform = transform;
        if (fireParticle != null) fireParticle.gameObject.SetActive(false);
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
    }
    public void Shoot(Vector3 from, Vector3 targetPos, float duration)
    {
        
        _transform.position = from;
        _transform.up = -(targetPos - from);
        Activate(0);
        _transform.DOKill();
        _transform.DOMove(targetPos, duration).SetEase(Ease.Linear).OnComplete(() => DeActivate(0)).SetLink(gameObject);
    }
    
    public override void OnEffectOther(Touchable buf)
    {
        buf.OnTouchedEnter();
    }
    public override void Activate(float duration)
    {
        base.Activate(duration);
        spriteRenderer.color = defaultColor;
        if (fireParticle != null)
        {
            fireParticle.gameObject.SetActive(true);
            fireParticle.Play();
        }
    }

    public override void DeActivate(float duration)
    {
        _transform.DOKill();
        spriteRenderer.color = Color.clear;
        if (fireParticle != null) fireParticle.gameObject.SetActive(false);
        if (explodeParticle != null) explodeParticle.Play();
        else base.DeActivate(duration);
    }
}
