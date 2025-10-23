using UnityEngine;

public abstract class Card
{

  // Public here ensures they can all be read, but set is private
  public string cardName { get; private set;}
  public string cardRarity { get; private set;}
  public int cardCost { get; private set;}
  public string cardDescription { get; private set;}
  public int cardLevel { get; private set;}

  protected Card(string cardName, string cardRarity, int cardCost, string cardDescription, int cardLevel)
  {
    this.cardName = cardName;
    this.cardRarity = cardRarity;
    this.cardCost = cardCost;
    this.cardDescription = cardDescription;
    this.cardLevel = cardLevel;
  }

  public abstract void Play(GameState state);
  public void Upgrade()
  {
    this.cardLevel += 1;
  }
}
