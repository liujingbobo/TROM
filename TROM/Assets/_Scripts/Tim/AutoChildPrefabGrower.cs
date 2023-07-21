using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoChildPrefabGrower : MonoBehaviour
{
    private GameObject _defaultChildPrefab;
    private Transform _parent;

    private List<GameObject> _childObjects = new List<GameObject>();
    private List<IAutoGrowChildData> _childData = new List<IAutoGrowChildData>();

    public void Init<T>(GameObject defaultPrefab, Transform parentTransform) where T:IAutoGrowChildData
    {
        _defaultChildPrefab = defaultPrefab;
        _parent = parentTransform;
        
        _childObjects.Clear();
        _childData.Clear();

        int childCount = parentTransform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            var childGO = parentTransform.GetChild(i).gameObject;
            
            var dataType = typeof(T);
            var instance = Activator.CreateInstance(dataType);
            var dataInstance = (T)instance;
            
            dataInstance.ReadDataFromGameObject(childGO);
            _childObjects.Add(childGO);
            _childData.Add(dataInstance);
        }
    }

    public GameObject AddNewChild<T>() where T:IAutoGrowChildData
    {
        var wasInactive = _defaultChildPrefab.activeSelf;
        _defaultChildPrefab.SetActive(true);
        var newChild = Instantiate(_defaultChildPrefab, _parent);
        
        var dataType = typeof(T);
        var instance = Activator.CreateInstance(dataType);
        var dataInstance = (T)instance;
        
        dataInstance.ReadDataFromGameObject(newChild);
        
        _childObjects.Add(newChild);
        _childData.Add(dataInstance);
        
        if(wasInactive)_defaultChildPrefab.SetActive(false);

        return newChild;
    }

    public void GetChildObjectAndData<T>(int index, out GameObject go, out T data) where T:IAutoGrowChildData
    {
        var extraCount = index - (_childObjects.Count -1);
        if (extraCount > 0)
        {
            for (int i = 0; i < extraCount; i++)
            {
                AddNewChild<T>();
            }
        }

        go = _childObjects[index];
        data = (T)_childData[index];
    }

    public void ShowByCount<T>(int count)where T:IAutoGrowChildData
    {
        var extraCount = count - (_childObjects.Count);
        if (extraCount > 0)
        {
            for (int i = 0; i < extraCount; i++)
            {
                AddNewChild<T>();
            }
        }

        for (int i = 0; i < _childObjects.Count; i++)
        {
            _childObjects[i].SetActive(i < count);
        }
    }
}

public interface IAutoGrowChildData
{
    public IAutoGrowChildData Instantiate();
    public void ReadDataFromGameObject(GameObject go);
}
