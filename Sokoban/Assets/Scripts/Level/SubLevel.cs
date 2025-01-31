using Interfaces;
using UnityEngine;

namespace Level
{
    public class SubLevel : MonoBehaviour, IInteracting
    {

        [SerializeField] GameObject deactivated;
        
        
        public void Affect(bool affect)
        {
            deactivated.SetActive(false);
            gameObject.SetActive(true);
        }
    }
}