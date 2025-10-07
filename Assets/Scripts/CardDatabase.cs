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
