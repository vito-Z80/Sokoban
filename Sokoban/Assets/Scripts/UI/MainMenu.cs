using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
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
        [SerializeField] ClassicLevelChooser classicLevelChooser;

        StartGame m_startGame;

        async void Start()
        {
            while (levelManager?.m_currentLevel == null)
            {
                await UniTask.Delay(100);
            }

            Show();
            Init();
            
            character.SetCharacterToLevelZero(levelManager.m_currentLevel.transform);
            cameraManager.SetCameraToLevelZeroLocation();
            await UniTask.Delay(1000);
            await Global.Instance.alphaScreen.Fade(null);
            
        }

        protected override ScrollViewMenu OnSelect(ButtonScaler buttonScaler, ScrollViewMenu scrollViewMenu)
        {
            switch (buttonScaler.itemName)
            {
                case MenuItemName.StartGame:
                    if (Global.Instance.gameMode == GameMode.Advanced)
                    {
                        m_startGame ??= new StartGame(character, levelManager.m_currentLevel as LevelZero, cameraManager);
                        _ = m_startGame.Run();
                        Hide();
                    }
                    else
                    {
                        Hide();
                        StartClassicGame();
                    }

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

        void StartClassicGame()
        {
            Global.Instance.alphaScreen.Fade(onComplete =>
            {
                if (onComplete)
                {
                    classicLevelChooser.gameObject.SetActive(true);
                    classicLevelChooser.ShowClassicLevelChooser(3);
                }
            }).Forget();
        }
    }
}