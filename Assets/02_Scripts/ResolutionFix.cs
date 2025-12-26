using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionFix : MonoBehaviour
{
    void Awake()
    {
        // PC(윈도우/맥)에서 실행될 때만 작동
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer)
        {
            // 너비 1080, 높이 1920, 전체화면 끄기(false)
            Screen.SetResolution(1080, 1920, false);
        }
    }
}
