using UnityEngine;

namespace Objects
{
    public class MainObject : MonoBehaviour
    {
        protected bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hit, float distance = 1.0f, int layerMask = 0)
        {
            return Physics.Raycast(origin, direction, out hit, distance, layerMask);
        }
    }
}