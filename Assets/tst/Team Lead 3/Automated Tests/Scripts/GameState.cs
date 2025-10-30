using UnityEngine;
using System.Collections.Generic;

public class GameState : MonoBehaviour
{
  public Player mainPlayer;
  public List<Enemy> enemies = new List<Enemy>();
  public Enemy currentEnemy;
  public List<Card> cards = new List<Card>();
  public bool turn = false; // 0 for Player, 1 for Enemy
  public Mana mana;
}
