using UnityEngine;

public class MergeInventoryButton : MonoBehaviour
{
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject lobbyStageSelectButton;
    [SerializeField] GameObject mainScrollView;
    [SerializeField] GameObject store;
    
    

    public void OnClickButton()
    {
        startButton.SetActive(false);
        lobbyStageSelectButton.SetActive(false);
        mainScrollView.SetActive(true);
        store.SetActive(false);
    }
}
