using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapReposition : MonoBehaviour
{
    [SerializeField] public float tileSize = 20.0f;

    Collider2D coll;
    private void Awake()
    {
        coll = GetComponent<Collider2D>();

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;

        Vector3 playerPos = GameObject.FindWithTag("Player").transform.position;//GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;

        switch (transform.tag)
        {
            case "Ground":
                float diffX = playerPos.x - myPos.x;
                float diffY = playerPos.y - myPos.y;

                int moveX = 0;
                int moveY = 0;

                if (Mathf.Abs(diffX) > Mathf.Abs(diffY))
                {
                    moveX = diffX > 0 ? 1 : -1;
                }

                else
                {
                    moveY = diffY > 0 ? 1 : -1;
                }

                Vector3 newPos = myPos + new Vector3(moveX * tileSize * 3.0f, moveY * tileSize * 3.0f, 0);

                newPos.x = Mathf.Round(newPos.x * 100f) / 100f;
                newPos.y = Mathf.Round(newPos.y * 100f) / 100f;

                transform.position = newPos;
                break;

            case "Enemy":
                if (coll.enabled)
                {
                    //너무 멀어진 적 다시 근처로 이동시키기 위한 로직
                    Vector3 dist = playerPos - myPos;
                    Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
                    transform.Translate(ran + dist * 2);
                }
                break;
        }
    }
}
