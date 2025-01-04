using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bridge;
using Objects;
using UnityEngine;

namespace Level
{
    public class Corridor : MonoBehaviour
    {
        public GameObject corridorFloorPrefab;
        List<MutagenFloor> m_corridorFloor;

        async void Start()
        {
            try
            {
                await CreateCorridorFloor();
            }
            catch (Exception e)
            {
                throw; // TODO handle exception
            }
        }

        Task CreateCorridorFloor()
        {
            m_corridorFloor = new List<MutagenFloor>();
            for (var i = 0; i < BridgeDisplay.Length - 1; i++)
            {
                var corridor = Instantiate(corridorFloorPrefab, Vector3.one * 100000, Quaternion.identity, transform);
                m_corridorFloor.Add(corridor.GetComponentInChildren<MutagenFloor>());
            }

            return Task.CompletedTask;
        }

        public async Task ShowCorridor(Vector3 exitDoorPosition, Vector3 exitDoorForward)
        {
            var position = exitDoorPosition + Vector3.down * 3f + exitDoorForward;
            var target = position + Vector3.up * 2.5f;
            var startTime = 0.1f;
            Enable();
            foreach (var floorCell in m_corridorFloor)
            {
                floorCell.Init(position, target, startTime);
                position += exitDoorForward;
                target += exitDoorForward;
                startTime += 0.1f;
            }

            while (!m_corridorFloor.All(floor => floor.IsMaterialize()))
            {
                await Task.Yield();
            }
        }

        public async Task HideCorridor()
        {
            var startTime = 0.1f;
            foreach (var floorCell in m_corridorFloor)
            {
                floorCell.Init(floorCell.transform.position, floorCell.transform.position + Vector3.down * 2.5f, startTime);
                startTime += 0.2f;
            }
            // m_corridorFloor.Reverse();
            while (!m_corridorFloor.All(floor => floor.IsMaterialize()))
            {
                await Task.Delay(100);
            }
        }

        public void Disable()
        {
            foreach (var floorCell in m_corridorFloor)
            {
                floorCell.gameObject.SetActive(false);
            }
        }
        
        void Enable()
        {
            foreach (var floorCell in m_corridorFloor)
            {
                floorCell.gameObject.SetActive(true);
            }
        }
    }
}