using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI
{
    public class AlphaScreen : MonoBehaviour
    {
        [SerializeField] float fadeSpeed;
        CanvasGroup m_canvasGroup;

        UniTask<bool> m_current;


        void Start()
        {
            m_canvasGroup = GetComponent<CanvasGroup>();
            m_canvasGroup.alpha = 1.0f;
            Debug.Log(m_canvasGroup.alpha);
        }


        public async UniTask<bool> Fade(Action<bool> onComplete)
        {
            Debug.Log(m_canvasGroup.alpha);

            if (m_canvasGroup.alpha == 0.0f)
            {
                m_current = FadeIn();
            }

            if (Mathf.Approximately(m_canvasGroup.alpha, 1.0f))
            {
                m_current = FadeOut();
            }

            var result = await m_current;
            onComplete?.Invoke(result);
            return result;
        }

        async UniTask<bool> FadeOut()
        {
            Debug.Log("FadeOut");
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
            Debug.Log("FadeIn");
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