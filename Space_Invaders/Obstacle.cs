using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Space_Invaders.Interfaces;
using System.Diagnostics;

namespace Space_Invaders
{
    class Obstacle : IDamageable
    {
        public Obstacle(Vector2 location, Texture2D texture) {
            Sprite = new Sprite(location, texture, new Rectangle(11, 9, 22, 15), Vector2.Zero);
            Sprite.Scale = 4.0f;
            Sprite.CollisionRadius = 40;
            HitSpots = new List<Vector2>();
        }

        public List<Vector2> HitSpots
        {
            get;set;
        }
        public void Damage(Vector2 location)
        {
            HitSpots.Add(location);
        }
        public void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
        }
        public void Draw(SpriteBatch _spriteBatch)
        {
            Sprite.Draw(_spriteBatch);
            foreach (Vector2 hitspot in HitSpots)
            {
                _spriteBatch.Draw(Sprite.texture, hitspot, new Rectangle(15, 3, 1, 1), Color.White, 0.0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0.0f);
            }
        }

        public Sprite Sprite
        {
            get;set;
        }

        public bool Destroyed
        {
            get;set;
        }
    }
}
