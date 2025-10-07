using UnityEngine;

public abstract class Effect
{
  abstract public void Apply();
}

public class DamageEffect : Effect
{
  int damageAmount;

  public DamageEffect(int x)
  {
    this.damageAmount = x;
  }
  public override void Apply()
  {
    // Apply enemy health change
  }
}

public class HealEffect : Effect
{
  int healAmount;

  public HealEffect(int x)
  {
    this.healAmount = x;
  }
  public override void Apply()
  {
    // Apply player health change
  }
}
