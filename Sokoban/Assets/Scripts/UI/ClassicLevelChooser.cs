using ClassicLevels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ClassicLevelChooser : MonoBehaviour
    {
        [SerializeField] ClassicLevelManager classicLevelManager;
        [SerializeField] GameObject levelIndexButton;
        [SerializeField] Transform content;

        


        public void ShowClassicLevelChooser(int openedLevels)
        {
            for (var i = 0; i < openedLevels; i++)
            {
                var go = Instantiate(levelIndexButton, content);
                var button = go.GetComponent<Button>();
                var s = i;
                button.onClick.AddListener( () =>  OnClick(s));
                var text = go.GetComponentInChildren<TextMeshProUGUI>();
                text.text = $"{i}";
            }
        }

        void OnClick(int levelIndex)
        {
            _ = classicLevelManager.StartClassicGame(levelIndex);
            gameObject.SetActive(false);
        }


        int GetLastLevel()
        {
            var id = PlayerPrefs.GetInt("LastClassicLevel", 1);
            return id;
        }
    }
}