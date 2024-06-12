using System.Collections.Generic;
using QFramework;

namespace Ressap.RadishCard {
    public interface IStrategySystem : ISystem {
        CardData AskPlay(int playerID);
    }

    public class DefaultStrategySystem : AbstractSystem, IStrategySystem {

        protected override void OnInit() {
        }
        public CardData AskPlay(int playerID) {
            IDeckSystem deckSystem = this.GetSystem<IDeckSystem>();

            List<CardData> legalCDs = deckSystem.GetLegalCardDatas(playerID);


            if (legalCDs.Count > 0) {
                int randIndex = UnityEngine.Random.Range(0, legalCDs.Count);
                CardData cdToPlay = legalCDs[randIndex];

                return cdToPlay;
            } else {
                return null;
            }
        }
    }
}