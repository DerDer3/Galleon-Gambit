using UnityEngine;

public class Slash : Card
{
    public Slash() : base("Slash", "Common", 1, "Apply 5 damage to enemy", 1) { }

    Effect effect1 = new DamageEffect(5);

    public override void Play()
    {
      Debug.Log("Played Slash");
      effect1.Apply();
    }
}

public class HealPotion : Card
{
  public HealPotion() : base("HealPotion", "Common", 1, "Heal 5 player health", 1) { }

  Effect effect1 = new HealEffect(5);

  public override void Play()
  {
    Debug.Log("Played Heal Potion");
    effect1.Apply();
  }
}
