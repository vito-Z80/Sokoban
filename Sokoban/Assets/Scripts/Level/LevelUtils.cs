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
            var fromExitDoor = fromLevel.exitDoor.transform;
            if (toLevel.enterDoor.transform.rotation.eulerAngles != fromExitDoor.rotation.eulerAngles)
            {
                toLevel.transform.rotation = Quaternion.FromToRotation(fromExitDoor.forward, -toLevel.enterDoor.transform.forward);    
            }
            var forward = fromExitDoor.forward;
            var newPosition = fromExitDoor.position - (toLevel.enterDoor.transform.position - toLevel.transform.position) + forward * (BridgeDisplay.Length + 1);
            toLevel.transform.position = newPosition;
        }
    }
}