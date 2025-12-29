using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreEntryButton : MonoBehaviour
{
    [SerializeField] private GameObject store;

    public void OnClickStore()
    {
        store.SetActive(true);
    }
}
