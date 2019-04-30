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

    private RectTransform _rectTransform;
    private Card[] _cards;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _cards = new Card[HandSize];
        FillHand();
    }

    private void FillHand()
    {
        float spacing = CardHolder.sizeDelta.x / HandSize;
        int halfHandSize = HandSize / 2;
        for (int i = -halfHandSize; i < halfHandSize + 1; i++)
        {
            Card c = Instantiate(CardPrefab, CardHolder);
            c.transform.localPosition = Vector3.right * spacing * i;
        }
    }
}
