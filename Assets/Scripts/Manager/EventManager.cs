using System.Collections;
using System.Collections.Generic;
using BigDream;
using UnityEngine;

public class EventManager : MonoSingleton<EventManager>
{
    /// <summary>
    /// 事件池
    /// </summary>
    private  EventPool<EventParams> m_EventPool;

    /// <summary>
    /// 获取已注册事件类型数量。
    /// </summary>
    public int SubscribedEventTypeCount
    {
        get
        {
            return m_EventPool.SubscribedEventTypeCount;
        }
    }

    /// <summary>
    /// 获取已注册指定事件类型的事件处理函数的数量。
    /// </summary>
    /// <param name="cmd">事件类型编号。</param>
    /// <returns>事件处理函数的数量。</returns>
    public int SubscribedEventCount(Common.EventCmd cmd)
    {
        return m_EventPool.SubscribedEventCount(cmd);
    }

    /// <summary>
    /// 获取待派发的事件数量。
    /// </summary>
    public int EventsForFireCount
    {
        get
        {
            return m_EventPool.EventsForFireCount;
        }
    }
    
    public override void Init()
    {
        m_EventPool = new EventPool<EventParams>();
   
    }

    private void Update()
    {
        m_EventPool.Update();
    }

    /// <summary>
    /// 关闭并清理管理器。
    /// </summary>
    public void Shutdown()
    {
        m_EventPool.Shutdown();
    }
    
    
    /// <summary>
    /// 检查是否存在事件处理函数。
    /// </summary>
    /// <param name="cmd">事件类型编号。</param>
    /// <param name="userData">用户数据。</param>
    /// <param name="handler">要检查的事件处理函数。</param>
    /// <returns>是否存在事件处理函数。</returns>
    public bool Check(Common.EventCmd cmd, object userData, SolarEventHandler<EventParams> handler)
    {
        return m_EventPool.Check(cmd, userData, handler);
    }
    
    /// <summary>
    /// 注册事件处理函数。
    /// </summary>
    /// <param name="cmd">事件类型编号。</param>
    /// <param name="userData">用户数据。</param>
    /// <param name="handler">要注册的事件处理函数。</param>
    public void Subscribe(Common.EventCmd cmd, object userData, SolarEventHandler<EventParams> handler)
    {
        m_EventPool.Subscribe(cmd, userData, handler);
    }

    /// <summary>
    /// 注销事件处理函数。
    /// </summary>
    /// <param name="cmd">事件类型编号。</param>
    /// <param name="userData">用户数据。</param>
    /// <param name="handler">要取消注册的事件处理函数。</param>
    public void Unsubscribe(Common.EventCmd cmd, object userData, SolarEventHandler<EventParams> handler)
    {
        m_EventPool.Unsubscribe(cmd, userData, handler);
    }

    /// <summary>
    /// 抛出事件，这个操作是线程安全的，即使不在主线程中抛出，也可保证在主线程中回调事件处理函数，但事件会在抛出后的下一帧分发。
    /// </summary>
    /// <param name="sender">事件源。</param>
    /// <param name="e">事件参数。</param>
    public void Fire(object sender, EventParams e)
    {
        m_EventPool.Fire(sender, e);
    }

    /// <summary>
    /// 抛出事件立即模式，这个操作不是线程安全的，事件会立刻分发。
    /// </summary>
    /// <param name="sender">事件源。</param>
    /// <param name="e">事件参数。</param>
    public void FireNow(object sender, EventParams e)
    {
        m_EventPool.FireNow(sender, e);
    }
}
