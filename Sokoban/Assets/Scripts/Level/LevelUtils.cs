using Bridge;
using JetBrains.Annotations;
using UnityEngine;

namespace Level
{
    public static class LevelUtils
    {
        public static void RotateAndOffsetLevel([CanBeNull] Level fromLevel, [NotNull] Level toLevel)
        {
            if (fromLevel is null) return;
        
            var exitPosition = fromLevel.exitDoor.transform.position;
            var exitForward = fromLevel.exitDoor.transform.forward;
            
            var targetRotation = Quaternion.FromToRotation(toLevel.enterDoor.transform.forward, exitForward) * toLevel.transform.rotation;
            targetRotation.eulerAngles = new Vector3(0, targetRotation.eulerAngles.y, 0);
            toLevel.transform.rotation = targetRotation;

            var entryOffset = toLevel.enterDoor.transform.position - toLevel.transform.position;
            var newPosition = exitPosition + exitForward * (BridgeDisplay.Length + 1) - entryOffset;
            toLevel.transform.position = newPosition;
        }
    }
}