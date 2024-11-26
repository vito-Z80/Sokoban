using System.Linq;
using Objects;
using UnityEngine;

namespace LevelEditor
{
    public class LevelDisassembler : MonoBehaviour
    {
        [SerializeField] GameObject corridor;


        SaveLevelData m_saveLevelData;

        void Start()
        {
            m_saveLevelData = new SaveLevelData
            {
                enterCorridor = Sort<GameObject>(corridor)
            };
        }


        T[] Sort<T>(GameObject o)
        {
            var r = o.GetComponentsInChildren<T>()
                .OrderBy(arg => (arg as Transform)!.position.y)
                .ThenBy(arg => (arg as Transform)!.position.x).ToArray();
            return r;
        }
    }
}