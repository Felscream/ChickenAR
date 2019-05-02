using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CardUI
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(UIAnimation))]
    [RequireComponent(typeof(RectTransform))]
    public class Hand : MonoBehaviour, IPointerExitHandler
    {
        public Card CardPrefab;
        public int HandSize;
        public RectTransform CardHolder;
        public Canvas ParentCanvas;
        public DropZone DropZone;
        public TileSelection TileSelection;
        public CancelCardSelection CancelCardSelection;

        private RectTransform _rectTransform;
        private List<Card> _cards;
        private Image _image;
        private Card _selectedCard;
        private HandAnimation _animation;

        private void Start()
        {
            _animation = GetComponent<HandAnimation>();
            _rectTransform = GetComponent<RectTransform>();
            _image = GetComponent<Image>();

            _cards = new List<Card>();
            if(DropZone != null)
            {
                DropZone.OnPointerEnterBehaviour += OnCardChosen;
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

            if(CancelCardSelection != null)
            {
                CancelCardSelection.OnCardDrop += CancelCardChoice;
                CancelCardSelection.OnCardEnter += CancelCardSelectionHover;
                CancelCardSelection.OnCardExit += CancelCardSelectionExit;
                CancelCardSelection.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("No reference to object of type CancelCardSelection", gameObject);
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
                DropZone.OnCardDrop -= PlayCard;
            }
            
            if(TileSelection != null)
            {
                TileSelection.IsTilePlayable -= IsTilePlayable;
            }

            if (CancelCardSelection != null)
            {
                CancelCardSelection.OnCardDrop -= CancelCardChoice;
                CancelCardSelection.OnCardEnter -= CancelCardSelectionHover;
                CancelCardSelection.OnCardExit -= CancelCardSelectionExit;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if(_selectedCard != null)
            {
                _image.raycastTarget = false;
                _animation.PlayAnimation(true);
                CancelCardSelection.gameObject.SetActive(true);
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

        private void CancelCardSelectionHover()
        {
            if(_selectedCard != null)
            {
                TileSelection.AreTilesSelectable = false;
            }
        }

        private void CancelCardSelectionExit()
        {
            if(_selectedCard != null)
            {
                TileSelection.AreTilesSelectable = true;
            }
        }

        private void CancelCardChoice()
        {
            if(_selectedCard != null)
            {
                TileSelection.AreTilesSelectable = false;
                _selectedCard.CanvasGroup.alpha = 1f;
                CancelCardSelection.gameObject.SetActive(false);
                _animation.PlayAnimation(false);
            }
        }

        private void PlayCard()
        {
            if(_selectedCard != null)
            {
                if(IsEffectPlayable())
                {
                    if (_selectedCard.CardEffect.PlayableTiles.Contains(TileSelection.CurrentTile.Type))
                    {
                        _selectedCard.OnCardDragEnd -= OnCardDragEnd;
                        Debug.Log("Played " + _selectedCard.transform.name);
                        _selectedCard.CardEffect.Execute(TileSelection.CurrentTile);
                        _cards.Remove(_selectedCard);
                        DestroyImmediate(_selectedCard.gameObject);
                        _animation.PlayAnimation(false);
                        CancelCardSelection.gameObject.SetActive(false);
                        TileSelection.AreTilesSelectable = false;
                    }
                }
                else
                {
                    CancelCardChoice();
                }
            }
            _selectedCard = null;

            _image.raycastTarget = true;
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
            return _selectedCard.CardEffect != null && TileSelection.CurrentTile != null && IsTilePlayable(TileSelection.CurrentTile);
        }
    }
}

