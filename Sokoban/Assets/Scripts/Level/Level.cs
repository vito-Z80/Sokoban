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

        public const float LevelDistance = 10.0f;

        public static Action OnLevelCompleted;

        bool m_levelCompleted;

        void Start()
        {
            Debug.Log(isActiveAndEnabled);
            m_points = points.GetComponentsInChildren<ContactorBoxContainer>();
            m_coloredBoxes = boxes.GetComponentsInChildren<Box>().Where(box => box.boxColor != BoxColor.None).ToArray();
        }

        public async Task Assemble()
        {
            await Initialize(floor);
            await Initialize(boxes);
            await Initialize(walls);
        }

        async Task Initialize(GameObject go)
        {
            if (go.TryGetComponent<LevelConstruction>(out var construction))
            {
                await construction.Initialize();
            }
        }

        void LateUpdate()
        {
            if (m_levelCompleted) return;
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

        // public Vector3 LevelOffset([CanBeNull] Transform previousExit)
        // {
        //     if (previousExit is null)
        //     {
        //         return Vector3.zero;
        //     }
        //
        //     var forward = previousExit.transform.forward;
        //     var offsetBetweenPreviousAndNextDoors = previousExit.position - (enterDoor.transform.position - transform.position) + forward * LevelDistance;
        //     return offsetBetweenPreviousAndNextDoors;
        // }

        public void RotateAndOffsetLevel(Level previousLevel)
        {
            if (previousLevel is null) return;

            var previousExitDoor = previousLevel.exitDoor.transform;

            var newRotation = new Quaternion(
                enterDoor.transform.rotation.x,
                enterDoor.transform.rotation.y,
                enterDoor.transform.rotation.z,
                -enterDoor.transform.rotation.w
            );

            transform.rotation *= newRotation;

            var forward = previousExitDoor.forward;
            var newPosition = previousExitDoor.position - (enterDoor.transform.position - transform.position) + forward * LevelDistance;
            transform.position = newPosition;
        }
    }
}