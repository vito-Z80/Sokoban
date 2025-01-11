using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] GameMenu gameMenu;

        bool m_isGameMenuVisibility;

        void Start()
        {
            Global.Instance.input.Player.Menu.started += ShowGameMenu;
            m_isGameMenuVisibility = gameMenu.gameObject.activeSelf;
        }

        void ShowGameMenu(InputAction.CallbackContext call)
        {
            m_isGameMenuVisibility = !m_isGameMenuVisibility;
            gameMenu.gameObject.SetActive(m_isGameMenuVisibility);
        }

        void OnDisable()
        {
            Global.Instance.input.Player.Menu.started -= ShowGameMenu;
        }
    }
}