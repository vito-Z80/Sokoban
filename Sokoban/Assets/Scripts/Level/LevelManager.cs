﻿using System;
using System.Threading.Tasks;
using Bridge;
using Data;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Level
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] CameraManager cameraManager;
        [SerializeField] Assembler electrician;
        [SerializeField] public BridgeDisplay bridge;
        int m_currentLevelId;
        public Level m_currentLevel;


        const string LevelIdFormat = "000";

        StepsController m_stepsController;

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

        async Task<Level> InstantiateNewLevel(int levelId)
        {
            var levelName = levelId.ToString(LevelIdFormat).Trim();
            var lp = await Addressables.InstantiateAsync(levelName).Task;
            var level = lp.GetComponent<Level>();
            LevelUtils.RotateAndOffsetLevel(m_currentLevel, level);
            return level;
        }

        public void UndoPop()
        {
            if (electrician.IsMoving()) return;
            StepsController.OnPop?.Invoke();
        }


        async void LevelCompleted()
        {
            try
            {
                //  условия уровня выполнены.
                Global.Instance.levelPhase = LevelPhase.SolutionFound;

                // TODO 4 уровень не правильно собирается если входная дверь под углом 0 градусов. Напольные кнопки тоже не работают...
                
                m_currentLevelId++;
                m_stepsController ??= new StepsController(electrician);
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
                //  Уровень завершен (был покинут через дверь выхода).
                Global.Instance.levelPhase = LevelPhase.Finished;

                //  Указываем позицию автопилота персонажа. 
                var stopPosition = (nextLevel.enterDoor.transform.position + nextLevel.enterDoor.transform.forward).RoundWithoutY();

                //  Устанавливаем персонажа на автопилот.
                electrician.SetAutoMove(stopPosition + Vector3.down * 0.5f, exitDoorForward);

                electrician.SetRightForward();
                cameraManager.SetFollow();

                //  показать мост.
                await bridge.Init(exitDoorPosition + Vector3.down + exitDoorForward, nextLevel.enterDoor.transform.forward);
                
                //  Обнулить шаги.
                Global.Instance.gameState.steps = 0;
                

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

                //  спрятать мост.
                await bridge.Init(exitDoorPosition + Vector3.down + exitDoorForward, nextLevel.enterDoor.transform.forward);

                //  закрываем дверь входа нового уровня когда игрок станет с другой стороны двери
                while (Vector3.Distance(electrician.transform.position, enterDoorTransform.position + enterDoorTransform.forward) > 1.3f)
                {
                    await Task.Yield();
                }

                nextLevel.enterDoor.CloseDoor();
                

                //  уничтожить предыдущий уровень.
                await Task.Yield();
                DestroyImmediate(m_currentLevel.gameObject);
                await Task.Yield();
                


                m_currentLevel = nextLevel;
                //  Передать управление игроку.
                electrician.autoMove = false;
                //  Можно приступить к решению уровня.
                Global.Instance.levelPhase = LevelPhase.SearchSolution;

                //  Активировать коробки.
                foreach (var coloredBox in m_currentLevel.GetColoredBoxes())
                {
                    coloredBox.EnableActions();
                }

                //  Получить все MainObject уровня для контроля Undo.
                m_stepsController.CollectMainObjects(nextLevel.gameObject);
            }
            catch (Exception e)
            {
                Debug.LogError($"Level Completed with ERROR: \n{e.Message}\n{e.StackTrace}");
            }
        }
    }
}