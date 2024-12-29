using JetBrains.Annotations;
using UnityEngine;

namespace Level
{
    public static class LevelUtils
    {
        public static void RotateAndOffsetLevel([CanBeNull]Level fromLevel, [NotNull]Level toLevel)
        {
            if (fromLevel is null) return;

            var previousExitDoor = fromLevel.exitDoor.transform;

            var newRotation = new Quaternion(
                toLevel.enterDoor.transform.rotation.x,
                toLevel.enterDoor.transform.rotation.y,
                toLevel.enterDoor.transform.rotation.z,
                -toLevel.enterDoor.transform.rotation.w
            );

            toLevel.transform.rotation *= newRotation;

            var forward = previousExitDoor.forward;
            var newPosition = previousExitDoor.position - (toLevel.enterDoor.transform.position - toLevel.transform.position) + forward * Level.LevelDistance;
            toLevel.transform.position = newPosition;
        }
    }
}