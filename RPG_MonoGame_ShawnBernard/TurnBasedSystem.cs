using Microsoft.Xna.Framework;
using Nez;
using System;
using System.Collections.Generic;

namespace RPG_MonoGame_ShawnBernard
{
    public class TurnBasedSystem : Component, IUpdatable
    {
        public List<Actor> Actors = new List<Actor>();

        public int order = 0;
        public TurnBasedSystem()
        {

        }

        public void AddActor(Actor actor)
        {
            Actors.Add(actor);
        }
        public void updateturn()
        {
            if (order < Actors.Count && order >= Actors.Count)
            {
                var ActorTurn = Actors[order];
                if (ActorTurn.isTurn)
                {
                    ActorTurn.StartTurn();
                }
                else
                {
                    ActorTurn.EndTurn();
                }
                order++;
            }
            else
            {
                order = 0;
                Actor actor = Actors[order];
            }
        }

        public void Update()
        {
            
        }
    }

}
