/***************************************************************
 * (c) copyright 2021 - 2025, Solar.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  EventPool.Event.cs
 * author:    taoye
 * created:   2021/1/8
 * descrip:   事件结点
 ***************************************************************/

namespace BigDream
{
    public sealed partial class EventPool<T> where T : EventParams
    {
        private sealed class Event
        {
            private object m_Sender;
            private T m_EventParams;

            public Event()
            {
                m_Sender = null;
                m_EventParams = null;
            }

            public object Sender
            {
                get
                {
                    return m_Sender;
                }
            }

            public T EventParams
            {
                get
                {
                    return m_EventParams;
                }
            }

            public static Event Create(object sender, T e)
            {
                Event eventNode = new Event();
                eventNode.m_Sender = sender;
                eventNode.m_EventParams = e;
                return eventNode;
            }

            public void Clear()
            {
                m_Sender = null;
                m_EventParams = null;
            }

        }
    }
}


