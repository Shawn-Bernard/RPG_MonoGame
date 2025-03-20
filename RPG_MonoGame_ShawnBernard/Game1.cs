using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;

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
            Scene scene = Scene.CreateWithDefaultRenderer(Color.Red);
            scene.ClearColor = Color.Red;
            TurnManager turnManager = new TurnManager();
            scene.AddEntity(turnManager);



            // Loads player texture 
            Texture2D playerTexture = scene.Content.Load<Texture2D>("Player");
            Texture2D enemyTexture = scene.Content.Load<Texture2D>("Enemy");


            Actor enemy = new Actor(enemyTexture);
            Actor player = new Actor(playerTexture);
            //player = scene.CreateEntity("Player");





            // Setting core Scene to my new scene that I made
            turnManager.turnSystem.AddActor(player);

            foreach (Actor actor in turnManager.turnSystem.Actors)
            {
                scene.AddEntity(actor);
            }
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
            Transform.Position = new Vector2(50, 50);
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
            Vector2 move = Vector2.Zero;
            if (Input.IsKeyPressed(Keys.W))
            {
                move.Y -= 100;
            }
            if (Input.IsKeyPressed(Keys.S))
            {
                move.Y += 100;
            }
            if (Input.IsKeyPressed(Keys.A))
            {
                move.X -= 100;
            }
            if (Input.IsKeyPressed(Keys.D))
            {
                move.X += 100;
            }
            if (Input.IsKeyDown(Keys.LeftShift))
            {
                speed = 200;
            }
            else
            {
                speed = 100;
            }
            // This gives a more player snap
            entity.TweenLocalPositionTo(Transform.Position += move, 0.2f);

            // This moves the player like normal
            // Transform.Position += move * speed * Time.DeltaTime;
        }
    }
    public class Actor : Entity
    {
        public SpriteRenderer spriteRenderer;

        public Entity entity;
        public Entity TurnManager;
        public bool isTurn;
        public bool WaitForTurn;
        public bool WaitAnimation;
        double animation;

        public Actor(Texture2D texture2D)
        {
            AddComponent(new SpriteRenderer(texture2D));
            spriteRenderer = GetComponent<SpriteRenderer>();

            AddComponent(new Movement(this));


        }

        public override void OnAddedToScene()
        {
        }
        public virtual void StartTurn()
        {
            //change color 
            //entity.GetComponent<>
            isTurn = true;
            WaitForTurn = false;
        }

        public virtual void EndTurn()
        {
            isTurn = false;
            WaitForTurn = true;
        }

        public virtual void UpdateTurn(GameTime gameTime)
        {
            if (WaitForTurn)
            {
                animation += gameTime.TotalGameTime.TotalSeconds;
                if (animation < 15f)
                {

                }
                else
                {
                    isTurn = true;
                }
            }
        }

        public virtual void Move(GameTime gameTime, Vector2 changeVector)
        {
            animation = gameTime.TotalGameTime.TotalSeconds;
            Vector2 MoveVector = new Vector2(entity.Position.X + changeVector.X, entity.Position.Y + changeVector.Y);
            entity.TweenPositionTo(MoveVector, (float)animation).Start();//SetCompletionHandler();


            WaitAnimation = true;

        }

    }
}
