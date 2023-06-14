using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(TouchableFriend))]
public class GoalSetter : MonoBehaviour
{
    [SerializeField] GameObject goal;
    TouchableFriend touchableFriend;
    // Start is called before the first frame update
    void Start()
    {
        touchableFriend = GetComponent<TouchableFriend>();
        touchableFriend.onDied += SetGoal;


    }

    public void SetGoal()
    {
        goal.SetActive(true);
    }
}
