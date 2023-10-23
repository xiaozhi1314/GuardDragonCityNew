using System.Collections;
using System.Collections.Generic;
using BigDream;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class NoticeUI : MonoBehaviour
{
   public Image m_HeadIconImage;
   public Text m_NameText;
   public Text m_ScoreText;
   
   public void UpdateData(string tikTokId, int score)
   {
      if (GameManager.Instance.m_NoticeMsgDic.ContainsKey(tikTokId))
      {
          var tikTokInfo = GameManager.Instance.m_NoticeMsgDic[tikTokId];
          if (m_HeadIconImage)
          {
              GameManager.Instance.LoadWebTexture(m_HeadIconImage,tikTokInfo.avatarUrl);
          }

          if (m_NameText)
          {
              m_NameText.text = tikTokInfo.nickName;
          }

          if (m_ScoreText)
          {
              var curValue = 0;
              var endStart = score;
              var animationDuration = 2.5f;
              var punchScaleAmount = 0.4f;
              var vibrato = 4;
              var delayTime = 1.0f;

              var punchTween = m_ScoreText.transform.DOPunchScale(new Vector3(1,1,1) * punchScaleAmount, animationDuration, vibrato);
              DOTween.To(() => { return curValue; }, (pos) => { curValue = pos; }, endStart , animationDuration).SetEase(Ease.Linear).OnUpdate(() =>
              {
                  m_ScoreText.text = Mathf.FloorToInt(curValue).ToString();
              }).OnStart(() => { punchTween.Play();}).OnComplete(() =>
              {
                  Common.Slog("动画结束");
              });
                  
       
          }


      }
   }

}
