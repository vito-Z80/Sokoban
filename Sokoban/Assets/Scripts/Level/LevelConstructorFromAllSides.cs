using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Level
{
    public class LevelConstructorFromAllSides : LevelConstructor
    {
        const float Distancing = 20.0f;
        const float BuildTime = 1.0f;
        const float InterpolateLerpTime = 4.0f;

        bool m_start;

        Transform[] m_transforms;
        Vector3[] m_basePositions;


        void Start()
        {
            SetPositionByDirection( Vector3.back, Distancing);
            m_start = true;
        }

        void Update()
        {
            if (!m_start) return;
            m_start = false;
            for (var i = 0; i < m_transforms.Length; i++)
            {
                m_start |= Vector3.Distance(m_transforms[i].position, m_basePositions[i]) > 0.01f;
                var time = WaitTime[i] -= Time.deltaTime;
                if (time > 0.0f) continue;
                m_transforms[i].position = Vector3.Lerp(m_transforms[i].position, m_basePositions[i], Time.deltaTime * InterpolateLerpTime);
            }

        }


        public override async Task DisassembleLevel()
        {
            m_transforms = await GetFirstLevelChildrenTransforms();
            var time = BuildTime / m_transforms.Length;
            WaitTime = Enumerable.Range(0, m_transforms.Length).Select(i => i * time).ToArray();
        }

        void SetPositionByDirection( Vector3 direction, float distance)
        {
            m_basePositions = new Vector3[m_transforms.Length];
            for (var i = 0; i < m_transforms.Length; i++)
            {
                var pos = m_transforms[i].position;
                m_basePositions[i] = pos;
                m_transforms[i].position = pos + m_transforms[i].TransformDirection(direction) * distance;
            }
        }
    }
}