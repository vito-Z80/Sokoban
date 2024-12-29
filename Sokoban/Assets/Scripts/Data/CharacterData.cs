using System;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class CharacterData
    {
        
        
        [Header("Immutable Variables")]
        public Vector3 characterInMenuPositionOffset;
        public Vector3 characterInMenuRotationOffset;
    }
}