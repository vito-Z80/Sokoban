using JetBrains.Annotations;
using UnityEngine;

namespace Objects
{
    public class MainObject : MonoBehaviour
    {
        public float moveSpeed = 1.0f;
        [SerializeField]protected Vector3 TargetPosition;

        protected bool Move(float deltaTime)
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetPosition, deltaTime * moveSpeed);
            return transform.position != TargetPosition;
        }


        protected T DetectNearestComponent<T>(Vector3 direction, float distance = 0.8f) where T : Component
        {
            if (Physics.Raycast(transform.position, direction, out var hit, distance))
            {
                if (hit.transform.TryGetComponent<T>(out var container))
                {
                    return container != this ? container : null;
                }
            }

            return null;
        }
    }
}