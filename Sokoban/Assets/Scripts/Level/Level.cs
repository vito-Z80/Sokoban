﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
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
        Box[] m_coloredBoxes;

        public const float LevelDistance = 10.0f;

        public static Action OnLevelCompleted;

        bool m_levelCompleted;

        // void OnEnable()
        // {
        //     DisableComponents();
        // }

        void Start()
        {
            m_points = points.GetComponentsInChildren<ContactorBoxContainer>();
            m_coloredBoxes = boxes.GetComponentsInChildren<Box>().Where(box => box.boxColor != BoxColor.None).ToArray();

            // StartCoroutine(CheckLevelCompletion());
        }

        void LateUpdate()
        {
            if (m_levelCompleted) return;
            CheckLevelState();
        }


        void CheckLevelState()
        {
            if (m_points.Count(container => container.GetContact()) != m_points.Length) return;
            if (m_coloredBoxes.Any(box => !box.DisableActions())) return;
            m_levelCompleted = true;
            OnLevelCompleted?.Invoke();
        }

        // public async Task MaterializeBoxes()
        // {
        //     var tasks = new List<Task>();
        //     foreach (var box in m_boxes)
        //     {
        //         tasks.Add(box.Materialize());
        //     }
        //
        //     await Task.WhenAll(tasks);
        // }

        public Vector3 LevelOffset([CanBeNull] Transform previousExit)
        {
            if (previousExit is null)
            {
                return Vector3.zero;
            }

            var forward = previousExit.transform.forward;
            var offsetBetweenPreviousAndNextDoors = previousExit.position - (enterDoor.transform.position - transform.position) + forward * LevelDistance;
            return offsetBetweenPreviousAndNextDoors;
        }


        IEnumerator CheckLevelCompletion()
        {
            var wait = new WaitForSeconds(0.5f);

            while (m_points.Count(container => container.GetContact()) < m_points.Length)
            {
                yield return wait;
            }

            while (!m_coloredBoxes.All(box => box.DisableActions()))
            {
                yield return null;
            }


            OnLevelCompleted?.Invoke();
        }


        // public void DisableComponents()
        // {
        //     points.SetActive(false);
        //     walls.SetActive(false);
        //     boxes.SetActive(false);
        //     enterDoor.gameObject.SetActive(false);
        //     exitDoor.gameObject.SetActive(false);
        //     floor.SetActive(false);
        // }

        // public void EnableComponents()
        // {
        //     points.SetActive(true);
        //     walls.SetActive(true);
        //     boxes.SetActive(true);
        //     enterDoor.gameObject.SetActive(true);
        //     exitDoor.gameObject.SetActive(true);
        //     floor.SetActive(true);
        //
        //
        //     // StaticBatchingUtility.Combine(floor);
        //     // StaticBatchingUtility.Combine(walls);
        //     // StaticBatchingUtility.Combine(points);
        // }
    }
}