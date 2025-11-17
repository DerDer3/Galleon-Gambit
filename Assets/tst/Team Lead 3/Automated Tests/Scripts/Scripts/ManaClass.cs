using UnityEngine;
using TMPro;
using System;

public class ManaClass : MonoBehaviour
{
    // --- DYNAMIC OBSERVER PATTERN IMPLEMENTATION ---
    public static Action<int> OnManaChanged;

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

        // Dynamic addition: Invoke the event whenever mana changes
        OnManaChanged?.Invoke(manaAmount);
    }

    public void set_max_amount(int x) { maxMana = x; }

    // Dynamic addition: Subscription logic to update the local UI text
    private void OnEnable()
    {
        OnManaChanged += UpdateManaTextUI;
        OnManaChanged?.Invoke(manaAmount);
    }

    private void OnDisable()
    {
        OnManaChanged -= UpdateManaTextUI;
    }

    private void UpdateManaTextUI(int newMana)
    {
        if (manaText != null)
        {
            manaText.text = "" + newMana;
        }
    }
}