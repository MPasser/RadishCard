using QFramework;

namespace Ressap.RadishCard {
    public interface IGameSaveSystem : ISystem {
        void SaveGame();

    }

    public class DefaultGameSaveSystem : AbstractSystem, IGameSaveSystem {
        protected override void OnInit() {
        }

        public void SaveGame() {
            this.GetModel<IGameInfoModel>().SaveGameInfos();

            this.GetSystem<IDeckSystem>().SaveDeckInfos();
        }
    }
}