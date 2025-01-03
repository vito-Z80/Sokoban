﻿using System;
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
        public Level m_currentLevel;


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
                await StartLevelZero();
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = 60;
            }
            catch (Exception e)
            {
                Debug.Log($"Level Manager\n{e.Message}\n{e.StackTrace}");
            }
        }

        public async Task StartLevelZero()
        {
            try
            {
                m_currentLevelId = 0;
                m_currentLevel = await InstantiateNewLevel(m_currentLevelId);
            }
            catch (Exception e)
            {
                Debug.Log($"Level Manager\n{e.Message}\n{e.StackTrace}");
            }
        }

        public void GoToFirstLevel()
        {
        }


        async Task<Level> InstantiateNewLevel(int levelId)
        {
            var levelName = levelId.ToString(LevelIdFormat).Trim();
            var lp = await Addressables.InstantiateAsync(levelName).Task;
            var level = lp.GetComponent<Level>();
            LevelUtils.RotateAndOffsetLevel(m_currentLevel, level);
            return level;
        }


        async void LevelCompleted()
        {
            try
            {
                m_currentLevelId++;
                var nextLevel = await InstantiateNewLevel(m_currentLevelId);
                await nextLevel.DisassembleLevel();
                var exitDoorPosition = m_currentLevel.exitDoor.transform.position.Round();
                var exitDoorPoint = exitDoorPosition;
                exitDoorPoint.y = electrician.transform.position.y;
                var exitDoorForward = m_currentLevel.exitDoor.transform.forward;
                //  открыть дверь выхода.
                m_currentLevel.exitDoor.OpenDoor();
                //  ждем пока игрок подойдет к выходу.
                while (Vector3.Distance(exitDoorPoint, electrician.transform.position) > 0.33f)
                {
                    await Task.Yield();
                }
                //  Забрать управление у игрока.
                electrician.autoMove = true;

                //  Указываем позицию автопилота персонажа. 
                var stopPosition = (nextLevel.enterDoor.transform.position + nextLevel.enterDoor.transform.forward).RoundWithoutY();

                //  Устанавливаем персонажа на автопилот.
                electrician.SetAutoMove(stopPosition + Vector3.down * 0.5f, exitDoorForward);

                electrician.SetRightForward();
                cameraManager.SetFollow();

                //  показать коридор.
                await corridor.ShowCorridor(exitDoorPoint, exitDoorForward);

                //  закрываем дверь выхода когда игрок прошел эту дверь.
                while (Vector3.Distance(electrician.transform.position, exitDoorPosition + exitDoorForward) > 0.7f)
                {
                    await Task.Yield();
                }
                m_currentLevel.exitDoor.CloseDoor();


                //  Ожидание персонажа, пока не достигнет 3х клеток до входной двери следующего уровня.
                while (Vector3.Distance(electrician.transform.position, stopPosition - exitDoorForward * 4) > 1.0f)
                {
                    await Task.Yield();
                }

                //  Активируем следующий уровень. При активации начнется его построение.
                nextLevel.gameObject.SetActive(true);

                var enterDoorTransform = nextLevel.enterDoor.transform;
                //  открываем дверь входа нового уровня когда игрок подходит к двери.
                while (Vector3.Distance(electrician.transform.position, enterDoorTransform.position) > 3.0f)
                {
                    await Task.Yield();
                }
                nextLevel.enterDoor.OpenDoor();

                //  закрываем дверь входа нового уровня когда игрок станет с другой стороны двери
                while (Vector3.Distance(electrician.transform.position, enterDoorTransform.position + enterDoorTransform.forward) > 1.3f)
                {
                    await Task.Yield();
                }
                nextLevel.enterDoor.CloseDoor();

                //  спрятать коридор.
                await corridor.HideCorridor();
                corridor.Disable();

                //  уничтожить предыдущий уровень.
                Destroy(m_currentLevel.gameObject);
                m_currentLevel = nextLevel;
                //  Передать управление игроку.
                electrician.autoMove = false;
            }
            catch (Exception e)
            {
                Debug.LogError($"Level Completed with ERROR: \n{e.Message}\n{e.StackTrace}");
            }
        }
    }
}