using UnityEngine;

namespace UI
{
    public class ButtonScaler : MonoBehaviour
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


        public void IncreaseScale(Vector3 scale)
        {
            m_targetScale += scale;
        }
    }
}