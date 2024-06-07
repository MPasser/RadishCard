using System.Collections.Generic;
using QFramework;

namespace Ressap.RadishCard {
    public interface IDeckSystem : ISystem {
        List<CardData> MainDeckDatas { get; }
        List<CardData>[] HandCardDatasArr { get; }

        void ResetMainDeck();
        void ShuffleMainDeck();
        void DealCards(int dealerID);

        void PlayCard(int playerID, CardData cardData);

        void SaveDeckInfos();
    }

    public class DefaultDeckSystem : AbstractSystem, IDeckSystem {
        public List<CardData> MainDeckDatas { get; set; }

        private List<CardData> playerHandDatas;
        private List<CardData> leftComHandDatas;
        private List<CardData> acrossComHandDatas;
        private List<CardData> rightComHandDatas;

        public List<CardData>[] HandCardDatasArr { get; } = new List<CardData>[4];


        private IStorageUtility storageUtility;
        protected override void OnInit() {
            storageUtility = this.GetUtility<IStorageUtility>();
        }


        public void ResetMainDeck() {

        }

        public void ShuffleMainDeck() {

        }

        public void DealCards(int dealerID) {

        }

        public void PlayCard(int playerID, CardData cardData) {

        }



        private const string savePath = Const.DECK_INFO_SAVE_PATH;
        public void SaveDeckInfos() {
            storageUtility.Save<List<CardData>>(nameof(MainDeckDatas), MainDeckDatas, savePath);

            storageUtility.Save<List<CardData>>(nameof(playerHandDatas), playerHandDatas, savePath);
            storageUtility.Save<List<CardData>>(nameof(leftComHandDatas), leftComHandDatas, savePath);
            storageUtility.Save<List<CardData>>(nameof(acrossComHandDatas), acrossComHandDatas, savePath);
            storageUtility.Save<List<CardData>>(nameof(rightComHandDatas), rightComHandDatas, savePath);
        }

        private void loadDeckInfos() {
            HandCardDatasArr[0] = playerHandDatas = storageUtility.Load<List<CardData>>(nameof(playerHandDatas), savePath, new List<CardData>());
            HandCardDatasArr[1] = leftComHandDatas = storageUtility.Load<List<CardData>>(nameof(leftComHandDatas), savePath, new List<CardData>());
            HandCardDatasArr[2] = acrossComHandDatas = storageUtility.Load<List<CardData>>(nameof(acrossComHandDatas), savePath, new List<CardData>());
            HandCardDatasArr[3] = rightComHandDatas = storageUtility.Load<List<CardData>>(nameof(rightComHandDatas), savePath, new List<CardData>());
        }
    }




    public class CardData {
        public CardType CardType;
        public int Index;
    }

    public enum CardType {
        POOP = 1,
        FORESEE = 2,
        SHUFFLE = 3,
        AWARE = 4,
        TURN = 5,
        DEMAND = 6,
        FORCE = 7,
        SKIP = 8,
        EXCHANGE = 9,
        SWEEP = 10
    }
}