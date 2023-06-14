using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DraggingFire : FireObject
{
    [SerializeField] ParticleSystem fireParticle;
    Transform _transform;
    private void Awake()
    {
        _transform = transform;
        if (fireParticle != null) fireParticle.gameObject.SetActive(false);
    }

    public override void OnEffectOther(Touchable buf)
    {

    }
    public override void Activate(float duration)
    {
        base.Activate(duration);
        if (fireParticle != null)
        {
            fireParticle.gameObject.SetActive(true);
            fireParticle.Play();
        }
    }

    public override void DeActivate(float duration)
    {
        base.DeActivate(duration);
        if (fireParticle != null)
        {
            fireParticle.Pause();
            fireParticle.gameObject.SetActive(false);
        }
    }
}
