using System;
using System.Buffers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace BigDream
{
    public class PoolManager : MonoSingleton<PoolManager>
    {

        /// <summary>
        /// Gameobject池子
        /// </summary>
        public Dictionary<string, ObjectPool<GameObject>> m_ObjPool;

        /// <summary>
        /// 入池后放入这个下面
        /// </summary>
        private Transform m_PoolParent;

        /// <summary>
        /// 父节点的挂载
        /// </summary>
        private Dictionary<string, Transform> m_ParentObject;

        /// <summary>
        /// 克隆的gameObject
        /// </summary>
        private Dictionary<string, GameObject> m_CloneObject;

        public override void Init()
        {
            m_PoolParent = GameObject.Find("ObjectPool").transform;
            m_ObjPool = new Dictionary<string, ObjectPool<GameObject>>();
            m_ParentObject = new Dictionary<string, Transform>();
            m_CloneObject = new Dictionary<string, GameObject>();
        }

        /// <summary>
        /// 设置对象池里面的名字对应的预制体
        /// </summary>
        /// <param name="objName"></param>
        /// <param name="prefab"></param>
        public void SetPoolGameObject(string objName, GameObject prefab)
        {
            if (m_CloneObject.ContainsKey(objName) == false)
            {
                m_CloneObject.Add(objName, prefab);
            }
        }


        /// <summary>
        /// 获取obj
        /// </summary>
        /// <param name="objName"></param>
        /// <returns></returns>
        public GameObject GetObj(string objName)
        {
            CheckPoolList(objName);
            var go = m_ObjPool[objName].Get();
            go.SetActive(true);
            go.name = objName;
            return go;
        }

        /// <summary>
        /// 获取Obj
        /// </summary>
        /// <param name="objName"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public GameObject GetObj(string objName, Transform parent, Vector3 pos, int scale = 1)
        {
            var go = GetObj(objName);
            if (parent != null)
            {
                go.transform.SetParent(parent);
                go.transform.position = pos;
               // go.transform.localScale = Vector3.one * scale;
            }
            return go;
        }

        /// <summary>
        /// 释放obj
        /// </summary>
        /// <param name="objName"></param>
        /// <param name="gameObject"></param>
        public void FreeObj(string objName, GameObject gameObject)
        {
            CheckPoolList(objName);
            gameObject.SetActive(false);
            gameObject.transform.SetParent(m_ParentObject[objName]);
            m_ObjPool[objName].Release(gameObject);
        }

        /// <summary>
        /// 获取地址池数量
        /// </summary>
        /// <param name="objName"></param>
        /// <returns></returns>
        public int GetCount(string objName)
        {
            if (m_ObjPool.ContainsKey(objName))
            {
                return m_ObjPool[objName].CountAll;
            }

            return 0;
        }

        /// <summary>
        /// 检查地址池里面是否有初始化过
        /// </summary>
        /// <param name="objName"></param>
        public void CheckPoolList(string objName)
        {
            if (m_ParentObject.ContainsKey(objName) == false)
            {
                var temp = new GameObject(objName).transform;
                temp.SetParent(m_PoolParent);
                m_ParentObject.Add(objName, temp);
                m_ObjPool.Add(objName, new ObjectPool<GameObject>((() => { return CreateObjectFunc(objName); })));
            }
        }

        /// <summary>
        /// 创建回调
        /// </summary>
        /// <param name="objName"></param>
        /// <returns></returns>
        public GameObject CreateObjectFunc(string objName)
        {
            if (m_CloneObject.ContainsKey(objName))
            {
                return Instantiate(m_CloneObject[objName]);
            }
            return null;
        }

        /// <summary>
        /// 预加载
        /// </summary>
        public void Preloading()
        {
            for (int idx = 0; idx < 30; idx++)
            {
                var go = CreateObjectFunc("普通怪物");
                if (go != null)
                {
                    FreeObj("普通怪物", go);
                }
            }
        }

    }
}