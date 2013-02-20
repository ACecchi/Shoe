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
     public class Bullet: Character  
    {

        
        

        public Vector2  Movement { get; set; }
        public int Life { get; set; }
        public int LifeTime { get; set; }
        public float MaxDistance{ get; set; }
        public float Distance { get; set; }
        public int Damage { get; set; }
       // public float DamageModifier;
       
        public Rectangle rectangle
        {
            get
            {
                int left = (int)Position.X;
                int width = Texture.Width;
                int top = (int)Position.Y;
                int height = Texture.Height;
                return new Rectangle(left, top, width, height);
            }
        }
       
        public Bullet(string AssetName, int lifeTime,  Color tint, float  speed, int maxdistance)
        {
            Position = Vector2.Zero;
            this.AssetName = AssetName;
            Origin = Vector2.Zero;
            Movement = Vector2 .Zero ;
            MaxDistance = maxdistance;
            Alive = false;
            this.LifeTime = lifeTime;
            Tint = tint;
            Speed = speed;
            Damage = 5;
        }
        public void Update(Map map, List<Enemy> Enemies, Dictionary<Vector2, Rectangle> AmmoClipMap)
        {
            
                //Only update them if they're alive
                if (Alive)
                {

                    Distance = Distance + Math.Abs( Speed * Movement.X) + Math.Abs( Speed * Movement.Y);
                    Position = Position + (Speed * Movement);
                    if (Distance >= MaxDistance)
                    {
                        Alive = false;
                        Distance = 0;
                    }
                    
                    CheckCollisions(map, Enemies, AmmoClipMap);
                }


            

        }
        public void Update(Map map, Player player, Dictionary<Vector2, Rectangle> AmmoClipMap)
        {

            //Only update them if they're alive
            if (Alive)
            {
                Distance = Distance + Math.Abs((int)(Speed * Movement.X)) + Math.Abs((int)(Speed * Movement.Y));
                Position = Position + (Speed * Movement);
                if (Distance >= MaxDistance)
                {
                    Alive = false;
                    Distance = 0;
                }

                CheckCollisions(map, player , AmmoClipMap);
            }




        }

        private void CheckCollisions(Map map, List<Enemy> Enemies, Dictionary<Vector2, Rectangle> ClipMap)
         {
            UpdateBounds(map.TileWidth, map.TileHeight);

            foreach (Rectangle clip in ClipMap.Values)
            {
                if (Bounds.Intersects(clip))
                {
                    Collision();
                    break;
                }
            }
            foreach (Enemy enemy in Enemies )
            {
                if (Bounds.Intersects(enemy.Bounds)& enemy.Alive)
                {
                    Collision();
                    enemy.TakesDamage((int)(Damage + DamageModifier));
                    break;
                }
            }
        }
        private void CheckCollisions(Map map, Player Player, Dictionary<Vector2, Rectangle> ClipMap)
        {
            UpdateBounds(map.TileWidth, map.TileHeight);

            foreach (Rectangle clip in ClipMap.Values)
            {
                if (Bounds.Intersects(clip))
                {
                    Collision();
                    break;
                }
            }
            
                if (Bounds.Intersects(Player.Bounds))
                {
                    Collision();
                    Player.TakeDamage((int)(Damage + DamageModifier));
                    
                }
            
        }
        public new void Collision()
        {
            Distance = 0;
            Alive = false;

        }
        public void FireBullet(Vector2 shooterPosition, Vector2 movement, int distance, float damageModifier)
        {
                     
                //Find a bullet that isn't alive
                                    //pistolSound.Play();
                    //And set it to alive.
                    Position = shooterPosition;
                    Movement = movement;
                    MaxDistance = distance;
                    DamageModifier = damageModifier;
                    Alive = true;
                         //Move our bullet based on it's velocity

                   

                    


                
            }

        
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, new Rectangle (0,0, Texture .Width ,Texture.Height ), Tint, MathHelper.ToRadians(RotationAngle), Origin, Scale , Orientation, LayerDepth);
        }

         
        public  void UpdateBounds()
        {
            Bounds = new Rectangle((int)(Position.X - (Texture.Width / 2)), (int)(Position.Y-(Texture.Height  / 2)),(int)(Position.X + (Texture.Width / 2)), (int)(Position.Y +(Texture.Height  / 2))); ;
        }
         

    }

}

     
