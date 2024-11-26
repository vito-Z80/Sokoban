using System;
using Objects;
using UnityEngine;

namespace LevelEditor
{
    [Serializable]
    public class SaveLevelData
    {
        public GameObject[] walls;
        public GameObject[] floor;
        public GameObject[] enterCorridor;
        public GameObject[] exitCorridor;
        public GameObject[] boxes;
        public GameObject[] points;
    }
}