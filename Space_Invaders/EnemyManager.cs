using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Space_Invaders
{
    class EnemyManager
    {
        private float enemySpawnTimer = 0.0f;
        private float enemySpawnWaitTime = 4.0f;
        public List<List<Enemy>> enemies = new List<List<Enemy>>();
        private Texture2D spriteSheet;
        private Rectangle initialFrame;
        public ShotManager enemyShotManager;
        public PlayerManager playerManager;
        private float shotTimer = 0.0f;
        private float minShotTimer = 1.0f;
        private int enemiesPerRow = 11;
        private int rows = 5;
        private int paddingBetweenColumns = 10;
        private int paddingBetweenRows = 20;
        private Random rand = new Random();
        private float enemyShotChance = 0.001f;
        private float enemySpeed = 50.0f;
        private float enemyScale = 2.0f;
        private float despawnTimer = 0.0f;
        private Vector2 direction = new Vector2(1, 0);

        private float timeUntilCloser = 0;
        private float closerTimer = 1.0f;
        public EnemyManager(Texture2D spriteSheet, Rectangle initialFrame, Rectangle screenBounds, PlayerManager playerManager)
        {
            this.spriteSheet = spriteSheet;
            this.initialFrame = initialFrame;
            this.playerManager = playerManager;

            for (int i = 0; i < rows; i++)
            {
                List<Enemy> row = new List<Enemy>();
                for (int x = 0; x < enemiesPerRow; x++)
                {
                    Enemy newEnemy = new Enemy(spriteSheet, new Vector2(50 + x * initialFrame.Width*enemyScale+ paddingBetweenColumns * x, 200 - i * initialFrame.Height*enemyScale- paddingBetweenRows*i), initialFrame, enemyScale, 150);
                    newEnemy.shotManager = new ShotManager(spriteSheet, new Rectangle(5, 2, 1, 4), 1, 2, screenBounds);
                    row.Add(newEnemy);
                }
                enemies.Add(row);
            }
        }
        public int enemiesLeft()
        {
            int count = 0;
            foreach(List<Enemy> row in enemies)
            {
                foreach(Enemy enemy in row)
                {
                    count++;
                }
            }
            return count;
        }
        public void Update(GameTime gameTime)
        {
            enemySpawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            shotTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            for(int i=enemies.Count-1; i>=0; i--) 
            {
                for(int j=enemies[i].Count-1; j>=0; j--)
                {
                    Sprite currentEnemySprite = enemies[i][j].Sprite;
                    timeUntilCloser += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    currentEnemySprite.Velocity = direction * enemySpeed;

                    if ((float) rand.Next(0,1000)/10 <= enemyShotChance)
                    {
                        Vector2 fireLocation = new Vector2(currentEnemySprite.Location.X, currentEnemySprite.Location.Y + currentEnemySprite.Destination.Height);
                        Vector2 shotDirection = playerManager.Sprite.Center - currentEnemySprite.Location;
                        shotDirection.Normalize();
                        shotDirection = new Vector2(0, 1);
                        enemies[i][j].FireShot(fireLocation, shotDirection);
                    }
                    
                    enemies[i][j].Update(gameTime);
                    enemies[i][j].shotManager.Update(gameTime);
                    if (!enemies[i][j].isActive())
                    {

                        despawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                        if (despawnTimer >= .1f)
                        {
                            enemies[i].RemoveAt(j);
                            despawnTimer = 0.0f;
                        }

                    }
                }
                if(enemies[enemies.Count-1][enemies[enemies.Count - 1].Count-1].Sprite.Destination.X > 600)
                {
                    direction = new Vector2(-1, 0);
                }
                if (enemies[enemies.Count - 1][0].Sprite.Destination.X < 0)
                {
                    direction = new Vector2(1, 0);
                }
            }

            if (shotTimer >= minShotTimer)
                shotTimer = 0.0f;
            if (timeUntilCloser >= closerTimer)
                timeUntilCloser = 0.0f;

        }
        public void Draw(SpriteBatch _spriteBatch)
        {
            foreach (List<Enemy> row in enemies)
            {
                foreach (Enemy enemy in row)
                {
                    enemy.Draw(_spriteBatch);
                }

            }
        }
    }
}
