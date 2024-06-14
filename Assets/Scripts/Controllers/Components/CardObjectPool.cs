using Ressap.Utils;
using UnityEngine;

namespace Ressap.RadishCard {
    public class CardObjectPool : ObjectPool<Card> {
        private Transform poolTransform;
        private Card cardPrefab;

        public CardObjectPool(Transform poolTransform, Card cardPrefab) {
            this.poolTransform = poolTransform;
            this.cardPrefab = cardPrefab;
        }

        protected override void DisposeObj(Card card) {
            Object.Destroy(card);
        }

        protected override Card InstantiateObj() {
            return Object.Instantiate(cardPrefab);
        }

        protected override void onCollected(Card card) {
            card.transform.SetParent(poolTransform);
            card.transform.localScale = Vector2.one;
            card.transform.localPosition = Vector2.zero;
        }

        protected override void onInstantiate(Card card) {
        }
    }
}