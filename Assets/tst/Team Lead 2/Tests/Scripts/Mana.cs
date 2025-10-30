using UnityEngine;
using TMPro;

public class Mana : MonoBehaviour
{
  private int manaAmount = 3;
  public TextMeshProUGUI manaText;

  public int get_amount() { return manaAmount; }
  public void set_amount(int x) { manaAmount = x; }

  void Update()
  {
    manaText.text = "" + manaAmount;
  }
}
