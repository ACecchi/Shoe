using System;
using Microsoft.Xna.Framework;
using Shoe.Lib.Input;
using Microsoft.Xna.Framework.Input;
using Shoe.Lib;
using Shoe.Lib.Characters;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
namespace Shoe.Lib.Characters
{


    public class Item : Character
    {
        public string Type { get; set; }
        private SoundEffect collectedSound { get; set; }
        private float bounce { get; set; }
        int randomInt;
        int range;
        string ItemName;
        int HealthRange;
        int ShotgunRange;
        int PistolRange;
        int ArmorRange;
        int DynamiteRange;
        int ExperienceRange;
        int InvincibilityRange;
        int UnlimitedAmmoRange;
        SoundEffect PickupSound;
      
        public Item(Vector2 startLocation)
        {
            Position = startLocation;
           
            FrameLength = 0.5f;
          
            Direction = Direction.South;
          
            IsAnimating = false;
         
            Alive = true;
            LayerDepth = 0.0f;
            ItemName = "";

            
        }

        public override void Update(GameTime gameTime)
        {

            if (Alive)
            {
                // Bounce control constants
                const float BounceHeight = 0.15f;
                const float BounceRate = 2.0f;
                const float BounceSync = -0.75f;

                // Bounce along a sine curve over time.
                // Include the X coordinate so that neighboring gems bounce in a nice wave pattern.            
                double t = gameTime.TotalGameTime.TotalSeconds * BounceRate + Position.X * BounceSync;
                bounce = (float)Math.Sin(t) * BounceHeight * Texture.Height;
            }
        }

      
        public void Collision(Player player)
        { 
            
            
            switch (Type)
            {
                case "HealthPickup":
                    if (player.Hitpoints != player.MaxHitpoints)
                    {
                        HealthPickup(player);
                        Alive = false;
                    }
                        break;
                case "Invincibility":
                        InvincibilityPickup(player);
                        Alive = false;
                        break;
                case "PistolAmmo":
                        PistolAmmoPickup(player);
                        Alive = false;
                        break;
                case "ShotgunAmmo":
                        ShotgunAmmoPickup(player);
                        Alive = false;
                        break;
                case "Armor":
                        if (player.Armor != player.MaxArmor )
                        {
                            ArmorPickup(player);
                            Alive = false;
                        }
                        break;
                case "Dynamite":
                        DynamitePickup(player);
                        Alive = false;
                       
                        break;
                case "Experience":
                        ExperienceGain(player);
                        Alive = false;
                        break;
                case "UnlimitedAmmo":
                        UnlimitedAmmoPickup(player);
                        Alive = false;
                        break;
                    
                    
            }
            
                    

        }
        private int  HealthPickup(Player player)
        {
            PickupSound.Play();
            return (player.Hitpoints += 15);
     
        }
        private int ExperienceGain(Player player)
        {

            PickupSound.Play();
            return (player.Experiance  += 15);

        }
        private void  InvincibilityPickup(Player player)
        {
            PickupSound.Play();
            player.Invincible = true;
            player.Tint = Color.Gold;
        }
        private void UnlimitedAmmoPickup(Player player)
        {
            PickupSound.Play();
            player.UnlimitedAmmo = true;
            player.Tint = Color.Silver;
        }
        private void ArmorPickup(Player player)
        {
            PickupSound.Play();
            player.Armor += 15;
        }
        private int PistolAmmoPickup(Player player)
            
        {

            PickupSound.Play();
            return (player.PistolAmmo += 8);
     
        }
        private   int ShotgunAmmoPickup(Player player)
        {
            PickupSound.Play();  
            return (player.ShotgunAmmo +=4);
        }
        private int DynamitePickup(Player player)
        {
            PickupSound.Play();
            return (player.DynamiteAmmo  += 2);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            if (Alive)
            {
                spriteBatch.Draw(Texture, new Vector2(Position.X, Position.Y + bounce), new Rectangle(0, 0, Texture.Width, Texture.Height), Tint, MathHelper.ToRadians(RotationAngle), Origin, Scale, Orientation, LayerDepth);

            }
        }
        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content, string assetName)
        {
            base.LoadContent(content, assetName);
            PickupSound  = content.Load<SoundEffect>("Sounds\\"+Type);
      
        }
        public string ItemDrop(Player player, Vector2 position, Microsoft.Xna.Framework.Content.ContentManager content, int check)
        {

            Alive = true;
             //UpdateBounds(map.TileWidth, map.TileHeight);
            Position = position;
            if (player.LowOnHealth == true)
            {
                HealthRange = 45;
            }
            else
            {
                HealthRange = 20;
            }
            if (player.LowOnPistol == true)
            {
                PistolRange = 45 + HealthRange;
            }
            else
            {
                PistolRange = 20 + HealthRange;
            }
            if (player.LowOnShotgun == true)
            {
                ShotgunRange = 25 + PistolRange;
            }
            else
            {
                ShotgunRange = 15 + PistolRange;
            }
            if (player.LowOnArmor == true)
            {
                ArmorRange = 25 + ShotgunRange;
            }
            else
            {
                ArmorRange = 10 + ShotgunRange;
            }
            if (player.LowOnDynamite == true)
            {
                DynamiteRange = 25 + ArmorRange;
            }
            else
            {
                DynamiteRange = 15 + ArmorRange;
            }

            ExperienceRange = 10 + DynamiteRange;
            UnlimitedAmmoRange   = 15 + ExperienceRange;
            InvincibilityRange = 15 + ExperienceRange;
            Random random = new Random(check); // Arbitrary, but constant seed
      
            range = InvincibilityRange;

            randomInt =  random.Next(1, range);
            //  random().Next(1, range);
            if (randomInt < HealthRange)
            {
                Type = "HealthPickup";
            }
            else if (randomInt < PistolRange)
            {
                Type = "PistolAmmo";
            }
            else if (randomInt < ShotgunRange)
            {
                Type = "ShotgunAmmo";
            }
            else if (randomInt < ArmorRange)
            {
                Type = "Armor";
            }
            else if (randomInt < DynamiteRange)
            {
                Type = "Dynamite";
            }
            else if (randomInt < ExperienceRange)
            {
                Type = "Experience";
            }
            else if (randomInt <= UnlimitedAmmoRange )
            {
                Type = "UnlimitedAmmo";
            }
            else if (randomInt <= InvincibilityRange)
            {
                Type = "Invincibility";
            }


            AssetName = "Textures\\Items\\" + Type.ToString();
            
            LoadContent(content, AssetName);
         

            return ItemName;

        }
    }
}
