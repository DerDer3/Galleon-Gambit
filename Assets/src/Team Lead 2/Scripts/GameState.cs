using UnityEngine;
using System.Collections.Generic;

public class GameState : MonoBehaviour
{
  public Player mainPlayer;
  public List<Enemy> enemies = new List<Enemy>();
  public Enemy currentEnemy;
  public PlayerDeck mainDeck = new PlayerDeck();
  public bool turn = false; // 0 for Player, 1 for Enemy
  public Mana mana;
}
