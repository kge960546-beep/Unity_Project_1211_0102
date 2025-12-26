using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] float maxRadius;
    [SerializeField] float headRadius;
    [SerializeField] float deadZoneRadius;
    [SerializeField] RectTransform JoystickRimRectTransform;
    [SerializeField] RectTransform JoystickBaseRectTransform;
    [SerializeField] RectTransform JoystickHeadRectTransform;
    [SerializeField] Image JoystickRimImage;
    [SerializeField] Image JoystickBaseImage;
    [SerializeField] Image JoystickHeadImage;

    PlayerFeedService pfs;

    private void Awake()
    {
        Image raycastCatcher = gameObject.AddComponent<Image>();
        raycastCatcher.color = Color.clear;
        raycastCatcher.raycastTarget = true;
        RectTransform rect = raycastCatcher.rectTransform;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    private void OnEnable()
    {
        JoystickRimRectTransform.sizeDelta = new Vector2(2 * maxRadius, 2 * maxRadius);
        JoystickBaseRectTransform.sizeDelta = new Vector2(2 * deadZoneRadius, 2 * deadZoneRadius);
        JoystickHeadRectTransform.sizeDelta = new Vector2(2 * headRadius, 2 * headRadius);
        pfs = GameManager.Instance.GetService<PlayerFeedService>();

        JoystickRimImage.enabled = false;
        JoystickBaseImage.enabled = false;
        JoystickHeadImage.enabled = false;
        pfs.userInputDirection = Vector2.zero;
    }

    private void OnDisable()
    {
        if(null != pfs) pfs.userInputDirection = Vector2.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        JoystickRimImage.enabled = true;
        JoystickBaseImage.enabled = true;
        JoystickHeadImage.enabled = true;
        JoystickRimRectTransform.position = eventData.position;
        JoystickBaseRectTransform.position = eventData.position;
        JoystickHeadRectTransform.position = eventData.position;
        pfs.userInputDirection = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 diff = eventData.position - (Vector2)JoystickBaseRectTransform.position;
        diff = Vector2.ClampMagnitude(diff, maxRadius);
        if (diff.magnitude <= deadZoneRadius) diff = Vector2.zero;
        JoystickHeadRectTransform.position = (Vector2)JoystickBaseRectTransform.position + diff;
        pfs.userInputDirection = diff / maxRadius;
        return;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        JoystickRimImage.enabled = false;
        JoystickBaseImage.enabled = false;
        JoystickHeadImage.enabled = false;
        pfs.userInputDirection = Vector2.zero;
    }
}
