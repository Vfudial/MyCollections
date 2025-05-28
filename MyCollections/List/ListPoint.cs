using Emojis;

namespace MyCollections.List
{
    public class ListPoint<T>
    {
        public T? Data {get; set;}
        public ListPoint<T> Next {get; set;}
        public ListPoint<T> Previous {get; set;}
        public ListPoint()
        {
            Data = default;
            Previous = null;
            Next = null;
        }
        public ListPoint(T? data)
        {
            Data = data;
            Previous = null;
            Next = null;
        }
        public override string ToString()
        {
            return Data?.ToString() ?? "null";
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is ListPoint<T> other)
            {
                return Data.Equals(other.Data);
            }
            if (obj is Emoji[])
            {
                return Data.Equals(obj);
            }
            return false;
        }

        protected bool Equals(ListPoint<T> other)
        {
            return EqualityComparer<T>.Default.Equals(Data, other.Data) && Equals(Next, other.Next) && Equals(Previous, other.Previous);
        }

        public override int GetHashCode()
        {
            return Data.GetHashCode();
        }
    }
}