using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonImageChangerGroup : MonoBehaviour
{
    public static ButtonImageChangerGroup Instance;

    public Image original;
    public Sprite newsprite;
    private Sprite orignalsprite;

    public bool isDefaultButton;

    private void Awake()
    {
        if (original == null)
            original = GetComponentInChildren<Image>();

        orignalsprite = original.sprite;        
    }
    private void Start()
    {
        if(isDefaultButton)
        {
            NewImage();
            Instance = this;
        }
    }
    public void NewImage()
    {
        original.sprite = newsprite;
    }
    public void ChangeBack()
    {
        original.sprite = orignalsprite;
    }
    public void OnClickButton()
    {
        if (Instance == this) return;

        if(Instance != null)
        {
            Instance.ChangeBack();
        }

        NewImage();

        Instance = this;
    }
}
