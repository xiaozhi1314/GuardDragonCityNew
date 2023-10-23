using System.Collections;
using System.Collections.Generic;
using BigDream;
using UnityEngine;
using UnityEngine.UI;

public class RankItemInfo : MonoBehaviour
{

   /// <summary>
   /// 排名
   /// </summary>
   public Text m_RankIndex;

   public Image m_IconImage;
   public Text m_NameText;
   public Text m_ScoreText;
   public Text m_WeekRankText;

   public void UpdateInfo(int index, string iconUrl, string name, int score, int weekRank, int lastWeekRank)
   {
      if (m_RankIndex)
      {
         m_RankIndex.text = (index + 1).ToString();
      }

      if (m_IconImage)
      {
         GameManager.Instance.LoadWebTexture(m_IconImage, iconUrl);
      }

      if (m_NameText)
      {
         m_NameText.text = name;
      }

      if (m_ScoreText)
      {
         m_ScoreText.text = score.ToString();
      }

      if (m_WeekRankText)
      {
         m_WeekRankText.text = weekRank.ToString();
      }

   }

}
