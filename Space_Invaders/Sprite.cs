
using System.Collections.Generic;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Space_Invaders
{
    public class Sprite
    {
        public Texture2D texture;
        protected List<Rectangle> frames = new List<Rectangle>();
        private int frameWidth = 0;
        private int frameHeight = 0;
        private int currentFrame;
        private float frameTime = 0.1f;
        private float timeForCurrentFrame = 0.0f;

        

        private Color tintColor = Color.White;
        private float rotation = 0.0f;
        private float scale = 1.0f;

        public int CollisionRadius = 0;
        public int BoundingXPadding = 0;
        public int BoundingYPadding = 0;

        protected Vector2 location = Vector2.Zero; 
        protected Vector2 velocity = Vector2.Zero;//distance sprite moves in 1 second

        public Sprite(Vector2 location, Texture2D texture, Rectangle initialFrame, Vector2 velocity)
        {
            this.location = location;
            this.texture = texture;
            this.velocity = velocity;

            frames.Add(initialFrame);
            frameWidth = initialFrame.Width;
            frameHeight = initialFrame.Height;
        }
        public Vector2 Location
        {
            get { return location; }
            set { location = value; }
        }
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        public Color TintColor
        {
            get { return tintColor; }
            set { tintColor = value; }
        }
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value % MathHelper.TwoPi; }
        }
        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }
        public int Frame
        {
            get { return currentFrame; }
            set { currentFrame = (int)MathHelper.Clamp(value, 0, frames.Count - 1); }
        }
        public float FrameTime
        {
            get { return frameTime; }
            set { frameTime = MathHelper.Max(0, value); }
        }
        public Rectangle Source
        {
            get { return frames[currentFrame]; }
        }
        public Rectangle Destination
        {
            get { return new Rectangle((int)location.X, (int)location.Y, frameWidth, frameHeight); }
        }
        public Vector2 Center
        {
            get { return location + new Vector2(frameWidth / 2, frameHeight / 2); }
        }
        public Rectangle BoundingBoxRect
        {
            get { return new Rectangle((int)location.X + BoundingXPadding, (int)location.Y + BoundingYPadding, frameWidth - (BoundingXPadding * 2), frameHeight - (BoundingYPadding * 2)); }
        }
        public bool IsBoxColliding(Rectangle otherBox)
        {
            return BoundingBoxRect.Intersects(otherBox);
        }
        public bool isCircleColliding(Vector2 otherCenter, float otherRadius)
        {
            if (Vector2.Distance(Center, otherCenter) < (CollisionRadius + otherRadius))
                return true;
            else
                return false;
        }
        public int AddFrame(Rectangle frameRectangle)
        {
            frames.Add(frameRectangle);
            return frames.Count -1;
        }
        public virtual void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timeForCurrentFrame += elapsed;
            if (timeForCurrentFrame >= FrameTime)
            {
                currentFrame = (currentFrame + 1) % (frames.Count);
                timeForCurrentFrame = 0.0f;

            }
            location += velocity * elapsed;
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Center, Source, TintColor, Rotation, new Vector2(frameWidth / 2, frameHeight / 2), Scale, SpriteEffects.None, 0.0f);
        }

        
    }
}
