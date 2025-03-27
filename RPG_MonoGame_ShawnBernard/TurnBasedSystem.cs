using Microsoft.Xna.Framework;
using Nez;
using System;
using System.Collections.Generic;

namespace RPG_MonoGame_ShawnBernard
{
    public class TurnBasedSystem : Component
    {
        public List<Actor> Actors = new List<Actor>();
        public int order = 0;

        public void AddActor(Actor actor)
        {
            Actors.Add(actor);
        }

        public void UpdateTurn()
        {
            //Debug.Log($"Current order: {order}, Actor count: {Actors.Count}");

            if (Actors.Count == 0) return; // Prevent errors if no actors exist

            if (order < Actors.Count)
            {
                Actor actorTurn = Actors[order];

                //Debug.Log($"Current Actor's Turn: {actorTurn}");

                if (!actorTurn.isTurn)
                {
                    actorTurn.StartTurn();
                }
                else
                {
                    actorTurn.UpdateTurn();
                }
            }
            else
            {
                order = 0;
                Actors[order].StartTurn();
            }
        }
    }
}
