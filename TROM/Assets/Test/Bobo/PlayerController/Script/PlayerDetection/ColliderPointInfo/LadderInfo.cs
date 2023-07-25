using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderInfo : MonoBehaviour
{
    public LadderColliderType ladderColliderType;
    
    public enum LadderColliderType
    {
        Top, Bottom
    }
}
