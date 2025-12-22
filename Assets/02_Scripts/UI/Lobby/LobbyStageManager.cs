using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyStageManager : MonoBehaviour
{
    [Header("스테이지 종류")]
    [SerializeField] private StageInfo[] stages;

    [Header("로비 UI")]
    [SerializeField] private Image lobbyStageImage;
    [SerializeField] private Button startButton;

    [Header("스테이지 선택 인덱스")]
    [SerializeField] private int stageIndex = 0;
    private int selectStageIndex;

    private void Awake()
    {
        selectStageIndex = Mathf.Clamp(stageIndex, 0, stages.Length - 1);

        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(OnClickStart);
        
    }
    public void SelectSatge(int index)
    {
        if (index < 0 || index >= stages.Length) return;

        selectStageIndex = index;
        StageSelectInLobby();
    }
    public void StageSelectInLobby()
    {
        if (stages == null || stages.Length == 0) return;
        lobbyStageImage.sprite = stages[selectStageIndex].lobbyStageSprite;
    }
    public void OnClickStart()
    {
        string scene = stages[selectStageIndex].SceneName;
        SceneManager.LoadScene(scene);
    }
}
