using dotSpace.BaseClasses.Space;

namespace dotSpace.Objects.Space
{
    /// <summary>
    /// Concrete implementation of a ObjectSpace datastructure.
    /// This class imposes fifo ordering on the underlying objects.
    /// </summary>
    public class SequentialObjectSpace : ObjectSpaceBase
    {
        public SequentialObjectSpace()
        {
        }

        protected override int GetIndex(int count)
        {
            return count;
        }
    }
}