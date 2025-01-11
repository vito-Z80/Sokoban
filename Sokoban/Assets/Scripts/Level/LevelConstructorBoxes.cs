using System.Linq;
using System.Threading.Tasks;
using Objects.Boxes;
using UnityEngine;

namespace Level
{
    public class LevelConstructorBoxes : LevelConstructor
    {
        Box[] m_boxes;
        bool m_start;


        void Start()
        {
            for (var i = 0; i < m_boxes.Length; i++)
            {
                m_boxes[i].targetPosition = BasePositions[i];
            }
        }

        void Update()
        {
            if (!m_start || m_boxes == null) return;
            var deltaTime = Time.deltaTime * 8.0f;
            m_start = false;

            for (var i = 0; i < m_boxes.Length; i++)
            {
                m_start |= m_boxes[i].Move(deltaTime);
            }

            // if (!m_start)
            // {
            //     foreach (var box in m_boxes)
            //     {
            //         box.SetPointContact();
            //     }
            // }
        }


        public override async Task DisassembleLevel()
        {
            await SetTransformsByDirection(Vector3.up, 10.0f);
            m_boxes = Transforms.Select(b => b.GetComponent<Box>()).ToArray();
            for (var i = 0; i < m_boxes.Length; i++)
            {
                m_boxes[i].targetPosition = BasePositions[i];
            }

            m_start = true;
        }
    }
}