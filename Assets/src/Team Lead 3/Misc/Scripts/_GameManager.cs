using System.Collections.Generic;
using UnityEngine;

namespace GallionGambit
{
    public class GameManager : MonoBehaviour
    {
        private Player player;
        private Enemy enemy;
        [SerializeField] private DeckManager deckManager;

        private List<Card> hand;
        private bool playerTurn = true;

        void Start()
        {
            player = new Player();
            enemy = new Enemy();

            StartPlayerTurn();
        }

        void StartPlayerTurn()
        {
            Debug.Log("=== Player Turn ===");
            playerTurn = true;
            player.RefillMana();

            hand = deckManager.Draw(5);
            for (int i = 0; i < hand.Count; i++)
            {
                Debug.Log($"{i + 1}. {hand[i].cardName} (Cost {hand[i].manaCost})");
            }
        }

        void Update()
        {
            if (!playerTurn) return;

            if (Input.GetKeyDown(KeyCode.Alpha1)) PlayCard(0);
            if (Input.GetKeyDown(KeyCode.Alpha2)) PlayCard(1);
            if (Input.GetKeyDown(KeyCode.Alpha3)) PlayCard(2);
            if (Input.GetKeyDown(KeyCode.Alpha4)) PlayCard(3);
            if (Input.GetKeyDown(KeyCode.Alpha5)) PlayCard(4);

            if (Input.GetKeyDown(KeyCode.Space)) EndPlayerTurn();
        }

        void PlayCard(int index)
        {
            if (index >= hand.Count) return;

            var card = hand[index];
            if (player.mana < card.manaCost)
            {
                Debug.Log("Not enough mana!");
                return;
            }

            player.SpendMana(card.manaCost);

            foreach (var type in card.cardType)
            {
                switch (type)
                {
                    case Card.CardType.Heal:
                        player.Heal(card.heal);
                        break;
                    case Card.CardType.Damage:
                        enemy.TakeDamage(card.damage);
                        break;
                    case Card.CardType.other:
                        Debug.Log("Special card effect not implemented yet.");
                        break;
                }
            }

            deckManager.Discard(card);
            hand.RemoveAt(index);

            if (enemy.IsDead())
            {
                Debug.Log("Enemy defeated! You win!");
                enabled = false;
            }
        }

        void EndPlayerTurn()
        {
            Debug.Log("=== Enemy Turn ===");
            playerTurn = false;

            enemy.Attack(player);
            if (player.IsDead())
            {
                Debug.Log("You were defeated!");
                enabled = false;
                return;
            }

            StartPlayerTurn();
        }
    }
}
