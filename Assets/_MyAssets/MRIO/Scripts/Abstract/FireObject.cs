using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FireObject : MonoBehaviour, IActivater
{
    [SerializeField] int deactivateNum = 10;
    int count = 0;
    Touchable touchableBuf = null;
    public virtual void Activate(float duration)
    {
        gameObject.SetActive(true);
        touchableBuf = null;
    }

    public virtual void DeActivate(float duration)
    {
        gameObject.SetActive(false);
    }

    public abstract void OnEffectOther(Touchable buf);
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Strings.FIREDELETER))
        {
            DeActivate(0);
            count = 0;
            touchableBuf = null;
            return;
        }

        if (collision.gameObject.TryGetComponent<Touchable>(out Touchable touchable) && touchableBuf == null)
        {
            touchableBuf = touchable;
            touchableBuf.OnTouchedEnter();
            count = 0;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(Strings.FIREDELETER) && touchableBuf != null || (touchableBuf != null && !touchableBuf.gameObject.activeInHierarchy))
        {
            DeActivate(0);
            touchableBuf.OnTouchedExit();
            touchableBuf = null;
            return;
        }
        if (touchableBuf != null && count < deactivateNum)
        {
            count++;
            OnEffectOther(touchableBuf);
        }
        else if (count == deactivateNum)
        {
            count++;
            DeActivate(0);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (touchableBuf != null)
        {
            touchableBuf.OnTouchedExit();
            touchableBuf = null;
        }
    }
}
