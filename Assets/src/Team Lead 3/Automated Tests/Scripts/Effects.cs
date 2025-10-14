using UnityEngine;

public abstract class Effect
{
  abstract public void Apply(GameState state);
}

public class DamageEffect : Effect
{
  int damageAmount;

  public DamageEffect(int x)
  {
    this.damageAmount = x;
  }
  public override void Apply(GameState state)
  {
    int currentHealth = state.currentEnemy.get_health();
    state.currentEnemy.set_health(currentHealth - damageAmount);
  }
}

public class HealEffect : Effect
{
  int healAmount;

  public HealEffect(int x)
  {
    this.healAmount = x;
  }
  public override void Apply(GameState state)
  {
    int currentHealth = state.mainPlayer.get_health();
    state.mainPlayer.set_health(currentHealth + healAmount);
  }
}
