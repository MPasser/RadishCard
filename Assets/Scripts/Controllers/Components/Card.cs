using UnityEngine;
using UnityEngine.UI;

namespace Ressap.RadishCard {
    public class Card : MonoBehaviour {
        public CardData Data;
        [SerializeField] private Image imgCard;

        private ClickHandler clickHandler;

        public void ResetWithData(CardType cardType, int index) {
            if (null == Data) {
                Data = new CardData(cardType, index);
            } else {
                Data.CardType = cardType;
                Data.Index = index;
            }
            SetOnClickEnable(false);
            loadCardbackImg();
        }

        public void SetOnClickAction(UnityEngine.Events.UnityAction action) {
            clickHandler.OnClick = action;
        }

        public void SetOnClickEnable(bool isEnable) {
            clickHandler.enabled = isEnable;
        }

        private void loadCardfaceImg() {
            imgCard.sprite = Resources.Load<Sprite>($"{Const.CARDFACE_IMG_DIR}/{Data.CardType}");
        }

        private void loadCardbackImg() {
            imgCard.sprite = Resources.Load<Sprite>($"{Const.CARDBACK_IMG_DIR}/Cardback");
        }

        public override string ToString() {
            return Data.ID;
        }
    }
}