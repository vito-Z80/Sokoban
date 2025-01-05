using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Level
{
    public class LevelConstructorWithRotation : LevelConstructor
    {
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


        public override async Task DisassembleLevel()
        {

            // var direction = transform.parent.GetComponent<Level>().enterDoor.transform.forward;
            await SetTransformsRotationInDirection(Vector3.forward, 180);
            // Transforms = await GetChildComponents();
            // BasePositions = new Vector3[Transforms.Length];

            var time = BuildTime / Transforms.Length;
            WaitTime = Enumerable.Range(0, Transforms.Length).Select(i => i * time).ToArray();

            // foreach (var t in Transforms)
            // {
                // t.rotation = m_invisible;
            // }

            m_start = true;
        }

        bool Rotate(int id)
        {
            if (Transforms[id].rotation == Quaternion.identity) return false;

            WaitTime[id] -= Time.deltaTime;
            if (WaitTime[id] > 0.0f) return true;


            Transforms[id].rotation = Quaternion.Lerp(
                Transforms[id].rotation,
                BaseQuaternions[id],
                Time.deltaTime * 4.0f
            );
            return true;
        }
    }
}