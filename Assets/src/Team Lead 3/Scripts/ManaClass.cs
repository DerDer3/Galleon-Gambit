using UnityEngine;
using TMPro;

public class ManaClass : MonoBehaviour
{
    private int maxMana = 100;
    private int manaAmount = 3;

    public TextMeshProUGUI manaText;

    public int get_amount() { return manaAmount; }
    public int get_max_amount() { return maxMana; }

    public void set_amount(int x)
    {
        manaAmount = x;
        // Ensure mana doesn't exceed max
        if (manaAmount > maxMana) manaAmount = maxMana;
    }

    public void set_max_amount(int x) { maxMana = x; }

    void Update()
    {
        // Update the UI text
        if (manaText != null)
        {
            manaText.text = "" + manaAmount;
        }
    }
}