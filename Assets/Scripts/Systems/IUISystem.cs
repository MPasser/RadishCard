using System.Collections.Generic;
using DG.Tweening;
using QFramework;
using UnityEngine;

namespace Ressap.RadishCard {
    public interface IUISystem : ISystem {
        void RegisterController(AbstractUIController uiController);
        T GetPanel<T>() where T : AbstractPanel;
        void CollectPanel<T>() where T : AbstractPanel;
        void CollectPanel(AbstractPanel panel);
    }

    public class DefaultUISystem : AbstractSystem, IUISystem {
        private const string panel_dir_path = "Prefabs/Panels/";
        private AbstractUIController uiController;
        private IOCContainer container;
        private HashSet<AbstractPanel> collectedSet;
        protected override void OnInit() {
            container = new IOCContainer();
            collectedSet = new HashSet<AbstractPanel>();
        }

        public void RegisterController(AbstractUIController uiController) {
            this.uiController = uiController;
        }

        public T GetPanel<T>() where T : AbstractPanel {
            T panel = container.Get<T>();
            if (null == panel) {
                T panelPrefab = Resources.Load<T>(panel_dir_path + typeof(T).Name);
                if (null == panelPrefab) {
                    Debug.LogError($"[{nameof(DefaultUISystem)}] {nameof(GetPanel)}: Can not find prefab `{panel_dir_path + typeof(T).Name}`");
                    return null;
                }

                panel = Object.Instantiate<T>(panelPrefab);

                if (null == panel) {
                    Debug.LogError($"[{nameof(DefaultUISystem)}] {nameof(GetPanel)}: Can not get {typeof(T).Name} component from instance.");
                    return null;
                } else {
                    panel.transform.SetParent(uiController.GetLayerRT(panel.PanelLayer));
                    panel.transform.SetAsLastSibling();
                    panel.OnInstantiate();
                    container.Register<T>(panel);
                }
            } else {
                if (collectedSet.Contains(panel)) {
                    panel.transform.SetParent(uiController.GetLayerRT(panel.PanelLayer));
                    panel.transform.SetAsLastSibling();
                    panel.OnInstantiate();
                    collectedSet.Remove(panel);
                }
            }
            return panel;
        }

        public void CollectPanel<T>() where T : AbstractPanel {
            T panel = container.Get<T>();
            if (null == panel || collectedSet.Contains(panel)) {
                return;
            } else {
                CollectPanel(panel);
            }
        }

        public void CollectPanel(AbstractPanel panel) {
            panel.OnCollected();
            collectedSet.Add(panel);
            uiController.CollectPanel(panel);
        }
    }

    public abstract class AbstractPanel : MonoBehaviour, IController {
        public PanelLayer PanelLayer;
        protected RectTransform rt;
        protected CanvasGroup canvasGroup;
        [SerializeField] protected RectTransform content;

        protected IUISystem uiSystem;
        protected virtual void Awake() {
            uiSystem = this.GetSystem<IUISystem>();

            canvasGroup = this.GetComponent<CanvasGroup>();
            rt = this.GetComponent<RectTransform>();
        }

        public virtual void OnInstantiate() {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
            rt.localPosition = Vector2.zero;
        }

        public virtual void OnCollected() {

        }

        public virtual Tween ShowPanelAnim() {
            Sequence anim = DOTween.Sequence();

            float dura = 0.3f;

            canvasGroup.interactable = false;
            content.localScale = Vector2.zero;
            rt.localScale = Vector2.one;

            anim.Append(content.DOScale(1, dura));
            anim.OnComplete(() => {
                canvasGroup.interactable = true;
            });

            return anim;
        }

        public virtual void ShowPanel() {
            content.localScale = Vector2.one;
            rt.localScale = Vector2.one;
            canvasGroup.interactable = true;
        }

        public virtual Tween HidePanelAnim() {
            Sequence anim = DOTween.Sequence();

            float dura = 0.3f;

            canvasGroup.interactable = false;
            content.localScale = Vector2.one;
            rt.localScale = Vector2.one;

            anim.Append(content.DOScale(0, dura));
            anim.OnComplete(() => {
                rt.localScale = Vector2.zero;
            });

            return anim;
        }

        public virtual void HidePanel() {
            canvasGroup.interactable = false;
            rt.localScale = Vector2.zero;
            content.localScale = Vector2.zero;
        }

        public IArchitecture GetArchitecture() {
            return RadishCardApp.Interface;
        }
    }

    public abstract class AbstractUIController : MonoBehaviour, IController {
        [SerializeField] private RectTransform bottomLayer;
        [SerializeField] private RectTransform hudLayer;
        [SerializeField] private RectTransform promptLayer;

        [SerializeField] private Transform collectedPanelContainer;


        private IUISystem uiSystem;
        private void Awake() {
            uiSystem = this.GetSystem<IUISystem>();
            uiSystem.RegisterController(this);

            RegisterUIEvents();
        }

        protected abstract void RegisterUIEvents();

        public RectTransform GetLayerRT(PanelLayer layer) {
            return layer switch {
                PanelLayer.BOTTOM => bottomLayer,
                PanelLayer.HUD => hudLayer,
                PanelLayer.PROMPT => promptLayer,
                _ => promptLayer,
            };
        }

        public void CollectPanel(AbstractPanel panel) {
            panel.transform.SetParent(collectedPanelContainer);
        }

        public IArchitecture GetArchitecture() {
            return RadishCardApp.Interface;
        }
    }


    public enum PanelLayer {
        BOTTOM = 1,
        HUD = 2,
        PROMPT = 3,
    }
}