/***************************************************************
 * (c) copyright 2021 - 2025, Solar.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  EventPool.Visitors.cs
 * author:    taoye
 * created:   2021/1/8
 * descrip:   事件池-访问器
 ***************************************************************/

using System.Collections.Generic;

namespace BigDream
{
    public sealed partial class EventPool<T> where T : EventParams
    {
        
        /// <summary>
        /// 所有已注册事件类型的事件处理集合
        /// <事件类型ID, <事件处理函数宿主对象，事件处理函数集合>>
        /// 注意：事件类型与事件处理函数为一对多的关系
        /// </summary>
        private readonly SolarMultiDictionary<Common.EventCmd, Dictionary<object, SolarEventHandler<T>>> m_SubscribedEventHandlers;

        /// <summary>
        /// 下一帧待派发的事件集合
        /// </summary>
        private readonly Queue<Event> m_EventsForFire;

        /// <summary>
        /// 缓存节点集合
        /// <事件参数，<事件处理函数宿主对象, 事件处理函数>>
        /// </summary>
        private readonly Dictionary<T, LinkedListNode<Dictionary<object, SolarEventHandler<T>>>> m_CachedNodes;

        /// <summary>
        /// 临时节点集合
        /// <事件参数，<事件处理函数宿主对象, 事件处理函数>>
        /// </summary>
        private readonly Dictionary<T, LinkedListNode<Dictionary<object, SolarEventHandler<T>>>> m_TempNodes;

        /// <summary>
        /// 获取已注册事件类型数量。
        /// </summary>
        public int SubscribedEventTypeCount
        {
            get
            {
                return m_SubscribedEventHandlers.Count;
            }
        }

        /// <summary>
        /// 获取已注册指定事件类型的事件处理函数的数量。
        /// </summary>
        /// <param name="cmd">事件类型编号。</param>
        /// <returns>事件处理函数的数量。</returns>
        public int SubscribedEventCount(Common.EventCmd cmd)
        {
            SolarLinkedListRange<Dictionary<object, SolarEventHandler<T>>> range = null;
            if (m_SubscribedEventHandlers.TryGetValue(cmd, out range))
            {
                return range.Count;
            }
            return 0;
        }

        /// <summary>
        /// 获取待派发的事件数量。
        /// </summary>
        public int EventsForFireCount
        {
            get
            {
                return m_EventsForFire.Count;
            }
        }

    }

}


