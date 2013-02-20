using System;
using Microsoft.Xna.Framework;
using Shoe.Lib.Input;
using Microsoft.Xna.Framework.Input;
using Shoe.Lib;
using Shoe.Lib.Characters;
using System.Collections.Generic;//daniel
using TiledLib;//daniel
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
namespace Shoe.Lib.Characters
{
    public class Player : Character
    {
        #region Fields

        #endregion

        #region Properties
        public int MaxBulletsOnScreen { get; set; }  
        public int MaxDynamiteOnScreen { get; set; }  
       
        public int MaxHitpoints { get; set; }  
        public Vector2 Target { get; set; } 
        public int Experiance { get; set; }  
        public int NeededExperiance { get; set; }  
        public int BulletsFired { get; set; }  
        public int BulletLife { get; set; }
        public int PistolAmmo { get; set; }
        public int ShotgunAmmo { get; set; }
        public int DynamiteAmmo { get; set; }
        public int ExpAddAmount { get; set; }
        public int MaxArmor { get; set; }
        public int Armor { get; set; }
        public bool Invincible { get; set; }
        public bool UnlimitedAmmo { get; set; }
        public int InvincibleTimer { get; set; }
        public int UnlimitedAmmoTimer { get; set; }
        public int ClockInvincible { get; set; }
        public int ClockUnlimitedAmmo { get; set; }
        public Dynamite [] dynamites;
        public Explosion [] explosions;
        private SoundEffect PistolShot;
        private SoundEffect ShotgunShot;
        private SoundEffect LevelUpSound;
         public Bullet[] bullets;
        public Gun currentGun { get; set; }
        public bool LowOnHealth;
        public bool LowOnShotgun;
        public bool LowOnPistol;
        public bool LowOnDynamite;
        public bool LowOnArmor;
        #endregion

        #region Constructor

        public Player()
        {
        }

        public Player(Vector2 startLocation, Vector2 frameSize, int framesPerDirection)
        {
            Position = startLocation;
            FrameSize = frameSize;
            Origin = frameSize / 2;
            FrameLength = 0.05f;
            MaxFramesPerDirection = framesPerDirection;
            Direction = Direction.South;
            Speed = 5f;
            MaxHitpoints = 100;
            Hitpoints = MaxHitpoints;
            DamageModifier = 1;
            Level = 0;
            Experiance = 0;
            NeededExperiance = 100;
            MaxBulletsOnScreen = 9;
            MaxDynamiteOnScreen = 2;
            bullets = new Bullet[MaxBulletsOnScreen];
            dynamites = new Dynamite[MaxDynamiteOnScreen];
            explosions = new Explosion[MaxDynamiteOnScreen];
            currentGun = Gun.Pistol;
            BulletLife = 600;
            Alive =true;
            PistolAmmo = 10;
            ShotgunAmmo = 5;
            DynamiteAmmo = 0;
            ExpAddAmount = 10;
            MaxArmor = 100;
            Armor = 0;
            Invincible = false;
            UnlimitedAmmo = false;
            InvincibleTimer = 600;
            UnlimitedAmmoTimer = 600;
            ClockInvincible  = 0;
            ClockUnlimitedAmmo = 0;
        }

        #endregion

