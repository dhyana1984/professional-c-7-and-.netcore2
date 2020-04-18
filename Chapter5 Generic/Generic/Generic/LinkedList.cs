using System;
using System.Collections;

namespace Generic
{
    public class LinkedListNode
    {
        public LinkedListNode(object value) => Value = value;
        public object Value { get;}
        public LinkedListNode Next { get; internal set; }
        public LinkedListNode Prev { get; internal set; }
    }

    public class LinkedList:IEnumerable
    {
        public LinkedListNode First { get; private set; }
        public LinkedListNode Last { get; private set; }
        public LinkedListNode AddLast(LinkedListNode node)
        {
            var newNode = new LinkedListNode(node);
            if(First == null)
            {
                First = newNode;
                Last = First;
            }
            else
            {
                var previous = Last;
                Last.Next = newNode;
                Last = newNode;
                Last.Prev = previous;
            }
            return newNode;
        }

        public IEnumerator GetEnumerator()
        {
            LinkedListNode current = First;
            while (current != null)
            {
                yield return current.Value;
                current = current.Next;
            }
        }

    }
}
