using QFramework;
namespace Ressap.RadishCard {
    public interface IGameInfoModel : IModel {
        int NextPlayerID { get; set; }
        int ExtraPlayCnt { get; set; }

        bool IsClockwise { get; set; }

        void SaveGameInfos();
    }

    public class DefaultGameInfoModel : AbstractModel, IGameInfoModel {
        public int NextPlayerID { get; set; }
        public int ExtraPlayCnt { get; set; }

        public bool IsClockwise { get; set; }

        IStorageUtility storageUtility;
        protected override void OnInit() {
            storageUtility = this.GetUtility<IStorageUtility>();

            loadGameInfos();
        }

        private const string save_path = Const.GAME_INFO_SAVE_PATH;

        public void SaveGameInfos() {
            storageUtility.Save<int>(nameof(NextPlayerID), NextPlayerID, save_path);
            storageUtility.Save<int>(nameof(ExtraPlayCnt), ExtraPlayCnt, save_path);
        }

        private void loadGameInfos() {
            NextPlayerID = storageUtility.Load<int>(nameof(NextPlayerID), save_path, 0);
            ExtraPlayCnt = storageUtility.Load<int>(nameof(ExtraPlayCnt), save_path, 0);
        }
    }
}