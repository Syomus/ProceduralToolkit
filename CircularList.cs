using System.Collections.Generic;

namespace ProceduralToolkit
{
    /// <summary>
    /// List with support of circular indexing
    /// </summary>
    public class CircularList<T> : List<T>
    {
        public new T this[int index]
        {
            get
            {
                if (index < 0)
                {
                    index = index%Count + Count;
                }
                else if (index >= Count)
                {
                    index %= Count;
                }
                return base[index];
            }
            set
            {
                if (index < 0)
                {
                    index = index%Count + Count;
                }
                else if (index >= Count)
                {
                    index %= Count;
                }
                base[index] = value;
            }
        }

        public CircularList() : base()
        {
        }

        public CircularList(IEnumerable<T> collection) : base(collection)
        {
        }

        public CircularList(int capacity) : base(capacity)
        {
        }
    }
}