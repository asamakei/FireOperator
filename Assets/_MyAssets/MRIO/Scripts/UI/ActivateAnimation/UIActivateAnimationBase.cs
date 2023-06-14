using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIActivateAnimationBase : MonoBehaviour
{
    [SerializeField] protected float duration;
    public abstract void Activate();
    public abstract void DeActivate();
}
