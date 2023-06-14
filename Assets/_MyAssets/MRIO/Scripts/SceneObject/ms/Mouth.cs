using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class Mouth : MonoBehaviour
{
    readonly float JUDGEINTERVAL = 10;
    public enum FireState
    {
        FireOn,
        FireOff
    }
    [SerializeField] ToPlayerShoot fireShooter;
    [SerializeField] Sprite fireOnMouth;
    [SerializeField] Sprite fireOffMouth;
    [SerializeField] float fireProbability = 50f;
    SpriteRenderer spriteRenderer;
    FireState fireState;
    float time;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ChangeState(FireState.FireOff);
    }
    private void Update()
    {
        time += Time.deltaTime;
        if (time < JUDGEINTERVAL) return;
        time = 0;
        float random = Random.Range(0, 100);
        if (random < fireProbability)
        {
            ChangeState(FireState.FireOn);
        }
        else
        {
            ChangeState(FireState.FireOff);
        }
    }

    public void ChangeState(FireState fireState)
    {
        this.fireState = fireState;
        switch (fireState)
        {
            case FireState.FireOn:
                fireShooter.ChangeState(ShooterState.Shoot);
                spriteRenderer.sprite = fireOnMouth;
                break;
            case FireState.FireOff:
                fireShooter.ChangeState(ShooterState.Idle);
                spriteRenderer.sprite = fireOffMouth;
                break;
        }
    }
}
