using System;
using Data;
using TMPro;
using UnityEngine;

namespace UI.Menu
{
    public class MenuEvent : MonoBehaviour
    {



        public void OnStartGame()
        {
            
        }

        public void OnClassicLevelSelected()
        {
            
        }

        public void OnAdvancedLevelSelected()
        {
            
        }
        
        public void OnGameModeChanged(MenuItem selectedMenuItem)
        {
            var text = selectedMenuItem.GetComponentInChildren<TextMeshProUGUI>();

            switch (Global.Instance.gameMode)
            {
                case GameMode.Advanced:
                    Global.Instance.gameMode = GameMode.Classic;
                    text.text = "Classic";
                    break;
                case GameMode.Classic:
                    Global.Instance.gameMode = GameMode.Advanced;
                    text.text = "Advanced";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        public void OnAudioChanged()
        {
            
        }

        public void OnGeneralVolumeChanged()
        {
            
        }

        public void OnMusicVolumeChanged()
        {
            
        }

        public void OnSoundVolumeChanged()
        {
            
        }
        
        
        
        public void OnVideoChanged()
        {
            
        }

        public void OnExit()
        {
            
        }
        
    }
}