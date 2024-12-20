using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Level
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] CameraManager cameraManager;
        [SerializeField] Assembler electrician;
        [SerializeField] public Corridor corridor;
        int m_currentLevelId;
        Level m_currentLevel;

        const string LevelIdFormat = "000";

        void OnEnable()
        {
            Level.OnLevelCompleted += LevelCompleted;
        }

        void OnDisable()
        {
            Level.OnLevelCompleted -= LevelCompleted;
        }

        async void Start()
        {
            try
            {
                m_currentLevelId = 1;
                m_currentLevel = await InstantiateNewLevel(m_currentLevelId);
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = 60;
            }
            catch (Exception e)
            {
                Debug.Log($"Level Manager\n{e.Message}\n{e.StackTrace}");
            }
        }


        async Task<Level> InstantiateNewLevel(int levelId)
        {
            var levelName = levelId.ToString(LevelIdFormat).Trim();
            var lp = await Addressables.InstantiateAsync(levelName).Task;
            var level = lp.GetComponent<Level>();
            lp.transform.position = level.LevelOffset(m_currentLevel?.exitDoor.transform);
            lp.SetActive(true);
            return level;
        }


        async void LevelCompleted()
        {
            try
            {
                electrician.autoMove = true;
                m_currentLevelId++;
                var nextLevel = await InstantiateNewLevel(m_currentLevelId);
                var exitDoorPosition = m_currentLevel.exitDoor.transform.position.Round();
                var exitDoorPoint = exitDoorPosition;
                exitDoorPoint.y = electrician.transform.position.y;
                var exitDoorForward = m_currentLevel.exitDoor.transform.forward;
                //  открыть дверь выхода.
                await m_currentLevel.exitDoor.OpenDoor();
                //  ждем пока игрок подойдет к выходу.
                while (Vector3.Distance(exitDoorPoint, electrician.transform.position) > 0.33f)
                {
                    await Task.Delay(16);
                }

                //  Указываем позицию автопилота персонажа. 
                var stopPosition = (nextLevel.enterDoor.transform.position + nextLevel.enterDoor.transform.forward).RoundWithoutY();
                //  Устанавливаем персонажа на автопилот.
                electrician.SetAutoMove(stopPosition, exitDoorForward);

                electrician.SetRightForward();
                cameraManager.SetFollow();

                //  показать коридор.
                await corridor.ShowCorridor(exitDoorPoint, exitDoorForward);

                //  закрываем дверь выхода когда игрок прошел эту дверь.
                await m_currentLevel.exitDoor.CloseDoor(electrician.gameObject);

                //  открываем дверь входа нового уровня когда игрок подходит к двери.
                await nextLevel.enterDoor.OpenDoor(electrician.gameObject);

                //  закрываем дверь входа нового уровня когда игрок станет с другой стороны двери
                var closeEnterDoor = nextLevel.enterDoor.CloseDoor(electrician.gameObject);

                //  спрятать коридор.
                var hideCorridor = corridor.HideCorridor();
                await Task.WhenAll(closeEnterDoor, hideCorridor);

                corridor.Disable();

                //  уничтожить предыдущий уровень.
                Destroy(m_currentLevel.gameObject);
                m_currentLevel = nextLevel;
                electrician.autoMove = false;
            }
            catch (Exception e)
            {
                Debug.Log($"Level Completed with ERROR: \n{e.Message}\n{e.StackTrace}");
            }
        }
    }
}