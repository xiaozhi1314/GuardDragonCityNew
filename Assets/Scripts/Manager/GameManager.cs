using System;
using System.Collections.Generic;
using DG.Tweening;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;

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
        }
        

    }
}