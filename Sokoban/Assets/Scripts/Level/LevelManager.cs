using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Level
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] Assembler electrician;

        int m_currentLevelId;
        Level m_currentLevel;

        const string LevelIdFormat = "000";


        async void Start()
        {
            try
            {
                m_currentLevelId = 1;
                await InstantiateNewLevel(m_currentLevelId);
            }
            catch (Exception e)
            {
                throw; // TODO handle exception
            }
        }


        async Task InstantiateNewLevel(int levelId)
        {
            if (m_currentLevel != null)
            {
                Level.OnLevelCompleted -= LevelCompleted;
            }

            var levelName = levelId.ToString(LevelIdFormat).Trim();
            var handle = await Addressables.InstantiateAsync(levelName).Task;
            var nextLevel = handle.GetComponent<Level>();
            
            
            //  TODO переделать метод ибо непонятки с m_currentLevel?.exit
            //  первый уровень как бы не строится а готовый, потому крашится из-за m_currentLevel?.exit
            
            
            
            nextLevel.Init(m_currentLevel?.exit);

            if (m_currentLevel != null)
            {
                Destroy(m_currentLevel.exit.gameObject);
            }


            //  забрать управление у игрока.
            electrician.SetMove(false);
            //  TODO  Подвести игрока к уровню.
            
            // await Task.Delay(1000);
            
            //  Материализовать кубы.
            await nextLevel.MaterializeBoxes();
            
            
            



            //  вернуть управление игроку.
            electrician.SetMove(true);

            if (m_currentLevel != null)
            {
                // Destroy(m_currentLevel.gameObject);
            }

            m_currentLevel = nextLevel;
            Level.OnLevelCompleted += LevelCompleted;
        }


        async void LevelCompleted()
        {
            m_currentLevelId++;
            await InstantiateNewLevel(m_currentLevelId);
        }
    }
}