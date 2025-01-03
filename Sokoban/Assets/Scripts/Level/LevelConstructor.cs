using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Level
{
    public abstract class LevelConstructor : MonoBehaviour
    {
        protected Transform[] Transforms;
        protected Vector3[] BasePositions;
        protected Quaternion[] BaseQuaternions;
        protected float[] WaitTime;


        public abstract Task DisassembleLevel();

        public void Move()
        {
            foreach (var t in Transforms)
            {
                t.rotation = Quaternion.Lerp(t.rotation, Quaternion.identity, Time.deltaTime * 4.0f);
                t.position = Vector3.Lerp(t.position, t.position + t.rotation * Vector3.forward, Time.deltaTime);
            }
        }


        protected bool IsAnimationFinished()
        {
            for (var i = 0; i < Transforms.Length; i++)
            {
                if (Vector3.Distance(Transforms[i].position, BasePositions[i]) <= 0.005f)
                {
                    Transforms[i].position = BasePositions[i];
                }
                else
                {
                    return false;
                }
            }

            return true;
        }


        protected async Task SetTransformsByDirection(Vector3 direction, float distance)
        {
            Transforms = await GetChildComponents();
            BasePositions = new Vector3[Transforms.Length];

            // var time = BuildTime / Transforms.Length;
            // WaitTime = Enumerable.Range(0, Transforms.Length).Select(i => i * time).ToArray();

            for (var i = 0; i < Transforms.Length; i++)
            {
                var pos = Transforms[i].position;
                BasePositions[i] = pos;
                Transforms[i].position = pos + Transforms[i].TransformDirection(direction) * distance;
            }
        }

        protected async Task SetTransformsRotationInDirection(Vector3 axis, float angle)
        {
            Transforms = await GetChildComponents();
            BaseQuaternions = Transforms.Select(t => t.rotation).ToArray();
            
            foreach (var t in Transforms)
            {
                t.Rotate(axis, angle);
            }
        }


        Task<Transform[]> GetChildComponents()
        {
            return Task.FromResult(transform.GetComponentsInChildren<Transform>()
                .Where(t => t != transform)
                .OrderBy(t => t.position.y)
                .ToArray());
        }
    }
}