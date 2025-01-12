using System.Linq;
using System.Threading.Tasks;
using Objects.Boxes;
using UnityEngine;

namespace Level
{
    public abstract class LevelConstructor : MonoBehaviour
    {
        // protected Transform[] Transforms;
        // protected Vector3[] BasePositions;
        // protected Quaternion[] BaseQuaternions;
        protected float[] WaitTime;


        public abstract Task DisassembleLevel();


        


        protected Task<Transform[]> GetChildComponents()
        {
            return Task.FromResult(transform.GetComponentsInChildren<Transform>()
                .Where(t => t != transform)
                .OrderBy(t => t.position.y)
                .ToArray());
        }
    }
}