using System;
using UnityEngine;



public static class Vector2Extensions
{
    public static Vector2 Round(this Vector2 v1)
    {
        return new Vector2((float)Math.Round(v1.x), (float)Math.Round(v1.y));
    }
}

public static class Vector3Extensions
{
    public static Vector3 Round(this Vector3 v1)
    {
        return new Vector3((float)Math.Round(v1.x), Mathf.Round(v1.y), (float)Math.Round(v1.z));
    }
    
    public static Vector3 RoundWithoutY(this Vector3 v1)
    {
        return new Vector3((float)Math.Round(v1.x), v1.y, (float)Math.Round(v1.z));
    }
}