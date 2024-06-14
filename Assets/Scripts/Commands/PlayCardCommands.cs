using QFramework;

namespace Ressap.RadishCard {
    public class AskPlayCardCmd : AbstractCommand {
        private int playerID;

        public AskPlayCardCmd(int playerID) {
            this.playerID = playerID;
        }

        protected override void OnExecute() {
            if (Const.SELF_INDEX == playerID) {
                this.SendEvent<AskPlayerPlayEvt>();
            } else {
                CardData cardData = this.GetSystem<IStrategySystem>().AskPlay(playerID);
                this.SendCommand<PlayCardCmd>(new PlayCardCmd(playerID, cardData));
            }
        }
    }

    public class AskPlayerPlayEvt {
        public AskPlayerPlayEvt() {
        }
    }


    public class PlayCardCmd : AbstractCommand {
        private int playerID;
        private CardData cardData;

        public PlayCardCmd(int playerID, CardData cardData) {
            this.playerID = playerID;
            this.cardData = cardData;
        }

        protected override void OnExecute() {
            bool isNeedTarget;
            switch (cardData.CardType) {
                case CardType.EXCHANGE:
                case CardType.LOOT:
                case CardType.PROMOTE:
                    isNeedTarget = true;
                    break;
                case CardType.REVERSE:
                case CardType.SEE_THREE:
                case CardType.DETECT_POOP:
                case CardType.SHUFFLE:
                case CardType.SKIP:
                    isNeedTarget = false;
                    break;
                default:
                    UnityEngine.Debug.LogError($"[{nameof(PlayCardCmd)}] {nameof(OnExecute)}: CardType {cardData.CardType} should not be play actively.");
                    isNeedTarget = false;
                    break;
            }

            this.SendEvent<PlayCardEvt>(new PlayCardEvt(playerID, cardData, isNeedTarget));
        }
    }

    public class PlayCardEvt {
        public int PlayerID;
        public CardData CardData;
        public bool IsNeedTarget;

        public PlayCardEvt(int playerID, CardData cardData, bool isNeedTarget) {
            PlayerID = playerID;
            CardData = cardData;
            IsNeedTarget = isNeedTarget;
        }
    }


    public class ChooseCardTargetCmd : AbstractCommand {
        private int playerID;
        private CardData cardData;

        public ChooseCardTargetCmd(int playerID, CardData cardData) {
            this.playerID = playerID;
            this.cardData = cardData;
        }

        protected override void OnExecute() {
            bool canChooseSelf;
            switch (cardData.CardType) {
                case CardType.EXCHANGE:
                case CardType.LOOT:
                    canChooseSelf = false;
                    break;
                case CardType.PROMOTE:
                    canChooseSelf = true;
                    break;
                default:
                    canChooseSelf = false;
                    UnityEngine.Debug.LogError($"[{nameof(ChooseCardTargetCmd)}] {nameof(OnExecute)}: CardType {cardData.CardType} should not be here.");
                    break;
            }
            this.SendEvent<ChooseCardTargetEvt>(new ChooseCardTargetEvt(playerID, canChooseSelf));
        }
    }

    public class ChooseCardTargetEvt {
        public int PlayerID;
        public bool CanChooseSelf;

        public ChooseCardTargetEvt(int playerID, bool canChooseSelf) {
            PlayerID = playerID;
            CanChooseSelf = canChooseSelf;
        }
    }

    public class CardTakeEffectCmd : AbstractCommand {
        private int playerID;
        private CardData cardData;
        private int targetID;

        public CardTakeEffectCmd(int playerID, CardData cardData) {
            this.playerID = playerID;
            this.cardData = cardData;
            this.targetID = -1;
        }

        public CardTakeEffectCmd(int playerID, CardData cardData, int targetID) : this(playerID, cardData) {
            this.targetID = targetID;
        }

        protected override void OnExecute() {
            // this.SendEvent<CardTakeEffectEvt>();

            IDeckSystem deckSystem = this.GetSystem<IDeckSystem>();

            switch (cardData.CardType) {
                case CardType.EXCHANGE:
                    deckSystem.ExchangeHandCards(playerID, targetID);
                    break;
                case CardType.LOOT:
                // DelareLootCmd
                case CardType.PROMOTE:
                case CardType.REVERSE:
                case CardType.SEE_THREE:
                case CardType.DETECT_POOP:
                    int closestPoopPos = deckSystem.DetectPoop();
                    break;
                case CardType.SHUFFLE:
                    deckSystem.ShuffleMainDeck();
                    break;
                case CardType.SKIP:
                case CardType.SWEEP:
                default:
                    break;
            }
        }
    }
}