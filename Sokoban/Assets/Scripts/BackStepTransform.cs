using UnityEngine;

public struct BackStepTransform 
{
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Scale;


    public BackStepTransform(Transform mo)
    {
        Position = mo.transform.position;
        Rotation = mo.transform.rotation;
        Scale = mo.transform.localScale;
    }
}
