using Emojis;
using System.Collections;

namespace MyCollections
{

    public class MyHashTable<T> : ICollection<T> where T : IInit, new()
    {
        public delegate void CollectionHandler(object source, CollectionHandlerEventArgs<T> args);
        public event CollectionHandler? CollectionCountChanged;
        public event CollectionHandler? CollectionReferenceChanged;
        public void OnCollectionCountChange(object source, CollectionHandlerEventArgs<T> args)
        {
            CollectionCountChanged?.Invoke(source, args);
        }

        public void OnCollectionReferenceChange(object source, CollectionHandlerEventArgs<T> args)
        {
            CollectionReferenceChanged?.Invoke(source, args);
        }
        
        public HashTablePoint<T>?[] set;
        public int Length => set.Length;
        private int _count;
        private readonly double _loadFactor;
        public int Count => _count;
        public bool IsReadOnly => throw new NotImplementedException();
        
        public MyHashTable()
        {
            set = new HashTablePoint<T>[10];
            _loadFactor = 0.72;
            _count = 0;
        }
        public MyHashTable(int capacity, double loadFactor)
        {
            if (capacity < 0)
                throw new Exception("Capacity cannot be less than zero.");

            if (loadFactor < 0 || loadFactor > 1)
                throw new Exception("LoadFactor cannot be less than zero or 1.");

            set = new HashTablePoint<T>[capacity];
            _loadFactor = loadFactor;
            _count = 0;
        }
        public MyHashTable(int elemCount)
        {
            if (elemCount < 0)
                throw new Exception("Count of elems cannot be less than zero.");

            set = new HashTablePoint<T>[elemCount];
            _loadFactor = 0.72;
            _count = 0;
            for (int i = 0; i < elemCount; i++)
            {
                T data = new();
                data.RandomInit();
                Add(data);
            }
        }
        public MyHashTable(MyHashTable<T>? otherCollection)
        {
            if (otherCollection == null) 
                throw new Exception("Collection cannot be null.");
            set = new HashTablePoint<T>[otherCollection.Length];
            _loadFactor = 0.72;
            _count = 0;
            foreach (HashTablePoint<T>? item in otherCollection.set)
            {
                if (item != null)
                {
                    Add(item);
                }
            }
        }
        public void Add(HashTablePoint<T>? item)
        {
            if (item != null) Add(item.Data);
        }
        public void Add(T? item)
        {
            if (item == null)
            {
                throw new Exception("Item cannot be null.");
            }
            
            OnCollectionCountChange(this, new CollectionHandlerEventArgs<T>("добавлен", ref item));
            
            if (_count >= Math.Round(_loadFactor * set.Length))
            {
                Resize();
            }

            int index = Math.Abs(item.GetHashCode() % set.Length);
            if (set[index] == null || set[index].IsDeleted)
            {
                set[index] = new HashTablePoint<T>(item);
                OnCollectionCountChange(this, new CollectionHandlerEventArgs<T>("добавлен", ref item));
                _count++;
            }
            else
            {
                for (int i = 1; i < set.Length; i++)
                {
                    int newIndex = (index + i) % set.Length;
                    if (set[newIndex] != null && set[newIndex].Data.Equals(item))
                    {
                        return;
                    }

                    if (set[newIndex] == null || set[newIndex].IsDeleted)
                    {
                        set[newIndex] = new HashTablePoint<T>(item);
                        OnCollectionCountChange(this, new CollectionHandlerEventArgs<T>("добавлен", ref item)); 
                        _count++;
                        return;
                    }
                }
            }

            if (_count >= set.Length * _loadFactor)
            {
                Resize();
            }
        }

        public void AddSomeItems(int countOfElems)
        {
            for (int i = 0; i < countOfElems; i++)
            {
                T data = new();
                data.RandomInit();
                Add(data);
            }
        }
        public void Print()
        {
            for (int i = 0; i < set.Length; i++)
            {
                if (set[i] == null || set[i].IsDeleted)
                {
                    Console.WriteLine($"{i}: empty");
                    continue;
                }

                Console.WriteLine($"{i}: {set[i].Data}");
            }
        }

        public void Clear()
        {
            for (int i = 0; i < set.Length; i++)
            {
                set[i] = null;
            }
        }

        public void Resize()
        {
            HashTablePoint<T>[] newSet = new HashTablePoint<T>[set.Length * 2];
            for (int i = 0; i < set.Length; i++)
            {
                HashTablePoint<T> item = set[i];
                if (item == null || item.IsDeleted) continue;
                int index = Math.Abs(item.Data.GetHashCode()) % newSet.Length;
                if (newSet[index] == null)
                {
                    newSet[index] = new HashTablePoint<T>(item);
                    OnCollectionReferenceChange(this, new CollectionHandlerEventArgs<T>("изменена ссылка на", ref item));
                }
                else
                {
                    for (int j = 0; j < newSet.Length; j++)
                    {
                        int newIndex = (index + j) % newSet.Length;
                        if (newSet[newIndex] == null)
                        {
                            newSet[newIndex] = new HashTablePoint<T>(item);
                            break;
                        }
                    }
                }
            }

            set = newSet;
        }

        public bool Contains(T? item)
        {
            if (item == null) return false;
            int index = Math.Abs(item.GetHashCode()) % set.Length;
            if (set[index] != null)
            {
                if (!set[index].IsDeleted && set[index].Data.Equals(item))
                {
                    return true;
                }
                else
                {
                    for (int i = 0; i < set.Length; i++)
                    {
                        int newIndex = (index + i) % set.Length;
                        if (set[newIndex] != null && !set[newIndex].IsDeleted && set[newIndex].Data.Equals(item))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public T Search(T? item)
        {
            if (item == null) return default(T);
            int index = Math.Abs(item.GetHashCode()) % set.Length;
            if (set[index] != null)
            {
                if (!set[index].IsDeleted && set[index].Data != null && set[index].Data.Equals(item))
                {
                    return set[index].Data;
                }

                for (int i = 0; i < set.Length; i++)
                {
                    int newIndex = (index + i) % set.Length;
                    if (set[newIndex] != null && set[newIndex].Data != null && set[newIndex].Data.Equals(item))
                    {
                        return set[newIndex].Data;
                    }
                }
            }

            return default(T);
        }

        public bool Remove(T? item)
        {
            if (item == null) return false;
            int index = Math.Abs(item.GetHashCode()) % set.Length;
            if (set[index] != null)
            {
                if (!set[index].IsDeleted && set[index].Data != null && set[index].Data.Equals(item))
                {
                    _count--;
                    set[index].IsDeleted = true;
                    OnCollectionCountChange(this, new CollectionHandlerEventArgs<T>("удалён",ref set[index]));
                    return true;
                }

                for (int i = 0; i < set.Length; i++)
                {
                    index = (index + i) % set.Length;
                    if (set[index] == null) return false;
                    if (!set[index].IsDeleted && set[index].Data != null && set[index].Data.Equals(item))
                    {
                        _count--;
                        set[index].IsDeleted = true;
                        OnCollectionCountChange(this, new CollectionHandlerEventArgs<T>("удалён",ref set[index]));
                        return true;
                    }
                }
            }

            return false;
        }

        public object this[int i]
        {
            get
            {
                OnCollectionReferenceChange(this,new CollectionHandlerEventArgs<T>("Изменена ссылка на", ref set[i]));
                return set[i];
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (set == null)
                yield break;

            foreach (HashTablePoint<T> point in set)
                if (point != null && !point.IsDeleted)
                    yield return point.Data;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}