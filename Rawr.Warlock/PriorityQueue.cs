using System;
using System.Collections.Generic;

namespace Rawr.Warlock
{
    /// <summary>
    /// This class is a modified version of Sven de Groot's generic PriorityQueue data structure for .NET.
    /// source: http://www.ookii.org/post/priority_queue_for_net.aspx 
    /// 
    /// The original implementation is (by default) a min-heap queue (i.e. lowest priority item is removed first). 
    /// It can also be used as a max-heap queue (highest priority item is removed first).
    /// However, when elements had the same priority, it did not guarentee any particular order for their removal.
    /// 
    /// This version modifies the DeQueue() method to ensure that elements with the same priority are removed as FIFO (first-in-first-out) order.
    /// </summary>
    /// <typeparam name="T">The type of elements in the priority queue.</typeparam>
    /// <remarks>
    /// <para>
    ///   The items must be immutable as long as they are in the <see cref="PriorityQueue{T}"/>. The only exception is the first
    ///   item, which you may modify if you call <see cref="AdjustFirstItem"/> immediately afterward.
    /// </para>
    /// <para>
    ///   The <see cref="PriorityQueue{T}"/> always places the element with the lowest value at the
    ///   front. If you wish to have the element with the highest value at the front, use a constructor
    ///   that takes a <see cref="IComparer{T}"/>, and pass an instance of the <see cref="InvertedComparer{T}"/> class.
    /// </para>
    /// </remarks>
    /// <threadsafety static="true" instance="false" />
    public sealed class PriorityQueue<T> : IEnumerable<T>, System.Collections.ICollection
    {
        private readonly List<T> _heap; // List that stores the binary heap tree.
        private object _syncRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityQueue{T}"/> class that is empty, 
        /// has the default initial capacity, and uses the default <see cref="IComparer{T}"/>
        /// implementation for the element type.
        /// </summary>
        /// <remarks>
        /// <para>
        ///   <see cref="PriorityQueue{T}"/> requires a comparer implementation to perform key
        ///   comparisons. This constructor uses the default generic equality comparer, 
        ///   <see cref="System.Collections.Generic.Comparer{T}.Default"/>. If type <typeparamref name="T"/>
        ///   implements the <see cref="IComparable{T}"/> generic interface, the default comparer
        ///   uses that implementation. Alternatively, you can specify an implementation of the
        ///   <see cref="IComparer{T}"/> generic interface by using a constructor that accepts a
        ///   <em>comparer</em> parameter.
        /// </para>
        /// <para>
        ///   This constructor is an O(1) operation.
        /// </para>
        /// </remarks>
        public PriorityQueue()
            : this((IComparer<T>)null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityQueue{T}"/> that contains elements
        /// copied from the specified <see cref="IEnumerable{T}"/> and that uses the specified 
        /// <see cref="IComparer{T}"/> implementation to compare keys.
        /// </summary>
        /// <param name="collection">The <see cref="IEnumerable{T}"/> whose elements are copied to 
        /// the new <see cref="PriorityQueue{T}"/>.</param>
        /// <param name="comparer">The <see cref="IComparer{T}"/> implementation to use when 
        /// comparing elements, or <see langword="null"/> to use the default <see cref="System.Collections.Generic.Comparer{T}"/>
        /// for the type of element.</param>
        /// <remarks>
        /// <para>
        ///   <see cref="PriorityQueue{T}"/> requires a comparer implementation to perform key
        ///   comparisons. If comparer is <see langword="null"/>, this constructor uses the default
        ///   generic equality comparer, <see cref="System.Collections.Generic.Comparer{T}.Default"/>. If type <typeparamref name="T"/>
        ///   implements the <see cref="IComparable{T}"/> generic interface, the default comparer
        ///   uses that implementation.
        /// </para>
        /// <para>
        ///   This constructor is an O(<em>n</em>) operation, where <em>n</em> is the number of elements
        ///   in <paramref name="collection"/>.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
        public PriorityQueue(IEnumerable<T> collection, IComparer<T> comparer)
            : this(null, comparer)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            _heap = new List<T>(collection);

            if (_heap.Count > 1)
            {
                // Starting at the parent of the last element (which is the last non-leaf node in the tree), perform the
                // down-heap operation to establish the heap property. This provides O(n) initialization, faster than calling
                // Enqueue for each item which would be O(n log n)
                for (int index = (_heap.Count - 1) >> 1; index >= 0; --index)
                {
                    DownHeap(index);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityQueue{T}"/> that contains elements copied from the specified <see cref="IEnumerable{T}"/>
        /// and uses the default <see cref="IComparer{T}"/> implementation for the element type.
        /// </summary>
        /// <param name="collection">The <see cref="IEnumerable{T}"/> whose elements are copied into the <see cref="PriorityQueue{T}"/>.</param>
        /// <remarks>
        /// <para>
        ///   <see cref="PriorityQueue{T}"/> requires a comparer implementation to perform key
        ///   comparisons. This constructor uses the default generic equality comparer, 
        ///   <see cref="System.Collections.Generic.Comparer{T}.Default"/>. If type <typeparamref name="T"/>
        ///   implements the <see cref="IComparable{T}"/> generic interface, the default comparer
        ///   uses that implementation. Alternatively, you can specify an implementation of the
        ///   <see cref="IComparer{T}"/> generic interface by using a constructor that accepts a
        ///   <em>comparer</em> parameter.
        /// </para>
        /// <para>
        ///   This constructor is an O(<em>n</em>) operation, where <em>n</em> is the number of elements
        ///   in <paramref name="collection"/>.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
        public PriorityQueue(IEnumerable<T> collection)
            : this(collection, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityQueue{T}"/> class that is empty, 
        /// has the specified initial capacity, and uses the specified <see cref="IComparer{T}"/>
        /// implementation to compare elements.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="PriorityQueue{T}"/> can contain.</param>
        /// <param name="comparer">The <see cref="IComparer{T}"/> implementation to use when 
        /// comparing elements, or <see langword="null"/> to use the default <see cref="System.Collections.Generic.Comparer{T}"/>
        /// for the type of element.</param>
        /// <remarks>
        /// <para>
        ///   <see cref="PriorityQueue{T}"/> requires a comparer implementation to perform key
        ///   comparisons. If comparer is <see langword="null"/>, this constructor uses the default
        ///   generic equality comparer, <see cref="System.Collections.Generic.Comparer{T}.Default"/>. If type <typeparamref name="T"/>
        ///   implements the <see cref="IComparable{T}"/> generic interface, the default comparer
        ///   uses that implementation.
        /// </para>
        /// <para>
        ///   This constructor is an O(<em>n</em>) operation, where <em>n</em> is <paramref name="capacity"/>.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than 0.</exception>
        public PriorityQueue(int capacity, IComparer<T> comparer)
            : this(new List<T>(capacity), comparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityQueue{T}"/> class that is empty, has the default initial capacity, and uses the
        /// specified <see cref="IComparer{T}"/> implementation to compare elements.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}"/> implementation to use when 
        /// comparing elements, or <see langword="null"/> to use the default <see cref="System.Collections.Generic.Comparer{T}"/>
        /// for the type of element.</param>
        /// <remarks>
        /// <para>
        ///   <see cref="PriorityQueue{T}"/> requires a comparer implementation to perform key
        ///   comparisons. If comparer is <see langword="null"/>, this constructor uses the default
        ///   generic equality comparer, <see cref="System.Collections.Generic.Comparer{T}.Default"/>. If type <typeparamref name="T"/>
        ///   implements the <see cref="IComparable{T}"/> generic interface, the default comparer
        ///   uses that implementation.
        /// </para>
        /// <para>
        ///   This constructor is an O(1) operation.
        /// </para>
        /// </remarks>
        public PriorityQueue(IComparer<T> comparer)
            : this(new List<T>(), comparer)
        {
        }

        private PriorityQueue(List<T> heap, IComparer<T> comparer)
        {
            Comparer = comparer ?? Comparer<T>.Default;
            _heap = heap;
        }

        /// <summary>
        /// Gets the <see cref="IComparer{T}"/> that is used to compare the elements of the
        /// priority queue.
        /// </summary>
        /// <value>
        /// The <see cref="IComparer{T}"/> generic interface implementation that is used to compare the elements of the
        /// priority queue.
        /// </value>
        /// <remarks>
        /// <para>
        ///   Getting the value of this property is an O(1) operation.
        /// </para>
        /// </remarks>
        public IComparer<T> Comparer { get; private set; }

        /// <summary>
        /// Gets or sets the total number of elements the internal data structure can hold without resizing.
        /// </summary>
        /// <value>
        /// The number of elements that the <see cref="PriorityQueue{T}"/> can contain before resizing is required.
        /// </value>
        /// <remarks>
        /// <para>
        ///   <see cref="Capacity"/> is the number of elements that the <see cref="PriorityQueue{T}"/> can store before 
        ///   resizing is required, while <see cref="Count"/> is the number of elements that are actually in the <see cref="PriorityQueue{T}"/>.
        /// </para>
        /// <para>
        ///   <see cref="Capacity"/> is always greater than or equal to <see cref="Count"/>. If <see cref="Count"/> exceeds
        ///   <see cref="Capacity"/> while adding elements, the capacity is increased by automatically reallocating the
        ///   internal array before copying the old elements and adding the new elements. 
        /// </para>
        /// <para>
        ///   The capacity can be decreased by setting the <see cref="Capacity"/> property explicitly. When the value of
        ///   <see cref="Capacity"/> is set explicitly, the internal array is also reallocated to accommodate the
        ///   specified capacity, and all the elements are copied. 
        /// </para>
        /// <para>
        ///   Retrieving the value of this property is an O(1) operation; setting the property is an O(<em>n</em>) operation, where <em>n</em> is the new capacity. 
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException"><see cref="Capacity"/> is set to a value that is less than count.</exception>
        public int Capacity
        {
            get { return _heap.Capacity; }
            set { _heap.Capacity = value; }
        }

        /// <summary>
        /// Adds an object to the <see cref="PriorityQueue{T}"/>.
        /// </summary>
        /// <param name="item">The object to add to the queue. The value can be <see langword="null"/> for reference types.</param>
        /// <remarks>
        /// <para>
        ///   <see cref="PriorityQueue{T}"/> accepts <see langword="null"/> as a valid value for reference types and allows duplicate elements.
        /// </para>
        /// <para>
        ///   The new element's position is determined by the <see cref="IComparable{T}"/> implementation used to compare elements.
        ///   If the new element is smaller than the current first element in the <see cref="PriorityQueue{T}"/>, the new element
        ///   will become the first element in the queue. Otherwise, the existing first element will remain the first element.
        /// </para>
        /// <para>
        ///   If <see cref="Count"/> already equals <see cref="Capacity"/>, the capacity of the <see cref="PriorityQueue{T}"/> is
        ///   increased by automatically reallocating the internal array, and the existing elements are copied to the new array
        ///   before the new element is added. 
        /// </para>
        /// <para>
        ///   If <see cref="Count"/> is less than <see cref="Capacity"/>, this method is an O(log <em>n</em>) operation, where <em>n</em> is <see cref="Count"/>.
        ///   If the capacity needs to be increased to accommodate the new element, this method becomes an O(<em>n</em>) operation, where <em>n</em> is <see cref="Count"/>.
        /// </para>
        /// </remarks>
        public void Enqueue(T item)
        {
            _heap.Add(item);
            UpHeap();
        }

        /// <summary>
        /// Removes and returns the element with the lowest value from the <see cref="PriorityQueue{T}"/>.
        /// </summary>
        /// <returns>The object with the lowest value that was removed from the <see cref="PriorityQueue{T}"/>.</returns>
        /// <remarks>
        /// <para>
        ///   This method is similar to the <see cref="Peek"/> method, but <see cref="Peek"/> does not modify the <see cref="PriorityQueue{T}"/>. 
        /// </para>
        /// <para>
        ///   If type <typeparamref name="T"/> is a reference type, <see langword="null"/> can be added to the <see cref="PriorityQueue{T}"/> as a value. 
        /// </para>
        /// <para>
        ///   This method is an O(log <em>n</em>) operation, where <em>n</em> is <see cref="Count"/>.
        /// </para>
        /// </remarks>
        /// <exception cref="InvalidOperationException">The <see cref="PriorityQueue{T}"/> is empty.</exception>
        public T Dequeue()
        {
            if (_heap.Count == 0)
                throw new InvalidOperationException("The PriorityQueue is empty");
            T result = _heap[0];

            /**
             * the following lines are removed to ensure that items of the same priority are treated as FIFO 
            int lastIndex = _heap.Count - 1;
            _heap[0] = _heap[lastIndex];
            _heap.RemoveAt(lastIndex);
            */

            //remove item at the front of the queue (FIFO)
            _heap.RemoveAt(0);

            if (_heap.Count > 0)
            {
                DownHeap(0);
            }
            return result;
        }

        /// <summary>
        /// Returns the object with the lowest value in the <see cref="PriorityQueue{T}"/> without removing it.
        /// </summary>
        /// <returns>The object with the lowest value in the <see cref="PriorityQueue{T}"/>.</returns>
        /// <remarks>
        /// <para>
        ///   This method is similar to the <see cref="Dequeue"/> method, but <see cref="Peek"/> does not modify the <see cref="PriorityQueue{T}"/>. 
        /// </para>
        /// <para>
        ///   If type <typeparamref name="T"/> is a reference type, <see langword="null"/> can be added to the <see cref="PriorityQueue{T}"/> as a value. 
        /// </para>
        /// <para>
        ///   This method is an O(1) operation.
        /// </para>
        /// </remarks>
        /// <exception cref="InvalidOperationException">The <see cref="PriorityQueue{T}"/> is empty.</exception>
        public T Peek()
        {
            if (_heap.Count == 0)
                throw new InvalidOperationException("The PriorityQueue is empty");
            return _heap[0];
        }

        /// <summary>
        /// Indicates that the current first item of the <see cref="PriorityQueue{T}"/> was modified and its priority has to be re-evaluated.
        /// </summary>
        /// <remarks>
        /// <para>
        ///   If <typeparamref name="T"/> is a reference type and not immutable, it may be possible to modify the value of
        ///   items in the queue. In general, this is not allowed and doing this will break the priority queue and lead to
        ///   undefined behaviour.
        /// </para>
        /// <para>
        ///   However, it is allowed to modify the current first element in the queue (which is returned by <see cref="Peek"/>)
        ///   if this change is followed by an immediate call to <see cref="AdjustFirstItem"/> which re-evaluates
        ///   the item's value and moves a different item to the front if necessary.
        /// </para>
        /// <para>
        ///   In the scenario that you are removing an item from the <see cref="PriorityQueue{T}"/> and immediately replacing it with a new one,
        ///   using this function can yield better performance, as the sequence of <see cref="Dequeue"/>, modify, <see cref="Enqueue"/> is twice as slow
        ///   as doing <see cref="Peek"/>, modify, <see cref="AdjustFirstItem"/>.
        /// </para>
        /// <para>
        ///   Because the first element may change after calling <see cref="AdjustFirstItem"/>, it is not safe to continue
        ///   modifying that same element afterwards. You must call <see cref="Peek"/> again to get the new front element which
        ///   may now be changed.
        /// </para>
        /// <para>
        ///   This method is an O(log <em>n</em>) operation, where <em>n</em> is <see cref="Count"/>.
        /// </para>
        /// </remarks>
        /// <exception cref="InvalidOperationException">The <see cref="PriorityQueue{T}"/> is empty.</exception>
        public void AdjustFirstItem()
        {
            if (_heap.Count == 0)
                throw new InvalidOperationException("The PriorityQueue is empty");

            DownHeap(0);
        }

        /// <summary>
        /// Removes all objects from the <see cref="PriorityQueue{T}"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        ///   <see cref="Count"/> is set to zero, and references to other objects from elements of the collection are also released.
        /// </para>
        /// <para>
        ///   The capacity remains unchanged. To reset the capacity of the <see cref="PriorityQueue{T}"/>, call <see cref="TrimExcess"/>.
        ///   Trimming an empty <see cref="PriorityQueue{T}"/> sets the capacity of the <see cref="PriorityQueue{T}"/> to the default capacity.
        /// </para>
        /// <para>
        ///   This method is an O(<em>n</em>) operation, where <em>n</em> is <see cref="Count"/>.
        /// </para>
        /// </remarks>
        public void Clear()
        {
            _heap.Clear();
        }

        /// <summary>
        /// Sets the capacity to the actual number of elements in the <see cref="PriorityQueue{T}"/>, if that number is less than a threshold value.
        /// </summary>
        /// <remarks>
        /// <para>
        ///   This method can be used to minimize a collection's memory overhead if no new elements will be added to the collection.
        ///   The cost of reallocating and copying a large <see cref="PriorityQueue{T}"/> can be considerable, however, so the <see cref="TrimExcess"/> method
        ///   does nothing if the list is at more than 90 percent of capacity. This avoids incurring a large reallocation cost for
        ///   a relatively small gain.
        /// </para>
        /// <note>
        ///   The current threshold of 90 percent it depends on <see cref="List{T}"/> and might change in future releases of the .Net Framework. 
        /// </note>
        /// <para>
        ///   This method is an O(<em>n</em>) operation, where <em>n</em> is <see cref="Count"/>.
        /// </para>
        /// <para>
        ///   To reset a <see cref="PriorityQueue{T}"/> to its initial state, call the <see cref="Clear"/> method before calling the <see cref="TrimExcess"/> method.
        ///   Trimming an empty <see cref="PriorityQueue{T}"/> sets the capacity of the <see cref="PriorityQueue{T}"/> to the default capacity. 
        /// </para>
        /// <para>
        ///   The capacity can also be set using the <see cref="Capacity"/> property.
        /// </para>
        /// </remarks>
        public void TrimExcess()
        {
            _heap.TrimExcess();
        }

        /// <summary>
        /// Determines whether an element is in the <see cref="PriorityQueue{T}"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="PriorityQueue{T}"/>. The value can be <see langword="null"/> for reference types. </param>
        /// <returns><see langword="true"/> if item is found in the <see cref="PriorityQueue{T}"/>; otherwise, <see langword="false"/>.</returns>
        /// <remarks>
        /// <para>
        ///   This method determines equality using the default equality comparer <see cref="EqualityComparer{T}.Default"/> for <typeparamref name="T"/>, the type of values in the priority queue.
        /// </para>
        /// <para>
        ///   This method performs a linear search; therefore, this method is an O(<em>n</em>) operation, where <em>n</em> is <see cref="Count"/>.
        /// </para>
        /// </remarks>
        public bool Contains(T item)
        {
            return _heap.Contains(item);
        }

        /// <summary>
        /// Copies the <see cref="PriorityQueue{T}"/> elements to a new array. 
        /// </summary>
        /// <returns>A new array containing elements copied from the <see cref="PriorityQueue{T}"/>.</returns>
        /// <remarks>
        /// <para>
        ///   The elements are copied to the <see cref="Array"/> in the same order in which the enumerator iterates through the <see cref="PriorityQueue{T}"/>.
        /// </para>
        /// <para>
        ///   This method is an O(<em>n</em> log <em>n</em>) operation, where <em>n</em> is <see cref="Count"/>.
        /// </para>
        /// </remarks>
        public T[] ToArray()
        {
            T[] result = _heap.ToArray();
            // We want to return the elements in the same order in which they are enumerated, which is sorted order, so we simply sort.
            Array.Sort(result, Comparer);
            return result;
        }

        /// <summary>
        /// Copies the <see cref="PriorityQueue{T}"/> elements to an existing one-dimensional <see cref="Array"/>, starting at the specified array index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from <see cref="PriorityQueue{T}"/>.
        /// The <see cref="Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <remarks>
        /// <para>
        ///   The elements are copied to the <see cref="Array"/> in the same order in which the enumerator iterates through the <see cref="PriorityQueue{T}"/>.
        /// </para>
        /// <para>
        ///   This method is an O(<em>n</em> log <em>n</em>) operation, where <em>n</em> is <see cref="Count"/>.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
        /// <exception cref="ArgumentException"><paramref name="arrayIndex"/> is equal to or greater than the length of <paramref name="array"/>, or the
        /// number of elements in the source <see cref="PriorityQueue{T}"/> is greater than the available space from <paramref name="arrayIndex"/> to
        /// the end of the destination array. 
        /// </exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            _heap.CopyTo(array, arrayIndex);
            // We want to return the elements in the same order in which they are enumerated, which is sorted order, so we simply sort.
            Array.Sort(array, arrayIndex, _heap.Count, Comparer);
        }

        private void UpHeap()
        {
            int index = _heap.Count - 1;
            T item = _heap[index];
            int parentIndex = (index - 1) >> 1;
            // Because we can't easily tell when parentIndex goes beyond 0, we check index instead; if that was already zero, then we're at the top
            while (index > 0 && Comparer.Compare(item, _heap[parentIndex]) < 0)
            {
                _heap[index] = _heap[parentIndex];
                index = parentIndex;
                parentIndex = (index - 1) >> 1;
            }
            _heap[index] = item;
        }

        private void DownHeap(int index)
        {
            T item = _heap[index];
            int count = _heap.Count;
            int firstChild = (index << 1) + 1;
            int secondChild = firstChild + 1;
            int smallestChild = (secondChild < count && Comparer.Compare(_heap[secondChild], _heap[firstChild]) < 0) ? secondChild : firstChild;
            while (smallestChild < count && Comparer.Compare(_heap[smallestChild], item) < 0)
            {
                _heap[index] = _heap[smallestChild];
                index = smallestChild;
                firstChild = (index << 1) + 1;
                secondChild = firstChild + 1;
                smallestChild = (secondChild < count && Comparer.Compare(_heap[secondChild], _heap[firstChild]) < 0) ? secondChild : firstChild;
            }
            _heap[index] = item;
        }

        #region IEnumerable<T> Members

        /// <summary>
        /// Returns an enumerator that iterates through the values in the <see cref="PriorityQueue{T}"/>.
        /// </summary>
        /// <returns>An enumerator that iterates through the values in the <see cref="PriorityQueue{T}"/>.</returns>
        /// <remarks>
        /// <para>
        ///   The elements of the <see cref="PriorityQueue{T}"/> will be enumerated in the same order as if you
        ///   had called <see cref="Dequeue"/> until the <see cref="PriorityQueue{T}"/> was empty. I.e. the
        ///   elements are enumerated from lowest to highest value, in sorted order.
        /// </para>
        /// <para>
        ///   The contents of the <see cref="PriorityQueue{T}"/> are not modified by enumerating.
        /// </para>
        /// <para>
        ///   This method is an O(<em>n</em> log <em>n</em>) operation, where <em>n</em> is <see cref="Count"/>.
        /// </para>
        /// </remarks>
        public IEnumerator<T> GetEnumerator()
        {
            // We want to enumerate in the order you would get if calling Dequeue until the queue is empty.
            // A simple way to achieve that is to simple sort the heap, and to return an iterator over
            // the sorted copy.
            List<T> heapCopy = new List<T>(_heap);
            heapCopy.Sort(Comparer);
            return heapCopy.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region ICollection Members

        /// <summary>
        /// Gets the number of elements contained in the <see cref="PriorityQueue{T}"/>. 
        /// </summary>
        /// <value>
        /// The number of elements contained in the <see cref="PriorityQueue{T}"/>. 
        /// </value>
        /// <remarks>
        /// <para>
        ///   <see cref="Capacity"/> is the number of elements that the <see cref="PriorityQueue{T}"/> can store before 
        ///   resizing is required, while <see cref="Count"/> is the number of elements that are actually in the <see cref="PriorityQueue{T}"/>.
        /// </para>
        /// <para>
        ///   <see cref="Capacity"/> is always greater than or equal to <see cref="Count"/>. If <see cref="Count"/> exceeds
        ///   <see cref="Capacity"/> while adding elements, the capacity is increased by automatically reallocating the
        ///   internal array before copying the old elements and adding the new elements. 
        /// </para>
        /// <para>
        ///   Retrieving the value of this property is an O(1) operation.
        /// </para>
        /// </remarks>
        public int Count
        {
            get
            {
                return _heap.Count;
            }
        }

        void System.Collections.ICollection.CopyTo(Array array, int index)
        {
            ((System.Collections.ICollection)_heap).CopyTo(array, index);
        }

        bool System.Collections.ICollection.IsSynchronized
        {
            get { return false; }
        }

        object System.Collections.ICollection.SyncRoot
        {
            get
            {
                if (_syncRoot == null)
                {
                    System.Threading.Interlocked.CompareExchange(ref _syncRoot, new object(), null);
                }
                return _syncRoot;
            }
        }

        #endregion
    }

    /// <summary>
    /// Provides an <see cref="IComparer{T}"/> that inverts the results of the specified <see cref="IComparer{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of objects to compare.</typeparam>
    /// <threadsafety instance="false" static="true" />
    public class InvertedComparer<T> : Comparer<T>
    {
        private readonly IComparer<T> _comparer;
        private static InvertedComparer<T> _defaultComparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="InvertedComparer{T}"/> class.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}"/> whose results to invert.</param>
        /// <exception cref="ArgumentNullException"><paramref name="comparer"/> is <see langword="null"/>.</exception>
        public InvertedComparer(IComparer<T> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            _comparer = comparer;
        }

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>Less than zero if <paramref name="y"/> is less than <paramref name="x"/>; zero if <paramref name="x"/> equals <paramref name="y"/>;
        /// greater than zero if <paramref name="y"/> is greater than <paramref name="x"/>.</returns>
        public override int Compare(T x, T y)
        {
            // Reverse the arguments to get the inverted result.
            return _comparer.Compare(y, x);
        }

        /// <summary>
        /// Gets an <see cref="InvertedComparer{T}"/> that inverts the default comparer for the type specified by the generic argument.
        /// </summary>
        /// <value>An instance of <see cref="InvertedComparer{T}"/> that serves as an inverted sort order comparer for type <typeparamref name="T"/>.</value>
        public static new InvertedComparer<T> Default
        {
            get
            {
                if (_defaultComparer == null)
                {
                    System.Threading.Interlocked.CompareExchange(ref _defaultComparer, new InvertedComparer<T>(Comparer<T>.Default), null);
                }
                return _defaultComparer;
            }
        }
    }
}