using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum MoverState
{
    Idle = 0,
    Walking = 1
}
public class LoopMover : MonoBehaviour, IFixedUpdater
{
    [SerializeField] float speed;
    [SerializeField] Vector2 defaultMoveWeight;
    [SerializeField] bool shouldReverseWithPlayer = false;
    MoverState moverState = MoverState.Idle;
    public MoverState fireonState
    {
        get { return moverState; }
    }
    Vector2 moveWeight;
    public Vector2 MoveWeight
    {
        set { moveWeight = value; }
    }
    Rigidbody2D _rigidbody2D;
    Transform _transform;
    Animator animator;
    SpriteRenderer spriteRenderer;
    public void ChangeState(MoverState fireonState)
    {
        this.moverState = fireonState;
        switch (moverState)
        {
            case MoverState.Idle:
                if (animator != null) animator.SetBool(Strings.ISWALK, false);
                break;
            case MoverState.Walking:
                if (animator != null) animator.SetBool(Strings.ISWALK, true);
                break;
        }
    }
    private void Awake()
    {
        moveWeight = defaultMoveWeight;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _transform = transform;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ChangeState(MoverState.Walking);
    }
    public void OnFixedUpdate()
    {
        if (moverState == MoverState.Idle) return;
        Vector2 toPosition = (Vector2)_transform.position + moveWeight * speed * Time.deltaTime;
        if (_rigidbody2D != null)
        {
            _rigidbody2D.MovePosition(toPosition);
        }
        else
        {
            _transform.position = toPosition;
        }
    }

    void Reverse()
    {
        moveWeight *= -1;
        if (spriteRenderer != null) spriteRenderer.flipX = moveWeight.x > 0;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!shouldReverseWithPlayer && collision.gameObject.CompareTag(Strings.PLAYER)) return;
        for (int i = collision.contactCount - 1; i >= 0; i--)
        {
            if ((collision.contacts[i].point.x - _transform.position.x) * moveWeight.x < 0) return;
            if (System.Math.Abs(Vector2.Dot(_transform.right, collision.contacts[i].normal)) > 0.8f)
            {
                Reverse();
                return;
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!shouldReverseWithPlayer && collision.gameObject.CompareTag(Strings.PLAYER)) return;
        for (int i = collision.contactCount - 1; i >= 0; i--)
        {
            if ((collision.contacts[i].point.x - _transform.position.x) * moveWeight.x < 0) return;
            if (System.Math.Abs(Vector2.Dot(_transform.right, collision.contacts[i].normal)) > 0.8f)
            {
                Reverse();
                return;
            }
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag(Strings.PLAYER)) return;
    //    if (System.Math.Abs(Vector2.Dot(_transform.right, collision.transform.position - _transform.position)) > 0.8f)
    //    {
    //        Reverse();
    //        return;
    //    }
    //}
}
