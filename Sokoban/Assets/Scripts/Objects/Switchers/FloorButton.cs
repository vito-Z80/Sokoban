using UnityEngine;

namespace Objects.Switchers
{
    public class FloorButton : Switcher
    {
        void Update()
        {
            if (transform.position == TargetPosition) return;
            Move(Time.deltaTime);
        }
    }
}