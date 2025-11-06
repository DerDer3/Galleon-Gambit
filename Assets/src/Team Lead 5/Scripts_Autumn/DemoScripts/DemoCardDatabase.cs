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

public class DemoSlash : DemoCard
{
    public DemoSlash() : base("Slash", "Common", 1, "Apply 5 damage to enemy", 1) { }

    DemoEffect effect1 = new DemoDamageEffect(5);

    public override void Play(DemoGameState state)
    {
      Debug.Log("Played Slash");
      effect1.Apply(state);

      int currentMana = state.mana.get_amount();
      state.mana.set_amount(currentMana - this.cardCost);
    }
}

public class DemoShipRepair : DemoCard
{
  public DemoShipRepair() : base("Ship Repair", "Common", 1, "Heal 5 ship health", 1) { }

  DemoEffect effect1 = new DemoHealEffect(5);

  public override void Play(DemoGameState state)
  {
    Debug.Log("Played Ship Repair");
    effect1.Apply(state);

    int currentMana = state.mana.get_amount();
    state.mana.set_amount(currentMana - this.cardCost);
  }
}

public class DemoPistolShot : DemoCard
{
  public DemoPistolShot() : base("Pistol Shot", "Common", 2, "Deal 12 Damage", 1) { }

  DemoEffect effect1 = new DemoDamageEffect(12);

  public override void Play(DemoGameState state)
  {
    Debug.Log("Played Pistol Shot");
    effect1.Apply(state);

    int currentMana = state.mana.get_amount();
    state.mana.set_amount(currentMana - this.cardCost);
  }

}

public class DemoTwinBlades : DemoCard
{
  public DemoTwinBlades() : base("Twin Blades", "Common", 1, "Deal 4 Damage Twice", 1) { }

  DemoEffect effect1 = new DemoDamageEffect(4);
  DemoEffect effect2 = new DemoDamageEffect(4);

  public override void Play(DemoGameState state)
  {
    Debug.Log("Played Twin Blades");
    effect1.Apply(state);
    effect2.Apply(state);

    int currentMana = state.mana.get_amount();
    state.mana.set_amount(currentMana - this.cardCost);
  }

}

public class DemoBoardingCharge : DemoCard
{
  public DemoBoardingCharge() : base("Boarding Charge", "Common", 1, "Deal 6 Damage and Gain 3 Health", 1) { }

  DemoEffect effect1 = new DemoDamageEffect(6);
  DemoEffect effect2 = new DemoHealEffect(3);

  public override void Play(DemoGameState state)
  {
    Debug.Log("Played Boarding Charge");
    effect1.Apply(state);

    int currentMana = state.mana.get_amount();
    state.mana.set_amount(currentMana - this.cardCost);
  }

}

public class DemoBackStab : DemoCard
{
  public DemoBackStab() : base("Back Stab", "Common", 1, "Deal 10 Damage if full on mana, otherwise deal 5", 1) { }

  DemoEffect effect1 = new DemoDamageEffect(10);
  DemoEffect effect2 = new DemoDamageEffect(5);

  public override void Play(DemoGameState state)
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

public class DemoWhispersBelow : DemoCard
{
  public DemoWhispersBelow() : base("Whispers Below", "Common", 1, "Lose 5 HP, Draw 2 Cards", 1) { }

  DemoEffect effect1 = new DemoHealEffect(-5);
  // Effect effect2 = new CardDraw();

  public override void Play(DemoGameState state)
  {
    Debug.Log("Played ");
    effect1.Apply(state);

    int currentMana = state.mana.get_amount();
    state.mana.set_amount(currentMana - this.cardCost);
  }

}

// Have more ideas for cards but need to add more effects
