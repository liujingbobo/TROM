using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    InValid = -1,
    Idle = 0,
    Move = 10,
    Jump = 20,
    Climb = 30,
    Fall = 40,
    Hang = 50,
    Ladder = 60,
    Attack = 70,
    
    CheckItemContainer = 100,
    Hit = 110, 
}

public enum AnimationType
{
    Empty,
    Idle,
            
    WalkStart,
    Walk,
    WalkEnd,
            
    Run,
            
    JumpRise,
    JumpMid,
    JumpFall,
            
    LedgeHangPreview,
    LedgeClimbPreview,
    LedgeClimbPreviewReverse,
            
    LadderClimb,
    LadderClimbReverse,
    LadderClimbFinish,
    LadderClimbFinishReverse,
            
    Attack,
    
    CheckItemContainer,
    
    Hit,
}
public enum FacingDirection
{
    Right,
    Left
}

