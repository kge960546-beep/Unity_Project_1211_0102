using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Carousel : MonoBehaviour
{
    public GameObject scrollbar;
    [SerializeField] float scrollPos = 0.0f;

    [SerializeField] Scrollbar scroll;

    float[] pos;
    
    void Start()
    {
        scroll = scrollbar.GetComponent<Scrollbar>();
        Debug.Log($"{name} childCount={transform.childCount}");
    }
    void Update()
    {
        pos = new float[transform.childCount];
        float distance = 1.0f / (pos.Length - 1);

        for(int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }

        if(Input.GetMouseButton(0))
        {
            scrollPos = scroll.value;
        }
        else
        {
            for(int i = 0; i < pos.Length; i++)
            {
                if (scrollPos < pos[i] + (distance/2) && scrollPos > pos[i] - (distance / 2))
                {
                    scroll.value = Mathf.Lerp(scroll.value, pos[i], 0.1f);
                }
            }
        }

        for(int i = 0; i < pos.Length; i++)
        {
            if (scrollPos < pos[i] + (distance / 2) && scrollPos > pos[i] - (distance / 2))
            {
                transform.GetChild(i).localScale =
                    Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1.0f, 1.0f), 0.1f);

                for (int j = 0; j < pos.Length; j++)
                {
                    if(j != i)
                    {
                        transform.GetChild(j).localScale =
                            Vector2.Lerp(transform.GetChild(j).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                    }
                }
            }
        }
    }
}
