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

        void Update()
        {
            if (!m_start) return;


            for (var i = 0; i < Transforms.Length; i++)
            {
                var time = WaitTime[i] -= Time.deltaTime;
                if (time > 0.0f) continue;
                Transforms[i].position = Vector3.Lerp(Transforms[i].position, BasePositions[i], Time.deltaTime * InterpolateLerpTime);
            }

            if (IsAnimationFinished()) m_start = false;
        }


        public override async Task DisassembleLevel()
        {

            await SetTransformsByDirection(Vector3.back, 20.0f);
            
            // Transforms = await GetChildComponents();
            // BasePositions = new Vector3[Transforms.Length];

            var time = BuildTime / Transforms.Length;
            WaitTime = Enumerable.Range(0, Transforms.Length).Select(i => i * time).ToArray();

            // for (var i = 0; i < Transforms.Length; i++)
            // {
            //     var pos = Transforms[i].position;
            //     BasePositions[i] = pos;
            //     Transforms[i].position = pos - Transforms[i].forward * Distancing;
            // }

            m_start = true;
        }
    }
}