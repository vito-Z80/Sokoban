using System;
using System.Linq;
using Interfaces;
using Objects.Switchers;

namespace Level.Tasks
{
    /// <summary>
    /// Правильная последовательность нажатий "switchesUsed" активирует "interacting".
    /// </summary>
    public class TaskOneTimeSequence : SwitchControl, ILevelTask
    {
        int m_sequenceId;
        bool m_isDone;

        Switch[] m_incorrectSwitches;

        public event Action OnTaskCompleted;


        void Start()
        {
            foreach (var s in AllSwitches)
            {
                s.OnSwich += Swich;
            }

            m_incorrectSwitches = AllSwitches.Where(s => !switchesUsed.Contains(s)).ToArray();
        }

        void UnsubscribeSwitches()
        {
            foreach (var s in AllSwitches)
            {
                s.OnSwich -= Swich;
            }
        }

        void Swich(Switch s)
        {
            if (m_incorrectSwitches.Any(incorrect => incorrect.isOn))
            {
                m_sequenceId = 0;
                return;
            }
            if (m_isDone || s.isOn) return;

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