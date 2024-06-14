using System.Collections.Generic;
using QFramework;
using Ressap.Utils;

namespace Ressap.RadishCard {
    public interface IDeckSystem : ISystem {
        List<CardData> MainDeckDatas { get; }
        List<CardData> WasteDeckDatas { get; }
        CardData[] PlayedDeckDatas { get; }

        List<CardData>[] HandCardDatasArr { get; }

        void ResetMainDeck();
        void ShuffleMainDeck();
        void DealCards(int dealerID);

        void ExchangeHandCards(int playerID, int targetID);
        int DetectPoop();

        List<CardData> GetLegalCardDatas(int playerID);

        void PlayCard(int playerID, CardData cardData);

        void SaveDeckInfos();
    }

    public class DefaultDeckSystem : AbstractSystem, IDeckSystem {
        public List<CardData> MainDeckDatas { get; set; }
        public List<CardData> WasteDeckDatas { get; set; }
        public CardData[] PlayedDeckDatas { get; set; }

        private List<CardData> playerHandDatas;
        private List<CardData> leftComHandDatas;
        private List<CardData> acrossComHandDatas;
        private List<CardData> rightComHandDatas;

        public List<CardData>[] HandCardDatasArr { get; } = new List<CardData>[4];


        private IStorageUtility storageUtility;
        protected override void OnInit() {
            storageUtility = this.GetUtility<IStorageUtility>();

            loadDeckInfos();
        }

        private readonly Dictionary<CardType, int> deck_distribution_map = new Dictionary<CardType, int>() {
            {CardType.POOP, 3},
            {CardType.DETECT_POOP, 4},
            {CardType.SHUFFLE, 3},
            {CardType.SEE_THREE, 4},
            {CardType.REVERSE, 3},
            {CardType.LOOT, 3},
            {CardType.PROMOTE, 4},
            {CardType.SKIP, 5},
            {CardType.EXCHANGE, 3},
            {CardType.SWEEP, 5},
        };

        public void ResetMainDeck() {
            MainDeckDatas.Clear();
            WasteDeckDatas.Clear();
            PlayedDeckDatas.Fill(null);

            foreach (var h in HandCardDatasArr) {
                h.Clear();
            }

            foreach (var kvp in deck_distribution_map) {
                CardType cardType = kvp.Key;
                int num = kvp.Value;

                for (int i = 0; i < num; i++) {
                    MainDeckDatas.Add(new CardData(cardType, i));
                }
            }
        }

        public void ShuffleMainDeck() {
            int len = MainDeckDatas.Count;
            for (int i = 0; i < len; i++) {
                int randomIndex = UnityEngine.Random.Range(i, len);
                (MainDeckDatas[randomIndex], MainDeckDatas[i]) = (MainDeckDatas[i], MainDeckDatas[randomIndex]);
            }
        }

        public void DealCards(int dealerID) {
            int sweepIndex = 0;
            for (int i = 0; i < MainDeckDatas.Count; i++) {
                CardData cardData = MainDeckDatas[i];
                if (CardType.SWEEP.Equals(cardData.CardType)) {
                    (MainDeckDatas[sweepIndex], MainDeckDatas[i]) = (MainDeckDatas[i], MainDeckDatas[sweepIndex]);
                    sweepIndex++;
                    if (4 == sweepIndex) {
                        break;
                    }
                }
            }

            for (int offset = 0; offset < 4 * 5; offset++) {
                int lastCDIndex = MainDeckDatas.Count - 1;
                CardData cdToDeal = MainDeckDatas[lastCDIndex];
                int playerID = (dealerID + offset) % 4;
                HandCardDatasArr[playerID].Add(cdToDeal);
                MainDeckDatas.RemoveAt(lastCDIndex);
            }
        }

        public void ExchangeHandCards(int playerID, int targetID) {
            List<CardData> tempCDs = new List<CardData>();

            List<CardData> playerHandCDs = HandCardDatasArr[playerID];
            List<CardData> targetHandCDs = HandCardDatasArr[targetID];

            foreach (var cd in playerHandCDs) {
                tempCDs.Add(cd);
            }

            playerHandCDs.Clear();

            foreach (var cd in targetHandCDs) {
                playerHandCDs.Add(cd);
            }

            foreach (var cd in tempCDs) {
                targetHandCDs.Add(cd);
            }
        }

