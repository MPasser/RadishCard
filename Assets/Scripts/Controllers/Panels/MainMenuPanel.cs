using QFramework;
using UnityEngine;
using UnityEngine.UI;

namespace Ressap.RadishCard {
    public class MainMenuPanel : AbstractPanel {
        [SerializeField] private Button btnStart;

        protected override void Awake() {
            base.Awake();

            btnStart.onClick.AddListener(OnBtnStartClick);
        }

        private void OnBtnStartClick() {
            this.HidePanel();
            this.SendCommand<NewGameCmd>();
            uiSystem.CollectPanel(this);
        }
    }
}