using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    public class MainMenu2 : MonoBehaviour
    {
        [SerializeField] ScrollRect scrollRect;

        int m_contentLength;
        int m_currentItemIndex;
        float m_itemStep;
        float m_targetNormalizedPosition;
        float m_buttonScaleFActor = 0.2f;

        void Start()
        {
            ButtonScale(0, m_buttonScaleFActor);
            scrollRect.verticalNormalizedPosition = 1.0f;
            m_targetNormalizedPosition = 1.0f;
            scrollRect.SetLayoutVertical();
            m_contentLength = scrollRect.content.childCount;
            m_itemStep = 1.0f / (m_contentLength - 1);
            Global.Instance.input.Player.Move.started += SelectItem;
        }

        void SelectItem(InputAction.CallbackContext callback)
        {
            var direction = callback.ReadValue<Vector2>();
            Control(direction);
        }


        void Update()
        {
            scrollRect.verticalNormalizedPosition = Mathf.MoveTowards(
                scrollRect.verticalNormalizedPosition,
                m_targetNormalizedPosition,
                Time.deltaTime * 4.0f);
        }


        void Control(Vector2 direction)
        {
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


            SetTargetNormalizedPosition();
            
        }



        void SetButtonScale(int lastItemIndex)
        {
            if (lastItemIndex != m_currentItemIndex)
            {
                ButtonScale(lastItemIndex, -m_buttonScaleFActor);
            }

            ButtonScale(m_currentItemIndex, m_buttonScaleFActor);
        }
        
        void ButtonScale(int buttonIndex, float scaleFactor)
        {
            if (scrollRect.content.GetChild(buttonIndex).TryGetComponent<ButtonScaler>(out var button))
            {
                button?.IncreaseScale(Vector3.one * scaleFactor);
            }
        }


        void SetTargetNormalizedPosition()
        {
            m_targetNormalizedPosition = 1.0f - m_itemStep * m_currentItemIndex;
        }
    }
}