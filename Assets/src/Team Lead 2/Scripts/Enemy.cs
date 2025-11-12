using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
  private int enemyHealth = 100;
  public TextMeshProUGUI healthText;
  // Can add more enemy stuff later

  public int get_health() { return enemyHealth; }
  public void set_health(int x) { enemyHealth = x; }

  void Update()
  {
    healthText.text = "" + enemyHealth;
  }
}
