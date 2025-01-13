using System;
using System.Threading.Tasks;
using Data;
using Level;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] LevelManager levelManager;
        [SerializeField] CameraManager cameraManager;
        [SerializeField] GameObject selectedItems;
        [SerializeField] Assembler character;

        StartGame m_startGame;

        LevelZero m_levelZero;

        MenuSelectedItem[] m_items;
        RectTransform m_rectTransform;

        Vector2 m_hidePosition;
        float m_hideSpeed = 32.0f;

        int m_selectedItemIndex;

        bool m_isGameStarted;


        async void Start()
        {
            Global.Instance.input.Player.Move.started += OnMove;
            Global.Instance.input.Player.Jump.started += OnButtonPush;

            try
            {
                while (levelManager?.m_currentLevel == null)
                {
                    await Task.Delay(100);
                }

                m_levelZero = levelManager.m_currentLevel as LevelZero;
                character.SetCharacterToLevelZero(levelManager.m_currentLevel.transform);
                cameraManager.SetCameraToLevelZeroLocation();

                m_items = selectedItems.GetComponentsInChildren<MenuSelectedItem>();
                foreach (var item in m_items)
                {
                    item.Deselect();
                }

                m_items[0].Select();
                m_rectTransform = GetComponent<RectTransform>();
                m_hidePosition = new Vector2(m_rectTransform.anchoredPosition.x, -1024);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        void Update()
        {
            if (m_isGameStarted)
            {
                HideMainMenu();
            }
        }

        void HideMainMenu()
        {
            m_rectTransform.anchoredPosition = Vector2.MoveTowards(m_rectTransform.anchoredPosition, m_hidePosition, m_hideSpeed);
            m_hideSpeed += 1.0f;
            if (m_rectTransform.anchoredPosition == m_hidePosition) Destroy(gameObject);
        }

        void OnButtonPush(InputAction.CallbackContext call)
        {
            if (m_isGameStarted) return;
            switch (m_items[m_selectedItemIndex].option)
            {
                case GameOptions.Start:
                    _ = StartGame();
                    break;
                case GameOptions.Options:
                    Debug.Log("Options");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        async Task StartGame()
        {
            OnDisable();
            m_isGameStarted = true;
            m_startGame ??= new StartGame(character, m_levelZero, cameraManager);

            var globalGameSpeed = Global.Instance.gameSpeed;
            Global.Instance.gameSpeed = 1.0f;

            await m_startGame.Run();
            character.autoMove = false;
            Global.Instance.gameSpeed = globalGameSpeed;
        }

        void OnMove(InputAction.CallbackContext input)
        {
            m_items[m_selectedItemIndex].Deselect();
            var selectedIndex = ChangeSelectedIndex(input.ReadValue<Vector2>());
            m_items[selectedIndex].Select();
        }

        int ChangeSelectedIndex(Vector2 input)
        {
            if (input.y > 0.0f)
            {
                m_selectedItemIndex--;
                if (m_selectedItemIndex < 0)
                {
                    m_selectedItemIndex = m_items.Length - 1;
                }
            }

            if (input.y < 0.0f)
            {
                m_selectedItemIndex++;
                if (m_selectedItemIndex >= m_items.Length)
                {
                    m_selectedItemIndex = 0;
                }
            }

            return m_selectedItemIndex;
        }


        void OnDisable()
        {
            Global.Instance.input.Player.Move.performed -= OnMove;
            Global.Instance.input.Player.Jump.performed -= OnButtonPush;
        }
    }
}