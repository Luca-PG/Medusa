/*
 * Copyright © 2011 Luca Pieracci Galante
 * 
 * This program is licensed under the Microsoft Public License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://medusa.codeplex.com/license.
 */

using System;

namespace TrailDLL
{
	internal class TrailStack<T>
	{
		private T[] m_hElements;
		private int m_iHead;
		private int m_iCount;
		private int m_iLength;

		public TrailStack(int iElements)
		{
			m_hElements = new T[iElements];
			m_iHead = 0;
			m_iCount = 0;
			m_iLength = iElements;
		}

		public void Push(T Value)
		{
			m_hElements[m_iHead] = Value;
			m_iHead++;
			CheckHead();
			m_iCount++;
		}

		public void Pop()
		{
			m_hElements[m_iHead] = default(T);
			m_iCount--;
			CheckHead();
		}

		public void Reset()
		{
			m_iCount = 0;
			m_iHead = 0;
		}

		public void Resize(int iNewSize)
		{
			if(m_hElements.Length < iNewSize)
				Array.Resize(ref m_hElements, iNewSize);
			
			m_iLength	= iNewSize;
			m_iCount	= Math.Min(m_iCount, m_iLength);
			m_iHead		= Math.Min(m_iHead, m_iLength);
		}

		private void CheckHead()
		{
			if(m_iHead < 0)
			{
				m_iHead = m_iLength - 1;
			}

			else if (m_iHead >= m_iLength)
			{
				m_iHead = 0;
			}
		}

		private int CheckIndex(int iIndex)
		{
			if(iIndex < 0)
			{
				iIndex = m_iLength + iIndex;
			}

			else if (iIndex >= m_iLength)
			{
				iIndex = 0;
			}

			return iIndex;
		}

		public int	Count			{ get { return m_iCount; } }

		public int MaxLenght		{ get { return m_iLength; } }

		public T	First			{ get { return m_hElements[CheckIndex(m_iHead - 1)]; } }

		public T	Last			{ get { return m_hElements[m_iHead]; } }

		public T	this[int index]
		{
			get
			{
				try
				{
					return m_hElements[CheckIndex(m_iHead - index - 1)];
				}
				catch
				{
					throw new IndexOutOfRangeException();
				}
			}
		}
	}
}