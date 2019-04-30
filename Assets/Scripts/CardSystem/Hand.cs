using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class Hand : MonoBehaviour
{
    public Card CardPrefab;
    public int HandSize;
    public RectTransform CardHolder;
    public Canvas ParentCanvas;

    private RectTransform _rectTransform;
    private Card[] _cards;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _cards = new Card[HandSize];
        FillHand();
    }

    private void OnDestroy()
    {
        for(int i = 0; i < HandSize; i++)
        {
            if(_cards[i] != null)
            {
                _cards[i].OnCardDragBegin -= OnCardDragBegin;
                _cards[i].OnCardDragEnd -= OnCardDragEnd;
            }
        }
    }

    private void FillHand()
    {
        for (int i = 0; i < HandSize; i++)
        {
            Card c = Instantiate(CardPrefab, CardHolder);
            c.ParentCanvas = ParentCanvas;
            c.AppendToCardName(" " + i);
            AddCardToHand(c);
        }
    }

    private void AddCardToHand(Card c)
    {
        c.OnCardDragBegin += OnCardDragBegin;
        c.OnCardDragEnd += OnCardDragEnd;
    }

    private void OnCardDragBegin(Card c)
    {
        c.OriginalParent = c.transform.parent;
        c.transform.SetParent(transform);
    }

    private void OnCardDragEnd(Card c)
    {
        c.transform.SetParent(c.OriginalParent);
    }
}
