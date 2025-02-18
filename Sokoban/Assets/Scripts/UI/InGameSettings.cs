using UnityEngine;

namespace UI
{
    public class InGameSettings : MonoBehaviour
    {
        [SerializeField] GameMenu gameMenu;
        [SerializeField] Vector2 targetPosition;
        
        RectTransform m_rectTransform;


        void Start()
        {
            m_rectTransform = GetComponent<RectTransform>();
        }

        void Update()
        {
            if (m_rectTransform.anchoredPosition == targetPosition) return;
            m_rectTransform.anchoredPosition = Vector2.MoveTowards(m_rectTransform.anchoredPosition, targetPosition, Time.deltaTime * 512.0f);
        }

        public void OnClick()
        {
            gameMenu.Init(inGame: true);
        }

        public void Show()
        {
            gameMenu.gameObject.SetActive(true);
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameMenu.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
        
    }
}