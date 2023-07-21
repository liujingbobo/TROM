using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TROMExtension
{
    public static bool SameXDirection(this Vector2 dir, Vector2 target)
    {
        return dir.x * target.x > 0;
    }    
    
    public static bool SameYDirection(this Vector2 dir, Vector2 target)
    {
        return dir.y * target.y > 0;
    }

    public static Vector2 xy(this Vector3 dir)
    {
        return new Vector2(dir.x, dir.y);
    }
    
}
