using Interfaces;
using JetBrains.Annotations;
using UnityEngine;

namespace Objects.Boxes
{
    public class GuidPipe : MonoBehaviour
    {

        [CanBeNull] IMovable m_movable;
        
        
        
        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IMovable>(out var movable))
            {
                m_movable = movable;
                
            }    
        }


        void OnTriggerExit(Collider other)
        {
            
        }
    }
}