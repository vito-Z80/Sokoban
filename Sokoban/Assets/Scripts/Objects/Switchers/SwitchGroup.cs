using System.Linq;

namespace Objects.Switchers
{
    /// <summary>
    /// Группа кнопок "switchesUsed" активирует "interacting", если все они зажаты (isOn)
    /// </summary>
    public class SwitchGroup : SwitchControl
    {
        void OnEnable()
        {
            foreach (var s in switchesUsed)
            {
                s.OnSwich += Switch;
            }
        }

        void OnDisable()
        {
            foreach (var s in switchesUsed)
            {
                s.OnSwich -= Switch;
            }
        }

        void Switch(Switcher s)
        {
            var isActivated = IsAllActivated();
            foreach (var affectObject in interacting)
            {
                if (affectObject is IInteracting ao)
                {
                    ao.Affect(isActivated);
                }
            }
        }
        
        bool IsAllActivated()
        {
            return switchesUsed.Length > 0 && !switchesUsed.Any(co => !co.isOn);
        }
    }
}