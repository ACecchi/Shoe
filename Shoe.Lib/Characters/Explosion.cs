using System;
using Microsoft.Xna.Framework;
using Shoe.Lib.Input;
using Microsoft.Xna.Framework.Input;
using Shoe.Lib;
using Shoe.Lib.Characters;
using TiledLib;
using System.Collections.Generic;//daniel


namespace Shoe.Lib.Characters
{
    public class Explosion : Character
    {

        public int Damage { get; set; }
        int ExplosionTimer { get; set; }
        public int Timer { get; set; }
       
      
        public Explosion(string AssetName, Color tint,  int timer)
        {
           // Position = position;
            this.AssetName = AssetName;
            Origin = Vector2.Zero;                
            Alive = false;
            Tint = tint;
            Timer = 0;
            ExplosionTimer = timer;
            Damage = 1;
          
            IsAnimating = false;
             
        }
       
        public  void Update(Map map, List<Enemy> Enemies)
        {

            //Only update them if they're alive
            if (Alive)
            {
               
                CheckCollisions(map, Enemies);
                Timer++;
                if (Timer >= ExplosionTimer)
                {

                    Alive = false;
                }

            }
        }
        private void CheckCollisions(Map map, List<Enemy> Enemies)
        {
            UpdateBounds();

            foreach (Enemy enemy in Enemies)
            {
                 if (Bounds.Intersects(enemy.Bounds))
                {
                    enemy.TakesDamage((int)(Damage ));
                    break;
                }
            }
        }
        public void UpdateBounds()
        {
            Bounds = new Rectangle((int)(Position.X - (Texture.Width / 2)), (int)(Position.Y - (Texture.Height / 2)), (int)( Texture.Width), (int)( Texture.Height)); ;
        }
       
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, new Rectangle(0, 0, Texture.Width, Texture.Height), Tint, MathHelper.ToRadians(RotationAngle), Origin, Scale, Orientation, LayerDepth);
        }
    }
}