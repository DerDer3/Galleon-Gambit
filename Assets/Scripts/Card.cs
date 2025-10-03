using UnityEngine;

public class Card
{
  string cardName;
  string cardRarity;
  int cardCost;
  string cardDescription;
  int cardLevel;

  protected Card(string cardName, string cardRarity, int cardCost, string cardDescription, int cardLevel)
  {
    this.cardName = cardName;
    this.cardRarity = cardRarity;
    this.cardCost = cardCost;
    this.cardDescription = cardDescription;
    this.cardLevel = cardLevel;
  }

  public abstract void Play();
  public abstract void Upgrade();
}
