using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace CardUI
{
    public class Card : MonoBehaviour, IDraggable
    {
        public delegate void CardDrag(Card c);
        public CardDrag OnCardDragBegin;
        public CardDrag OnCardDragEnd;

        public TextMeshProUGUI CardName;
        public float SmoothTime = 0.3f;
        public CardEffect CardEffect;

        private RectTransform _rectTransform;
        private Vector2 _originalPivot;
        private Vector2 _dragPivot;
        private bool _isDragged;
        private PointerEventData _eventTarget;

        private CanvasGroup _canvasGroup;
        private Vector3 _vel = Vector3.zero;

        public Transform OriginalParent { get; set; }
        public Canvas ParentCanvas { get; set; }
        public CanvasGroup CanvasGroup { get { return _canvasGroup; } }

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _originalPivot = _rectTransform.pivot;
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Update()
        {
            if (_isDragged)
            {
                transform.position = Vector3.SmoothDamp(transform.position, _eventTarget.position, ref _vel, SmoothTime);
            }

        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _rectTransform.pivot = _dragPivot;
            _rectTransform.position = eventData.position;
            if (OnCardDragBegin != null)
            {
                OnCardDragBegin(this);
            }
            _canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _eventTarget = eventData;
            _isDragged = true;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("DragEnd");
            if (OnCardDragEnd != null)
            {
                OnCardDragEnd(this);
            }
            _canvasGroup.blocksRaycasts = true;
            _isDragged = false;
        }

        private Vector2 ComputeNewPivot(Vector2 dragPosition)
        {
            Vector2 newPivot = new Vector2(.5f, .5f);
            Vector2 pivotDifference = _originalPivot - _rectTransform.pivot;
            Vector2 center = (Vector2)transform.position + _rectTransform.sizeDelta * ParentCanvas.scaleFactor * _rectTransform.localScale * pivotDifference;
            Vector2 localPosition = (dragPosition - center) / (ParentCanvas.scaleFactor * _rectTransform.localScale) + _rectTransform.sizeDelta / 2f;
            newPivot = localPosition / _rectTransform.sizeDelta;
            return newPivot;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _dragPivot = ComputeNewPivot(eventData.pressPosition);
        }

        public void SetCardName(string newName)
        {
            CardName.text = newName;
        }

        public void AppendToCardName(string toAppend)
        {
            CardName.text += toAppend;
        }
    }
}