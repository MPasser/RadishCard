using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Ressap.RadishCard {
    public class ClickHandler : MonoBehaviour, IPointerClickHandler {
        public UnityAction OnClick;

        private void Start() {
        }

        public void OnPointerClick(PointerEventData eventData) {
            if (null != OnClick) {
                OnClick.Invoke();
            } else {
                Debug.Log($"[{nameof(ClickHandler)}] {nameof(OnPointerClick)}: OnClick is null");
            }
        }
    }
}