using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI.Menu
{
    public class ScrollViewManager : MonoBehaviour
    {
        [SerializeField] float scrollSpeed;


        InputAction m_input;
        ScrollRect m_scrollRect;
        RectTransform[] m_menuItemRects;
        MenuItem[] m_menuItem;

        Vector2 m_previousMousePosition;

        float m_targetNormalizedPosition;
        int m_contentLength;
        int m_itemIndex;
        float m_itemStep;


        void Start()
        {
            m_scrollRect = GetComponent<ScrollRect>();
            m_targetNormalizedPosition = 1.0f;
            m_scrollRect.verticalNormalizedPosition = m_targetNormalizedPosition;
            m_contentLength = m_scrollRect.content.childCount;
            m_menuItemRects = new RectTransform[m_contentLength];
            m_menuItem = new MenuItem[m_contentLength];
            for (var i = 0; i < m_contentLength; i++)
            {
                m_menuItemRects[i] = m_scrollRect.content.GetChild(i).GetComponent<RectTransform>();
                m_menuItem[i] = m_scrollRect.content.GetChild(i).GetComponentInChildren<MenuItem>();
            }

            m_itemStep = 1.0f / (m_contentLength - 1);
            // Global.Instance.input.Player.MenuMove.started += OnMove;
        }

        void OnMove(InputAction.CallbackContext obj)
        {
            var direction = obj.ReadValue<Vector2>();
            var lastItemIndex = m_itemIndex;
            m_itemIndex = NextItem(direction);
            if (lastItemIndex == m_itemIndex) return;
            m_isKeys = true;
            var buttonScaleUp = m_menuItem[m_itemIndex];
            var buttonScaleDown = m_menuItem[lastItemIndex];
            buttonScaleUp.ScaleUpButton();
            buttonScaleDown.ScaleDownButton();
        }


        bool m_isKeys;

        void Update()
        {
            var mousePosition = Mouse.current.position.ReadValue();
            if (mousePosition != m_previousMousePosition || Mouse.current.scroll.ReadValue().y != 0.0f)
            {
                MouseControl(mousePosition);
            }

            if (m_isKeys)
            {
                m_scrollRect.verticalNormalizedPosition = Mathf.MoveTowards(m_scrollRect.verticalNormalizedPosition, m_targetNormalizedPosition, Time.deltaTime * scrollSpeed / m_contentLength);
            }

            m_previousMousePosition = mousePosition;
        }

        void MouseControl(Vector2 mousePosition)
        {
            var transformRect = transform.GetComponent<RectTransform>();
            if (!RectTransformUtility.RectangleContainsScreenPoint(transformRect, mousePosition)) return;
            for (var id = 0; id < m_contentLength; id++)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(m_menuItemRects[id], mousePosition))
                {
                    m_itemIndex = id;
                    break;
                }
            }

            for (var id = 0; id < m_contentLength; id++)
            {
                m_menuItem[id].ScaleDownButton();
            }

            m_menuItem[m_itemIndex].ScaleUpButton();

            m_isKeys = false;
        }

        int NextItem(Vector2 direction)
        {
            var itemIndex = m_itemIndex;
            if (direction.y < 0.0f)
            {
                itemIndex = Mathf.Clamp(m_itemIndex + 1, 0, m_contentLength - 1);
            }

            if (direction.y > 0.0f)
            {
                itemIndex = Mathf.Clamp(m_itemIndex - 1, 0, m_contentLength - 1);
            }

            m_targetNormalizedPosition = (m_contentLength - 1 - itemIndex) * m_itemStep;
            return itemIndex;
        }
    }
}