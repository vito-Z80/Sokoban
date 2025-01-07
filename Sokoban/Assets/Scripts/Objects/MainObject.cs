using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Objects
{
    public class MainObject : MonoBehaviour
    {
        public float moveSpeed = 1.0f;
        [HideInInspector] public Vector3 targetPosition;
        List<MainObject> m_undoStack;

        public bool Move(float deltaTime)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, deltaTime * moveSpeed);
            return transform.position != targetPosition;
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

        readonly List<BackStepTransform> m_stack = new();

        public virtual void Pop()
        {
            if (m_stack.Count == 0) return;
            var data = m_stack.Last();
            transform.rotation = data.Rotation;
            transform.localScale = data.Scale;
            targetPosition = data.Position;
            transform.position = data.Position;
            m_stack.RemoveAt(m_stack.Count - 1);
        }

        public virtual void Push()
        {
            m_stack.Add(new BackStepTransform(transform));
        }
    }
}