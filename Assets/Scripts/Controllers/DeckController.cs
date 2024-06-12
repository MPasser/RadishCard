using QFramework;
using UnityEngine;

namespace Ressap.RadishCard {
    public class DeckController : MonoBehaviour, IController {

        private void Awake() {
            this.RegisterEvent<NewGameEvt>(OnNewGame).UnRegisterWhenGameObjectDestroyed(this.gameObject);
            this.RegisterEvent<InitialShuffleEvt>(OnInitialShuffle).UnRegisterWhenGameObjectDestroyed(this.gameObject);
            this.RegisterEvent<DealCardEvt>(OnDealCard).UnRegisterWhenGameObjectDestroyed(this.gameObject);
            this.RegisterEvent<AskPlayerPlayEvt>(OnAskPlayerPlay).UnRegisterWhenGameObjectDestroyed(this.gameObject);
            this.RegisterEvent<PlayCardEvt>(OnPlayCard).UnRegisterWhenGameObjectDestroyed(this.gameObject);
            this.RegisterEvent<ChooseCardTargetEvt>(OnChooseCardTarget).UnRegisterWhenGameObjectDestroyed(this.gameObject);
        }

        private void OnNewGame(NewGameEvt evt) {
            // resetMainDeck();
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

        public IArchitecture GetArchitecture() {
            return RadishCardApp.Interface;
        }
    }
}