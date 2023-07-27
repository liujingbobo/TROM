using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtensions
{
    #region Vector3

    //Vector3
    public static Vector3 SetX(this Vector3 vector, float x)
    {
        return new Vector3(x, vector.y, vector.z);
    }

    public static Vector3 SetY(this Vector3 vector, float y)
    {
        return new Vector3(vector.x, y, vector.z);
    }

    public static Vector3 SetZ(this Vector3 vector, float z)
    {
        return new Vector3(vector.x, vector.y, z);
    }
    public static Vector3 OffsetX(this Vector3 vector, float offsetX)
    {
        return new Vector3(vector.x + offsetX, vector.y, vector.z);
    }

    public static Vector3 OffsetY(this Vector3 vector, float offsetY)
    {
        return new Vector3(vector.x, vector.y + offsetY, vector.z);
    }

    public static Vector3 OffsetZ(this Vector3 vector, float offsetZ)
    {
        return new Vector3(vector.x, vector.y, vector.z + offsetZ);
    }

    public static Vector3 Offset(this Vector3 vector, float offsetX=0, float offsetY=0, float offsetZ=0)
    {
        return new Vector3(vector.x + offsetX, vector.y + offsetY, vector.z + offsetZ);
    }

    public static Vector2 XZ(this Vector3 dir)
    {
        return new Vector2(dir.x, dir.z);
    }
    #endregion
    

    #region Vector2

    //Vector2
    public static Vector2 SetX(this Vector2 vector, float x)
    {
        return new Vector2(x, vector.y);
    }

    public static Vector2 SetY(this Vector2 vector, float y)
    {
        return new Vector2(vector.x, y);
    }

    public static Vector2 OffsetX(this Vector2 vector, float offsetX)
    {
        return new Vector2(vector.x + offsetX, vector.y);
    }

    public static Vector2 OffsetY(this Vector2 vector, float offsetY)
    {
        return new Vector2(vector.x, vector.y + offsetY);
    }

    public static Vector2 Offset(this Vector2 vector, float offsetX, float offsetY)
    {
        return new Vector2(vector.x + offsetX, vector.y + offsetY);
    }

    public static Vector3 ToVector3XZ(this Vector2 vector)
    {
        return new Vector3(vector.x, 0.0f, vector.y);
    }
    #endregion


    #region Color

    public static Color SetAlpha(this Color color, float a)
    {
        return new Color(color.r,color.g,color.b,a);
    }

    #endregion"
}
