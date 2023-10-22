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

   public Image m_Icon;
   public Text m_Name;
   public Text m_Score;
   public Text m_WeekRank;

   public void UpdateInfo(int index, string iconUrl, string name, int score, int weekRank, int lastWeekRank)
   {
      if (m_RankIndex)
      {
         m_RankIndex.text = (index + 1).ToString();
      }

      if (m_Icon)
      {
         GameManager.Instance.LoadWebTexture(m_Icon, iconUrl);
      }

      if (m_Name)
      {
         m_Name.text = name;
      }

      if (m_Score)
      {
         m_Score.text = score.ToString();
      }

      if (m_WeekRank)
      {
         m_WeekRank.text = weekRank.ToString();
      }

   }

}
