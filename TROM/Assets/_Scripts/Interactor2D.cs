using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Interactor2D : MonoBehaviour
{
    // Current touching interactable objects. 
    public LayerMask targetLayer;
    
    public List<InteractableObject> touchedInteractableObjects = new List<InteractableObject>();
    
    public InteractableObject nearestInteractable2D;

    public InteractableObject currentInteractingIO;

    private bool _isInteracting;
    public bool IsInteracting
    {
        get => _isInteracting;
    }
    public bool CheckInteractable()
    {
        return nearestInteractable2D != null && nearestInteractable2D.CheckInteractable(this);
    }

    public void Interact(params object[] objects)
    {
        if (nearestInteractable2D)
        {
            if (!nearestInteractable2D.CheckInteractable(this)) return;
            
            var interactResult = nearestInteractable2D.TryInteract(this, objects);
            
            _isInteracting = interactResult;
        }
    }

    // TODO: something might be wrong
    public void StopInteract()
    {
        if (nearestInteractable2D)
        {
            _isInteracting = false;

            nearestInteractable2D.TryStopInteract(this);
        }
    }
    
    private void Update()
    {
        if (!_isInteracting)
        {
            UpdateNearestRegistrableObject();
        }
    }

    private void UpdateNearestRegistrableObject()
    {
        var newNearestIO = GetNearestRegistrableObject();
        
        if (newNearestIO == nearestInteractable2D) return;
        
        if (nearestInteractable2D) nearestInteractable2D.OnUnRegistered(this);
            
        if (newNearestIO) newNearestIO.OnRegistered(this);

        nearestInteractable2D = newNearestIO;
    }
    
    public InteractableObject GetNearestRegistrableObject()
    {
        var dis = float.MaxValue;
        InteractableObject target = null;
        foreach (var interactable in touchedInteractableObjects)
        {
            if (!interactable.CheckRegistrable(this)) continue;
            
            var newDis = Vector3.Distance(transform.position , interactable.transform.position);
            
            if (!(newDis < dis)) continue;
            
            dis = newDis;
            target = interactable;
        }
        return target;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Might switch 
        if (targetLayer.MMContains(other.gameObject.layer))
        {
            if (other.GetComponentInChildren<InteractableObject>() is { } io)
            {
                if (!touchedInteractableObjects.Contains(io))
                {
                    touchedInteractableObjects.Add(io);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Might switch 
        if (targetLayer.MMContains(other.gameObject.layer))
        {
            if (other.GetComponentInChildren<InteractableObject>() is { } io)
            {
                if (touchedInteractableObjects.Contains(io))
                {
                    touchedInteractableObjects.Remove(io);
                }
            }
        }
    }

    [Button]
    public void LogNearestInteractableObject()
    {
        Debug.Log($"Nearest: {GetNearestRegistrableObject().name}");
    }
    
    [Button]
    public void InteractNearestInteractableStart()
    {
        var interactable = GetNearestRegistrableObject();
        if (interactable)
        {
            interactable.TryInteract(this);
        }
    }
}
