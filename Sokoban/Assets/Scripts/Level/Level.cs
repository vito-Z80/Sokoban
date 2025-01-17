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
        [Header("<color=red><size=32>ОТКЛЮЧИ ОБЪЕКТ ПЕРЕД СБОРКОЙ ИЛИ PLAY MODE !!!!!!!!!!!!!!!!!!!!!!!!!!</size></color>\n" +
                "<color=green>КРОМЕ LEVEL ZERO !!!!!!!!!!!!!!!</color>")]
        [Space(24)]
        [SerializeField]
        GameObject walls;

        [SerializeField] GameObject floor;
        [SerializeField] public GameObject boxes;

        [SerializeField] public Door enterDoor;
        [SerializeField] public Door exitDoor;

        [Header("<color=green><size=16>Level Tasks</size></color>\n" +
                "<size=11>" +
                "Добавленные сюда задачи должны быть все выполнены для завершения уровня.\n" +
                "Если задача внутренняя и не должна влиять на завершение уровня, не нужно добавлять ее сюда.\n" +
                "</size>")] [SerializeField]
        GameObject[] levelTasks;

        ILevelTask[] m_levelTasks;
        int m_taskCount;
        
        Box[] m_coloredBoxes;


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

        void SumUpTask()
        {
            m_taskCount++;
            if (m_taskCount == m_levelTasks.Length)
            {
                OnLevelCompleted?.Invoke();
            }
        }

        void SubscribeTasks()
        {
            if (levelTasks == null || levelTasks.Length == 0) return;
            
            m_levelTasks = new ILevelTask[levelTasks.Length];
            for (var i = 0; i < levelTasks.Length; i++)
            {
                if (levelTasks[i].TryGetComponent<ILevelTask>(out var task))
                {
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