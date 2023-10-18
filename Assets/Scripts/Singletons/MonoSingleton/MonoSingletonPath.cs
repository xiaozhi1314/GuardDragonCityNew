/***************************************************************
 * (c) copyright 2019 - 2020, XTeamFramework.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MonoSingletonPath.cs
 * author:    yuanjiashun
 * created:   2021/12/9
 * descrip:   组件化单例路径
 ***************************************************************/
namespace BigDream
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public class MonoSingletonPath : Attribute
    {
		private string _pathInHierarchy;

        public MonoSingletonPath(string pathInHierarchy)
        {
            _pathInHierarchy = pathInHierarchy;
        }

        public string PathInHierarchy
        {
            get { return _pathInHierarchy; }
        }
    }
}
