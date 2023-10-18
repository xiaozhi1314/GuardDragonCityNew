/***************************************************************
 * (c) copyright 2019 - 2020, XTeamFramework.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  State.cs
 * author:    yuanjiashun
 * created:   2021/12/9
 * descrip:   单例
 ***************************************************************/
namespace BigDream
{
	public abstract class Singleton<T> : ISingleton where T : Singleton<T>
	{
		protected static T _instance;

		static object _lock = new object();

		protected Singleton()
		{
		}

		public static T Instance
		{
			get
			{
				lock (_lock)
				{
					if (_instance == null)
					{
                        // 创建实例对象
                        _instance = SingletonCreator.CreateSingleton<T>();
					}
				}
				return _instance;
			}
		}

		public virtual void Init()
		{
		}

        public virtual void Proc()
        {

        }

        public virtual void FixedProc()
        {

        }

        public virtual void LateProc()
        {

        }

        public virtual void Destroy()
        {
            _instance = null;
        }

    }

}