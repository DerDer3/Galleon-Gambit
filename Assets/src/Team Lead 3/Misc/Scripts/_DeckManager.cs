using System.Collections.Generic;
using UnityEngine;

namespace GallionGambit
{
    public class DeckManager : MonoBehaviour
    {
        [SerializeField] private List<Card> allCards; // assign in Inspector
        private List<Card> deck = new();
        private List<Card> discardPile = new();
        private System.Random rng = new();

        void Awake()
        {
            // Copy all cards into the active deck
            deck = new List<Card>(allCards);
            Shuffle(deck);
        }

        public List<Card> Draw(int count)
        {
            List<Card> hand = new();
            for (int i = 0; i < count; i++)
            {
                if (deck.Count == 0) ReshuffleDiscardIntoDeck();
                if (deck.Count == 0) break;

                var card = deck[0];
                deck.RemoveAt(0);
                hand.Add(card);
            }
            return hand;
        }

        public void Discard(Card card)
        {
            discardPile.Add(card);
        }

        private void ReshuffleDiscardIntoDeck()
        {
            if (discardPile.Count == 0) return;
            deck.AddRange(discardPile);
            discardPile.Clear();
            Shuffle(deck);
            Debug.Log("Deck reshuffled!");
        }

        private void Shuffle(List<Card> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }
}
