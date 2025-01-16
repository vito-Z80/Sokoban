using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Level
{
    public abstract class LevelConstructor : MonoBehaviour
    {
        protected float[] WaitTime;
        public abstract Task DisassembleLevel();
        
        protected Task<Transform[]> GetFirstLevelChildrenTransforms()
        {
            var childCount = transform.childCount;
            var firstLevelChildren = new Transform[childCount];

            for (var i = 0; i < childCount; i++)
            {
                firstLevelChildren[i] = transform.GetChild(i);
            }

            return Task.FromResult(firstLevelChildren
                .OrderBy(t => t.position.y)
                .ToArray());
        }
    }
}