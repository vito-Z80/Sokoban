using UnityEngine;

namespace Objects.Switchers
{
    public class FloorButton : Switcher
    {
        void Update()
        {
            if (transform.position == targetPosition) return;
            Move(Time.deltaTime);
        }
    }
}