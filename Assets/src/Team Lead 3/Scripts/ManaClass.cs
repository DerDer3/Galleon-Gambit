using UnityEngine;
using TMPro;

// Renamed Mana (from TL2_Mana.cs)
public class ManaClass : MonoBehaviour
{
    // Changed to private fields with properties/getters for controlled access
    private int maxMana = 3;
    private int manaAmount = 3;

    public TextMeshProUGUI manaText;

    public int get_amount() { return manaAmount; }
    public int get_max_amount() { return maxMana; }

    // Use PascalCase for public methods in C# convention
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