/***************************************************************
 * (c) copyright 2021 - 2025, Solar.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  SolarLinkedListRange.cs
 * author:    taoye
 * created:   2020/12/16
 * descrip:   游戏框架链表范围
 ***************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

namespace BigDream
{
    /// <summary>
    /// 游戏框架链表范围。
    /// </summary>
    /// <typeparam name="T">指定链表范围的元素类型。</typeparam>
    public class SolarLinkedListRange<T> : IEnumerable<T>, IEnumerable
    {
        private readonly LinkedListNode<T> m_First;
        private readonly LinkedListNode<T> m_Terminal;

        /// <summary>
        /// 初始化游戏框架链表范围的新实例。
        /// </summary>
        /// <param name="first">链表范围的开始结点。</param>
        /// <param name="terminal">链表范围的终结标记结点。</param>
        public SolarLinkedListRange(LinkedListNode<T> first, LinkedListNode<T> terminal)
        {
            if (first == null || terminal == null || first == terminal)
            {
                throw new Exception("范围无效。");
            }

            m_First = first;
            m_Terminal = terminal;
        }

        /// <summary>
        /// 获取链表范围是否有效。
        /// </summary>
        public bool IsValid
        {
            get
            {
                return m_First != null && m_Terminal != null && m_First != m_Terminal;
            }
        }

        /// <summary>
        /// 获取链表范围的开始结点。
        /// </summary>
        public LinkedListNode<T> First
        {
            get
            {
                return m_First;
            }
        }

        /// <summary>
        /// 获取链表范围的终结标记结点。
        /// </summary>
        public LinkedListNode<T> Terminal
        {
            get
            {
                return m_Terminal;
            }
        }

        /// <summary>
        /// 获取链表范围的结点数量。
        /// </summary>
        public int Count
        {
            get
            {
                if (!IsValid)
                {
                    return 0;
                }

                int count = 0;
                for (LinkedListNode<T> current = m_First; current != null && current != m_Terminal; current = current.Next)
                {
                    count++;
                }

                return count;
            }
        }

        /// <summary>
        /// 检查是否包含指定值。
        /// </summary>
        /// <param name="value">要检查的值。</param>
        /// <returns>是否包含指定值。</returns>
        public bool Contains(T value)
        {
            for (LinkedListNode<T> current = m_First; current != null && current != m_Terminal; current = current.Next)
            {
                if (current.Value.Equals(value))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 返回循环访问集合的枚举数。
        /// </summary>
        /// <returns>循环访问集合的枚举数。</returns>
        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// 循环访问集合的枚举数。
        /// </summary>
        public struct Enumerator : IEnumerator<T>, IEnumerator
        {
            private readonly SolarLinkedListRange<T> m_SolarLinkedListRange;
            private LinkedListNode<T> m_Current;
            private T m_CurrentValue;

            internal Enumerator(SolarLinkedListRange<T> range)
            {
                if (!range.IsValid)
                {
                    throw new Exception("范围无效。");
                }

                m_SolarLinkedListRange = range;
                m_Current = m_SolarLinkedListRange.m_First;
                m_CurrentValue = default(T);
            }

            /// <summary>
            /// 获取当前结点。
            /// </summary>
            public T Current
            {
                get
                {
                    return m_CurrentValue;
                }
            }

            /// <summary>
            /// 获取当前的枚举数。
            /// </summary>
            object IEnumerator.Current
            {
                get
                {
                    return m_CurrentValue;
                }
            }

            /// <summary>
            /// 清理枚举数。
            /// </summary>
            public void Dispose()
            {
            }

            /// <summary>
            /// 获取下一个结点。
            /// </summary>
            /// <returns>返回下一个结点。</returns>
            public bool MoveNext()
            {
                if (m_Current == null || m_Current == m_SolarLinkedListRange.m_Terminal)
                {
                    return false;
                }

                m_CurrentValue = m_Current.Value;
                m_Current = m_Current.Next;
                return true;
            }

            /// <summary>
            /// 重置枚举数。
            /// </summary>
            void IEnumerator.Reset()
            {
                m_Current = m_SolarLinkedListRange.m_First;
                m_CurrentValue = default(T);
            }
        }
    }
}


