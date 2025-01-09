﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Objects
{
    public class MainObject : MonoBehaviour
    {
        public float moveSpeed = 1.0f;
        [HideInInspector] public Vector3 targetPosition;
        protected readonly List<BackStepTransform> Stack = new();


        [HideInInspector] public bool isMoving;
        [HideInInspector] public bool isDisable;

        public bool Move(float deltaTime)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, deltaTime * moveSpeed);
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