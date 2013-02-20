using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Shoe.Lib;
using Shoe.Lib.Characters;
using Shoe.Lib.Screens;
using Shoe.Lib.Hud;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Shoe
{
    class HUD
    {
        
        //                  --Robert--                      \\
        //  Creating the health, experience, and armor bars \\

        SpriteFont hudFont;

        static Vector2 healthBarPos = new Vector2(149, 2);
        static Vector2 healthBarDimension = new Vector2(268, 20);
        GraphicBar healthBar = new GraphicBar(healthBarPos, healthBarDimension);

        static Vector2 expBarPos = new Vector2(149, 58);
        static Vector2 expBarDim = new Vector2(268, 20);
        GraphicBar expBar = new GraphicBar(expBarPos, expBarDim);

        static Vector2 armorBarPos = new Vector2(148, 30);
        static Vector2 armorBarDim = new Vector2(268, 20);
        GraphicBar armorBar = new GraphicBar(armorBarPos, armorBarDim);

        // Texture2D vars to hold the textures for the HUDs and weapons
        Texture2D upperHud;
        Texture2D lowerHud;
        Texture2D primaryWep;
        Texture2D secondaryWep;
        Texture2D thirdWep;
        
        // int variable to hold the value for the player's current equipped weapon or primary weapon
        int primary = 0;
        
        // int vars to hold the player's weapon ammo amounts.
        int primaryAmmo = 0;
        int secondaryAmmo = 0;
        int thirdAmmo = 0;

        // int variable to hold the player's current level
        int playerLevel = 0;

        // Loads the necessary content for the HUD
        public void LoadContent(ContentManager content)
        {
            hudFont = content.Load<SpriteFont>("Fonts\\hudFont");
            upperHud = content.Load<Texture2D>("HUD\\upperHud");
            lowerHud = content.Load<Texture2D>("HUD\\lowerHud");
            updatePrimary(content, primary);
        }
        
        // Updates the player's equipped weapons and their ammo as well as which weapon textures correspond to each slot in
        //      the lower HUD
        public void updatePrimary(ContentManager content, int primary)
        {
            switch (primary)
            {
                case 0:
                    primaryWep = content.Load<Texture2D>("HUD\\Weapons\\largePistol");
                    secondaryWep = content.Load<Texture2D>("HUD\\Weapons\\smallshotgun");
                    thirdWep = content.Load<Texture2D>("HUD\\Weapons\\smallDynamite");
                    
                    //checks to see if player is null before initializing values for the ammo
                    if (World.Player != null)
                    {
                        primaryAmmo = World.Player.PistolAmmo;
                        secondaryAmmo = World.Player.ShotgunAmmo;
                        thirdAmmo = World.Player.DynamiteAmmo;
                        playerLevel = World.Player.Level;
                    }
                    break;

                case 1:
                    primaryWep = content.Load<Texture2D>("HUD\\Weapons\\largeShotgun");
                    secondaryWep = content.Load<Texture2D>("HUD\\Weapons\\smallDynamite");
                    thirdWep = content.Load<Texture2D>("HUD\\Weapons\\smallPistol");
                    primaryAmmo = World.Player.ShotgunAmmo;
                    secondaryAmmo = World.Player.DynamiteAmmo;
                    thirdAmmo = World.Player.PistolAmmo;
                    playerLevel = World.Player.Level;
                    break;

                case 2:
                    primaryWep = content.Load<Texture2D>("HUD\\Weapons\\largeDynamite");
                    secondaryWep = content.Load<Texture2D>("HUD\\Weapons\\smallPistol");
                    thirdWep = content.Load<Texture2D>("HUD\\Weapons\\smallShotgun");
                    primaryAmmo = World.Player.DynamiteAmmo;
                    secondaryAmmo = World.Player.PistolAmmo;
                    thirdAmmo = World.Player.ShotgunAmmo;
                    playerLevel = World.Player.Level;
                    break;
            }
        }

        // Updates the HUDs components for drawing and loading.
        public void Update(ContentManager content)
        {
            // updates primary to hold the int value of the player's current weapon
            primary = (int) World.Player.currentGun;
            
            // updates the loaded textures in accordance with the player's equipped weapon
            updatePrimary(content, primary);
            
            // Get and update the values for the health, armor, and experience bars.
            healthBar.getValues(healthBarPos, healthBarDimension);
            healthBar.update((float)World.Player.Hitpoints, (float)World.Player.MaxHitpoints);
            expBar.getValues(expBarPos, expBarDim);
            expBar.update((float)World.Player.Experiance, (float)World.Player.NeededExperiance);
            armorBar.getValues(armorBarPos, armorBarDim);
            armorBar.update((float)World.Player.Armor, (float)World.Player.MaxArmor);
        }
        
        // Draws the HUD to the screen
        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            // Draw the health bar
            healthBar.Draw(spriteBatch, graphicsDevice);
            // Draw the armor bar
            armorBar.Draw(spriteBatch, graphicsDevice, Color.Orange);
            // Draw the experience bar
            expBar.Draw(spriteBatch, graphicsDevice, Color.AliceBlue);
            // Draw the upper HUD over the bars
            spriteBatch.Draw(upperHud, new Vector2(0, 0), Color.White);
            // Draw the lower HUD
            spriteBatch.Draw(lowerHud, new Vector2(0, 572), Color.White);
            // Draw the Weapons over the lower HUD
            spriteBatch.Draw(primaryWep, new Vector2(25, 585), Color.White);
            spriteBatch.Draw(secondaryWep, new Vector2(176, 632), Color.White);
            spriteBatch.Draw(thirdWep, new Vector2(285, 635), Color.White);
            // Draw the Ammo Counts by their corresponding weapons on the lower HUD
            spriteBatch.DrawString(hudFont, primaryAmmo.ToString(), new Vector2(90, 650), Color.Black);
            spriteBatch.DrawString(hudFont, secondaryAmmo.ToString(), new Vector2(210, 650), Color.Black);
            spriteBatch.DrawString(hudFont, thirdAmmo.ToString(), new Vector2(315, 650), Color.Black);
            // Draw the player's current level over the upper HUD
            spriteBatch.DrawString(hudFont, (playerLevel + 1).ToString(), new Vector2(165, 88), new Color(255, 229, 135));
        }
    }
}
