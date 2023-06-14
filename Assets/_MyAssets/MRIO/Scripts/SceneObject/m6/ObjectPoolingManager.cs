using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    [SerializeField] protected GameObject poolObjectPrefab;
    [SerializeField] protected Transform poolParent;
    [SerializeField] int defaultObjectNum = 10;
    protected List<GameObject> InstantiatedObjects;
    protected Transform _transform;
    protected  virtual void Start()
    {
        _transform = transform;
        InstantiatedObjects = new List<GameObject>();
        for(int i = 0; i < defaultObjectNum; i++)
        {
            GameObject obj = Instantiate(poolObjectPrefab, poolParent);
            InstantiatedObjects.Add(obj);
            obj.SetActive(false);
        }
    }
    public GameObject SearchUnusedObject()
    {
        if(InstantiatedObjects == null) InstantiatedObjects = new List<GameObject>();
        foreach (GameObject obj in InstantiatedObjects)
        {
            if (!obj.activeSelf) return obj;
        }
        return null;
    }

    public GameObject CreateInstance()
    {
        GameObject instance = SearchUnusedObject();
        return instance;
    }
}
