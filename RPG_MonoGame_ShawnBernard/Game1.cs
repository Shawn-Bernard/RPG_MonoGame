using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;
using Nez.Tweens;
using System;

namespace RPG_MonoGame_ShawnBernard
{
    public class Game1 : Core
    {
        //private GraphicsDeviceManager _graphics;
        //private SpriteBatch _spriteBatch;

        public Game1() : base()
        {
            //_graphics = new GraphicsDeviceManager(this);
            //Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            // Making a new scene with default renderer for background
            Scene scene = Scene.CreateWithDefaultRenderer(Color.Gray);
            scene.ClearColor = Color.Red;
            TurnManager turnManager = new TurnManager();
            scene.AddEntity(turnManager);

            Map map = new Map();
            // Loads player texture 
            Texture2D playerTexture = scene.Content.Load<Texture2D>("Player");
            Texture2D enemyTexture = scene.Content.Load<Texture2D>("Enemy");

            Entity Map = new Map();

            Actor player = new Actor(new Vector2 (10,10));
            scene.Camera.SetPosition(player.Position);
            player.Scale = new Vector2 (1, 1);
            scene.AddEntity(player);
            // Setting core Scene to my new scene that I made
            scene.AddEntity(Map);
            Scene = scene;
        }

        // Player Movement Component

    }
    public class Movement : Component, IUpdatable
    {
        private int speed = 100;
        Actor entity;
        public Movement(Actor actor)
        {
            entity = actor;
        }
        // This is basically start method
        public override void OnAddedToEntity()
        {
            Transform.Scale = new Vector2(5, 5);


            //Changing scene color


        }
        //Component, IUpdatable lets me use the update method
        public void Update()
        {
            Controller();
        }

        public virtual void Controller()
        {
            // If the actor is waiting for animation
            if (entity.WaitAnimation)
            {   //Returns if when the actor is waiting for animation
                return;
            }

            Vector2 move = Vector2.Zero;

            // Check for movement input and adjust the move vector accordingly
            if (Input.IsKeyPressed(Keys.W)) move.Y -= 16; // Move up
            if (Input.IsKeyPressed(Keys.S)) move.Y += 16; // Move down
            if (Input.IsKeyPressed(Keys.A)) move.X -= 16; // Move left
            if (Input.IsKeyPressed(Keys.D)) move.X += 16; // Move right

            // Only execute movement if there is a change in position
            if (move != Vector2.Zero)
            {
                entity.Move(move); // Call Move() to update the entity’s position
            }
        }

    }
}