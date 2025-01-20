using UnityEngine;

namespace Level
{
    public class LevelZero : Level
    {
        public void OpenDoor()
        {
            foreach (var box in GetColoredBoxes())
            {
                box.Freezed  = false;
                box.CanMove(Vector3.down);
            }
        }
    }
}