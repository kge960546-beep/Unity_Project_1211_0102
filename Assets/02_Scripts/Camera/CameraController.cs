using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera cam;

    public float normalCamSize = 5f;
    public float bossCamSize = 9f;
    public float zoomSpeed = 5f;

    private float defaultSize;

    private Coroutine zoomRoutine;

    private void Awake()
    {
        cam = Camera.main;
    }
    public void ZoomOutForBoss()
    {
        if(zoomRoutine != null)
            StopCoroutine(zoomRoutine);

        zoomRoutine = StartCoroutine(Zoom(bossCamSize));
    }
    public void ResetZoom()
    {
        if(zoomRoutine != null)
            StopCoroutine(zoomRoutine);

        StartCoroutine (Zoom(normalCamSize));
    }
    public IEnumerator Zoom(float targetCamSize)
    {
        if (cam == null)
        {
            Debug.LogError("Zoom failed: Camera is null");
            yield break;
        }

        float zoomDuration = 1f / Mathf.Max(zoomSpeed, 0.1f); // ÁÜ Áö¼Ó ½Ã°£
        float elapsed = 0f;
        float start = cam.orthographicSize;

        while (elapsed < zoomDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / zoomDuration);
            cam.orthographicSize = Mathf.Lerp(start, targetCamSize, t);
            yield return null;
        }

        cam.orthographicSize = targetCamSize;
    }
}
