using System;
using System.Linq;
using System.Threading.Tasks;
using Level.Tasks;
using Objects;
using Objects.Boxes;
using UnityEngine;

namespace Level
{
    public class Level : MonoBehaviour
    {
        [Header("<color=red><size=32>ОТКЛЮЧИ ОБЪЕКТ ПЕРЕД СБОРКОЙ ИЛИ PLAY MODE !!!!!!!!!!!!!!!!!!!!!!!!!!</size></color>\n<color=green>КРОМЕ LEVEL ZERO !!!!!!!!!!!!!!!</color>")]
        [Space(24)]
        [SerializeField]
        GameObject walls;

        [SerializeField] GameObject floor;
        [SerializeField] GameObject points;
        [SerializeField] public GameObject boxes;

        [SerializeField] public Door enterDoor;
        [SerializeField] public Door exitDoor;

        [Header("Level Tasks")] [SerializeField]
        GameObject[] levelTasks;

        ILevelTask[] m_levelTasks;
        int m_taskCount;


        // ContactorBoxContainer[] m_points;
        Box[] m_coloredBoxes;

        // Vector3[] m_wallPositions, m_boxPositions, m_enterDoorPosition, m_exitDoorPosition;
        // Quaternion[] m_floorQuaternions, m_pointQuaternions;

        public static Action OnLevelCompleted;

        bool m_floorAssemblyProcess, m_pointsAssemblyProcess, m_wallsAssemblyProcess, m_boxesAssemblyProcess;


        void OnEnable()
        {
            SubscribeTasks();
        }

        void OnDisable()
        {
            UnsubscribeTasks();
        }


        void Start()
        {
            // m_points = points.GetComponentsInChildren<ContactorBoxContainer>(); //  need ????
            m_coloredBoxes = boxes.GetComponentsInChildren<Box>().ToArray();
        }

        public async Task DisassembleLevel()
        {
            var constructors = transform.GetComponentsInChildren<LevelConstructor>();
            foreach (var constructor in constructors)
            {
                await constructor.DisassembleLevel();
            }
        }

        public Box[] GetColoredBoxes()
        {
            return m_coloredBoxes;
        }

        // void CheckLevelState()
        // {
        //     if (m_points.Count(container => container.GetContact()) != m_points.Length) return;
        //     m_levelCompleted = true;
        //
        //     foreach (var box in m_coloredBoxes)
        //     {
        //         box.DisableActions();
        //     }
        //
        //     foreach (var point in m_points)
        //     {
        //         point.PlayEffectWhirlCube();
        //     }
        //
        //     OnLevelCompleted?.Invoke();
        // }


        void SumUpTask()
        {
            m_taskCount++;
            Debug.Log($"{m_levelTasks.Length} | {m_taskCount}");

            if (m_taskCount == m_levelTasks.Length)
            {
                OnLevelCompleted?.Invoke();
            }
        }

        void SubscribeTasks()
        {
            if (levelTasks == null || levelTasks.Length == 0)
            {
                Debug.LogError($"Level {gameObject.name} has not tasks for complete.");
                return;
            }
            m_levelTasks = new ILevelTask[levelTasks.Length];
            for (var i = 0; i < levelTasks.Length; i++)
            {
                if (levelTasks[i].TryGetComponent<ILevelTask>(out var task))
                {
                    Debug.Log($"{gameObject.name} has task {task.GetType().Name}");
                    task.OnTaskCompleted += SumUpTask;
                    m_levelTasks[i] = task;
                }
                else
                {
                    Debug.LogError($"Task {levelTasks[i].name} has no component ITask.");
                }
            }
        }

        void UnsubscribeTasks()
        {
            if (levelTasks == null || levelTasks.Length == 0) return;

            for (var i = 0; i < levelTasks.Length; i++)
            {
                if (levelTasks[i].TryGetComponent<ILevelTask>(out var task))
                {
                    task.OnTaskCompleted -= SumUpTask;
                    m_levelTasks[i] = task;
                }
            }
        }
    }
}