        #region Methods
        public void LevelUp()
        {

            Experiance = Experiance - NeededExperiance;
            NeededExperiance = (int)(NeededExperiance * 1.15);
            Level++;
            Speed = Speed + (.25f);
            MaxHitpoints = MaxHitpoints + 10;
            Hitpoints = MaxHitpoints;
            DamageModifier = DamageModifier +.50f;
            LevelUpSound.Play();

        }
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            foreach (Bullet bullet in bullets)
            {
                if (bullet.Alive == true)
                {
                    bullet.Draw(spriteBatch);
                }
            }
            foreach (Dynamite  dynamite in dynamites )
            {
                if (dynamite.Alive == true)
                {
                    dynamite.Draw(spriteBatch);
                }
                if (dynamite.Explosion.Alive == true)
                {
                    dynamite.Explosion.Draw(spriteBatch);
                }
            }
        }
        public void HandleInput(InputState input, PlayerIndex? ControllingPlayer)
        {
            Vector2 playerMovement = Vector2.Zero;
            int playerIndex = (int)ControllingPlayer;

            if (input.GamePadWasConnected[playerIndex])
            {
                playerMovement.X = input.CurrentGamePadStates[playerIndex].ThumbSticks.Left.X;
                playerMovement.Y = -input.CurrentGamePadStates[playerIndex].ThumbSticks.Left.Y;
                 
            
                if ((input.CurrentGamePadStates[playerIndex].Buttons.RightShoulder == ButtonState.Pressed )& (input.LastGamePadStates[playerIndex].Buttons.RightShoulder == ButtonState.Released))
                {
                    if (currentGun == Gun.Pistol)
                    {
                        currentGun = Gun.Shotgun;
                    }
                    else if (currentGun == Gun.Shotgun)
                    {
                        currentGun = Gun.Dynamite;
                    }
                    else
                    {
                        currentGun = Gun.Pistol;
                    }

                }
                if ((input.CurrentGamePadStates[playerIndex].Buttons.LeftShoulder  == ButtonState.Pressed) & (input.LastGamePadStates[playerIndex].Buttons.LeftShoulder  == ButtonState.Released))
                {
                    if (currentGun == Gun.Pistol)
                    {
                        currentGun = Gun.Dynamite ;
                    }
                    else if (currentGun == Gun.Shotgun)
                    {
                        currentGun = Gun.Pistol ;
                    }
                    else
                    {
                        currentGun = Gun.Shotgun ;
                    }

                }
                

            if(input.CurrentGamePadStates [playerIndex ].Triggers .Right > 0.9f & input.LastGamePadStates  [playerIndex ].Triggers .Right <0.9f)
            {
                if (currentGun == Gun.Pistol & PistolAmmo > 0)
                {
                    FirePistol();

                }
                else if (currentGun == Gun.Shotgun & ShotgunAmmo > 0)
                {
                    FireShotgun();

                }
                else if (currentGun == Gun.Dynamite & DynamiteAmmo > 0)
                {
                    ThrowDynamite();
                }
            }
            if (input.CurrentGamePadStates[playerIndex].ThumbSticks.Right.X == 1.0f & input.CurrentGamePadStates[playerIndex].ThumbSticks.Right.Y < 0.25f & input.CurrentGamePadStates[playerIndex].ThumbSticks.Right.Y > -.25f)
            {
                Direction = Direction.East;
            }
            else if (input.CurrentGamePadStates [playerIndex].ThumbSticks .Right.Y  == -1.0f &input.CurrentGamePadStates[playerIndex].ThumbSticks.Right.X < 0.25f & input.CurrentGamePadStates[playerIndex].ThumbSticks.Right.X  > -.25f)
           
            {
                Direction = Direction.South ;
            }
            else if (input.CurrentGamePadStates [playerIndex].ThumbSticks .Right.X == -1.0f & input.CurrentGamePadStates[playerIndex].ThumbSticks.Right.Y < 0.25f & input.CurrentGamePadStates[playerIndex].ThumbSticks.Right.Y > -.25f)
               {
                Direction = Direction.West ;
            }
            else if (input.CurrentGamePadStates [playerIndex].ThumbSticks .Right.Y  == 1.0f&input.CurrentGamePadStates[playerIndex].ThumbSticks.Right.X < 0.25f & input.CurrentGamePadStates[playerIndex].ThumbSticks.Right.X  > -.25f)
             {
                Direction = Direction.North ;
            }
            if (input.CurrentGamePadStates[playerIndex].ThumbSticks.Right.Y >0.0f & input.CurrentGamePadStates[playerIndex].ThumbSticks.Right.Y < 1.0f &
                input.CurrentGamePadStates[playerIndex].ThumbSticks.Right.X > 0.0f & input.CurrentGamePadStates[playerIndex].ThumbSticks.Right.X < 1.0f)
            {
                Direction = Direction.NorthEast;
            }
            else if (input.CurrentGamePadStates[playerIndex].ThumbSticks.Right.Y < 0.0f & input.CurrentGamePadStates[playerIndex].ThumbSticks.Right.Y > -1.0f &
                input.CurrentGamePadStates[playerIndex].ThumbSticks.Right.X > 0.0f & input.CurrentGamePadStates[playerIndex].ThumbSticks.Right.X < 1.0f)
            {
                Direction = Direction.SouthEast;
            }
            else if (input.CurrentGamePadStates[playerIndex].ThumbSticks.Right.Y > 0.0f & input.CurrentGamePadStates[playerIndex].ThumbSticks.Right.Y < 1.0f & 
                input.CurrentGamePadStates[playerIndex].ThumbSticks.Right.X < 0.0f & input.CurrentGamePadStates[playerIndex].ThumbSticks.Right.X > -1.0f)
            {
                Direction = Direction.NorthWest ;
            }
            if (input.CurrentGamePadStates[playerIndex].ThumbSticks.Right.Y < 0.0f & input.CurrentGamePadStates[playerIndex].ThumbSticks.Right.Y > -1.0f &
                input.CurrentGamePadStates[playerIndex].ThumbSticks.Right.X < 0.0f & input.CurrentGamePadStates[playerIndex].ThumbSticks.Right.X > -1.0f)
            {
                Direction = Direction.SouthWest  ;
            }
               

        }
            else
            {
                if (input.CurrentKeyboardStates[playerIndex].IsKeyDown(Keys.A) & !(input.CurrentKeyboardStates[playerIndex].IsKeyDown(Keys.D)))
                {
                    playerMovement.X = -1;
                }
                else if (input.CurrentKeyboardStates[playerIndex].IsKeyDown(Keys.D) & !(input.CurrentKeyboardStates[playerIndex].IsKeyDown(Keys.A)))
                {
                    playerMovement.X = 1;
                }
                if (input.CurrentKeyboardStates[playerIndex].IsKeyDown(Keys.W) & !(input.CurrentKeyboardStates[playerIndex].IsKeyDown(Keys.S)))
                {
                    playerMovement.Y = -1;
                }
                else if (input.CurrentKeyboardStates[playerIndex].IsKeyDown(Keys.S) & !(input.CurrentKeyboardStates[playerIndex].IsKeyDown(Keys.W)))
                {
                    playerMovement.Y = 1;
                }
                
                if (playerMovement != Vector2.Zero)
                {
                    playerMovement.Normalize();
                }
                if ((input.CurrentKeyboardStates[playerIndex].IsKeyDown(Keys.D1) & input.LastKeyboardStates[playerIndex].IsKeyUp(Keys.D1)) ||(input.CurrentKeyboardStates[playerIndex].IsKeyDown(Keys.NumPad1) & input.LastKeyboardStates[playerIndex].IsKeyUp(Keys.NumPad1)))
                {
                    currentGun = Gun.Pistol;
                }
           
                if ((input.CurrentKeyboardStates[playerIndex].IsKeyDown(Keys.D2) & input.LastKeyboardStates[playerIndex].IsKeyUp(Keys.D2))||(input.CurrentKeyboardStates[playerIndex].IsKeyDown(Keys.NumPad2) & input.LastKeyboardStates[playerIndex].IsKeyUp(Keys.NumPad2)))
                {
                    currentGun = Gun.Shotgun;
                }
               
                if ((input.CurrentKeyboardStates[playerIndex].IsKeyDown(Keys.D3) & input.LastKeyboardStates[playerIndex].IsKeyUp(Keys.D3))||(input.CurrentKeyboardStates[playerIndex].IsKeyDown(Keys.NumPad3) & input.LastKeyboardStates[playerIndex].IsKeyUp(Keys.NumPad3)))
           
                {
                    currentGun = Gun.Dynamite;
                }
                

                if (input.CurrentKeyboardStates[playerIndex].IsKeyDown(Keys.Space) & input.LastKeyboardStates[playerIndex].IsKeyUp(Keys.Space))
                {
                    if (currentGun == Gun.Pistol & PistolAmmo >0)
                    {
                        FirePistol();
                       
                    }
                    else if (currentGun == Gun.Shotgun & ShotgunAmmo >0)
                    {
                        FireShotgun();
                        
                    }
                    else if (currentGun == Gun.Dynamite & DynamiteAmmo > 0)
                    {
                        ThrowDynamite();
                    }

                }

            }

            if (playerMovement == Vector2.Zero)
                IsAnimating = false;
            else
                IsAnimating = true;
            
            if (input.CurrentKeyboardStates[playerIndex].IsKeyDown(Keys.Up) & input.CurrentKeyboardStates[playerIndex].IsKeyDown(Keys.Right))
            {
                Direction = Direction.NorthEast;
            }
            else if (input.CurrentKeyboardStates[playerIndex].IsKeyDown(Keys.Right) & input.CurrentKeyboardStates[playerIndex].IsKeyDown(Keys.Down))
            {
                Direction = Direction.SouthEast;
            }
            else if (input.CurrentKeyboardStates[playerIndex].IsKeyDown(Keys.Down) & input.CurrentKeyboardStates[playerIndex].IsKeyDown(Keys.Left))
            {
                Direction = Direction.SouthWest;
            }
            else if (input.CurrentKeyboardStates[playerIndex].IsKeyDown(Keys.Left) & input.CurrentKeyboardStates[playerIndex].IsKeyDown(Keys.Up))
            {
                Direction = Direction.NorthWest;
            }
            else if (input.CurrentKeyboardStates[playerIndex].IsKeyDown(Keys.Right))
            {
                Direction = Direction.East;
            }
            else if (input.CurrentKeyboardStates[playerIndex].IsKeyDown(Keys.Down))
            {
                Direction = Direction.South;
            }
            else if (input.CurrentKeyboardStates[playerIndex].IsKeyDown(Keys.Left))
            {
                Direction = Direction.West;
            }
            else if (input.CurrentKeyboardStates[playerIndex].IsKeyDown(Keys.Up))
            {
                Direction = Direction.North;
            }




            LastMovement = Position;

            Position += Speed * playerMovement;

            
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content, string assetName)
        {
            base.LoadContent(content, assetName);
            for (int i = 0; i < MaxBulletsOnScreen; i++)
            {
                bullets[i] = new Bullet("Textures\\Characters\\bullet", 3000, Color.Yellow, 9.0f,BulletLife);
                bullets[i].LoadContent(content, bullets[i].AssetName);
            }
            for (int i = 0; i < MaxDynamiteOnScreen ; i++)
            {
                explosions[i] = new Explosion("Textures\\Characters\\explosion", Color.White, 50);
                dynamites[i] = new Dynamite("Textures\\Characters\\Dynamite", 3000, Color.Red, 300, 0.15f, explosions[i],90);
                dynamites[i].LoadContent(content, dynamites[i].AssetName);
                explosions[i].LoadContent(content, explosions[i].AssetName); 
            }
            PistolShot = content.Load<SoundEffect>("Sounds/pistol_heavy");
            ShotgunShot = content.Load<SoundEffect>("Sounds/shotgun");
            LevelUpSound  = content.Load<SoundEffect>("Sounds/level up sound");
            HitSound  = content.Load<SoundEffect>("Sounds/ShoeHit");
            DeathSound  = content.Load<SoundEffect>("Sounds/ShoeDeath");
           
             }


