using UnityEngine;

namespace Bridge
{
    public class BridgeDisplay : MonoBehaviour
    {
        [SerializeField] BridgeFloorCell floorPrefab;

        const int Length = 10;

        BridgeFloorCell[] m_bridge;


        void Start()
        {
            m_bridge = new BridgeFloorCell[Length];
            for (int i = 0; i < Length; i++)
            {
                m_bridge[i] = Instantiate(floorPrefab, transform);
                
            }
            Init(Vector3.zero, Vector3.forward);
        }

     


        public void Init(Vector3 position, Vector3 forward)
        {
            var timeToStartNextCell = 0.0f;
            for (var floor = 0; floor < Length; floor++)
            {
                m_bridge[floor].Init(position, timeToStartNextCell);
                position += forward;
                timeToStartNextCell += 0.25f;
            }
        }

        public void Update()
        {
            foreach (var floorCell in m_bridge)
            {
                floorCell?.Show();
            }
        }
    }
}