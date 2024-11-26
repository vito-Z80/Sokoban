using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Objects;
using UnityEngine;

namespace Level
{
    public class Level : MonoBehaviour
    {
        [SerializeField] GameObject level;
        [SerializeField] GameObject points;
        [SerializeField] GameObject walls;
        [SerializeField] GameObject floor;
        [SerializeField] public GameObject boxes;
        [SerializeField] GameObject enter;
        [SerializeField] public GameObject exit;
        Mutagen[] m_corridor;

        Point[] m_points;


        public static Action OnLevelCompleted;


        /*
         * Вычислить по каким осям будет производиться сортировка всех объектов для появления на экране.
         * Сохранить начальную позицию для каждого объекта.
         * Установить позицию появления для каждого объекта.
         * Спрятать/убрать объекты со сцены.
         * Выполнить материализацию объектов согласно сортировке с интервалом.
         *
         */

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

        public void Init([CanBeNull] GameObject exitBox)
        {
            if (exitBox is null)
            {
                return;
            }

            transform.position = Vector3.zero;

            var position = exitBox.transform.position;
            var forward = exitBox.transform.forward;


            var offset = position + forward * 10.0f;
            offset += forward * 10.0f;

            LevelOffset(points, offset);
            LevelOffset(walls, offset);
            LevelOffset(floor, offset);
            LevelOffset(boxes, offset);
            
            enter.transform.SetParent(level.transform);
            exit.transform.SetParent(level.transform);

            enter.transform.position += offset;
            exit.transform.position += offset;
            
        }

        void LevelOffset(GameObject obj, Vector3 offset)
        {
            Debug.Log(obj.transform.childCount);

            for (var i = 0; i < obj.transform.childCount; i++)
            {
                obj.transform.GetChild(i).position += offset;
                if (obj.transform.GetChild(i).gameObject.TryGetComponent<Box>(out var mainObject))
                {
                    mainObject.Init();
                    Debug.Log("Materialized Box");
                }
            }
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


        async Task MaterializeCorridor()
        {
            var sort = m_corridor.OrderBy(mutagen => mutagen.gameObject.transform.position.x);
            foreach (var mutagen in sort)
            {
                mutagen.Materialize();
                await Task.Delay(500);
            }
        }
    }
}