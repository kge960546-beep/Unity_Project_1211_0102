using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonImageChanger : MonoBehaviour
{
    public Image original;
    public Sprite newsprite;
    private Sprite orignalsprite;

    private bool isChanged = false;

    private void Awake()
    {
        if(original == null)
            original = GetComponentInChildren<Image>();
        
        orignalsprite = original.sprite;
        isChanged = false;
    }
    public void NewImage()
    {        
        original.sprite = newsprite;
    }
    public void ChangeBack()
    {        
        original.sprite = orignalsprite;
    }
}
