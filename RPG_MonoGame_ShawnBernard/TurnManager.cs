using RPG_MonoGame_ShawnBernard;
using Nez;

public class TurnManager : Entity, IUpdatable
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

    public override void Update()
    {
        turnSystem.UpdateTurn();
    }







}
