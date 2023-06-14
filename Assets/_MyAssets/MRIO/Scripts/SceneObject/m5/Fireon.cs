using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Fireon : Touchable
{
    static float EPSILON = 0.1f;
    [SerializeField] float _changingSpeed = 0.01f;
    [SerializeField] int scaleAnimNum = 5;
    [SerializeField] float scaleAnimDuration = 0.1f;
    [SerializeField] Color firedColor = Color.red;
    SpriteRenderer spriteRenderer;
    Transform _transform;
    float defaultmass;
    float defaultScaleX;
    Vector3 normalizedDefaultScale;
    Rigidbody2D _rigidbody2d;
    int animationCount = 0;

    private void Awake()
    {
        _transform = transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2d = GetComponent<Rigidbody2D>();
        defaultmass = _rigidbody2d.mass;
        defaultScaleX = _transform.localScale.x;
        normalizedDefaultScale = _transform.localScale;
        normalizedDefaultScale /= Mathf.Max(_transform.localScale.x,
                                            _transform.localScale.y,
                                            _transform.localScale.z);
    }
    protected override void ThermalEvent(float diff)
    {
        if (_thermalEnergy >= MaxEnergy)
        {
            if (animationCount > scaleAnimNum)
            {
                animationCount = 0;
                _transform.DOKill();
                _transform.DOScale(normalizedDefaultScale * _changingSpeed * Time.deltaTime * scaleAnimNum, scaleAnimDuration).SetRelative().SetEase(Ease.OutBack).SetLink(gameObject);
            }
            else
            {
                animationCount++;
                if (spriteRenderer != null)
                {
                    spriteRenderer.material.DOColor(firedColor, scaleAnimDuration).SetEase(Ease.InFlash, 2, 0).OnComplete(() => spriteRenderer.material.color = Color.white).SetLink(gameObject);
                }
            }

        }
        else if (_thermalEnergy < MinEnergy)
        {
            _transform.localScale -= normalizedDefaultScale * _changingSpeed * Time.deltaTime;
        }
        if (_transform.localScale.x <= EPSILON) gameObject.SetActive(false);
    }

}
