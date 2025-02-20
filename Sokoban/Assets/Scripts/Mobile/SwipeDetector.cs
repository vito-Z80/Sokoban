namespace Mobile
{
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class SwipeDetector : MonoBehaviour
    {
        Vector2 m_startTouchPosition;
        Vector2 m_endTouchPosition;
        const float SwipeThreshold = 50f; // Минимальная дистанция свайпа (в пикселях)

        public delegate void SwipeAction(Vector2 direction);

        public static event SwipeAction OnSwipe; // Событие для передачи направления

        void Update()
        {
            if (Touchscreen.current == null) return;

            // Начало касания
            if (Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            {
                m_startTouchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            }

            // Конец касания
            if (Touchscreen.current.primaryTouch.press.wasReleasedThisFrame)
            {
                m_endTouchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
                DetectSwipe();
            }
        }

        void DetectSwipe()
        {
            var delta = m_endTouchPosition - m_startTouchPosition;

            if (delta.magnitude < SwipeThreshold) return; // Если свайп слишком короткий — игнорируем

            Vector2 swipeDirection;

            // Определяем главное направление свайпа
            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            {
                swipeDirection = delta.x > 0 ? Vector2.right : Vector2.left;
            }
            else
            {
                swipeDirection = delta.y > 0 ? Vector2.up : Vector2.down;
            }

            OnSwipe?.Invoke(swipeDirection); // Вызываем событие с направлением
        }
    }
}