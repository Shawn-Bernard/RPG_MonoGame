using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Nez.Sprites;
using Nez;
using Nez.Textures;
using System.Linq;

namespace RPG_MonoGame_ShawnBernard
{
    public class Actor : Entity
    {
        public SpriteRenderer spriteRenderer;
        public Entity TurnManager;
        public bool isTurn;
        public bool WaitForTurn;
        public bool WaitAnimation;
        private float animationTime = 0.5f; // Default animation time

        public Texture2D playerTexture;
        public Vector2 startPosition;

        Map map;
        public Actor(Vector2 startPosition)
        {
            Transform.Position = new Vector2 (1, 1);
            //Transform.Position = startPosition;  // Correctly position player on grid
            AddComponent(new Movement(this));
            this.startPosition = startPosition;
            Debug.Log("?????????????????????????????????????????????????????");
            Debug.Log(this.Position);
            Debug.Log("?????????????????????????????????????????????????????");
        }

        public override void OnAddedToScene()
        {
            playerTexture = Scene.Content.Load<Texture2D>("Player");
            map = Scene.EntitiesOfType<Map>().FirstOrDefault();
            map.addTile(playerTexture, Transform.Position);
        }

        public virtual void StartTurn()
        {
            isTurn = true;
            WaitForTurn = false;
        }

        public virtual void EndTurn()
        {
            isTurn = false;
            WaitForTurn = true;
        }

        public virtual void UpdateTurn()
        {
            if (WaitForTurn)
            {

            }
        }

        public virtual void Move(Vector2 changeVector)
        {
            Vector2 MoveVector = new Vector2(Position.X + changeVector.X, Position.Y + changeVector.Y);
            WaitAnimation = true;
            this.TweenPositionTo(MoveVector, 0.5f).SetCompletionHandler(tween =>{Debug.Log("Movement complete.");WaitAnimation = false;}).Start();
            Scene.Camera.SetPosition(this.Position);
        }
    }
}
