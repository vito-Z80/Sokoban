using JetBrains.Annotations;
using Objects.Boxes;
using UnityEngine;

namespace TransportSystem
{
    
    
    //  TODO когда короб выпадает сверху из транспортной системы и летит уже вниз (не находясь в транспортной системе)
    //   и если повернуть турникет, то вращение передается этому коробу  - ИСПРАВИТЬ !
    
    public class BoxTransportSystemPart : MonoBehaviour
    {
        [CanBeNull] Box m_box;
        bool m_isBusy;

        void Update()
        {
            if (m_box is null) return;
            if (!m_box.CanMove(transform.forward)) return;
            m_box.canFall = true;
            m_box.boxSpeed /= 2.0f;
            m_box = null;
        }

        public bool IsBusy()
        {
            return m_isBusy;
        }


        void OnTriggerEnter(Collider other)
        {
            m_isBusy = true;
            if (other.TryGetComponent<Box>(out var box))
            {
                box.boxSpeed *= 2.0f;
                box.canFall = false;
                m_box = box;
            }
        }

        void OnTriggerExit(Collider other)
        {
            m_isBusy = false;
        }
    }
}