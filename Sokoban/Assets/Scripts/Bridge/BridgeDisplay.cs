using System.Threading.Tasks;
using UnityEngine;

namespace Bridge
{
    public class BridgeDisplay : MonoBehaviour
    {
        [SerializeField] BridgeFloorCell floorPrefab;

        public const int Length = 10;

        BridgeFloorCell[] m_bridge;
        

        public  Task Init(Vector3 position, Vector3 forward)
        {
            m_bridge ??= new BridgeFloorCell[Length];
            
            for (var i = 0; i < Length; i++)
            {
                m_bridge[i] ??= Instantiate(floorPrefab, transform);
            }
            
            
            var timeToStartNextCell = 0.0f;
            for (var cellId = 0; cellId < Length; cellId++)
            {
                m_bridge[cellId].Init(position, timeToStartNextCell, forward);
                position += forward;
                timeToStartNextCell += 0.25f;
            }
            return Task.CompletedTask;
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