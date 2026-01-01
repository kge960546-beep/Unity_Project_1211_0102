using UnityEngine;

public class InventoryButton : MonoBehaviour
{
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject lobbyStageSelectButton;
    [SerializeField] GameObject mainScrollView;
    [SerializeField] GameObject store;   
    

    public void OnClickButton()
    {
        startButton.SetActive(false);
        lobbyStageSelectButton.SetActive(false);       
        store.SetActive(false);
        mainScrollView.SetActive(true);
    }
}
