using UnityEngine;

public abstract class DemoEffect
{
  abstract public void Apply(DemoGameState state);
}

public class DemoDamageEffect : DemoEffect
{
  int damageAmount;

  public DemoDamageEffect(int x)
  {
    this.damageAmount = x;
  }
  public override void Apply(DemoGameState state)
  {
    //int currentHealth = state.currentEnemy.get_health();
    //state.currentEnemy.set_health(currentHealth - damageAmount);
  }
}

public class DemoHealEffect : DemoEffect
{
  int healAmount;

  public DemoHealEffect(int x)
  {
    this.healAmount = x;
  }
  public override void Apply(DemoGameState state)
  {
    int currentHealth = state.mainPlayer.get_health();
    state.mainPlayer.set_health(currentHealth + healAmount);
  }
}
