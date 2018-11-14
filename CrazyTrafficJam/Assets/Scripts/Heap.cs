using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam
{
	public class Heap<T> where T : IComparable<T>
	{
		private HeapNode<T>[] items;
		private int currentItemCount;

		public int Count => currentItemCount;
		public int MaxSize => items.Length;

		public Heap(int maxSize)
		{
			items = new HeapNode<T>[maxSize];
			currentItemCount = 0;
		}

		public void Push(T item)
		{
			HeapNode<T> newNode = new HeapNode<T>(item);
			newNode.HeapIndex = currentItemCount;

			items[currentItemCount] = newNode;
			SortUp(newNode);
			++currentItemCount;
		}

		public T Peek()
		{
			if (currentItemCount == 0)
				return default(T);
			return items[0].Node;
		}

		public T Pop()
		{
			if (currentItemCount == 0)
				return default(T);
			T tmp = items[0].Node;
			--currentItemCount;
			items[0].Node = items[currentItemCount].Node;
			SortDown(items[0]);

			return tmp;
		}

		public bool Contains(T item)
		{
			for (int i = 0 ; i < currentItemCount ; ++i)
			{
				if (Equals(items[i].Node, item))
					return true;
			}
			return false;
		}

		public void UpdateItem(T item)
		{
			for (int i = 0 ; i < currentItemCount ; ++i)
			{
				if (Equals(items[i].Node, item))
				{
					SortUp(items[i]);
				}
			}
		}

		private void SortUp(HeapNode<T> item)
		{
			int parentIndex = (item.HeapIndex - 1) / 2;

			while (true)
			{
				HeapNode<T> parentItem = items[parentIndex];

				if (item.Node.CompareTo(parentItem.Node) > 0)
					Swap(item, parentItem);
				else
				{
					break;
				}
				parentIndex = (item.HeapIndex - 1) / 2;
			}
		}

		private void SortDown(HeapNode<T> item)
		{
			while (true)
			{
				int childIndexLeft = item.HeapIndex * 2 + 1;
				int childIndexRight = item.HeapIndex * 2 + 2;
				int swapIndex = 0;

				if (childIndexLeft < currentItemCount)
				{
					swapIndex = childIndexLeft;

					if (childIndexRight < currentItemCount &&
						items[childIndexLeft].Node.CompareTo(items[childIndexRight].Node) < 0)
					{
						swapIndex = childIndexRight;
					}

					if (item.Node.CompareTo(items[swapIndex].Node) < 0)
						Swap(item, items[swapIndex]);
					else
						return;
				}
				else
					return;
			}
		}

		private void Swap(HeapNode<T> itemA, HeapNode<T> itemB)
		{
			items[itemA.HeapIndex] = itemB;
			items[itemB.HeapIndex] = itemA;
			int itemAIndex = itemA.HeapIndex;
			itemA.HeapIndex = itemB.HeapIndex;
			itemB.HeapIndex = itemAIndex;
		}
	}

	public class HeapNode<T> where T : IComparable<T>
	{
		public T Node;
		public int HeapIndex;

		public HeapNode(T node)
		{
			Node = node;
			HeapIndex = 0;
		}
	}
}