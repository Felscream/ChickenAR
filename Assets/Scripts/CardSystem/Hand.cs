using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace CardUI
{
    [RequireComponent(typeof(RectTransform))]
    public class Hand : MonoBehaviour
    {
        public Card CardPrefab;
        public int HandSize;
        public RectTransform CardHolder;
        public Canvas ParentCanvas;
        public DropZone DropZone;
        public TileSelection TileSelection;

        private RectTransform _rectTransform;
        private List<Card> _cards;

        private Card _selectedCard;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _cards = new List<Card>();
            if(DropZone != null)
            {
                DropZone.OnPointerEnterBehaviour += OnCardChosen;
                DropZone.OnPointerExitBehaviour += CancelCardChoice;
                DropZone.OnCardDrop += PlayCard;
            }
            else
            {
                Debug.LogError("No reference to object of type DropZone", gameObject);
            }

            if (TileSelection != null)
            {
                TileSelection.IsTilePlayable += IsTilePlayable;
            }
            else
            {
                Debug.LogError("No reference to object of type TileSelection", gameObject);
            }
            FillHand();
        }

        private void OnDestroy()
        {
            for (int i = 0; i < _cards.Count; i++)
            {
                if (_cards[i] != null)
                {
                    _cards[i].OnCardDragBegin -= OnCardDragBegin;
                    _cards[i].OnCardDragEnd -= OnCardDragEnd;
                }
            }

            if(DropZone != null)
            {
                DropZone.OnPointerEnterBehaviour -= OnCardChosen;
                DropZone.OnPointerExitBehaviour -= CancelCardChoice;
                DropZone.OnCardDrop -= PlayCard;
            }
            
            if(TileSelection != null)
            {
                TileSelection.IsTilePlayable -= IsTilePlayable;
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
            _selectedCard = c;
        }

        private void OnCardDragEnd(Card c)
        {
            c.transform.SetParent(c.OriginalParent);
            _selectedCard = null;
        }

        private void OnCardChosen()
        {
            if(_selectedCard != null)
            {
                TileSelection.AreTilesSelectable = true;
                _selectedCard.CanvasGroup.alpha = 0;
            }
        }

        private void CancelCardChoice()
        {
            if(_selectedCard != null)
            {
                TileSelection.AreTilesSelectable = false;
                _selectedCard.CanvasGroup.alpha = 1f;
            }
        }

        private void PlayCard()
        {
            if(_selectedCard != null)
            {
                if(IsEffectPlayable())
                {
                    if (_selectedCard.CardEffect.PlayableTiles.Contains(TileSelection.SelectedTile.Type))
                    {
                        _selectedCard.OnCardDragEnd -= OnCardDragEnd;
                        Debug.Log("Played " + _selectedCard.transform.name);
                        _selectedCard.CardEffect.Execute(TileSelection.SelectedTile);
                        _cards.Remove(_selectedCard);
                        DestroyImmediate(_selectedCard.gameObject);
                    }
                }
                else
                {
                    CancelCardChoice();
                }
            }
            _selectedCard = null;
        }

        private bool IsTilePlayable(WorldGenerator.TerrainTile tile)
        {
            bool isPlayable = false;
            if (_selectedCard != null)
            {
                CardEffect effect = _selectedCard.CardEffect;

                if (effect.PlayableTiles.Contains(tile.Type))
                {
                    if (effect.CanBePlayedOnFeatures)
                    {
                        isPlayable = true;
                    }
                    else if (!tile.HasFeature)
                    {
                        isPlayable = true;
                    }
                }

                if (tile.HasFeature && effect.OnlyPlayableOnFeatures)
                {
                    isPlayable = true;
                }
            }
            return isPlayable;
        }

        private bool IsEffectPlayable()
        {
            return _selectedCard.CardEffect != null && TileSelection.SelectedTile != null && IsTilePlayable(TileSelection.SelectedTile);
        }
    }
}

