using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Ressap.RadishCard {
    public class DeckController : MonoBehaviour, IController {

        [SerializeField] private Transform collectedCardsTF;
        private const string card_prefab_path = "Prefabs/Card";
        private CardObjectPool cardObjPool;

        [SerializeField] private MainDeck mainDeck;
        [SerializeField] private PlayedDeck playedDeck;
        [SerializeField] private WasteDeck wasteDeck;
        [SerializeField] private Hand selfHand;
        [SerializeField] private Hand leftHand;
        [SerializeField] private Hand acrossHand;
        [SerializeField] private Hand rightHand;

        private Dictionary<string, Card> cardMap;

        private IDeckSystem deckSystem;
        private void Awake() {
            Card cardPrefab = Resources.Load<Card>(card_prefab_path);
            cardObjPool = new CardObjectPool(collectedCardsTF, cardPrefab);

            deckSystem = this.GetSystem<IDeckSystem>();

            this.RegisterEvent<NewGameEvt>(OnNewGame).UnRegisterWhenGameObjectDestroyed(this.gameObject);
            this.RegisterEvent<InitialShuffleEvt>(OnInitialShuffle).UnRegisterWhenGameObjectDestroyed(this.gameObject);
            this.RegisterEvent<DealCardEvt>(OnDealCard).UnRegisterWhenGameObjectDestroyed(this.gameObject);
            this.RegisterEvent<AskPlayerPlayEvt>(OnAskPlayerPlay).UnRegisterWhenGameObjectDestroyed(this.gameObject);
            this.RegisterEvent<PlayCardEvt>(OnPlayCard).UnRegisterWhenGameObjectDestroyed(this.gameObject);
            this.RegisterEvent<ChooseCardTargetEvt>(OnChooseCardTarget).UnRegisterWhenGameObjectDestroyed(this.gameObject);
        }

        private void OnNewGame(NewGameEvt evt) {
            resetAllCards();
            this.SendCommand<InitialShuffleCmd>();
        }

        private void OnInitialShuffle(InitialShuffleEvt evt) {
            // shuffleAnim();
            this.SendCommand<DealCardCmd>();
        }

        private void OnDealCard(DealCardEvt evt) {
            // dealCardAnim();
            this.SendCommand<AskPlayCardCmd>(new AskPlayCardCmd(evt.NextPlayerID));
        }

        private void OnAskPlayerPlay(AskPlayerPlayEvt evt) {
            // make player hand interactable
        }

        private void OnPlayCard(PlayCardEvt evt) {
            // play card anim
            int playerID = evt.PlayerID;
            CardData cardData = evt.CardData;

            if (evt.IsNeedTarget) {
                this.SendCommand(new ChooseCardTargetCmd(playerID, cardData));
            } else {
                this.SendCommand<CardTakeEffectCmd>(new CardTakeEffectCmd(playerID, cardData));
            }
        }

        private void OnChooseCardTarget(ChooseCardTargetEvt evt) {
            // make players interactable
        }

        private void resetAllCards() {
            foreach (var kvp in cardMap) {
                cardObjPool.Collect(kvp.Value);
            }
            cardMap.Clear();


            IEnumerable<CardData>[] cardPileDatas = new IEnumerable<CardData>[]{
                deckSystem.MainDeckDatas,
                deckSystem.WasteDeckDatas,
                deckSystem.HandCardDatasArr[Const.SELF_INDEX],
                deckSystem.HandCardDatasArr[Const.LEFT_INDEX],
                deckSystem.HandCardDatasArr[Const.ACROSS_INDEX],
                deckSystem.HandCardDatasArr[Const.RIGHT_INDEX],
                deckSystem.PlayedDeckDatas,
            };

            foreach (var cdp in cardPileDatas) {
                foreach (var cd in cdp) {
                    if (null == cd) {
                        continue;
                    }
                    Card card = cardObjPool.Get();
                    card.ResetWithData(cd.CardType, cd.Index);
                    cardMap[cd.ID] = card;
                }
            }
        }

        public IArchitecture GetArchitecture() {
            return RadishCardApp.Interface;
        }
    }
}