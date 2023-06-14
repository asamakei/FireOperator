using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightMover : MonoBehaviour,IFixedUpdater
{
    [SerializeField] float speed;
    [SerializeField] Vector2 defaultMoveWeight;
    MoverState moverState = MoverState.Idle;
    public MoverState fireonState
    {
        get { return moverState; }
    }
    Vector2 moveWeight;
    Rigidbody2D _rigidbody2D;
    Transform _transform;
    Animator animator;
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
}