        public void Update(Map map, GameTime gameTime, Camera camera, List<Enemy> Enemies, Dictionary<Vector2, Rectangle> AmmoClipMap)
        {
            base.Update(gameTime, camera);
            if (Hitpoints > MaxHitpoints)
            {
                Hitpoints = MaxHitpoints;
            }
            if (Armor > MaxArmor)
            {
                Armor = MaxArmor;
            }
            Tint = Color.White;
            if (UnlimitedAmmo == true)
            {
                ClockUnlimitedAmmo++;
                Tint = Color.Red;
                if (ClockUnlimitedAmmo  >= UnlimitedAmmoTimer )
                {
                    UnlimitedAmmo  = false;
                    
                    ClockUnlimitedAmmo = 0;
                    
                }
            }
            if (Invincible == true)
            {
                ClockInvincible  ++;
                Tint = Color.Gold ;
              if (ClockInvincible  >= InvincibleTimer)
                {
                    Invincible = false;
                    ClockInvincible  = 0;
                   
                }
            }
            if (Invincible == true & UnlimitedAmmo == true)
            {
                Tint = Color.OrangeRed;
            }

            CheckStatus();
            foreach (Bullet bullet in bullets)
            {
                if (bullet.Alive == true)
                {
                    bullet.Update(map, Enemies, AmmoClipMap);
                }
            }
            foreach (Dynamite  dynamite in dynamites )
            {
                if (dynamite.Alive == true )
                {
                    dynamite.Update(map, Enemies, AmmoClipMap);
                }
                 if (dynamite.Explosion.Alive == true)
                {
                    dynamite.Explosion.Update(map, Enemies);
                }

            }
            if (Experiance >= NeededExperiance)
            {
                LevelUp();
            }
        }

     
        private void CheckStatus()
    {
        if (Hitpoints < (MaxHitpoints / 5))
        { LowOnHealth = true; }
        else
        { LowOnHealth = false; }
        if (Armor  < (MaxArmor  / 5))
        { LowOnArmor  = true; }
        else
        { LowOnArmor = false; }
        if (PistolAmmo  <= 15)
        { LowOnPistol  = true; }
        else
        { LowOnPistol = false; }
        if (ShotgunAmmo  <= 5)
        { LowOnShotgun  = true; }
        else
        { LowOnShotgun  = false; }
        if (DynamiteAmmo   <= 3 )
        { LowOnDynamite  = true; }
        else
        { LowOnDynamite  = false; }




    }
        private void FirePistol()
        {
            foreach (Bullet bullet in bullets)
            {
                if (bullet.Alive == false)
                {
                    PistolShot.Play();
                    switch (Direction)
                    {
                        case Direction.North:
                            Position = Position;
                            Target = new Vector2(0, -1);
                            break;
                        case Direction.NorthEast:
                            Position = Position;
                            Target = new Vector2(1, -1);
                            break;
                        case Direction.East:
                            Position = Position;
                            Target = new Vector2(1, 0);
                            break;
                        case Direction.SouthEast:
                            Position = Position;
                            Target = new Vector2(1, 1);
                            break;
                        case Direction.South:
                            Position = Position;
                            Target = new Vector2(0, 1);
                            break;
                        case Direction.SouthWest:
                            Position = Position;
                            Target = new Vector2(-1, 1);
                            break;
                        case Direction.West:
                            Position = Position;
                            Target = new Vector2(-1, 0);
                            break;
                        case Direction.NorthWest:
                            Position = Position;
                            Target = new Vector2(-1, -1);
                            break;

                    }
                    bullet.FireBullet(Position, Target,600, DamageModifier );
                    bullet.Alive = true;
                  
                    if(!UnlimitedAmmo )
                         PistolAmmo--;
                    break;
                }
            }
        }
        private void ThrowDynamite()
        {
            foreach (Dynamite  dynamite in dynamites )
            {
                if (dynamite.Alive == false)
                {
                    switch (Direction)
                    {
                        case Direction.North:
                            Position = Position;
                            Target = new Vector2(0, -1);
                            break;
                        case Direction.NorthEast:
                            Position = Position;
                            Target = new Vector2(1, -1);
                            break;
                        case Direction.East:
                            Position = Position;
                            Target = new Vector2(1, 0);
                            break;
                        case Direction.SouthEast:
                            Position = Position;
                            Target = new Vector2(1, 1);
                            break;
                        case Direction.South:
                            Position = Position;
                            Target = new Vector2(0, 1);
                            break;
                        case Direction.SouthWest:
                            Position = Position;
                            Target = new Vector2(-1, 1);
                            break;
                        case Direction.West:
                            Position = Position;
                            Target = new Vector2(-1, 0);
                            break;
                        case Direction.NorthWest:
                            Position = Position;
                            Target = new Vector2(-1, -1);
                            break;

                    }

                    dynamite.ThrowDynamite(Position, Target, 9.0f);
                    dynamite.Alive = true;

                    if (!UnlimitedAmmo)
                        DynamiteAmmo--;
                   

                    break;
                }
            }
        }
        private void FireShotgun()
        {
            foreach (Bullet bullet in bullets)
            {
                ShotgunShot.Play();
                if (bullet.Alive == false)
                {
                    
                    switch (Direction)
                    {
                        case Direction.North:
                            Position = Position;
                            Target = new Vector2(0, -1);
                            break;
                        case Direction.NorthEast:
                            Position = Position;
                            Target = new Vector2(.8f, -.8f);
                            break;
                        case Direction.East:
                            Position = Position;
                            Target = new Vector2(1, 0);
                            break;
                        case Direction.SouthEast:
                            Position = Position;
                            Target = new Vector2(.8f, .8f);
                            break;
                        case Direction.South:
                            Position = Position;
                            Target = new Vector2(0, 1);
                            break;
                        case Direction.SouthWest:
                            Position = Position;
                            Target = new Vector2(-.8f, .8f);
                            break;
                        case Direction.West:
                            Position = Position;
                            Target = new Vector2(-1, 0);
                            break;
                        case Direction.NorthWest:
                            Position = Position;
                            Target = new Vector2(-.8f, -.8f);
                            break;

                    }

                    bullet.FireBullet(Position, Target,200,DamageModifier );
                    bullet.Alive = true;
                    
                    
                    break;
                }
            }
            foreach (Bullet bulletLeft in bullets)
            {
                if (bulletLeft.Alive == false)
                {
                    switch (Direction)
                    {
                        case Direction.North:
                            Position = Position;
                            Target = new Vector2(-.33f, -1);
                            break;
                        case Direction.NorthEast:
                            Position = Position;
                            Target = new Vector2(.67f, -1);
                            break;
                        case Direction.East:
                            Position = Position;
                            Target = new Vector2(1, -.33f);
                            break;
                        case Direction.SouthEast:
                            Position = Position;
                            Target = new Vector2(1, .67f);
                            break;
                        case Direction.South:
                            Position = Position;
                            Target = new Vector2(.33f, 1);
                            break;
                        case Direction.SouthWest:
                            Position = Position;
                            Target = new Vector2(-.67f, 1);
                            break;
                        case Direction.West:
                            Position = Position;
                            Target = new Vector2(-1, .33f);
                            break;
                        case Direction.NorthWest:
                            Position = Position;
                            Target = new Vector2(-1, -.67f);
                            break;

                    }
                   // Target = new Vector2 (0.33f *Target .X, Target .Y ) ;
                    bulletLeft.FireBullet(Position, Target,200,DamageModifier );
                    bulletLeft.Alive = true;
                    
                    break;
                }
            }
            foreach (Bullet bulletRight in bullets)
            {
                if (bulletRight.Alive == false)
                {
                    switch (Direction)
                    {
                        case Direction.North:
                            Position = Position;
                            Target = new Vector2(.33f, -1);
                            break;
                        case Direction.NorthEast:
                            Position = Position;
                            Target = new Vector2(1, -.67f);
                            break;
                        case Direction.East:
                            Position = Position;
                            Target = new Vector2(1, .33f);
                            break;
                        case Direction.SouthEast:
                            Position = Position;
                            Target = new Vector2(.67f, 1);
                            break;
                        case Direction.South:
                            Position = Position;
                            Target = new Vector2(-.33f, 1);
                            break;
                        case Direction.SouthWest:
                            Position = Position;
                            Target = new Vector2(-1, .67f);
                            break;
                        case Direction.West:
                            Position = Position;
                            Target = new Vector2(-1, -.33f);
                            break;
                        case Direction.NorthWest:
                            Position = Position;
                            Target = new Vector2(-.67f, -1);
                            break;

                    }
                    bulletRight.FireBullet(Position, Target,200,DamageModifier );
                    bulletRight.Alive = true;
                    if (!UnlimitedAmmo)
                        ShotgunAmmo--;

                    break; 
                }
            }
        }

        public  void TakeDamage(int damage)
        {
            if (Invincible == false )
            {
                if (Armor > damage)
                {
                    Armor = Armor - damage;
                   
                }
                else
                {
                    damage = damage - Armor;
                    Armor = 0;
                    Hitpoints = Hitpoints - damage;
                } 
                HitSound.Play();
            }
            if (Hitpoints <= 0)
            {
                Alive = false;
            }

        }
        #endregion
    }
}
