using Data;
using Level.Tasks;
using Objects.Boxes;
using UnityEngine;

namespace Objects
{
    public class ContactorBoxContainer : MainObject
    {
        [SerializeField] public BoxColor pointColor;
        [SerializeField] ParticleSystem magicPoint;
        [SerializeField] ParticleSystem whirlCube;
        bool m_contacted;


        public bool GetContact()
        {
            return m_contacted;
        }

        public override void ClearStack()
        {
        }

        public override void PopState()
        {
        }

        public override void PushState()
        {
        }

        void PlayEffectMagicPoint()
        {
            if (magicPoint.isPlaying)
            {
                magicPoint.Stop(true);
            }

            magicPoint.Play();
        }

        public void PlayEffectWhirlCube()
        {
            if (whirlCube.isPlaying)
            {
                whirlCube.Stop(true);
            }

            whirlCube.Play();
        }


        void OnTriggerEnter(Collider other)
        {
            Debug.Log($"Trigger Enter: {other.name}");
            if (other.TryGetComponent<Box>(out var box))
            {
                if (box.boxColor == pointColor)
                {
                    Debug.Log(box.boxColor.ToString());
                    m_contacted = true;
                    TaskActivatePoints.OnPointContact?.Invoke();
                    PlayEffectMagicPoint();
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            Debug.Log($"Trigger Exit: {other.name}");
            m_contacted = false;
            TaskActivatePoints.OnPointContact?.Invoke();
        }
    }
}