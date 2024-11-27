using System;
using System.Threading.Tasks;
using Objects;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Level
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] Assembler electrician;
        [SerializeField] public Corridor corridor;
        int m_currentLevelId;
        Level m_currentLevel;

        const string LevelIdFormat = "000";


        async void Start()
        {
            try
            {
                m_currentLevelId = 1;
                m_currentLevel = await InstantiateNewLevel(m_currentLevelId);
                await m_currentLevel.MaterializeBoxes();
                Level.OnLevelCompleted += LevelCompleted;
            }
            catch (Exception e)
            {
                throw; // TODO handle exception
            }
        }


        async Task<Level> InstantiateNewLevel(int levelId)
        {
            var levelName = levelId.ToString(LevelIdFormat).Trim();
            var handle = await Addressables.InstantiateAsync(levelName).Task;
            var nextLevel = handle.GetComponent<Level>();

            await nextLevel.LevelOffset(m_currentLevel?.exitDoor.transform);
            nextLevel.EnableComponents();


            return nextLevel;
        }


        async void LevelCompleted()
        {
            Level.OnLevelCompleted -= LevelCompleted;
            m_currentLevelId++;
            var nextLevel = await InstantiateNewLevel(m_currentLevelId);
            var exitDoorPosition = m_currentLevel.exitDoor.transform.position;
            var exitDoorForward = m_currentLevel.exitDoor.transform.forward;
            var openExitDoor = m_currentLevel.exitDoor.OpenDoor();
            //  показать коридор.
            var generateCorridor = corridor.ShowCorridor(exitDoorPosition, exitDoorForward);
            await Task.WhenAll(openExitDoor, generateCorridor);
            //  закрываем дверь выхода когда игрок прошел эту дверь.
            await m_currentLevel.exitDoor.CloseDoor(electrician.gameObject);
            //  открываем дверь входа нового уровня когда игрок ближе чем на 2 клетки от двери.
            await nextLevel.enterDoor.OpenDoor(electrician.gameObject);
            //  закрываем дверь входа нового уровня когда игрок станет с другой стороны двери
            var closeExitDoor = nextLevel.enterDoor.CloseDoor(electrician.gameObject);
            //  Материализовать кубы.
            var materializeBoxes = nextLevel.MaterializeBoxes();
            //  спрятать коридор.
            var hideCorridor = corridor.HideCorridor();
            await Task.WhenAll(closeExitDoor, materializeBoxes, hideCorridor);
            corridor.Disable();
            //  уничтожить предыдущий уровень.
            Destroy(m_currentLevel.gameObject);
            m_currentLevel = nextLevel;
            Level.OnLevelCompleted += LevelCompleted;
        }
    }
}