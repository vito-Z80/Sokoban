using System;
using UnityEngine;

namespace Level.Tasks
{
    public class TaskSequence : MonoBehaviour, ILevelTask
    {
        public event Action OnTaskCompleted;
        
        
    }
}