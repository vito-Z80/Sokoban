using Cysharp.Threading.Tasks;
using Level;
using UnityEngine;

namespace UI
{
    public class GameMenu : MonoBehaviour
    {
        [SerializeField] GameObject start;
        [SerializeField] GameObject restart;

        [SerializeField] Vector2 showPosition;
        [SerializeField] Vector2 hidePosition;

        RectTransform m_rectTransform;
        CameraManager m_cameraManager;
        Assembler m_character;
        LevelManager m_levelManager;
        AlphaScreen m_alphaScreen;
        Vector2 m_targetPosition;

        bool m_isStarted = false;

        async void Start()
        {
            m_rectTransform = GetComponent<RectTransform>();
            m_levelManager = Global.Instance.levelManager;
            m_cameraManager = Global.Instance.cameraManager;
            m_character = Global.Instance.character;
            m_alphaScreen = Global.Instance.alphaScreen;
            m_targetPosition = showPosition;
            while (m_levelManager.m_currentLevel == null)
            {
                await UniTask.Delay(100);
            }

            m_character.SetCharacterToLevelZero(m_levelManager.m_currentLevel.transform);
            m_cameraManager.SetCameraToLevelZeroLocation();
            await UniTask.Delay(1000);
            await m_alphaScreen.Fade(null);
        }


        public void Init(bool inGame)
        {
            if (inGame)
            {
                start.SetActive(false);
                restart.SetActive(true);
            }
            else
            {
                start.SetActive(true);
                restart.SetActive(false);
            }

            m_targetPosition = m_targetPosition == hidePosition ? showPosition : hidePosition;
        }

        void Update()
        {
            Move();
        }

        void Move()
        {
            m_rectTransform.anchoredPosition = Vector2.MoveTowards(m_rectTransform.anchoredPosition, m_targetPosition, Time.deltaTime * 1024.0f);
        }


        public void Restart()
        {
            m_levelManager.LevelRestart();
        }
        
        public void StartGame()
        {
            if (m_isStarted) return;
            m_isStarted = true;
            var startGame = new StartGame(m_character, m_levelManager.m_currentLevel as LevelZero, m_cameraManager);
            _ = startGame.Run();
            m_targetPosition = hidePosition;
        }
    }
}