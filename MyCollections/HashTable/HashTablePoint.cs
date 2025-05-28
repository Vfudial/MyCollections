namespace MyCollections
{
    public class HashTablePoint<T>
    {
        public T? Data {get; set;}
        public bool IsDeleted {get; set;}

        public HashTablePoint()
        {
            Data = default(T);
            IsDeleted = false;
        }
        public HashTablePoint(T data)
        {
            Data = data;
            IsDeleted = false;
        }
        public HashTablePoint(HashTablePoint<T> other)
        {
            Data = other.Data;
            IsDeleted = other.IsDeleted;
        }
        public override string ToString()
        {
            return Data?.ToString()??"NULL";
        }
        
    }
}