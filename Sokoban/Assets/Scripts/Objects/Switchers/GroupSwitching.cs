using System;
using System.Linq;
using Level.Tasks;

namespace Objects.Switchers
{
    /// <summary>
    /// Группа кнопок "switchesUsed" активирует/деактивирует "interacting", если все они зажаты/отжаты (isOn)
    /// </summary>
    public class GroupSwitching : SwitchControl, ILevelTask
    {
        
        public event Action OnTaskCompleted;

        
        void Start()
        {
            foreach (var s in switchesUsed)
            {
                s.OnSwich += Swich;
            }
        }

        void OnDisable()
        {
            foreach (var s in switchesUsed)
            {
                s.OnSwich -= Swich;
            }
        }

        void Swich(Switch s)
        {
            var isActivated = IsAllActivated();
            foreach (var affectObject in interacting)
            {
                if (affectObject is IInteracting ao)
                {
                    ao.Affect(isActivated);
                    OnTaskCompleted?.Invoke();
                }
            }
        }
        
        bool IsAllActivated()
        {
            return switchesUsed.Length > 0 && !switchesUsed.Any(co => !co.isOn);
        }
    }
}