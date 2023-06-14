using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class DeathArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Strings.PLAYER) && (UIManager.uIState != UIState.Clear || UIManager.uIState != UIState.Fail) && !SceneTransManager.IsTransitioning)
        {
            UIManager.uIState = UIState.Fail;
            Variables.failState = FailState.Fall;
            collision.gameObject.SetActive(false);
        }
    }
}
