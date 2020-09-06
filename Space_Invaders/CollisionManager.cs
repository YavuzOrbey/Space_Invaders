using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space_Invaders.Interfaces;
namespace Space_Invaders
{
    class CollisionManager
    {
        private PlayerManager playerManager;
        private EnemyManager enemyManager;
        private List<Enemy> listOfEnemies = new List<Enemy>();
        private List<Obstacle> obstacles = new List<Obstacle>();
        private List<IDamageable> damageables = new List<IDamageable>();
        public CollisionManager(PlayerManager playerManager, EnemyManager enemyManager)
        {
            this.playerManager = playerManager;
            this.enemyManager = enemyManager;
            listOfEnemies = enemyManager.enemies.SelectMany(x => x).ToList();
            damageables.Add(playerManager);
            damageables.AddRange(listOfEnemies);

        }

        public void addDamageables(List<IDamageable> listOfDamageables)
        {
            damageables.AddRange(listOfDamageables);
        }

        

        private void checkPlayerShots()
        {
            foreach (Sprite shot in playerManager.shotManager.Shots)
            {
                foreach (IDamageable damageable in damageables)
                {
                    if (!(damageable is PlayerManager) && shot.isCircleColliding(damageable.Sprite.Center, damageable.Sprite.CollisionRadius) 
                        && !damageable.Destroyed)
                    {
                        damageable.Damage(shot.Location);
                        shot.Location = new Vector2(-500, 500);

                    }
                }
            }
        }

        private void checkEnemyShots()
        {
            foreach (Enemy enemy in listOfEnemies)
            {
                foreach (Sprite shot in enemy.shotManager.Shots)
                {

                    foreach(IDamageable damageable in damageables)
                    {

                        //as long as IDamageable is not an enemy damage it 
                        if (!(damageable is Enemy) && shot.isCircleColliding(damageable.Sprite.Center, damageable.Sprite.CollisionRadius))
                        {
                            damageable.Damage(shot.Location);
                            shot.Location = new Vector2(-500, 500);
                           
                        }
                    }
                    
                }
            }
        }
        private void checkShotToEnemyCollisions()
        {
            foreach(Sprite shot in playerManager.shotManager.Shots)
            {
                
                foreach (Enemy enemy in listOfEnemies)
                {
                    if(shot.isCircleColliding(enemy.Sprite.Center, enemy.Sprite.CollisionRadius))
                    {
                        shot.Location = new Vector2(-500, 500);
                        enemy.Sprite = new Sprite(enemy.Sprite.Location, enemy.Sprite.texture, new Rectangle(24,0,11,8), Vector2.Zero);
                        enemy.Destroyed = true;
                        playerManager.PlayerScore += 10;
                    }
                }
            }
        }
        private void checkShotToPlayerCollisions()
        {
            foreach (Enemy enemy in listOfEnemies)
            {
                foreach (Sprite shot in enemy.shotManager.Shots)
                {
                    if (shot.isCircleColliding(playerManager.Sprite.Center, playerManager.Sprite.CollisionRadius))
                    {
                        shot.Location = new Vector2(-500, 500);
                        playerManager.Sprite = new Sprite(playerManager.Sprite.Location, playerManager.Sprite.texture, new Rectangle(24, 0, 11, 8), Vector2.Zero);
                        playerManager.Destroyed = true;
                    }
                }
            }
        }

        public void updateEnemies()
        {
            listOfEnemies = enemyManager.enemies.SelectMany(x => x).ToList();
        }
        public void checkCollisions()
        {
            checkEnemyShots();
            checkPlayerShots();
        }
    }
}
