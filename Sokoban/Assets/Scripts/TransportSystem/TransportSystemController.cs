using Objects.Boxes;
using UnityEngine;

namespace TransportSystem
{
    /// <summary>
    /// Если в транспортной системе есть объект, то система запрещает вращение турникета.
    /// </summary>
    public class TransportSystemController : MonoBehaviour
    {
        [Header("Турникет. ")] [SerializeField]
        Turnstile turnstile;


        BoxTransportSystemPart[] m_transportSystems;

        void Start()
        {
            m_transportSystems = GetComponentsInChildren<BoxTransportSystemPart>();
        }


        void Update()
        {
            turnstile.Freezed = SystemIsBusy();
        }

        bool SystemIsBusy()
        {
            foreach (var system in m_transportSystems)
            {
                if (system.IsBusy()) return true;
            }

            return false;
        }
    }
}