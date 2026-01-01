using System.Collections.Generic;
using UnityEngine;

public class OffScreenIndicator : MonoBehaviour
{
    public GameObject[] targets;
    public GameObject indicatorPrefab;

    [Range(0.0f, 0.5f)] public float screenMargin = 0.1f;
    [Range(0.0f, 0.5f)] public float topMargin = 0.2f;

    public float arrowRadius = 0.2f;

    private SpriteRenderer spriteRenderer;
    private float spriteHeight;
    private float spriteWidth;

    private Camera cam;

    private Dictionary<GameObject, GameObject> targetIndicator = new Dictionary<GameObject, GameObject>();

    private void Start()
    {
        cam =Camera.main;
        spriteRenderer = indicatorPrefab.GetComponent<SpriteRenderer>();

        if(spriteRenderer != null)
        {
            var bounds = spriteRenderer.bounds;
            spriteHeight = bounds.size.y / 2f;
            spriteWidth = bounds.size.x / 2f;
        }
        else
        {
            spriteHeight = 1.0f;
            spriteWidth = 1.0f;
        }

        foreach (var target in targets)
        {
            var indicator = Instantiate(indicatorPrefab);
            indicator.SetActive(false);
            targetIndicator.Add(target, indicator);
        }
    }
    private void Update()
    {
        foreach(KeyValuePair<GameObject,GameObject> entry in targetIndicator)
        {
            var target = entry.Key;
            var indicator = entry.Value;

            UpdateTarget(target, indicator);
        }
    }
    private void UpdateTarget(GameObject target, GameObject indicator)
    {
        var screenPos = cam.WorldToViewportPoint(target.transform.position);

        bool isOffScreen = screenPos.x <= 0 || screenPos.x >= 1 || screenPos.y <= 0 || screenPos.y >= 1;

        if(isOffScreen)
        {
            indicator.SetActive(true);

            var spriteSizeInViewPort = cam.WorldToViewportPoint(new Vector3(spriteWidth, spriteHeight, 0))
                - cam.WorldToViewportPoint(Vector3.zero);

            float limitX = spriteSizeInViewPort.x + screenMargin;
            float limitYBottom = spriteSizeInViewPort.y + screenMargin;
            float limitYTop = spriteSizeInViewPort.y + topMargin;

            screenPos.x = Mathf.Clamp(screenPos.x, limitX, 1 - limitX);
            screenPos.y = Mathf.Clamp(screenPos.y, limitYBottom, 1 - limitYTop);

            var worldPos = cam.ViewportToWorldPoint(screenPos);
            worldPos.z = 0;
            indicator.transform.position = worldPos;

            Vector3 direction = target.transform.position - indicator.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            Transform arrowTransform = indicator.transform.Find("Arrow");

            if(arrowTransform != null)
            {
                SpriteRenderer sr = indicator.GetComponent<SpriteRenderer>();
                Vector3 orbCenter = indicator.transform.position;

                if(sr != null)
                {
                    orbCenter = sr.bounds.center;
                }

                indicator.transform.rotation = Quaternion.identity;
                arrowTransform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
                //arrowTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));             

                Vector3 dir = direction.normalized;
                
                arrowTransform.position = orbCenter + (dir * arrowRadius);
            }            
        }
        else
        {
            indicator.SetActive(false);
        }
    }  
}
