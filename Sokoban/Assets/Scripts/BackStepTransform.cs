using UnityEngine;

public struct BackStepTransform 
{
    public GameObject GameObject;
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Scale;


    public BackStepTransform(GameObject gameObject)
    {
        GameObject = gameObject;
        Position = gameObject.transform.position;
        Rotation = gameObject.transform.rotation;
        Scale = gameObject.transform.localScale;
    }
}
