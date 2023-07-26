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

    public static void SetXY(this Transform transform, Vector2 xy)
    {
        transform.position = new Vector3(xy.x, xy.y, transform.position.z);
    }
    
    public static void SetXY(this Transform transform, Vector3 xy)
    {
        transform.position = new Vector3(xy.x, xy.y, transform.position.z);
    }
    
    public static void SetXY(this Transform transform, Transform xy)
    {
        var position = xy.position;
        transform.position = new Vector3(position.x, position.y, transform.position.z);
    }
}
