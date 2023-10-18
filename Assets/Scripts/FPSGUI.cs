using UnityEngine;
using System.Collections;
using TMPro;

/// <summary>
/// ONGUI帧显示
/// </summary>
public class FPSGUI : MonoBehaviour
{
    private float currentTime = 0; 
    private float lateTime = 0;
    private float framesNum = 0; 
    private float fpsTime = 0;

    private TextMeshProUGUI m_TextMeshProUGUI;
    
    //private TextMesh
    private void Awake()
    {
        m_TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        framesNum++;
        if (currentTime - lateTime >= 1.0f)
        {
            fpsTime = framesNum / (currentTime - lateTime);
            lateTime = currentTime;
            framesNum = 0;
        }

        if (m_TextMeshProUGUI)
        {
            m_TextMeshProUGUI.text = $"FPS: {fpsTime.ToString()}";
        }
    }
}