using UnityEngine;
//using TMPro;

public class Player : MonoBehaviour
{
    private int playerHealth = 100;
    private int playerMana;
    //public TextMeshProUGUI healthText;

    public int get_health() { return playerHealth; }
    public int get_mana() { return playerMana; }
    public void set_health(int x) { playerHealth = x; }
    public void set_mana(int x) { playerMana = x; }

    void Update()
    {
      //healthText.text = "" + playerHealth;
    }
}
