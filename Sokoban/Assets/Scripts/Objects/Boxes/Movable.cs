using UnityEngine;

namespace Objects.Boxes
{
    public class Movable : Box
    {
        public float moveSpeed = 1.0f;
        Vector3 m_targetPosition;

        public bool Move(float deltaTime)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_targetPosition, deltaTime * moveSpeed);
            return transform.position != m_targetPosition;
        }

        T DetectNearestContact<T>(Vector3 direction, float distance = 0.8f) where T : MonoBehaviour
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