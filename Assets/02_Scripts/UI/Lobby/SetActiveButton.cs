using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveButton : MonoBehaviour
{
    [SerializeField] private GameObject lobbyCanvasGameObject;
    [SerializeField] private GameObject stageSelectGameObject;

    private void Awake()
    {
        if(lobbyCanvasGameObject == null)
        {
            Debug.Log("데이터 넣어라");
            return;
        }
        if(stageSelectGameObject == null)
        {
            Debug.Log("데이터 넣어라");
            return;
        }
    }
    public void LobbySetActiveOn()
    {
        lobbyCanvasGameObject.SetActive(true);
        stageSelectGameObject.SetActive(false);
    }
    public void LobbySetActiveOff()
    {
        lobbyCanvasGameObject.SetActive(false);
        stageSelectGameObject.SetActive(true);
    }
}
