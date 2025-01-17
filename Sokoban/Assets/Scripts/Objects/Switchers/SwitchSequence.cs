using System;
using Level.Tasks;

namespace Objects.Switchers
{
    /// <summary>
    /// Правильная последовательность нажатий "switchesUsed" активирует "interacting".
    /// </summary>
    public class SwitchSequence : SwitchControl, ILevelTask
    {
        int m_sequenceId;
        bool m_isDone;
        
        public event Action OnTaskCompleted;


        void Start()
        {
            foreach (var s in AllSwitches)
            {
                s.OnSwich += Swich;
            }
        }

        void UnsubscribeSwitches()
        {
            foreach (var s in AllSwitches)
            {
                s.OnSwich -= Swich;
            }
        }

        void Swich(Switcher s)
        {
            if (m_isDone || !s.isOn) return;

            if (switchesUsed[m_sequenceId] == s)
            {
                m_sequenceId++;
                CheckState();
            }
            else
            {
                m_sequenceId = 0;
            }
        }

        void CheckState()
        {
            if (m_sequenceId >= switchesUsed.Length)
            {
                m_isDone = true;
                OnTaskCompleted?.Invoke();
                UnsubscribeSwitches();
                foreach (var affectObject in interacting)
                {
                    if (affectObject is IInteracting ao)
                    {
                        ao.Affect(true);
                    }
                }
            }
        }
    }
}