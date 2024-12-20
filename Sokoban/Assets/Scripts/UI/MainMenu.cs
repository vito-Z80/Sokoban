using System;
using Data;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] GameObject selectedItems;

        InputSystemActions m_inputActions;
        MenuSelectedItem[] m_items;
        RectTransform m_rectTransform;

        Vector2 m_hidePosition;
        float m_hideSpeed = 32.0f;

        int m_selectedItemIndex;

        bool m_isGameStarted;

        void OnEnable()
        {
            m_inputActions = new InputSystemActions();
            m_inputActions.Player.Move.performed += OnMove;
            m_inputActions.Player.Jump.performed += OnButtonPush;
            m_inputActions.Enable();
        }

        void Start()
        {
            m_items = selectedItems.GetComponentsInChildren<MenuSelectedItem>();
            foreach (var item in m_items)
            {
                item.Deselect();
            }

            m_items[0].Select();
            m_rectTransform = GetComponent<RectTransform>();
            m_hidePosition = new Vector2(m_rectTransform.anchoredPosition.x, -1024);
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

        void OnButtonPush(InputAction.CallbackContext input)
        {
            if (input.ReadValue<float>() < 1.0f) return;

            switch (m_items[m_selectedItemIndex].option)
            {
                case GameOptions.Start:
                    StartGame();
                    break;
                case GameOptions.Options:
                    Debug.Log("Options");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void StartGame()
        {
            OnDisable();
            m_isGameStarted = true;
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
            m_inputActions.Player.Move.performed -= OnMove;
            m_inputActions.Player.Attack.performed -= OnButtonPush;
            m_inputActions.Disable();
        }
    }
}