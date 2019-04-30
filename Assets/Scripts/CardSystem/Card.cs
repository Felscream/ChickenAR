using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IDraggable
{
    private RectTransform _rectTransform;
    private Vector2 _originalPivot;
    private Vector2 _dragPivot;
    private Canvas _parentCanvas;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _originalPivot = _rectTransform.pivot;
        _parentCanvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _rectTransform.pivot = _dragPivot;
        Debug.Log("Pos : " + eventData.position);
        Debug.Log("Pivot  : "+ _originalPivot);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("On drag");
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }

    private Vector2 ComputeNewPivot(Vector2 dragPosition)
    {
        Vector2 newPivot = new Vector2(.5f, .5f);
        Vector2 pivotDifference = _originalPivot - _rectTransform.pivot;
        Vector2 center = (Vector2)transform.position + _rectTransform.sizeDelta * _parentCanvas.scaleFactor * _rectTransform.localScale * pivotDifference;
        Vector2 localPosition = (dragPosition - center) / (_parentCanvas.scaleFactor * _rectTransform.localScale) + _rectTransform.sizeDelta / 2f;
        newPivot = localPosition / _rectTransform.sizeDelta;
        Debug.Log(newPivot);
        return newPivot;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _dragPivot = ComputeNewPivot(eventData.pressPosition);
    }
}
