using UnityEngine;

namespace Interfaces
{
    public interface IMovable
    {
        public Transform GetTransform { get; }
        public Vector3 TargetPosition { get; set; }
        public bool Freezed { get; set; }
        public bool CanMove(Vector3 direction);
    }
}