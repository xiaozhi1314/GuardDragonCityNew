using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ResultUI : MonoBehaviour
{
    /// <summary>
    /// 展示ui
    /// </summary>
    public void Open()
    {
      //  DOTween.Sequence():Append(transform:DOScale(1, self._rootOpenAnimationDelay):SetEase(Ease.OutBack)
        //    :OnComplete(function() self:OnOpenOver() end))
        var delayTime = 0.5f;
        DOTween.Sequence().Append(transform.DOScale(UnityEngine.Vector3.one, delayTime).SetEase(Ease.OutBack).OnComplete(() =>
        {
            OpenCover();
        }));
    }

    public void OpenCover()
    {
        
    }
}
