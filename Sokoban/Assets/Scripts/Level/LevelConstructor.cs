using System.Linq;
using System.Threading.Tasks;
using Objects.Boxes;
using UnityEngine;

namespace Level
{
    public abstract class LevelConstructor : MonoBehaviour
    {
        protected float[] WaitTime;
        public abstract Task DisassembleLevel();

        protected Task<Transform[]> GetChildTransforms()
        {
            return Task.FromResult(transform.GetComponentsInChildren<Transform>()
                .Where(t => t != transform)
                .OrderBy(t => t.position.y)
                .ToArray());
        }
    }
}