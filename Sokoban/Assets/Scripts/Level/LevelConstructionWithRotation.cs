using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Level
{
    public class LevelConstructionWithRotation : LevelConstruction
    {
        readonly Quaternion m_invisible = Quaternion.Euler(180, 0, 0);
        const float BuildTime = 2.0f;

        bool m_start;


        void Update()
        {
            if (!m_start) return;
            var floorCellCounter = 0;
            for (var i = 0; i < Transforms.Length; i++)
            {
                if (Rotate(i))
                {
                    floorCellCounter++;
                }
            }

            m_start = floorCellCounter > 0;
        }


        public override async Task Initialize()
        {
            Transforms = await GetTransforms();
            BasePositions = new Vector3[Transforms.Length];

            var time = BuildTime / Transforms.Length;
            WaitTime = Enumerable.Range(0, Transforms.Length).Select(i => i * time).ToArray();

            foreach (var t in Transforms)
            {
                t.rotation = m_invisible;
            }

            m_start = true;
        }

        bool Rotate(int id)
        {
            if (Transforms[id].rotation == Quaternion.identity) return false;

            WaitTime[id] -= Time.deltaTime;
            if (WaitTime[id] > 0.0f) return true;


            Transforms[id].rotation = Quaternion.Lerp(
                Transforms[id].rotation,
                Quaternion.identity,
                Time.deltaTime * 4.0f
            );
            return true;
        }
    }
}