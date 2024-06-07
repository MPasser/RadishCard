using QFramework;
namespace Ressap.RadishCard {
    public interface IGameInfoModel : IModel {
        int DealerID { get; set; }
        
        bool IsClockwise { get; set; }
        void SaveGameInfos();
    }

    public class DefaultGameInfoModel : AbstractModel, IGameInfoModel {
        public int DealerID { get; set; }

        public bool IsClockwise { get; set; }

        IStorageUtility storageUtility;
        protected override void OnInit() {
            storageUtility = this.GetUtility<IStorageUtility>();

            loadGameInfos();
        }

        public void SaveGameInfos() {

        }

        private void loadGameInfos() {

        }
    }
}