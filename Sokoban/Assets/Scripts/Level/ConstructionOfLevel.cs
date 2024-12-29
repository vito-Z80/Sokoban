using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Level
{
    public abstract class ConstructionOfLevel : MonoBehaviour
    {
        protected Transform[] Transforms;
        protected Vector3[] BasePositions;
        protected float[] WaitTime;


        public abstract Task Initialize();

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

        protected Task<Transform[]> GetTransforms()
        {
            return Task.FromResult(gameObject.GetComponentsInChildren<Transform>()
                .Where(r => !r.TryGetComponent<ConstructionOfLevel>(out _))
                .OrderBy(t => t.position.y)
                .ToArray());
        }
    }
}