namespace Objects.Switchers
{
    /// <summary>
    /// Правильная последовательность нажатий "switchesUsed" активирует "interacting".
    /// </summary>
    public class SwitchSequence : SwitchControl
    {
        int m_sequenceId;
        bool m_isDone;

        void Start()
        {
            foreach (var s in AllSwitches)
            {
                s.OnSwich += Swich;
            }
        }

        void DisableSwitches()
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
                DisableSwitches();
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