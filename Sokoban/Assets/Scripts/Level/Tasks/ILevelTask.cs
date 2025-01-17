using System;

namespace Level.Tasks
{
    public interface ILevelTask
    {
        public event Action OnTaskCompleted;
    }
}