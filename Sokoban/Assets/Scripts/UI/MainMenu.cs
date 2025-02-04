using System;
using System.Threading.Tasks;
using Data;
using Level;
using TMPro;
using UnityEngine;

namespace UI
{
    public class MainMenu : ScrollViewMenu
    {
        [SerializeField] LevelManager levelManager;
        [SerializeField] CameraManager cameraManager;
        [SerializeField] Assembler character;

        [SerializeField] ScrollViewMenu options;

        StartGame m_startGame;

        void Start()
        {
            Initsdfdsf();
            // Stack.Push(this);
            Show();
            Init();
        }


        async void Initsdfdsf()
        {
            try
            {
                while (levelManager?.m_currentLevel == null)
                {
                    await Task.Delay(100);
                }

                // m_levelZero = levelManager.m_currentLevel as LevelZero;
                character.SetCharacterToLevelZero(levelManager.m_currentLevel.transform);
                cameraManager.SetCameraToLevelZeroLocation();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        protected override ScrollViewMenu OnSelect(ButtonScaler buttonScaler, ScrollViewMenu scrollViewMenu)
        {
            switch (buttonScaler.itemName)
            {
                case MenuItemName.StartGame:
                    m_startGame ??= new StartGame(character, levelManager.m_currentLevel as LevelZero, cameraManager);
                    _ = m_startGame.Run();
                    Hide();
                    return null;
                case MenuItemName.ModeAdvanced:
                    Global.Instance.gameMode = GameMode.Classic;
                    buttonScaler.itemName = MenuItemName.ModeClassic;
                    buttonScaler.GetComponentInChildren<TextMeshProUGUI>().text = "Game Mode<br><size=32>Classic</size>";
                    return null;
                case MenuItemName.ModeClassic:
                    Global.Instance.gameMode = GameMode.Advanced;
                    buttonScaler.itemName = MenuItemName.ModeAdvanced;
                    buttonScaler.GetComponentInChildren<TextMeshProUGUI>().text = "Game Mode<br><size=32>Advanced</size>";
                    return null;
                case MenuItemName.MainOptions:
                    return options;
                case MenuItemName.ExitApp:
                    //
                    Application.Quit();
                    return null;
                case MenuItemName.About:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException(nameof(buttonScaler), buttonScaler.itemName, null);
            }
        }

    }
}