using System;

namespace Data
{
    [Serializable]
    public enum LevelPhase
    {
        Pause,
        SearchSolution,
        SolutionFound,
        Finished,
    }
}