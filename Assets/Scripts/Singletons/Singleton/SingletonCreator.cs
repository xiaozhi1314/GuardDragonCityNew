/***************************************************************
 * (c) copyright 2019 - 2020, XTeamFramework.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  State.cs
 * author:    yuanjiashun
 * created:   2021/12/9
 * descrip:   单例生成器
 ***************************************************************/
using System;
using System.Reflection;

namespace BigDream
{
    public static class SingletonCreator
    {
        public static T CreateSingleton<T>() where T : class, ISingleton
        {
            // 获取私有构造函数
            var ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
            
            // 获取无参构造函数
            var ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);

            if (ctor == null)
            {
                throw new Exception("Non-Public Constructor() not found! in " + typeof(T));
            }

            // 通过构造函数，常见实例
            var retInstance = ctor.Invoke(null) as T;
            //retInstance.Init();

            return retInstance;
        }
    }
}