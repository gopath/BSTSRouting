using System;
using System.Collections;

namespace BSTSRouting
{
    public class NodeList : IEnumerable
    {
        private Hashtable data = new Hashtable();

        #region Public Methods
        
        public virtual void Add(Node n)
        {
            data.Add(n.Key, n);
        }

        public virtual void Remove(Node n)
        {
            data.Remove(n.Key);
        }

        public virtual bool ContainsKey(string key)
        {
            return data.ContainsKey(key);
        }

        public virtual void Clear()
        {
            data.Clear();
        }

        public IEnumerator GetEnumerator()
        {
            return new NodeListEnumerator(data.GetEnumerator());
        }
        #endregion

        #region Public Properties
        public virtual Node this[string key]
        {
            get
            {
                return (Node)data[key];
            }
        }

        public virtual int Count
        {
            get
            {
                return data.Count;
            }
        }
        #endregion

        #region NodeList Enumerator
        
        public class NodeListEnumerator : IEnumerator, IDisposable
        {
            IDictionaryEnumerator list;
            public NodeListEnumerator(IDictionaryEnumerator coll)
            {
                list = coll;
            }

            public void Reset()
            {
                list.Reset();
            }

            public bool MoveNext()
            {
                return list.MoveNext();
            }

            public Node Current
            {
                get
                {
                    return (Node)((DictionaryEntry)list.Current).Value;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return (Current);
                }
            }

            public void Dispose()
            {
                list = null;
            }
        }
        #endregion
    }
}