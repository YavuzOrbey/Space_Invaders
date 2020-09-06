using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Space_Invaders.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Space_Invaders
{
    //parent class
    public abstract class Ship: IDamageable
    {
        private float speed;
        private float scale;
        private Texture2D texture;
        private Vector2 initialPos;
        protected float bulletSpeed;
        public ShotManager shotManager {
            get; set;
        }
        public Ship(Texture2D texture, Vector2 initialPos, Rectangle initialFrame) {
            Sprite = new Sprite(initialPos, texture,initialFrame, Vector2.Zero);
        }

        public virtual void FireShot(Vector2 location, Vector2 velocity)
        {
            Sprite thisShot = new Sprite(location, ShotManager.Texture, shotManager.InitialFrame, velocity);
            thisShot.Scale = 2.0f;
            thisShot.Velocity *= bulletSpeed;

            for (int x = 1; x < shotManager.FrameCount; x++)
            {
                /* thisShot.AddFrame(new Rectangle(InitialFrame.X + InitialFrame.Width * x, InitialFrame.Y, InitialFrame.Width, InitialFrame.Height));*/
                thisShot.AddFrame(new Rectangle(4, 12, 3, 5));
            }
            thisShot.CollisionRadius = shotManager.CollisionRadius;
            shotManager.Shots.Add(thisShot);
        }

        public virtual void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
        }
        public virtual void Draw(SpriteBatch _spriteBatch)
        {
            Sprite.Draw(_spriteBatch);
        }

        public Sprite Sprite
        {
            get; set;
        }
        public bool Destroyed
        {
            get; set;
        }
        public void Damage(Vector2 location)
        {
            Sprite = new Sprite(Sprite.Location, Sprite.texture, new Rectangle(24, 0, 11, 8), Vector2.Zero);
            Destroyed = true;
        }
    }
}
