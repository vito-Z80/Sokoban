using UnityEngine;

namespace UI.Menu
{
    public class MenuItem : MonoBehaviour
    {
        Vector3 m_targetScale;


        void Start()
        {
            m_targetScale = transform.localScale;
        }

        void Update()
        {
            if (m_targetScale == transform.localScale) return;
            transform.localScale = Vector3.MoveTowards(transform.localScale, m_targetScale, Time.deltaTime * 2.0f);
        }
        
        public void ScaleUpButton()
        {
            m_targetScale = Vector3.one * 1.1f;
        }
        public void ScaleDownButton()
        {
            m_targetScale = Vector3.one;
        }
    }
}