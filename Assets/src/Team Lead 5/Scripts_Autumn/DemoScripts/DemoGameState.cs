using UnityEngine;
using System.Collections.Generic;

public class DemoGameState : MonoBehaviour
{
  public DemoPlayer mainPlayer;
  public List<DemoEnemy> enemies = new List<DemoEnemy>();
  //public DemoEnemy currentEnemy;
  public DemoPlayerDeck mainDeck = new DemoPlayerDeck();
  public bool turn = false; // 0 for Player, 1 for Enemy
  public DemoMana mana;
}
