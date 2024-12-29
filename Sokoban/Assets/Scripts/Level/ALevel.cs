using UnityEngine;

namespace Level
{
    public abstract class ALevel:MonoBehaviour
    {
        public abstract void RotateAndOffsetLevel(ALevel previousLevel);
    }
}