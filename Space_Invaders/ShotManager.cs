using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Space_Invaders
{

    public class ShotManager //shouldn't be responsible for firing the shots just updating and drawing the shots already fired
    {
        public List<Sprite> Shots = new List<Sprite>();
        private Rectangle screenBounds;
        public static Texture2D Texture;
        public Rectangle InitialFrame;
        public int FrameCount;
        private float shotSpeed;
        public int CollisionRadius;
        public ShotManager(Texture2D texture, Rectangle initialFrame, int frameCount, int collisionRadius, Rectangle screenBounds) {
            Texture = texture;
            InitialFrame = initialFrame;
            FrameCount = frameCount;
            CollisionRadius = collisionRadius;
            this.screenBounds = screenBounds;

        }

        public void FireShot(Vector2 location, Vector2 velocity)
        {
            Sprite thisShot = new Sprite(location, Texture, InitialFrame, velocity);
            thisShot.Scale = 2.0f;
            thisShot.Velocity *= shotSpeed;

            for (int x = 1; x < FrameCount; x++)
            {
                /* thisShot.AddFrame(new Rectangle(InitialFrame.X + InitialFrame.Width * x, InitialFrame.Y, InitialFrame.Width, InitialFrame.Height));*/
                thisShot.AddFrame(new Rectangle(4, 12, 3, 5));
            }
            thisShot.CollisionRadius = CollisionRadius;
            Shots.Add(thisShot);
        }

        public void Update(GameTime gameTime)
        {
            for (int x = Shots.Count - 1; x >= 0; x--)
            {
                Shots[x].Update(gameTime);
                
                if (!screenBounds.Intersects(Shots[x].Destination))
                {
                    Shots.RemoveAt(x);
                }
            }
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            foreach (Sprite shot in Shots) //this is not being called
            {
                
                shot.Draw(_spriteBatch);
            }
        }
    }
}
