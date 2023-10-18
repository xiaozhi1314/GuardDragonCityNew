/***************************************************************
 * (c) copyright 2019 - 2020, XTeamFramework.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MonoSingleton.cs
 * author:    yuanjiashun
 * created:   2021/12/9
 * descrip:   组件化单例
 ***************************************************************/
using UnityEngine;
namespace BigDream
{
    public abstract class MonoSingleton<T> : MonoBehaviour, ISingleton where T : MonoSingleton<T>
    {
        protected static T _instance = null;

        // 应用程序是否正在退出（仅用于规避DontDestroyOnLoad的Manager单件在应用进程结束时提前销毁的特殊报错情况）
        public static bool _applicationIsQuitting = false;

        public static T Instance
        {
            get
            {
                if (_applicationIsQuitting)
                {
                    return null;
                }
                if (_instance == null)
                {
                    _instance = MonoSingletonCreator.CreateMonoSingleton<T>();
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
            if (MonoSingletonCreator.IsUnitTestMode)
            {
                var curTrans = transform;
                do
                {
                    var parent = curTrans.parent;
                    DestroyImmediate(curTrans.gameObject);
                    curTrans = parent;
                } while (curTrans != null);

                _instance = null;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            _instance = null;
        }
    }
}
