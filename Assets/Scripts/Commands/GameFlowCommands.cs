using QFramework;

namespace Ressap.RadishCard {
    public class NewGameCmd : AbstractCommand {
        protected override void OnExecute() {
            IGameInfoModel gameInfoModel = this.GetModel<IGameInfoModel>();

            gameInfoModel.NextPlayerID = UnityEngine.Random.Range(0, 4);
            gameInfoModel.IsClockwise = true;

            this.GetSystem<IDeckSystem>().ResetMainDeck();

            this.SendEvent<NewGameEvt>();
        }
    }

    public class NewGameEvt {
        public NewGameEvt() {
        }
    }

    public class InitialShuffleCmd : AbstractCommand {
        protected override void OnExecute() {
            this.GetSystem<IDeckSystem>().ShuffleMainDeck();

            this.SendEvent<InitialShuffleEvt>();
        }
    }

    public class InitialShuffleEvt {
        public InitialShuffleEvt() {
        }
    }

    public class DealCardCmd : AbstractCommand {
        protected override void OnExecute() {
            int nextPlayerID = this.GetModel<IGameInfoModel>().NextPlayerID;
            
            this.GetSystem<IDeckSystem>().DealCards(nextPlayerID);
            this.SendEvent<DealCardEvt>(new DealCardEvt(nextPlayerID));
        }
    }

    public class DealCardEvt {
        public int NextPlayerID;

        public DealCardEvt(int nextPlayerID) {
            NextPlayerID = nextPlayerID;
        }
    }
}