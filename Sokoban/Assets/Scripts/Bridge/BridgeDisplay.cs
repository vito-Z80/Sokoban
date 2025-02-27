﻿
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Bridge
{
    public class BridgeDisplay : MonoBehaviour
    {
        [SerializeField] BridgeFloorCell floorPrefab;

        public const int Length = 10;

        BridgeFloorCell[] m_bridge;
        

        public  UniTask Init(Vector3 position, Vector3 forward, bool hideAfterUpdate)
        {
            m_bridge ??= new BridgeFloorCell[Length];
            
            for (var i = 0; i < Length; i++)
            {
                m_bridge[i] ??= Instantiate(floorPrefab, transform);
            }
            
            
            var timeToStartNextCell = 0.0f;
            for (var cellId = 0; cellId < Length; cellId++)
            {
                m_bridge[cellId].Init(position, timeToStartNextCell, forward, hideAfterUpdate);
                position += forward;
                timeToStartNextCell += 0.25f;
            }
            return UniTask.CompletedTask;
        }

        public void Update()
        {
            if (m_bridge == null) return;
            foreach (var floorCell in m_bridge)
            {
                floorCell.Show();
            }
        }
    }
}