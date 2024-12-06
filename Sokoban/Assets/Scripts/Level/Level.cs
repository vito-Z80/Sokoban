using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Objects;
using Objects.Boxes;
using UnityEngine;

namespace Level
{
    public class Level : MonoBehaviour
    {
        [SerializeField] GameObject points;
        [SerializeField] GameObject walls;
        [SerializeField] GameObject floor;
        [SerializeField] public GameObject boxes;

        [SerializeField] public Door enterDoor;
        [SerializeField] public Door exitDoor;
        

        ContactorBoxContainer[] m_points;

        public const float LevelDistance = 10.0f;

        public static Action OnLevelCompleted;

        void OnEnable()
        {
            DisableComponents();
        }

        void Start()
        {
            m_points = points.GetComponentsInChildren<ContactorBoxContainer>();
            StartCoroutine(LookedCircuit());
        }

        public async Task MaterializeBoxes()
        {
            var boxesGo = boxes.GetComponentsInChildren<Box>();
            var tasks = new List<Task>();
            foreach (var box in boxesGo)
            {
                tasks.Add(box.Materialize());
            }

            await Task.WhenAll(tasks);
        }

        public async Task LevelOffset([CanBeNull] Transform previousExit)
        {
            Vector3 offsetBetweenPreviousAndNextExits;

            if (previousExit is null)
            {
                offsetBetweenPreviousAndNextExits = Vector3.zero;
            }
            else
            {
                var forward = previousExit.transform.forward;
                offsetBetweenPreviousAndNextExits = previousExit.position - (enterDoor.transform.position - transform.position) + forward * LevelDistance;
            }
            
            transform.position = offsetBetweenPreviousAndNextExits;
        }
        

        IEnumerator LookedCircuit()
        {
            var wait = new WaitForSeconds(0.5f);
            while (m_points.Count(container => container.GetContact()) < m_points.Length)
            {
                Debug.Log(m_points.Count(container => container.GetContact()));
                yield return wait;
            }

            var boxesGo = boxes.GetComponentsInChildren<Box>();

            while (!boxesGo.All(box => box.DisableActions()))
            {
                yield return null;
            }
            

            OnLevelCompleted?.Invoke();
        }
        

        public void DisableComponents()
        {
            points.SetActive(false);
            walls.SetActive(false);
            boxes.SetActive(false);
            enterDoor.gameObject.SetActive(false);
            exitDoor.gameObject.SetActive(false);
            floor.SetActive(false);
        }

        public void EnableComponents()
        {
            points.SetActive(true);
            walls.SetActive(true);
            boxes.SetActive(true);
            enterDoor.gameObject.SetActive(true);
            exitDoor.gameObject.SetActive(true);
            floor.SetActive(true);
        }
    }
}