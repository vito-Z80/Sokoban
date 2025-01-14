using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Level
{
    public class LevelConstructorWithRotation : LevelConstructor
    {
        const float BuildTime = 2.0f;

        bool m_start;

        Transform[] m_transforms;
        Quaternion[] m_quaternions;


        void Start()
        {
            SetRotationInDirection(Vector3.forward, 180);
            m_start = true;
        }

        void Update()
        {
            if (!m_start) return;
            m_start = false;
            for (var i = 0; i < m_transforms.Length; i++)
            {
                m_start |= Rotate(i);
            }
        }


        public override async Task DisassembleLevel()
        {
            m_transforms = await GetChildTransforms();
            m_quaternions = m_transforms.Select(t => t.rotation).ToArray();
            var time = BuildTime / m_transforms.Length;
            WaitTime = Enumerable.Range(0, m_transforms.Length).Select(i => i * time).ToArray();
        }

        void SetRotationInDirection(Vector3 axis, float angle)
        {
            foreach (var t in m_transforms)
            {
                t.Rotate(axis, angle);
            }
        }

        bool Rotate(int id)
        {
            if (m_transforms[id].rotation == m_quaternions[id]) return false;

            WaitTime[id] -= Time.deltaTime;
            if (WaitTime[id] > 0.0f) return true;


            m_transforms[id].rotation = Quaternion.RotateTowards(
                m_transforms[id].rotation,
                m_quaternions[id],
                Time.deltaTime * 192.0f
            );
            return true;
        }
    }
}