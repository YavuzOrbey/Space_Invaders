using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space_Invaders.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Space_Invaders
{
    class Enemy: Ship
    {
        protected List<Vector2> wayPoints = new List<Vector2>();
        private int currentWaypoint=0;
        private Vector2 heading;
        public Enemy(Texture2D texture, Vector2 initialPos, Rectangle initialFrame, float enemyScale, int bulletSpeed):base(texture,initialPos,initialFrame)
        {
            Vector2 destination = new Vector2(300, 300);
            wayPoints.Add(destination);
            wayPoints.Add(new Vector2(300, 100));
            wayPoints.Add(new Vector2(50, 100));
            wayPoints.Add(new Vector2(50, 300));
            heading = Vector2.Normalize(destination - initialPos);
            Sprite.AddFrame(new Rectangle(11, 0, 11, 8));
            this.bulletSpeed = bulletSpeed;
            Sprite.Scale = enemyScale;
            Sprite.CollisionRadius = 10;
            Sprite.FrameTime = 0.5f;
        }
        public bool isActive()
        {
            if (Destroyed)
                return false;
            return true;
        }
        public bool wayPointReached(Vector2 wayPoint)
        {
            return (Vector2.Distance(Sprite.Location, wayPoint) < 1);
        }
        public override void Update(GameTime gameTime)
        {
            if (isActive())
            {
                if (wayPointReached(wayPoints[currentWaypoint])){
                    currentWaypoint = (currentWaypoint +1) % wayPoints.Count;
                    heading = Vector2.Normalize(wayPoints[currentWaypoint] - Sprite.Location);
                }
            }
            Sprite.Update(gameTime);
        }
        public override void Draw(SpriteBatch _spriteBatch)
        {
            shotManager.Draw(_spriteBatch);
            base.Draw(_spriteBatch);
        }

    }
}
