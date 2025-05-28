using Emojis;
using Emoji = Emojis.Emoji;

namespace MyCollections.List
{
    public class MyList<T> where T : IInit, new()
    {
        private ListPoint<T> _begin;
        private ListPoint<T> _end;

        public int Count
        {
            get
            {
                int count = 0;
                if (_begin == null) return count;
                ListPoint<T> current = _begin;
                while (current != null)
                {
                    count++;
                    current = current.Next;
                }

                return count;
            }
        }

        public MyList()
        {
            _begin = null;
            _end = null;
        }

        public MyList(int length)
        {
            for (int i = 0; i < length; i++)
            {
                T data = new();
                data.RandomInit();
                ListPoint<T> item = new(data);
                AddToBegin(item);
            }
        }

        public void AddToEnd(ListPoint<T> item)
        {
            if (_begin == null)
            {
                AddToBegin(item);
                return;
            }
            _end.Next = item;
            item.Previous = _end;
            item.Next = null;
            _end = item;
        }
        public void AddToBegin(ListPoint<T> item)
        {
            if (_begin == null)
            {
                _begin = item;
                _begin.Previous = null;
                _begin.Next = null;
                _end = _begin;
                return;
            }
            item.Next = _begin;
            item.Previous = null;
            _begin = item;
            if (_begin.Next != null)
            {
                _begin.Next.Previous = _begin;
            }
        }
        public void Add(T item)
        {
            ListPoint<T> newListPoint = new(item);
            if (_begin == null)
            {
                _begin = newListPoint;
                _end = _begin;   
            }
            else AddToEnd(newListPoint);
        }
        public void Add(int count)
        {
            if (count < 0) throw new ArgumentException("Count cannot be negative");
            if (count == 0) return;
            if (count == 1)
            {
                T data = new();
                data.RandomInit();
                AddToBegin(new ListPoint<T>(data));
                return;
            }
            for (int i = 0; i < count; i++)
            {
                T data = new();
                data.RandomInit();
                AddToBegin(new ListPoint<T>(data));
            }

        }
        
        public void Clear()
        {
            _begin = null;
            _end = null;
        }
        public bool Contains(T item)
        {
            ListPoint<T> current = _begin;
            while (current != null && !current.Data.Equals(item))
                current = current.Next;
            return current != null;
        }
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                throw new Exception("Массив не может быть null");
            if (arrayIndex < 0 || arrayIndex >= array.Length)
                throw new Exception("Индекс массива выходит за границы");
            if (array.Length - arrayIndex < Count)
                throw new Exception("Список не помещается в массив");

            ListPoint<T> current = _begin;
            int i = arrayIndex;
            while (current != null && i < array.Length)
            {
                array[i++] = current.Data;
                current = current.Next;
            }
        }

        public bool Remove(T item)
        {
            if (_begin.Next == null)
            {
                Clear();
                return true;
            }
            if (_begin.Data.Equals(item))
            {
                _begin = _begin.Next;
                _begin.Previous = null;
                GC.Collect();
                return true;
            }
            if (_end.Data.Equals(item))
            {
                _end = _end.Previous;
                _end.Next.Previous = null;
                _end.Next = null;
                GC.Collect();
                return true;
            }
            
            ListPoint<T> current = _begin;
            while (current.Next != null && !current.Next.Data.Equals(item))
                current = current.Next;
            if (current.Next == null)
                return false;
            
            current.Next = current.Next.Next;
            if (current.Next != null)
            {
                current.Next.Previous = current;
            }

            return true;
        }

        public void PrintList()
        {
            ListPoint<T> current = _begin;
            int count = 1;
            while (current != null)
            {
                Console.WriteLine($"{count}: {current.Data}");
                current = current.Next;
                count++;
            }
        }

        public object Clone()
        {
            MyList<T> newMyList = new();
            ListPoint<T> current = _begin;
            if (_begin == null) return null;
            newMyList._begin = new ListPoint<T>(current.Data);
            newMyList._end = newMyList._begin;
            current = current.Next;
            while (current != null)
            {
                ListPoint<T> newListPoint = new(current.Data);
                newMyList.AddToEnd(newListPoint);
                current = current.Next;
            }

            return newMyList;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj is MyList<T> other)
            {
                if (other.Count != Count) return false;
                bool result = true;
                ListPoint<T> current = _begin;
                ListPoint<T> otherCurrent = other._begin;
                while (current != null)
                {
                    result = current.Equals(otherCurrent);
                    current = current.Next;
                    otherCurrent = otherCurrent.Next;
                    if (!result) return false;
                }
                if (result) return true;
            }
            if (obj is Emoji[] other2)
            {
                if (other2.Length != Count) return false;
                bool result = true;
                ListPoint<T> current = _begin;
                foreach (Emoji emoji in  other2)
                {
                    result = current.Data.Equals(emoji);
                    if (!result) return false;
                    current = current.Next;

                }
                if (result) return true;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return _begin.GetHashCode() ^ _end.GetHashCode();
        }
    }
}