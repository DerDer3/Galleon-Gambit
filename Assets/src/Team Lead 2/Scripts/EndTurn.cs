using UnityEngine;

public class EndTurn : MonoBehaviour
{
  public void OnClick_EndTurn()
  {
    GameManager2.Instance.EndPlayerTurn();
  }
}
