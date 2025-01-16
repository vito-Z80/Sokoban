using System;
using System.Linq;
using System.Threading.Tasks;
using Objects;
using Objects.Boxes;
using UnityEngine;

namespace Level
{
    public class Level : MonoBehaviour
    {
        [Header("<color=red><size=32>ОТКЛЮЧИ ОБЪЕКТ ПЕРЕД СБОРКОЙ ИЛИ PLAY MODE !!!!!!!!!!!!!!!!!!!!!!!!!!</size></color>\n<color=green>КРОМЕ LEVEL ZERO !!!!!!!!!!!!!!!</color>")]
        [Space(24)]
        [SerializeField] GameObject walls;
        [SerializeField] GameObject floor;
        [SerializeField] GameObject points;
        [SerializeField] public GameObject boxes;

        [SerializeField] public Door enterDoor;
        [SerializeField] public Door exitDoor;

        ContactorBoxContainer[] m_points;
        Box[] m_coloredBoxes;

        Vector3[] m_wallPositions, m_boxPositions, m_enterDoorPosition, m_exitDoorPosition;
        Quaternion[] m_floorQuaternions, m_pointQuaternions;

        public static Action OnLevelCompleted;
        
        bool m_levelCompleted;
        bool m_floorAssemblyProcess, m_pointsAssemblyProcess, m_wallsAssemblyProcess, m_boxesAssemblyProcess;
        
        void Start()
        {
            m_points = points.GetComponentsInChildren<ContactorBoxContainer>();
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


        void Update()
        {
            if (m_levelCompleted) return;

            CheckLevelState();
            // Debug.Log(m_points.Count(container => container.GetContact()));
        }

        public Box[] GetColoredBoxes() => m_coloredBoxes;

        void CheckLevelState()
        {

            if (m_points.Count(container => container.GetContact()) != m_points.Length) return;
            m_levelCompleted = true;

            foreach (var box in m_coloredBoxes)
            {
                box.DisableActions();
            }

            foreach (var point in m_points)
            {
                point.PlayEffectWhirlCube();
            }
            
            OnLevelCompleted?.Invoke();
        }
    }
}