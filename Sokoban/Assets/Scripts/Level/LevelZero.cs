using UnityEngine;

namespace Level
{
    public class LevelZero : Level
    {
        public void OpenDoor()
        {
            foreach (var box in GetColoredBoxes())
            {
                box.CanStep(Vector3.down);
            }
        }
    }
}