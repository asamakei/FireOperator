using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ThermalFun : Touchable
{
    [SerializeField] Vector2 funDirection;
    [SerializeField] float forceAmount;
    [SerializeField] float colliderStrechSpeed = 5;
    [SerializeField] float funSizeMaxY = 6;
    [SerializeField] float maxSpeed = 6;
    [SerializeField] float maxLifeTime = 6;
    [SerializeField] float tThreshold = 0.2f;

    private List<Rigidbody2D> _rigidbodies = new List<Rigidbody2D>();
    private Touchable _touchable;
    [SerializeField] BoxCollider2D boxCollider2D;
    Transform _transform;
    Animator animator;
    ParticleSystem _particleSystem;
    void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _transform = transform;
        _rigidbodies.Clear();
        _touchable = GetComponent<Touchable>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        if (animator != null) animator.speed = 0;
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        AddForce();
    }

    private void AddForce()
    {
        if (_rigidbodies == null) return;
        foreach (Rigidbody2D _rigidbody in _rigidbodies)
        {
            _rigidbody.AddForce(funDirection * forceAmount,ForceMode2D.Force);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Strings.PLAYER) && collision.TryGetComponent(out Rigidbody2D _rigidbody))
        {
            _rigidbodies.Add(_rigidbody);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Strings.PLAYER) && collision.TryGetComponent(out Rigidbody2D _rigidbody))
        {
            int index = _rigidbodies.IndexOf(_rigidbody);
            if (index >= 0)
            {
                _rigidbodies.RemoveAt(index);
            }
        }
    }
    protected override void ThermalEvent(float diff)
    {
        if (_thermalEnergy >= MaxEnergy)
        {
            StrechColliderTipY(colliderStrechSpeed * Time.deltaTime);
        }
    }

    public void StrechColliderTipY(float amount)
    {
        if (boxCollider2D == null) return;
        if (boxCollider2D.size.y >= funSizeMaxY) return;
        float t = boxCollider2D.size.y / funSizeMaxY;
        if (t < tThreshold)
        {
            _particleSystem.Pause();
        }
        else
        {
            _particleSystem.Play(false);

        }
        if (_particleSystem != null)
        {
            var main = _particleSystem.main;

            main.startLifetimeMultiplier = t * maxLifeTime;
        }
        if (animator != null) animator.speed = (t < tThreshold) ? 0 : maxSpeed * t;
        boxCollider2D.offset += new Vector2(0, amount / 3);
        boxCollider2D.size += new Vector2(0, amount / 2);
    }
}
