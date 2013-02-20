using System;
using Microsoft.Xna.Framework;
using Shoe.Lib.Input;
using Microsoft.Xna.Framework.Input;
using Shoe.Lib;
using Shoe.Lib.Characters;
using TiledLib;
using System.Collections.Generic;//daniel
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Shoe.Lib.Characters
{
    public class Dynamite : Character
    {
        private int ThrowDistance { get; set; }
        private Rectangle Exposion { get; set; }
        public int Damage { get; set; }
        public int Distance { get; set; }
        public Vector2 Movement { get; set; }
        public float SpeedDecay { get; set; }
       // GameTime DetonatorTime;
        int FuseTime { get; set; }
        int ExplodeTime { get; set; }
        public Explosion Explosion { get; set; }
        private SoundEffect dynamiteExplosion;


        public Dynamite(string AssetName, int lifeTime, Color tint, int throwDistance, float speedDecay, Explosion explosion,int explodeTime)
        {
            Position = Vector2.Zero;
            this.AssetName = AssetName;
            Origin = Vector2.Zero;
            Movement = Vector2 .Zero ;
            ThrowDistance = throwDistance;
            Alive = false;
            Tint = tint;
            Explosion = explosion;
            SpeedDecay = speedDecay;
            Damage = 10;
            IsAnimating = false;
            ExplodeTime = explodeTime;
       
         }
        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content, string assetName)
        {
            base.LoadContent(content, assetName);
            dynamiteExplosion = content.Load<SoundEffect>("Sounds/dynamite");
      
        }
        public void Update(Map map, List<Enemy> Enemies, Dictionary<Vector2, Rectangle> AmmoClipMap)
        {

            //Only update them if they're alive
            if (Alive)
            {
                Distance = Distance + Math.Abs((int)Speed * (int)Movement.X) + Math.Abs((int)Speed * (int)Movement.Y);
                Position = Position + (Speed * Movement);
                Speed = Speed - SpeedDecay;
                if (Speed < 0)
                    Speed = 0;
                CheckCollisions(map, Enemies, AmmoClipMap);
                FuseTime++;
                if (FuseTime >= ExplodeTime)
                {
                    Explosion.Alive = true;
                    Explosion.Position   =  new Vector2 ((Position.X+Texture .Width /2) - (Explosion .Texture .Width/2) ,(Position.Y+Texture .Height/2 ) - (Explosion .Texture .Height /2));
                    Explosion.Timer = 0;
                    Alive = false;
                    dynamiteExplosion.Play();
                    FuseTime = 0;
       
                }

            }
        }
        public void ThrowDynamite(Vector2 shooterPosition, Vector2 movement, float speed)
        {

            //Find a bullet that isn't alive
            //pistolSound.Play();
            //And set it to alive.
            Position = shooterPosition;
            Movement = movement;
            Alive = true;
            Speed = speed;
            //Move our bullet based on it's velocity

            FuseTime = 0;
     
            
        }
      

        private void CheckCollisions(Map map, List<Enemy> Enemies, Dictionary<Vector2, Rectangle> ClipMap)
        {
            UpdateBounds(map.TileWidth, map.TileHeight);

            foreach (Rectangle clip in ClipMap.Values)
            {
                if (Bounds.Intersects(clip))
                {
                    Speed = 0 ;
                    break;
                }
            }
            foreach (Enemy enemy in Enemies)
            {
                if (Bounds.Intersects(enemy.Bounds) & enemy.Alive)
                {
                    Speed = 0;
                    break;
                }
            }
        }
        public void UpdateBounds()
        {
            Bounds = new Rectangle((int)(Position.X - (Texture.Width / 4)), (int)(Position.Y - (Texture.Height / 4)), (int)(Position.X + (Texture.Width / 4)), (int)(Position.Y + (Texture.Height / 4))); ;
        }



        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, new Rectangle(0, 0, Texture.Width, Texture.Height), Tint, MathHelper.ToRadians(RotationAngle), Origin, Scale, Orientation, LayerDepth);
        }
        
        


    }


}


