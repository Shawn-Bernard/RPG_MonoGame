using RPG_MonoGame_ShawnBernard;
using Nez;

public class TurnManager : Entity
{
    public TurnBasedSystem turnSystem;

    public TurnManager()
    {
        AddComponent(new TurnBasedSystem());
        turnSystem = GetComponent<TurnBasedSystem>();
    }
    public override void OnAddedToScene()
    {

    }






}
