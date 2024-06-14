using QFramework;
using UnityEngine;

namespace Ressap.RadishCard {
    public class GameController : MonoBehaviour, IController {
        private void Awake() {
        }

        private void Start() {
            this.GetSystem<IUISystem>().GetPanel<MainMenuPanel>().ShowPanel();
        }

        public IArchitecture GetArchitecture() {
            return RadishCardApp.Interface;
        }
    }
}