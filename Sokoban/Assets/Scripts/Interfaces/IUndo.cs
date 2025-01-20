using System.Collections.Generic;
namespace Interfaces
{
    public interface IUndo
    {
        List<BackStepTransform> Stack { get; }

        public void Push();
        public void Pop();
    }
}