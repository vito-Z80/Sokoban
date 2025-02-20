using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI
{
    public class AlphaScreen : MonoBehaviour
    {
        [SerializeField] float fadeSpeed;
        CanvasGroup m_canvasGroup;

        bool m_current;


        void Start()
        {
            m_canvasGroup = GetComponent<CanvasGroup>();
            m_canvasGroup.alpha = 1.0f;
        }


        public async UniTask<bool> Fade(Action<bool> onComplete)
        {
            if (m_canvasGroup.alpha == 0.0f)
            {
                m_current = await FadeIn();
            }
            else if (Mathf.Approximately(m_canvasGroup.alpha, 1.0f))
            {
                m_current = await FadeOut();
            }

            onComplete?.Invoke(m_current);
            return m_current;
        }

        async UniTask<bool> FadeOut()
        {
            while (m_canvasGroup.alpha > 0.0f)
            {
                m_canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
                await UniTask.Yield();
            }

            m_canvasGroup.alpha = 0.0f;
            return true;
        }

        async UniTask<bool> FadeIn()
        {
            while (m_canvasGroup.alpha < 1.0f)
            {
                m_canvasGroup.alpha += Time.deltaTime * fadeSpeed;
                await UniTask.Yield();
            }

            m_canvasGroup.alpha = 1.0f;
            return true;
        }
    }
}