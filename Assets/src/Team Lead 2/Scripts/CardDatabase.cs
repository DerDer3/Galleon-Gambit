using UnityEngine;

/* Template

public class  : Card
{
  public () : base("", "", , "", 1) { }

  Effect effect1;

  public override void Play(GameState state)
  {
    Debug.Log("Played ");
    effect1.Apply(state);

    int currentMana = state.mana.get_amount();
    state.mana.set_amount(currentMana - this.cardCost);
  }

}

*/

public class Slash : Card
{
    public Slash() : base("Slash", "Common", 1, "Apply 5 damage to enemy", 1) { }

    Effect effect1 = new DamageEffect(5);

    public override void Play(GameState state)
    {
      Debug.Log("Played Slash");
      effect1.Apply(state);

      int currentMana = state.mana.get_amount();
      state.mana.set_amount(currentMana - this.cardCost);
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
    state.mana.set_amount(currentMana - this.cardCost);
  }
}

public class PistolShot : Card
{
  public PistolShot() : base("Pistol Shot", "Common", 2, "Deal 12 Damage", 1) { }

  Effect effect1 = new DamageEffect(12);

  public override void Play(GameState state)
  {
    Debug.Log("Played Pistol Shot");
    effect1.Apply(state);

    int currentMana = state.mana.get_amount();
    state.mana.set_amount(currentMana - this.cardCost);
  }

}

public class TwinBlades : Card
{
  public TwinBlades() : base("Twin Blades", "Common", 1, "Deal 4 Damage Twice", 1) { }

  Effect effect1 = new DamageEffect(4);
  Effect effect2 = new DamageEffect(4);

  public override void Play(GameState state)
  {
    Debug.Log("Played Twin Blades");
    effect1.Apply(state);
    effect2.Apply(state);

    int currentMana = state.mana.get_amount();
    state.mana.set_amount(currentMana - this.cardCost);
  }

}

public class BoardingCharge : Card
{
  public BoardingCharge() : base("Boarding Charge", "Common", 1, "Deal 6 Damage and Gain 3 Health", 1) { }

  Effect effect1 = new DamageEffect(6);
  Effect effect2 = new HealEffect(3);

  public override void Play(GameState state)
  {
    Debug.Log("Played Boarding Charge");
    effect1.Apply(state);

    int currentMana = state.mana.get_amount();
    state.mana.set_amount(currentMana - this.cardCost);
  }

}

public class BackStab : Card
{
  public BackStab() : base("Back Stab", "Common", 1, "Deal 10 Damage if full on mana, otherwise deal 5", 1) { }

  Effect effect1 = new DamageEffect(10);
  Effect effect2 = new DamageEffect(5);

  public override void Play(GameState state)
  {
    Debug.Log("Played Back Stab");
    int currentMana = state.mana.get_amount();

    if(currentMana == 3)
      effect1.Apply(state);
    else
      effect2.Apply(state);

    state.mana.set_amount(currentMana - this.cardCost);
  }

}

public class WhispersBelow : Card
{
  public WhispersBelow() : base("Whispers Below", "Common", 1, "Lose 5 HP, Draw 2 Cards", 1) { }

  Effect effect1 = new HealEffect(-5);
  // Effect effect2 = new CardDraw();

  public override void Play(GameState state)
  {
    Debug.Log("Played ");
    effect1.Apply(state);

    int currentMana = state.mana.get_amount();
    state.mana.set_amount(currentMana - this.cardCost);
  }

}

// Have more ideas for cards but need to add more effects
