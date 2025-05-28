using Emojis;

namespace  MyCollections
{
    public class TreePoint<T> where T: IInit, new()
    {
        public T data;
        public TreePoint<T> left, right;

        public TreePoint(T data = default(T))
        {
            this.data = data;
            left = right = null;
        }

        public override string ToString()
        {
            return data.ToString() + " ";
        }
    }
}