using System;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Objects;
using Objects.Boxes;
using UnityEngine;

namespace Level
{
    public class Level : MonoBehaviour
    {
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

        bool m_levelCompleted, m_isAssembling;
        bool m_floorAssemblyProcess, m_pointsAssemblyProcess, m_wallsAssemblyProcess, m_boxesAssemblyProcess;

        void Start()
        {
            m_points = points.GetComponentsInChildren<ContactorBoxContainer>();
            m_coloredBoxes = boxes.GetComponentsInChildren<Box>().Where(box => box.boxColor != BoxColor.None).ToArray();
            m_isAssembling = true;
        }

        public async Task DisassembleLevel()
        {
            var constructors = transform.GetComponentsInChildren<LevelConstructor>();
            foreach (var constructor in constructors)
            {
                await constructor.DisassembleLevel();
            }
        }

        public void SetAssembling()
        {
            m_isAssembling = true;
        }

        void AssembleLevel()
        {
            // foreach (var VARIABLE in COLLECTION)
            // {
            //     
            // }
        }


        void LateUpdate()
        {
            if (m_levelCompleted) return;
            if (m_isAssembling)
            {
                AssembleLevel();
            }

            CheckLevelState();
        }

        protected Box[] GetColoredBoxes() => m_coloredBoxes;

        void CheckLevelState()
        {
            if (m_points.Count(container => container.GetContact()) != m_points.Length) return;
            if (m_coloredBoxes.Any(box => !box.DisableActions())) return;
            m_levelCompleted = true;
            OnLevelCompleted?.Invoke();
        }
    }
}