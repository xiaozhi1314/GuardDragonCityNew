using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace BigDream
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public Common.GameState GameState = Common.GameState.Start;


        public override void Init()
        {
            DOTween.Sequence().AppendInterval(3.0f).AppendCallback(() =>
            {
                GameManager.Instance.GameState = Common.GameState.Playing;
            });
            
            RVOManager.Instance.AutoCreateSolider();
            RVOManager.Instance.AutoCreateBigSolider();
            TableManager.Instance.GetArrayDatasGroup<TableMasterData>().Values.ToList().ForEach(masterDataObject =>
            {
                var masterData = (TableMasterData)masterDataObject;
                if (masterData.ID > 3)
                {
                    PoolManager.Instance.SetPoolGameObject(masterData.PoolName, Resources.Load<GameObject>(masterData.PrefabPath));
                }
            });
        }
        
    


    }
}