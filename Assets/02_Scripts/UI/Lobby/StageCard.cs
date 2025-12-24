using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageCard : MonoBehaviour
{
    [SerializeField] private int stageIndex;
    [SerializeField] private LobbyStageManager stageManager;
    [SerializeField] private Button button;

    private void Awake()
    {
        if(stageManager == null)
        {
            Debug.Log("³Ö¾î¶ó");
            return;
        }

        if(button == null)
            button = GetComponent<Button>();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(StageSelect);
    }
    public void StageSelect()
    {
        stageManager.SelectSatge(stageIndex);
    }
}
