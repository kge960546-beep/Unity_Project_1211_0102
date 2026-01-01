using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundChange : MonoBehaviour
{
    private bool isMuted = false;
    private float prevVolume = 1.0f;
    private ButtonImageChanger buttonImageChanger;
    private void Awake()
    {
        if(buttonImageChanger == null)
            buttonImageChanger = GetComponent<ButtonImageChanger>();
    }
    public void SoundButtonClick()
    {
        if (isMuted == false)
        {
            isMuted = true;
            SoundOff();
            buttonImageChanger.NewImage();

        }
        else
        {
            isMuted = false;
            SoundOn();
            buttonImageChanger.ChangeBack();
        }                 
    }
    public void SoundOff()
    {
        prevVolume = AudioListener.volume;
        AudioListener.volume = 0;
    }
    public void SoundOn()
    {
        AudioListener.volume = Mathf.Clamp01(prevVolume);
    }
}
