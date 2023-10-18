/***************************************************************
 * (c) copyright 2021 - 2025, Solar.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  SolarLinkedSet.cs
 * author:    taoye
 * created:   2023/2/15
 * descrip:   游戏框架Set容器类
 ***************************************************************/
using System;
using System.Collections.Generic;

namespace BigDream
{
	public class SolarLinkedSet<T> : IEnumerable<T>
	{
		/// <summary>
		/// 链表
		/// </summary>
		private LinkedList<T> m_List;

		/// <summary>
		/// 字典
		/// </summary>
		private Dictionary<T, LinkedListNode<T>> m_Dictionary;

		/// <summary>
		/// 初始化新实例
		/// </summary>
		public SolarLinkedSet()
		{
			m_List = new LinkedList<T>();
			m_Dictionary = new Dictionary<T, LinkedListNode<T>>();
		}

		/// <summary>
		/// 初始化新实例
		/// </summary>
		/// <param name="comparer"></param>
		public SolarLinkedSet(IEqualityComparer<T> comparer)
		{
			m_List = new LinkedList<T>();
			m_Dictionary = new Dictionary<T, LinkedListNode<T>>(comparer);
		}
		
		/// <summary>
		/// 追加元素
		/// </summary>
		/// <param name="t"></param>
		/// <returns></returns>
		public bool Add(T t)
		{
			if (m_Dictionary.ContainsKey(t))
            {
				return false;
			}

			LinkedListNode<T> node = m_List.AddLast(t);
			m_Dictionary.Add(t, node);
			return true;
		}

		/// <summary>
		/// 移除元素
		/// </summary>
		/// <param name="t"></param>
		/// <returns></returns>
        public bool Remove(T t)
        {
            LinkedListNode<T> node;

            if (m_Dictionary.TryGetValue(t, out node))
            {
                m_Dictionary.Remove(t);
                m_List.Remove(node);
                return true;
            }
            else
            {
                return false;
            }
        }

		/// <summary>
		/// 清空
		/// </summary>
        public void Clear()
		{
			m_List.Clear();
			m_Dictionary.Clear();
		}

		/// <summary>
		/// 是否包含
		/// </summary>
		/// <param name="t"></param>
		/// <returns></returns>
        public bool Contains(T t) => m_Dictionary.ContainsKey(t);

		/// <summary>
		/// 数量
		/// </summary>
        public int Count => m_List.Count;

		/// <summary>
		/// 返回循环访问集合的枚举
		/// </summary>
		/// <returns></returns>
		public IEnumerator<T> GetEnumerator() => m_List.GetEnumerator();

		/// <summary>
		/// 返回循环访问集合的枚举
		/// </summary>
		/// <returns></returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => m_List.GetEnumerator();
	}
}

