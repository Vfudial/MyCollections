using System.Drawing;
using Emojis;

namespace MyCollections;

public class CollectionHandlerEventArgs<T> : EventArgs where T : IInit, new()
{
        public string Type { get; set; }
        public T Item { get; set; }

        public CollectionHandlerEventArgs(string type, ref HashTablePoint<T> item)
        {
                Type = type;
                Item = item.Data;
        }
        public CollectionHandlerEventArgs(string type, ref T item)
        {
                Type = type;
                Item = item;
        }
}