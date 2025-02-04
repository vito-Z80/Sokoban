using System;
using Data;
using UnityEngine;

namespace UI
{
    public class MainOptions : ScrollViewMenu
    {

        [SerializeField] ScrollViewMenu audioVolume;
        
        void Start()
        {
            Init();
        }

        protected override ScrollViewMenu OnSelect(ButtonScaler buttonScaler, ScrollViewMenu scrollViewMenu)
        {

          
            switch (buttonScaler.itemName)
            {
                case MenuItemName.Audio:
                    return audioVolume;
                case MenuItemName.ModeAdvanced:
                    return null;
                case MenuItemName.ModeClassic:
                    return null;
                case MenuItemName.MainOptions:
                    return null;
                case MenuItemName.ExitApp:
                    return null;
                case MenuItemName.About:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}