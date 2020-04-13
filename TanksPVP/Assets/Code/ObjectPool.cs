using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[CreateAssetMenu]
public class ObjectPool : ScriptableObject, IPunPrefabPool
{
    [SerializeField] private List<GameObject> _objectToPool;
    [SerializeField] private bool _usePool;
    private Dictionary<string, GameObject> _cachedObjects;
    private Dictionary<string, List<GameObject>> _pools;

    public GameObject Get(string name, Vector3 position, Quaternion rotation)
    {
        GameObject instance;
        if(_usePool)
        {
            if(_pools == null)
            {
                CreatePools();
            }
            
            var lastIndex = _pools[name].Count - 1;
            if (lastIndex >= 0)
            {
                instance = _pools[name][lastIndex];
                instance.transform.position = position;
                instance.transform.rotation = rotation;
                instance.SetActive(true);
                _pools[name].RemoveAt(lastIndex);
            } else
            {
                instance = Instantiate(_cachedObjects[name], position, rotation);
                instance.name = name;
            }
        }
        else
        {
            instance = Instantiate(_cachedObjects[name], position, rotation);
            instance.name = name;
        }
        return instance;
    }

    private void CreatePools()
    {
        _pools = new Dictionary<string, List<GameObject>>();
        _cachedObjects = new Dictionary<string, GameObject>();
        foreach (var obj in _objectToPool)
        {
            _pools.Add(obj.name, new List<GameObject>());
            _cachedObjects.Add(obj.name, obj);
        }
    }
    
    public void Reclaim(GameObject objectToRecycle)
    {
        if(_usePool)
        {
            if(_pools == null)
            {
                CreatePools();
            }
            _pools[objectToRecycle.name].Add(objectToRecycle);
            objectToRecycle.SetActive(false);
        }
        else
        {
            Destroy(objectToRecycle);
        }
    }

    public void Clear()
    {
        CreatePools();
    }

    public GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
    {
        return Get(prefabId, position, rotation);
    }

    public void Destroy(GameObject gameObject)
    {
        Reclaim(gameObject);
    }
}
