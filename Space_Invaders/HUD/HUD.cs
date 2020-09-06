using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Space_Invaders.HUD
{
    class HUD
    {
        public int Lives{get; set;}
        public int Time { get; set; }
        public int Score { get; set; }
        public Game1 game;
        public HUD(Game game)
        {
            this.game = (Game1) game;
            Lives = 3;
            Time = 60;
            Score = 0;
        }
    }
}
