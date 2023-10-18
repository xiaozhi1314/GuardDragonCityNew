/***************************************************************
 * (c) copyright 2019 - 2020, XTeamFramework.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MonoSingletonCreator.cs
 * author:    yuanjiashun
 * created:   2021/12/9
 * descrip:   组件化单例生成器
 ***************************************************************/
using System.Reflection;
using UnityEngine;
namespace BigDream
{
	public static class MonoSingletonCreator
	{
		public static bool IsUnitTestMode { get; set; }

		public static T CreateMonoSingleton<T>() where T : MonoBehaviour, ISingleton
		{
			T _instance = null;

			if (!IsUnitTestMode && !Application.isPlaying) return _instance;
			_instance = Object.FindObjectOfType<T>();

			if (_instance != null)
			{
				//_instance.Init();
				return _instance;
			}

			MemberInfo info = typeof(T);
			var attributes = info.GetCustomAttributes(true);
			foreach (var atribute in attributes)
			{
				var defineAttri = atribute as MonoSingletonPath;
				if (defineAttri == null)
				{
					continue;
				}

				_instance = CreateComponentOnGameObject<T>(defineAttri.PathInHierarchy, true);
				break;
			}

			if (_instance == null)
			{
				var obj = new GameObject(typeof(T).Name);
				if (!IsUnitTestMode)
                {
                    Object.DontDestroyOnLoad(obj);
                }
				_instance = obj.AddComponent<T>();
			}

			//instance.Init();
			return _instance;
		}

		private static T CreateComponentOnGameObject<T>(string path, bool dontDestroy) where T : MonoBehaviour
		{
			var obj = FindGameObject(path, true, dontDestroy);
			if (obj == null)
			{
				obj = new GameObject("Singleton of " + typeof(T).Name);
				if (dontDestroy && !IsUnitTestMode)
				{
					Object.DontDestroyOnLoad(obj);
				}
			}

			return obj.AddComponent<T>();
		}

		private static GameObject FindGameObject(string path, bool build, bool dontDestroy)
		{
			if (string.IsNullOrEmpty(path))
			{
				return null;
			}

			var subPath = path.Split('/');
			if (subPath == null || subPath.Length == 0)
			{
				return null;
			}

			return FindGameObject(null, subPath, 0, build, dontDestroy);
		}

		private static GameObject FindGameObject(GameObject root, string[] subPath, int index, bool build, bool dontDestroy)
		{
			GameObject client = null;

			if (root == null)
			{
				client = GameObject.Find(subPath[index]);
			}
			else
			{
				var child = root.transform.Find(subPath[index]);
				if (child != null)
				{
					client = child.gameObject;
				}
			}

			if (client == null)
			{
				if (build)
				{
					client = new GameObject(subPath[index]);
					if (root != null)
					{
						client.transform.SetParent(root.transform);
					}

					if (dontDestroy && index == 0 && !IsUnitTestMode)
					{
						GameObject.DontDestroyOnLoad(client);
					}
				}
			}

			if (client == null)
			{
				return null;
			}

			return ++index == subPath.Length ? client : FindGameObject(client, subPath, index, build, dontDestroy);
		}
	}
}
