using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

public class UIStack : MonoBehaviour
{
    private Stack<UIPanel> _uiStack = new Stack<UIPanel>();
    private Dictionary<UIPanelTypeName, UIPanel> _cachedUIPanels = new Dictionary<UIPanelTypeName, UIPanel>();
    private static UIStack _singleton;
    public static UIStack Singleton
    {
        get
        {
            if (_singleton == null)
            {
                _singleton = FindObjectOfType<UIStack>();
                if (_singleton == null)
                {
                    //Basic use case of forcing a synchronous load of a GameObject
                    var op = Addressables.LoadAssetAsync<GameObject>("Assets/AddressablePrefabs/UIStack.prefab");
                    var go = op.WaitForCompletion();
                    _singleton = Instantiate(go).GetComponent<UIStack>();
                    Initialize();
                }
            }
            return _singleton;
        }
    }

    public enum UIPanelTypeName
    {
        None,
        UIPanel,
        UIExploreMap,
    }

    public UIPanel GetUIPanelInstance(UIPanelTypeName typeName)
    {
        if (_cachedUIPanels.ContainsKey(typeName))
        {
            return _cachedUIPanels[typeName];
        }
        else
        {
            var op = Addressables.LoadAssetAsync<GameObject>($"Assets/AddressablePrefabs/{typeName}.prefab");
            var go = op.WaitForCompletion();
            var instance = Instantiate(go,transform).GetComponent<UIPanel>();
            _cachedUIPanels[typeName] = instance;
            return instance;
        }
    }
    
    private void ReadAndInitChildUIPanel()
    {
        _cachedUIPanels.Clear();
        foreach (Transform child in transform)
        {
            var panel = child.GetComponent<UIPanel>();
            if (panel)
            {
                var hasEnum = Enum.TryParse(panel.GetType().ToString(), out UIPanelTypeName typeName);
                if (hasEnum)
                {
                    _cachedUIPanels[typeName] = panel;
                    panel.Initialize();
                    _uiStack.Push(panel);
                }
            }
        }
    }
    private void Awake()
    {
        _singleton = this;
        ReadAndInitChildUIPanel();
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            CloseTopUIPanel();
        }
    }
    
    public static void Initialize()
    {
        
    }

    public void OpenUIPanel(UIPanelTypeName panelTypeName)
    {
        var instance = GetUIPanelInstance(panelTypeName);
        instance.Initialize();
        _uiStack.Push(instance);
        instance.OpenUI(null);
    }

    public void CloseTopUIPanel()
    {
        if (_uiStack.Count <= 0)
        {
            Debug.Log("UI Stack Is Empty");
            return;
        }
        var topPanel = _uiStack.Pop();
        topPanel.CloseUI(null);
        
    }
}
