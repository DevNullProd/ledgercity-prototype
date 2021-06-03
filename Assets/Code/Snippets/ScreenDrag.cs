using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScreenDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    RectTransform rectTransform;
    Vector2 ObjectStartPos;
    Vector2 ObjectCurentPos;
    Vector2 MouseStartPos;
    Vector2 MouseCurrentPos;

    bool Drag = false;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = this.gameObject.GetComponent<RectTransform>();
        ObjectStartPos = rectTransform.anchoredPosition;
        ObjectCurentPos = ObjectStartPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (Drag)
            rectTransform.anchoredPosition = Vector2.Lerp (rectTransform.anchoredPosition, ObjectCurentPos - (MouseStartPos - MouseCurrentPos) * 1.5f, 5f * Time.deltaTime);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        MouseStartPos = eventData.position;
        ObjectCurentPos = rectTransform.anchoredPosition;
        Drag = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        MouseCurrentPos = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ObjectCurentPos = rectTransform.anchoredPosition;
        Drag = false;
    }

    private void OnEnable()
    {
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = ObjectStartPos;
            MouseStartPos = ObjectStartPos;
            MouseCurrentPos = ObjectStartPos;
        }
    }
}
