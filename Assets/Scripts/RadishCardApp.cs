using QFramework;

namespace Ressap.RadishCard {
    public class RadishCardApp : Architecture<RadishCardApp> {
        protected override void Init() {
            this.RegisterUtility<IStorageUtility>(new ES3StorageUtility());

            this.RegisterModel<IGameInfoModel>(new DefaultGameInfoModel());

            this.RegisterSystem<IDeckSystem>(new DefaultDeckSystem());
            this.RegisterSystem<IGameSaveSystem>(new DefaultGameSaveSystem());
            this.RegisterSystem<IStrategySystem>(new DefaultStrategySystem());
            this.RegisterSystem<IUISystem>(new DefaultUISystem());
        }
    }
}
