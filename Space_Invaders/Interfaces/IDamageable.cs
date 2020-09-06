using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
namespace Space_Invaders.Interfaces
{
    interface IDamageable
    {
        Sprite Sprite { get; set; }
        bool Destroyed { get; set; }
        void Damage(Vector2 location);
    }
}
