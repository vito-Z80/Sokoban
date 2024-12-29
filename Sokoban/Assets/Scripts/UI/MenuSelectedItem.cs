using Data;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MenuSelectedItem : MonoBehaviour
    {
        [SerializeField] public GameOptions option;

        Image m_image;

        Color m_imageColor;
        Vector3 m_imageScale;


        Color m_deselectColor;
        Color m_selectColor;

        readonly Vector3 m_deselectScale = Vector3.one;
        readonly Vector3 m_selectScale = new(1.1f, 1.1f, 1.1f);

        bool m_isSelected = false;

        void Start()
        {
            m_image = GetComponent<Image>();
            m_imageScale = m_image.rectTransform.localScale;
            m_imageColor = m_image.color;
            m_deselectColor = m_image.color;
            m_selectColor = new Color(m_image.color.r, m_image.color.g, m_image.color.b, m_image.color.a * 2.0f);
        }


        public void Select()
        {
            m_isSelected = true;
        }

        public void Deselect()
        {
            m_isSelected = false;
        }


        void Update()
        {
            var delta = Time.deltaTime * 2.0f;
            if (m_isSelected)
            {
                m_imageScale = Vector3.MoveTowards(m_imageScale, m_selectScale, delta);
                m_imageColor = Color.Lerp(m_imageColor, m_selectColor, delta);
            }
            else
            {
                m_imageScale = Vector3.MoveTowards(m_imageScale, m_deselectScale, delta);
                m_imageColor = Color.Lerp(m_imageColor, m_deselectColor, delta);
            }

            m_image.color = m_imageColor;
            m_image.rectTransform.localScale = m_imageScale;
        }
    }
}