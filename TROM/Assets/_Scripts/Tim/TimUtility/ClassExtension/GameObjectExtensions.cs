using UnityEngine;

public static class GameObjectExtensions
{
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        T component = gameObject.GetComponent<T>();

        if (component == null)
        {
            component = gameObject.AddComponent<T>();
        }

        return component;
    }
    
    public static T GetOrAddChildComponent<T>(this GameObject gameObject) where T : Component, new()
    {
        T child = gameObject.GetComponentInChildren<T>();
        
        if (child != null)
            return child;
        
        GameObject childObject = new GameObject(typeof(T).Name);
        child = childObject.AddComponent<T>();
        child.transform.SetParent(gameObject.transform);
        
        return child;
    }
}
