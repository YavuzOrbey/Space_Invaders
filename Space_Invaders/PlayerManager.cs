using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Space_Invaders
{
    class PlayerManager: Ship
    {
        private float playerSpeed = 160.0f;
        private float shotSpeed = 500.0f;
        public Rectangle playerAreaLimit;
        public long PlayerScore = 0;
        public int livesRemaining = 3;
        private Vector2 gunOffSet = new Vector2(0, 0);
        private float shotTimer = 0.0f;
        private float minShotTimer = 0.4f;
        private float respawnTimer = 0.0f;
        private float timeUntilRespawn = 2.0f;
        private int playerRadius = 15;

        public PlayerManager(Texture2D texture, Vector2 initialPos, Rectangle initialFrame, Rectangle screenBounds): base(texture, initialPos, initialFrame)
        {
            Sprite.Scale = 2.0f;
            playerAreaLimit = new Rectangle(0, 0 //screenBounds.Height - sprite.texture.Height-100
                , screenBounds.Width, screenBounds.Height);
            shotManager = new ShotManager(texture, new Rectangle(4, 2, 1, 4), 1, 2, screenBounds);
            Sprite.CollisionRadius = playerRadius;
            bulletSpeed = 250.0f;

        }
        public void handleKeyboardInput(KeyboardState kState)
        {
            if (kState.IsKeyDown(Keys.Left))
            {
                Sprite.Velocity = new Vector2(-1, 0);
            }
            if (kState.IsKeyDown(Keys.Right))
            {
                Sprite.Velocity = new Vector2(1, 0);
            }
            if (kState.IsKeyDown(Keys.Space) && shotTimer >= minShotTimer)
            {
                FireShot(Sprite.Location, new Vector2(0, -1));
                shotTimer = 0.0f;
            }
        }
        public bool isOutofBounds()
        {
            return !Sprite.Destination.Intersects(playerAreaLimit);
        }
        public override void Update(GameTime gameTime)
        {
            shotTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            Sprite.Velocity = Vector2.Zero;
            if (Destroyed)
            {

                respawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (respawnTimer >= timeUntilRespawn)
                {
                    livesRemaining--;
                    Sprite = new Sprite(Sprite.Location, Sprite.texture, new Rectangle(0, 9, 11, 8), Vector2.Zero);
                    Sprite.Scale = 2.0f;
                    Sprite.CollisionRadius = playerRadius;
                    respawnTimer = 0.0f;
                    Destroyed = false;
                }

            }
            else
            {
                KeyboardState kState = Keyboard.GetState();
                handleKeyboardInput(kState);


                Sprite.Velocity.Normalize();
                Sprite.Velocity *= playerSpeed;
                Sprite.Update(gameTime);
            }

            shotManager.Update(gameTime);
        }

        public override void Draw(SpriteBatch _spriteBatch)
        {
            shotManager.Draw(_spriteBatch);
            base.Draw(_spriteBatch);
        }


    }
}
