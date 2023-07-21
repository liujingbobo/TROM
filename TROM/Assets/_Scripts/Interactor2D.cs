using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Interactor2D : MonoBehaviour
{
    public List<Interactable2D> registeredInteractables = new List<Interactable2D>();
    public Interactable2D nearestInteractable2D;
    
    public void RegisterInteractable(Interactable2D interactable2D)
    {
        registeredInteractables.Add(interactable2D);
    }
    
    public void UnregisterInteractable(Interactable2D interactable2D)
    {
        if (nearestInteractable2D && nearestInteractable2D == interactable2D)
        {
            nearestInteractable2D.OnUnsetNearestByInteractor(this);
            nearestInteractable2D = null;
        }
        if (registeredInteractables.Contains(interactable2D))
        {
            registeredInteractables.Remove(interactable2D);
        }
    }

    private void Update()
    {
        UpdateNearestInteractable();
    }

    private void UpdateNearestInteractable()
    {
        var newNearest = GetNearestInteractable();
        if (newNearest)
        {
            if (nearestInteractable2D == newNearest) return;
            if(nearestInteractable2D) nearestInteractable2D.OnUnsetNearestByInteractor(this);
            nearestInteractable2D = newNearest;
            nearestInteractable2D.OnSetAsNearestByInteractor(this);
        }
    }
    
    public Interactable2D GetNearestInteractable()
    {
        var dis = float.MaxValue;
        Interactable2D target = null;
        foreach (var interactable in registeredInteractables)
        {
            var newDis = Vector3.Distance(transform.position , interactable.transform.position);
            if (newDis < dis)
            {
                dis = newDis;
                target = interactable;
            }
        }
        return target;
    }

    [Button]
    public void LogNearestInteractable()
    {
        Debug.Log($"Nearest: {GetNearestInteractable().name}");
    }
    
    [Button]
    public void InteractNearestInteractableStart()
    {
        var interactable = GetNearestInteractable();
        if (interactable)
        {
            interactable.InteractionStart(this);
        }
    }
}
