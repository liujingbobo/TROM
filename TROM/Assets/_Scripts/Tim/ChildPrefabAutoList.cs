using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildPrefabAutoList<T> where T:IAutoListChildData
{
    private GameObject _defaultChildPrefab;
    private Transform _parent;

    private List<GameObject> _childObjects = new List<GameObject>();
    private List<T> _childData = new List<T>();
    private Action<T,int> _setupAction; //T for dataType, int for index in List
    
    public void Init(GameObject defaultPrefab, Transform parentTransform, Action<T,int> setupAction)
    {
        _defaultChildPrefab = defaultPrefab;
        _parent = parentTransform;
        _setupAction = setupAction;
        
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
            _setupAction(dataInstance, i);
            
            _childObjects.Add(childGO);
            _childData.Add(dataInstance);
        }
    }

    public List<T> GetDataList()
    {
        return _childData;
    }

    public GameObject AddNewChild()
    {
        int newIndex = _childObjects.Count;
        var wasInactive = _defaultChildPrefab.activeSelf;
        _defaultChildPrefab.SetActive(true);
        var newChild = GameObject.Instantiate(_defaultChildPrefab, _parent);
        
        var dataType = typeof(T);
        var instance = Activator.CreateInstance(dataType);
        var dataInstance = (T)instance;
        
        dataInstance.ReadDataFromGameObject(newChild);
        _setupAction(dataInstance, newIndex);
        
        _childObjects.Add(newChild);
        _childData.Add(dataInstance);
        
        if(wasInactive)_defaultChildPrefab.SetActive(false);

        return newChild;
    }

    public void GetChildObjectAndData(int index, out GameObject go, out T data)
    {
        var extraCount = index - (_childObjects.Count -1);
        if (extraCount > 0)
        {
            for (int i = 0; i < extraCount; i++)
            {
                AddNewChild();
            }
        }

        go = _childObjects[index];
        data = (T)_childData[index];
    }

    public void ShowByCount(int count)
    {
        var extraCount = count - (_childObjects.Count);
        if (extraCount > 0)
        {
            for (int i = 0; i < extraCount; i++)
            {
                AddNewChild();
            }
        }

        for (int i = 0; i < _childObjects.Count; i++)
        {
            _childObjects[i].SetActive(i < count);
        }
    }
}

public interface IAutoListChildData
{
    public IAutoListChildData Instantiate();
    public void ReadDataFromGameObject(GameObject go);
}
