using UnityEngine;

public struct BackStepTransform 
{
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Scale;


    public BackStepTransform(Transform transform)
    {
        Position = transform.transform.position;
        Rotation = transform.transform.rotation;
        Scale = transform.transform.localScale;
    }
}
