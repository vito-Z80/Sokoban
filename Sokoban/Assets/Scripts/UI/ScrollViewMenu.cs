using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    public abstract class ScrollViewMenu : MonoBehaviour
    {
        ScrollRect m_scrollRect;
        Animator m_animator;


        static readonly Stack<ScrollViewMenu> Stack = new();


        int m_contentLength;
        int m_currentItemIndex;
        float m_itemStep;
        float m_targetNormalizedPosition;
        const float ButtonScaleFActor = 0.2f;


        protected void Init()
        {
            m_scrollRect = GetComponent<ScrollRect>();
            m_animator ??= GetComponent<Animator>();

            ButtonScale(0, ButtonScaleFActor);

            m_scrollRect.verticalNormalizedPosition = 1.0f;
            m_targetNormalizedPosition = 1.0f;
            m_scrollRect.vertical = false;
            m_scrollRect.horizontal = false;
            // m_scrollRect.SetLayoutVertical();
            m_contentLength = m_scrollRect.content.childCount;
            m_itemStep = 1.0f / (m_contentLength - 1);
        }

        void OnSelectItem(InputAction.CallbackContext obj)
        {
            if (m_scrollRect.content.GetChild(m_currentItemIndex).TryGetComponent<ButtonScaler>(out var button))
            {
                var nextMenu = OnSelect(button, this);
                if (nextMenu != null)
                {
                    Hide();
                    nextMenu.Show();
                    Stack.Push(this);
                }
            }
        }

        protected abstract ScrollViewMenu OnSelect(ButtonScaler buttonScaler, ScrollViewMenu scrollViewMenu);

        void OnBackMenu(InputAction.CallbackContext obj)
        {
            if (Stack.Count == 0) return;
            Hide();
            Stack.Pop().Show();
        }


        void OnNextItem(InputAction.CallbackContext callback)
        {
            var direction = callback.ReadValue<Vector2>();
            Control(direction);
        }


        void Update()
        {
            if (m_scrollRect == null) return;
            m_scrollRect.verticalNormalizedPosition = Mathf.MoveTowardsAngle(
                m_scrollRect.verticalNormalizedPosition,
                m_targetNormalizedPosition,
                Time.deltaTime / m_contentLength *8);
        }


        void Control(Vector2 direction)
        {
            if (direction.y == 0.0f) return;
            var lastItemIndex = m_currentItemIndex;
            if (direction.y < 0.0f)
            {
                m_currentItemIndex++;
                if (m_currentItemIndex >= m_contentLength)
                {
                    m_currentItemIndex = m_contentLength - 1;
                }
                else
                {
                    SetButtonScale(lastItemIndex);
                }
            }
            else if (direction.y > 0.0f)
            {
                m_currentItemIndex--;
                if (m_currentItemIndex < 0)
                {
                    m_currentItemIndex = 0;
                }
                else
                {
                    SetButtonScale(lastItemIndex);
                }
            }


            SetTargetVerticalNormalizedPosition();
        }


        void SetButtonScale(int lastItemIndex)
        {
            if (lastItemIndex != m_currentItemIndex)
            {
                ButtonScale(lastItemIndex, -ButtonScaleFActor);
            }

            ButtonScale(m_currentItemIndex, ButtonScaleFActor);
        }

        void ButtonScale(int buttonIndex, float scaleFactor)
        {
            if (m_scrollRect.content.GetChild(buttonIndex).TryGetComponent<ButtonScaler>(out var button))
            {
                button?.IncreaseScale(Vector3.one * scaleFactor);
            }
        }

        void SetTargetVerticalNormalizedPosition()
        {
            m_targetNormalizedPosition = 1.0f - m_itemStep * m_currentItemIndex;
        }


        protected void Show()
        {
            gameObject.SetActive(true);
            StartCoroutine(OnShow());
        }

        protected void Hide()
        {
            Global.Instance.input.Player.MenuMove.started -= OnNextItem;
            Global.Instance.input.Player.MovesBack.started -= OnBackMenu;
            Global.Instance.input.Player.Menu.started -= OnSelectItem;
            gameObject.SetActive(false);
        }


        IEnumerator OnShow()
        {
            m_animator ??= GetComponent<Animator>();
            
            while (m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                yield return null;
            }

            Global.Instance.input.Player.MenuMove.started += OnNextItem;
            Global.Instance.input.Player.MovesBack.started += OnBackMenu;
            Global.Instance.input.Player.Menu.started += OnSelectItem;
            
        }
    }
}