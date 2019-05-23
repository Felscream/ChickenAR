using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardUI;

public class Deck : MonoBehaviour
{
    public List<Card> DeckComposition;

    private Stack<Card> _cardStack;

    public void ShuffleDeckIntoStack()
    {
        List<Card> temp = new List<Card>(DeckComposition);
        for(int i = 0; i < DeckComposition.Count; i++)
        {
            int index = Random.Range(0, temp.Count);
            _cardStack.Push(temp[index]);
            temp.RemoveAt(index);
        }
    }

    public void ShuffleDeckIntoStack(List<Card> cards)
    {
        List<Card> temp = new List<Card>(cards);
        for (int i = 0; i < cards.Count; i++)
        {
            int index = Random.Range(0, temp.Count);
            _cardStack.Push(temp[index]);
            temp.RemoveAt(index);
        }
    }

    public void EmptyDeckStack()
    {
        _cardStack.Clear();
    }

    public Card DrawCard()
    {
        Card c = null;
        if(_cardStack.Count > 0)
            c = _cardStack.Pop();
        return c;
    }
}
