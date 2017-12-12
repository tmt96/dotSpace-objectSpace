using dotSpace.BaseClasses.Space;

namespace dotSpace.Objects.Space
{
    public class SequentialObjectSpaceSimple : ObjectSpaceBaseSimple
    {
        public SequentialObjectSpaceSimple()
        {
        }

        protected override int GetIndex(int count)
        {
            return count;
        }
    }
}
