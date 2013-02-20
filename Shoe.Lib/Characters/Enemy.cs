using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TiledLib;
using Shoe.Lib.Characters;
using Shoe.Lib.Sprites;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;


namespace Shoe.Lib.Characters
{
    public class Enemy : Character
    {
        public string Type { get; set; }
        private int ExperianceValue { get; set; }
        Pathfinder pathfinder;
        public Vector2 enemyMovement = Vector2.Zero;
        public List<Vector2> path { get; set; }
      //  Map Map;
        TileLayer clipMap;
        Texture2D  check;
        public Bullet[] bullets;
        Random random = new Random(243223);
        int randomCheck;
        public int DropRate;
        public SoundEffect shot;
        Vector2 shootVector;
        public Vector2 ratio;
        public int shotTimer;
        public int ShootDistance;
        public int shotTimerMax;
        public int BulletLife;
        public float BulletSpeed;
        public Enemy(Vector2 startLocation, Vector2 frameSize, int framesPerDirection, int experianceValue, Map map, float speed, int hitpoints, float damageModifier, int numberOfBullets, int shootdistance, int shottimer, int bulletlife, float bulletspeed)
        {
            Position = startLocation;
            FrameSize = frameSize;
            Origin = frameSize / 2;
            FrameLength = 0.5f;
            MaxFramesPerDirection = framesPerDirection;
            Direction = Direction.South;
            Speed = 3.0f;
            IsAnimating = false;
            Hitpoints = hitpoints ;
            Alive = true;
            ExperianceValue = experianceValue;
            pathfinder = new Pathfinder(map);
            Map = map;
            bullets = new Bullet[numberOfBullets];
            DropRate = 25;
            DamageModifier = damageModifier;
            shotTimerMax = shottimer;
            ShootDistance = shootdistance;
            shotTimer = shotTimerMax;
            BulletLife = bulletlife ;
            BulletSpeed = bulletspeed;
           
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content, string assetName)
        {
            base.LoadContent(content, assetName);
            check = content.Load<Texture2D>("Textures\\Characters\\bullet");
            for (int i = 0; i < bullets .Count() ; i++)
            {
                bullets[i] = new Bullet("Textures\\Characters\\bullet", 3000, Color.Yellow, BulletSpeed, BulletLife);
                bullets[i].Tint = Color.Red;
                bullets[i].LoadContent(content, bullets[i].AssetName);
            }
            shot  = content.Load<SoundEffect>("Sounds/pistol_heavy");
            HitSound = content.Load<SoundEffect>("Sounds/enemyDamaged");
            DeathSound = content.Load<SoundEffect>("Sounds/enemyDeath");
   
        }
        public void Shoot(Vector2 direction)
        {
            
            foreach (Bullet bullet in bullets)
            {
                if (bullet.Alive == false)
                {
                    shot.Play();
                    bullet.FireBullet(Position, direction, BulletLife  , DamageModifier);
                    shotTimer  = shotTimerMax;
                    break;
                }
            }
                //Find a bullet that isn't alive
                                    //pistolSound.Play();
                    //And set it to alive.
                   
                         //Move our bullet based on it's velocity

                   

                    


                
         }

        public void Update(GameTime gameTime, Player player, Dictionary<Vector2, Rectangle> AmmoClipMap, Map map)
        {
            if (Hitpoints <= 0)
            {
                player.Experiance += ExperianceValue;
                Alive = false;
                // IsVisible = false;
                randomCheck = random.Next(100);
                
            }

            base.Update(gameTime);
            #region Astar
            path = pathfinder.FindPath(new Point((int)Position.X, (int)Position.Y), new Point((int)player.Position.X, (int)player.Position.Y));
            if (path != null)
            {


                clipMap = Map.GetLayer("Clip") as TileLayer;
                // if(Map.GetLayer(
                if (path.Count < 15)
                {

                    if (path.Count >= 0 && path.Count < 3)
                    {
                        enemyMovement = Vector2.Zero;
                    }



                    else
                    {

                        Tile tile = clipMap.Tiles[(int)(path[1].X / 64), (int)(path[1].Y / 64)];

                        if (path[1].X < Position.X)
                        {
                            enemyMovement.X = -1;

                        }
                        else if (path[1].X > Position.X)
                        {
                            enemyMovement.X = 1;
                        }
                        else
                        {
                            enemyMovement.X = 0;
                        }
                        if (path[1].Y < Position.Y)
                        {
                            enemyMovement.Y = -1;
                        }
                        else if (path[1].Y > Position.Y)
                        {
                            enemyMovement.Y = 1;
                        }
                        else
                        {
                            enemyMovement.Y = 0;
                        }
                       
                        
                    }

                }

                else
                {
                    enemyMovement = Vector2.Zero;

                }
            }
            else
            {
                Alive = false;
            }    
           



            if (path.Count < ShootDistance & shotTimer <= 0)
            {
                ratio.X = (   player.Position.X- Position.X);
                ratio.Y = (   player.Position.Y-Position.Y);
                shootVector = ratio;
                ratio.X = Math.Abs ( ratio.Y)+Math.Abs (ratio.X);
                ratio.Y = 1 / ratio.X;
                shootVector.X = (ratio.Y * shootVector.X);
                shootVector.Y =  (ratio.Y * shootVector.Y);
                Shoot(shootVector);
             }                
                      shotTimer--;



            LastMovement = Position;
            Position += enemyMovement * Speed;
            
           
            foreach (Bullet bullet in bullets)
            {
                if (bullet.Alive == true)
                {
                    bullet.Update(map, player , AmmoClipMap);
                }
            } 
           // enemyMovement = Vector2.Zero;

            #endregion Astar
        }
       
        public  void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch,SpriteFont Font)
        {
            
                base.Draw(spriteBatch);
            //  if (path != null )
             //   {
                  //  for (int i = 0; i < path.Count; i++)
                  // {
                    //    spriteBatch.Draw(check, path[i], new Rectangle(0, 0, check.Width, check.Height), Tint, MathHelper.ToRadians(RotationAngle), Origin, Scale, Orientation, LayerDepth);

                   // }
              // }
               foreach (Bullet bullet in bullets)
               {
                   if (bullet.Alive == true)
                   {
                       bullet.Draw(spriteBatch);
                   }
               }

            
            
        }
            
    }
    
}
