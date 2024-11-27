using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Objects;
using UnityEngine;
using UnityEngine.Serialization;

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
        

        Point[] m_points;

        const float LevelDistance = 10.0f;

        public static Action OnLevelCompleted;


        /*
         * Вычислить по каким осям будет производиться сортировка всех объектов для появления на экране.
         * Сохранить начальную позицию для каждого объекта.
         * Установить позицию появления для каждого объекта.
         * Спрятать/убрать объекты со сцены.
         * Выполнить материализацию объектов согласно сортировке с интервалом.
         *
         */

        void OnEnable()
        {
            DisableComponents();
        }

        void Start()
        {
            // m_corridor = corridor.GetComponentsInChildren<Mutagen>();
            // _ = MaterializeCorridor();
            m_points = points.GetComponentsInChildren<Point>();
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

            // exitDoor.gameObject.SetActive(true);
            // enterDoor.gameObject.SetActive(true);
        }
        

        IEnumerator LookedCircuit()
        {
            var wait = new WaitForSeconds(0.5f);
            while (m_points.Count(p => p.isInvolved) < m_points.Length)
            {
                yield return wait;
            }

            var boxesGo = boxes.GetComponentsInChildren<Box>();
            foreach (var box in boxesGo)
            {
                box.DisableActions();
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