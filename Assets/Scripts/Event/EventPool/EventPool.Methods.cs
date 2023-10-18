/***************************************************************
 * (c) copyright 2021 - 2025, Solar.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  EventPool.Methods.cs
 * author:    taoye
 * created:   2021/1/8
 * descrip:   事件池-方法
 ***************************************************************/

using System.Collections.Generic;

namespace BigDream
{

    public delegate void SolarEventHandler<TEventArgs>(object sender, object userData, TEventArgs e);

    public sealed partial class EventPool<T> where T : EventParams
    {
        /// <summary>
        /// 处理事件结点。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">事件参数。</param>
        private void HandleEvent(object sender, T e)
        {
            SolarLinkedListRange<Dictionary<object, SolarEventHandler<T>>> range = null;
            if (m_SubscribedEventHandlers.TryGetValue(e.Cmd, out range))
            {
                LinkedListNode<Dictionary<object, SolarEventHandler<T>>> current = range.First;
                while (current != null && current != range.Terminal)
                {
                    foreach (KeyValuePair<object, SolarEventHandler<T>> itr in current.Value)
                    {
                        itr.Value(sender, itr.Key, e);
                    }
                    current = m_CachedNodes[e] = current.Next != range.Terminal ? current.Next : null;
                }
                m_CachedNodes.Remove(e);
            }
        }
    }

}


