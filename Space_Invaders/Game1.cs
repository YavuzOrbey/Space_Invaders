using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Space_Invaders.Interfaces;
using System.Diagnostics;

namespace Space_Invaders
{

    public class Game1 : Game
    {
        enum GameStates { TitleScreen, Playing, Dead, GameOver }
        GameStates gameState;
        Texture2D titleScreen;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public Texture2D spriteSheet;

        PlayerManager playerManager;
        EnemyManager enemyManager;
        CollisionManager collisionManager;
        List<IDamageable> obstacles;
        public int continues = 1;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);


            
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            /*_graphics.PreferredBackBufferWidth = 100;
            _graphics.PreferredBackBufferHeight = 200;
            _graphics.ApplyChanges();*/
            gameState = GameStates.TitleScreen;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteSheet = Content.Load<Texture2D>("spriteSheet");
            titleScreen = Content.Load<Texture2D>("titleScreen");
            // TODO: use this.Content to load your game content here
            playerManager = new PlayerManager(spriteSheet, new Vector2(100,450),new Rectangle(0, 9, 11, 8), new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height));
            enemyManager = new EnemyManager(spriteSheet, new Rectangle(0, 0, 11, 8), new Rectangle(0,0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height), playerManager);
            collisionManager = new CollisionManager(playerManager, enemyManager);
            obstacles = new List<IDamageable>();
            int x = this.Window.ClientBounds.Width / 4;
            obstacles.Add(new Obstacle(new Vector2(x*0+100 , 350), spriteSheet));
            obstacles.Add(new Obstacle(new Vector2(x*1 , 350), spriteSheet));
            obstacles.Add(new Obstacle(new Vector2(x*2 , 350), spriteSheet));
            obstacles.Add(new Obstacle(new Vector2(x*3 , 350), spriteSheet));
            collisionManager.addDamageables(obstacles);
            Globals.font = Content.Load<SpriteFont>("Score");
        }

        protected override void Update(GameTime gameTime)
        {
            if (continues < 0) {
                Trace.WriteLine("started up again");
            }
            switch (gameState)
            {

                case GameStates.TitleScreen:
                    KeyboardState kState = Keyboard.GetState();
                    handleKeyboardInput(kState);
                    break;
                case GameStates.Playing:
                    playerManager.Update(gameTime);
                    if (enemyManager.enemiesLeft() == 0)
                    {
                        gameState = GameStates.TitleScreen;
                        break;
                    }
                    if(playerManager.livesRemaining <= 0)
                    {
                        gameState = GameStates.TitleScreen;
                        break;
                    }
                    enemyManager.Update(gameTime);
                    collisionManager.checkCollisions();
                    foreach (Obstacle obstacle in obstacles)
                    {
                        obstacle.Update(gameTime);
                    }
                    if (playerManager.isOutofBounds())
                    {
                        gameState = GameStates.TitleScreen;
                    }
                    break;
                case GameStates.Dead:
                    break;
                case GameStates.GameOver:
                    break;
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }
        public void handleKeyboardInput(KeyboardState kState)
        {
            if(kState.IsKeyDown(Keys.Enter)){
                gameState = GameStates.Playing;
                playerManager.livesRemaining = 3;
                continues--;
            }
        }
        protected override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null);
            if (gameState == GameStates.TitleScreen)
            {
                _spriteBatch.Draw(titleScreen, new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height), Color.White);
            }
            else
            {
                GraphicsDevice.Clear(Color.Black);
                enemyManager.Draw(_spriteBatch);
                playerManager.Draw(_spriteBatch);
                foreach (Obstacle obstacle in obstacles)
                {
                    obstacle.Draw(_spriteBatch);
                }

            }
            //GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            _spriteBatch.End();
           base.Draw(gameTime);
        }
    }
}
