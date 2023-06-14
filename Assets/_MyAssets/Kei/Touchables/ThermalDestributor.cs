using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermalDestributor : MonoBehaviour
{
    [SerializeField] private float _incrRate = 20;
    [SerializeField] private float _decrRate = 5;

    private List<Touchable> _touchables = new List<Touchable>();
    private Touchable _touchable;

    void Awake()
    {
        _touchables.Clear();
        _touchable = GetComponent<Touchable>();
    }
    private void FixedUpdate()
    {
        DestributeHeat();
    }
    private void DestributeHeat()
    {
        if (_touchables == null) return;
        for (int i = 0; i < _touchables.Count; i++)
        {
            if (!_touchables[i].gameObject.activeInHierarchy)
            {
                _touchables.RemoveAt(i);
                continue;
            }
            if (_touchables[i].GetThermal() < _touchable.GetThermal())
            {
                _touchable.AddThermal(-_decrRate);
                _touchables[i].AddThermal(_incrRate);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Touchable t))
        {
            _touchables.Add(t);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Touchable t))
        {
            int index = _touchables.IndexOf(t);
            if (index >= 0)
            {
                _touchables.RemoveAt(index);
            }
        }
    }
}
