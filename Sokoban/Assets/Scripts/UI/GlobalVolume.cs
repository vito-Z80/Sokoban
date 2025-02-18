using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GlobalVolume : MonoBehaviour
    {
        [SerializeField] Slider volumeSlider;

        void Start()
        {
            volumeSlider.value = 0.5f;
            OnVolumeChanged();
        }

        public void OnVolumeChanged()
        {
            AudioListener.volume = volumeSlider.value;
        }
        
    }
}