        public int DetectPoop() {
            int closestPoopPos = 0;
            for (int i = MainDeckDatas.Count - 1; i >= 0; i--) {
                closestPoopPos++;
                if (CardType.POOP.Equals(MainDeckDatas[i].CardType)) {
                    break;
                }
            }
            return closestPoopPos;
        }



        public void PlayCard(int playerID, CardData cardData) {
            if (HandCardDatasArr[playerID].Remove(cardData)) {
                WasteDeckDatas.Add(cardData);
            } else {
                UnityEngine.Debug.LogError($"[{nameof(IDeckSystem)}] {nameof(PlayCard)}: Can not find card {cardData} in player-{playerID}'s hand.");
            }
        }

        public List<CardData> GetLegalCardDatas(int playerID) {
            List<CardData> legalCDs = new List<CardData>();
            foreach (var cd in HandCardDatasArr[playerID]) {
                if (!CardType.SWEEP.Equals(cd.CardType)) {
                    legalCDs.Add(cd);
                }
            }

            return legalCDs;
        }


        private const string save_path = Const.DECK_INFO_SAVE_PATH;
        public void SaveDeckInfos() {
            storageUtility.Save<List<CardData>>(nameof(MainDeckDatas), MainDeckDatas, save_path);
            storageUtility.Save<List<CardData>>(nameof(WasteDeckDatas), WasteDeckDatas, save_path);
            storageUtility.Save<CardData[]>(nameof(PlayedDeckDatas), PlayedDeckDatas, save_path);

            storageUtility.Save<List<CardData>>(nameof(playerHandDatas), playerHandDatas, save_path);
            storageUtility.Save<List<CardData>>(nameof(leftComHandDatas), leftComHandDatas, save_path);
            storageUtility.Save<List<CardData>>(nameof(acrossComHandDatas), acrossComHandDatas, save_path);
            storageUtility.Save<List<CardData>>(nameof(rightComHandDatas), rightComHandDatas, save_path);
        }

        private void loadDeckInfos() {
            MainDeckDatas = storageUtility.Load<List<CardData>>(nameof(MainDeckDatas), save_path, new List<CardData>());
            WasteDeckDatas = storageUtility.Load<List<CardData>>(nameof(WasteDeckDatas), save_path, new List<CardData>());
            PlayedDeckDatas = storageUtility.Load<CardData[]>(nameof(PlayedDeckDatas), save_path, new CardData[4]);

            HandCardDatasArr[0] = playerHandDatas = storageUtility.Load<List<CardData>>(nameof(playerHandDatas), save_path, new List<CardData>());
            HandCardDatasArr[1] = leftComHandDatas = storageUtility.Load<List<CardData>>(nameof(leftComHandDatas), save_path, new List<CardData>());
            HandCardDatasArr[2] = acrossComHandDatas = storageUtility.Load<List<CardData>>(nameof(acrossComHandDatas), save_path, new List<CardData>());
            HandCardDatasArr[3] = rightComHandDatas = storageUtility.Load<List<CardData>>(nameof(rightComHandDatas), save_path, new List<CardData>());
        }
    }




    public class CardData {
        public CardType CardType;
        public int Index;

        public string ID {
            get {
                return $"{CardType}-{Index}";
            }
        }

        public CardData() {
        }

        public CardData(CardType cardType, int index) {
            CardType = cardType;
            Index = index;
        }

        public override string ToString() {
            return ID;
        }
    }

    public enum CardType {
        NONE = 0,
        POOP = 1,
        DETECT_POOP = 2,
        SHUFFLE = 3,
        SEE_THREE = 4,
        REVERSE = 5,
        LOOT = 6,
        PROMOTE = 7,
        SKIP = 8,
        EXCHANGE = 9,
        SWEEP = 10
    }
}