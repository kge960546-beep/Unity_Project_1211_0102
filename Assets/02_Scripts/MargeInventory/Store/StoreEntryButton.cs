using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreEntryButton : MonoBehaviour
{
    [SerializeField] private GameObject store;
    [SerializeField] private GameObject UpgreadWindow;

    public void OnClickStore()
    {
        store.SetActive(true);
        UpgreadWindow.SetActive(false);
    }
}
