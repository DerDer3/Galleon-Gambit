using UnityEngine;

public class Slash : Card
{
    public Slash() : base("Slash", "Common", 1, "Apply 5 damage to enemy", 1) { }

    Effect effect1 = new DamageEffect(5);

    public override void Play(GameState state)
    {
      Debug.Log("Played Slash");
      effect1.Apply(state);

      int currentMana = state.mana.get_amount();
      state.mana.set_amount(currentMana - 1);
    }
}

public class ShipRepair : Card
{
  public ShipRepair() : base("Ship Repair", "Common", 1, "Heal 5 ship health", 1) { }

  Effect effect1 = new HealEffect(5);

  public override void Play(GameState state)
  {
    Debug.Log("Played Ship Repair");
    effect1.Apply(state);

    int currentMana = state.mana.get_amount();
    state.mana.set_amount(currentMana - 1);
  }
}
