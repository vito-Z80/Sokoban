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
            SetPositionByDirection(m_boxes, Vector3.up, 10.0f);
            m_start = true;
        }

        void Update()
        {
            if (!m_start || m_boxes == null) return;
            var deltaTime = Time.deltaTime * 8.0f;
            m_start = false;

            foreach (var box in m_boxes)
            {
                // m_start |= box.Move(deltaTime);
            }
        }


        public override async Task DisassembleLevel()
        {
            var transforms = await GetFirstLevelChildrenTransforms();
            m_boxes = transforms.Select(t => t.GetComponent<Box>())
                .Where(box => box != null)
                .ToArray();
        }

        void SetPositionByDirection(Box[] boxes, Vector3 direction, float distance)
        {
            foreach (var box in boxes)
            {
                // box.targetPosition = box.transform.position;
                // box.transform.position += transform.TransformDirection(direction) * distance;
            }
        }
    }
}