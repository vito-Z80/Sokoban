using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Objects
{
    public class MainObject : MonoBehaviour
    {
        [HideInInspector] public Vector3 targetPosition;
        protected readonly List<BackStepTransform> Stack = new();


        [HideInInspector] public bool isMoving;
        [HideInInspector] public bool isDisable;


        protected int PortalLayerId;

        void Awake()
        {
            PortalLayerId = LayerMask.NameToLayer("Portal");
        }


        public bool Move(float deltaTime)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, deltaTime * Global.Instance.gameSpeed);
            return transform.position != targetPosition;
        }


        public virtual void ClearStack()
        {
            Stack.Clear();
        }

        public int StackCount()
        {
            return Stack.Count;
        }

        protected bool DirectionComponent<T>(Vector3 direction, out T component, float distance = 1.0f) where T : Component
        {
            if (isMoving)
            {
                component = null;
                return false;
            }

            if (isDisable)
            {
                transform.position = transform.position.Round();
                targetPosition = transform.position;
                component = null;
                return false;
            }

            var c = DetectNearestComponent<T>(direction, distance);
            if (c is null)
            {
                component = null;
                return false;
            }

            // targetPosition = transform.position + direction.Round();
            component = c;
            return true;
        }


        protected T DetectNearestComponent<T>(Vector3 direction, float distance = 0.8f) where T : Component
        {
            if (Physics.Raycast(transform.position, direction, out var hit, distance))
            {
                if (hit.transform.TryGetComponent<T>(out var component))
                {
                    return component != this ? component : null;
                }
            }

            return null;
        }

        // protected void ToLeft()
        // {
        // }
        // protected void ToRight()
        // {
        // }
        // protected void Forward()
        // {
        // }
        // protected void Backward()
        // {
        // }

        protected bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hit, float distance = 1.0f, int layerMask = 0)
        {
            return Physics.Raycast(origin, direction, out hit, distance, layerMask);
        }

        public virtual void PopState()
        {
            if (Stack.Count == 0) return;
            var data = Stack.Last();
            transform.rotation = data.Rotation;
            transform.localScale = data.Scale;
            targetPosition = data.Position;
            transform.position = data.Position;
            Stack.RemoveAt(Stack.Count - 1);
        }

        public virtual void PushState()
        {
            Stack.Add(new BackStepTransform(transform));
        }
    }
}