using dotSpace.BaseClasses.Space;

namespace dotSpace.Objects.Space
{
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