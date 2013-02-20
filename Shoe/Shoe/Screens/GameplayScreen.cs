#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Shoe.Lib.Screens;
using Shoe.Lib.Input;
using TiledLib;
using Shoe.Lib;
using Shoe.Lib.Characters;
using Shoe.Lib.Objects;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

#endregion

namespace Shoe.Screens
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class GameplayScreen : GameScreen
    {
        #region Fields

		Texture2D whitePixel;

        ContentManager content;
        SpriteFont gameFont;
        
		Map map;
		Camera camera;
        HUD hud = new HUD();
        private Random random = new Random(354668); // Arbitrary, but constant seed
        private int randomCheck;
        bool ShowCollisions = false;
        string currentMap;
        public bool complete = false;
		
               #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null) content = new ContentManager(ScreenManager.Game.Services, "Content");

            gameFont = content.Load<SpriteFont>("Fonts\\gamefont");

            hud.LoadContent(content);
            //Global.LoadContent(ScreenManager.Game.GraphicsDevice);
            currentMap = "Maps\\FirstArea";
            LoadMap(currentMap);
             SoundEffect.MasterVolume = ((float)(Settings .SFXVolume)/100);
             MediaPlayer.Volume = ((float)(Settings.MainVolume ) / 100);
            
            
             whitePixel = new Texture2D(ScreenManager.Game.GraphicsDevice, 1, 1);
			Color[] pixels = new Color[1 * 1];
			pixels[0] = Color.White;
			whitePixel.SetData<Color>(pixels);
            
            ScreenManager.Game.ResetElapsedTime();
            
        }

        private void LoadMap(string assetName)
        {
            World.Portals = new List<Portal>();
            World.ClipMap = new Dictionary<Vector2, Rectangle>();
            World.Enemies = new List<Enemy>();
            World.Items = new List<Item>();
            World.Chests = new List<Chest>();
            World.Spawns  = new List<Spawn>();
      
            map = content.Load<Map>(assetName);

            if (map.Properties["Music"].RawValue != "")
            {
                MediaPlayer.Play(content.Load<Song>("Sounds\\" + map.Properties["Music"].RawValue));
                MediaPlayer.IsRepeating = true;
            }
             
            
            camera = new Camera(new Vector2(map.Width, map.Height), new Vector2(map.TileWidth, map.TileHeight), new Vector2(ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height));
           
            MapObjectLayer objects = map.GetLayer("Objects") as MapObjectLayer;

            foreach (MapObject obj in objects.Objects)
            {
                switch (obj.Type)
                {
                    case "PlayerStart":
                        if (World.Player == null)
                        {
                            World.Player = new Player(new Vector2(obj.Bounds.X + (obj.Bounds.Width / 2), obj.Bounds.Y), new Vector2(64, 64), 6);
                            World.Player.LoadContent(ScreenManager.Game.Content, "Textures\\Characters\\Shoe1Walk");
                            World.Player.Map = map;
                        }
                        else
                        {
                            World.Player.Position = new Vector2(obj.Bounds.X + (obj.Bounds.Width / 2), obj.Bounds.Y);
                        }
                        break;
                    case "Portal":
                        Portal portal = new Portal();
                        portal.Position = new Vector2(obj.Bounds.X, obj.Bounds.Y);
                        portal.Size = new Vector2(obj.Bounds.Width, obj.Bounds.Height);
                        portal.DestinationMap = obj.Properties["DestinationMap"].RawValue;
                        string[] tileLoc = obj.Properties["DestinationTileLocation"].RawValue.Split(',');
                        portal.DestinationTileLocation = new Vector2(Convert.ToInt32(tileLoc[0]), Convert.ToInt32(tileLoc[1]));
                        World.Portals.Add(portal);
                        break;

                    case "Chest":
                        Chest chest = new Chest();
                        chest.Position = new Vector2(obj.Bounds.X, obj.Bounds.Y);
                        chest.Size = new Vector2(obj.Bounds.Width, obj.Bounds.Height);
                        chest.numberOfItems = Convert .ToInt32 (obj.Properties["NumberOfItems"].RawValue.ToString() );
                        chest.Alive = true;
                        chest .AssetName = "Textures\\Characters\\Chest";
                        chest.LoadContent(ScreenManager.Game.Content, chest.AssetName);
                        World.Chests.Add(chest);
                        break;
                    case "Bandit":
                       Enemy enemy = new Enemy(new Vector2(obj.Bounds.X + (obj.Bounds.Width / 2), obj.Bounds.Y + (obj.Bounds.Width / 2)), new Vector2(64f, 64f), 6, 15, map,4,10,0,1,10,60,500,5.0f);
                        enemy.LoadContent(ScreenManager.Game.Content, "Textures\\Characters\\Bandit2Walk");
                        enemy.UpdateBounds(map.TileWidth, map.TileHeight);
                        World.Enemies .Add(enemy);
                        // chest.numberOfItems = (int)(obj.Properties["NumberOfItems"].RawValue);
                      //  string[] tileLoc = obj.Properties["DestinationTileLocation"].RawValue.Split(',');
                       // chest.DestinationTileLocation = new Vector2(Convert.ToInt32(tileLoc[0]), Convert.ToInt32(tileLoc[1]));
                        
                        break;
                    case "Boss":
                        Enemy boss = new Enemy(new Vector2(obj.Bounds.X + (obj.Bounds.Width / 2), obj.Bounds.Y + (obj.Bounds.Width / 2)), new Vector2(64f, 64f), 6, 1500, map, 5, 150, 10,20,120,40,10000,7.0f);
                        boss.LoadContent(ScreenManager.Game.Content, "Textures\\Characters\\Villian1Walk");
                        boss.UpdateBounds(map.TileWidth, map.TileHeight);
                        World.Enemies.Add(boss);
                     
                        break;
                   
                    case "Spawn":


                        Spawn spawn = new Spawn(new Point(obj.Bounds.X + (obj.Bounds.Width / 2), obj.Bounds.Y), obj.Properties["Type"].RawValue, 1800  );
                     
                       
                         World.Spawns.Add(spawn);
                     
                        break;
                   
                }
            }

            World.Player.UpdateBounds(map.TileWidth, map.TileHeight);
            camera.FocusOn(World.Player);
           
            LoadClipMap();
            LoadAmmoClipMap();
          
            LoadItems();
            

        }
                  
        private void LoadItems()
        {
            World.Items   = new List<Item>();
            TileLayer itemLayer = map.GetLayer("Items") as TileLayer;
            if (itemLayer.Tiles.Width > 0)
            {
                for (int y = 0; y < itemLayer.Height ; y++)
                {
                    for (int x = 0; x < itemLayer.Width ; x++)
                    {
                        Tile tile = itemLayer.Tiles[x, y];
                        if (tile != null)
                        {
                            Item item = new Item(new Vector2((x * map.TileWidth) + (map.TileWidth / 2), y * map.TileHeight));
                            item.AssetName = GetItemAssetName(tile.GetTileIndex());
                            item.Type = GetItemType(tile.GetTileIndex());
                            item.LoadContent(ScreenManager.Game.Content, item.AssetName);
                            item.UpdateBounds(map.TileWidth, map.TileHeight);
                            World.Items.Add(item);
                        }
                    }
                }
            }
        }

        private string GetItemType(int tokenIndex)
        {
            switch (tokenIndex)
            {
                case 0: return "HealthPickup";
                case 1: return "ShotgunAmmo";
                case 2: return "PistolAmmo";
                case 3: return "Invincibility";
                case 4: return "Armor";
                case 5: return "Dynamite";

                case 6: return "Experience";
                case 7: return "UnlimitedAmmo";


            }
            return "";
        }
        private string GetItemAssetName(int tokenIndex)
        {
            switch (tokenIndex)
            {
                case 0: return "Textures\\Items\\HealthPickup";
                case 1: return "Textures\\Items\\ShotgunAmmo";

                case 2: return "Textures\\Items\\PistolAmmo";

                case 3: return "Textures\\Items\\Invincibility";
                case 4: return "Textures\\Items\\Armor";
                case 5: return "Textures\\Items\\Dynamite";
                case 6: return "Textures\\Items\\Experience";
                case 7: return "Textures\\Items\\UnlimitedAmmo";
          
            }
            return "";
        }

        private void LoadClipMap()
		{
			World.ClipMap = new Dictionary<Vector2, Rectangle>();
			TileLayer clipLayer = map.GetLayer("Clip") as TileLayer;
			if (clipLayer.Tiles.Width > 0)
			{
				for (int y = 0; y < clipLayer.Height ; y++)
				{
					for (int x = 0; x < clipLayer.Width ; x++)
					{
						Tile tile = clipLayer.Tiles[x, y];
						if (tile != null)
						{
							World.ClipMap.Add(new Vector2(x, y), new Rectangle(x * tile.Source.Width, y * tile.Source.Height, tile.Source.Width, tile.Source.Height));
                          
                        }
					}
				}
			}
		}

        private void LoadAmmoClipMap()
        {
            World.AmmoClipMap  = new Dictionary<Vector2, Rectangle>();
            TileLayer clipLayer = map.GetLayer("AmmoClip") as TileLayer;
            if (clipLayer.Tiles.Width > 0)
            {
                for (int y = 0; y < clipLayer.Height ; y++)
                {
                    for (int x = 0; x < clipLayer.Width ; x++)
                    {
                        Tile tile = clipLayer.Tiles[x, y];
                        if (tile != null)
                        {
                            World.AmmoClipMap.Add(new Vector2(x, y), new Rectangle(x * tile.Source.Width, y * tile.Source.Height, tile.Source.Width, tile.Source.Height));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (IsActive)
            {
                if (World.Player.Alive != false)
                {
                    World.Player.Update(map, gameTime, camera, World.Enemies, World.AmmoClipMap);
                    hud.Update(content);
                   
                }
                else
                {

                    World.Player.DeathSound.Play();
                    ScreenManager.AddScreen(new DeathMenuScreen(), ControllingPlayer);
                    World.Player.Hitpoints = World.Player.MaxHitpoints;
                    World.Player.Alive = true;
                    LoadMap(currentMap);

                }
                if (complete == true)
                {
                    ScreenManager.AddScreen(new ReplayMenuScreen(), ControllingPlayer);
                    Enemy boss = new Enemy(new Vector2(World.Player.Position.X, World.Player.Position.Y), Vector2.Zero, 6, 6, map, 5f, 5, 5f, 5, 5, 5, 5, 5f);
                    World.Enemies.Add(boss);
                    currentMap = "Maps\\FirstArea";
                    LoadMap(currentMap);
                    complete = false;

                }
                if (World.Enemies.Count == 0 & currentMap == "Maps\\BossRoom")
                {
                    complete = true;
                    //while (complete == true)
                    //{
                    //    ScreenManager.AddScreen(new ReplayMenuScreen(), ControllingPlayer);
                    //    Enemy boss = new Enemy(new Vector2 (World.Player.Position.X , World.Player .Position .Y ) ,Vector2 .Zero , 6, 6, map, 5f, 5, 5f, 5, 5, 5, 5, 5f);
                    //    World.Enemies.Add(boss);
                    //    LoadMap("Maps\\FirstArea");
                    //    complete = false;

                    //}
                }

                
                if (World.Enemies.Count > 0)
                {
                    foreach (Enemy enemy in World.Enemies)
                    {
                        if (enemy.Alive == false)
                        {
                            enemy.DeathSound.Play();
                            randomCheck = random.Next(100);
                            if (randomCheck < enemy.DropRate)
                            {
                                randomCheck = random.Next(10000);
                                Item NewItem = new Item (enemy .Position );
                                NewItem.UpdateBounds(map.TileWidth , map.TileHeight );
                                NewItem.ItemDrop(World.Player,enemy .Position ,content,  randomCheck);
                                NewItem.Alive = true;
                                World.Items.Add(NewItem);
                             }
                             World.Enemies.Remove(enemy);
                          
                           
                           
                           
                            break;
                        }       
                        enemy.UpdateBounds(map.Width ,map.TileHeight );
                
                        enemy.Update(gameTime, World.Player,World.AmmoClipMap ,map);
                     }
                }
                foreach (Spawn  spawn in World.Spawns )
                {

                    if (camera.VisibleArea.Contains((spawn.Position)))
                    {

                    }
                    else
                    {
                        randomCheck = random.Next( spawn.Rate);
                        if (randomCheck == 0)
                        {
                            Enemy enemy = new Enemy(new Vector2(spawn.Position.X, spawn.Position.Y), new Vector2(64f, 64f), 6, 15, map, 4, 10, 0, 1, 10, 60,500,5.0f);
                            enemy.LoadContent(ScreenManager.Game.Content, "Textures\\Characters\\Bandit2Walk");
                            enemy.UpdateBounds(map.TileWidth, map.TileHeight);
                            World.Enemies.Add(enemy);
                        }
                    }
                    
                }
                foreach (Item item in World.Items)
                {
                    if (item.Alive == true)
                        item.Update(gameTime);
                }
                foreach (Chest  chest in World.Chests )
                {
                    if (chest.Alive == true)
                        chest.Update(gameTime);
                }

            }
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null) throw new ArgumentNullException("input");

            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];


            bool gamePadDisconnected = !gamePadState.IsConnected && input.GamePadWasConnected[playerIndex];

            if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                PlayerIndex index = PlayerIndex.One;
                if (input.IsNewButtonPress(Buttons.LeftShoulder, ControllingPlayer.Value, out index) ||
                    input.IsNewKeyPress(Keys.OemTilde, ControllingPlayer.Value, out index))
                {
                    ShowCollisions = !ShowCollisions;
                }
                World.Player.HandleInput(input, ControllingPlayer);
                CheckCollisions( );
                camera.Update(input, ControllingPlayer);
            }
        }

        private void CheckCollisions()
        {
            World.Player.UpdateBounds(map.TileWidth, map.TileHeight);

            foreach (Rectangle clip in World.ClipMap.Values)
            {
                if (World.Player.Bounds.Intersects(clip))
                {
                    World.Player.Collision();
                    break;
                }
            }
            foreach (Enemy enemy in World.Enemies)
            {

                foreach (Rectangle clip in World.ClipMap.Values)
                {
                    if (enemy.Bounds.Intersects(clip))
                    {
                        enemy.Collision();
                        break;
                    }
                }
               
            }
            foreach (Item item in World.Items)
            {
                if (World.Player.Bounds.Intersects(item.Bounds) & item.Alive)
                {
                    item.Collision(World.Player);
                    
                }
            }
            foreach (Portal portal in World.Portals)
            {
                if (World.Player.Bounds.Intersects(portal.Bounds))
                {
                    if (portal.DestinationMap != "Same")
                    {
                        currentMap = portal.DestinationMap;
                        LoadMap(currentMap);
                    }
                    World.Player.Position = new Vector2((portal.DestinationTileLocation.X * map.TileWidth) + (map.TileWidth / 2), (portal.DestinationTileLocation.Y * map.TileHeight));
                }
            }
            foreach (Chest chest in World.Chests)
            {
                if (World.Player.Bounds.Intersects(chest.Bounds) & chest .Alive ==true)
                {
                    
                        for (int i = 0; i < chest.numberOfItems; i++ )
                        {
                            randomCheck = random.Next(10000);
                                
                           Item NewItem = new Item(new Vector2(chest.Position.X + ((i + 1) * 15), chest.Position.Y));
                           NewItem.ItemDrop(World.Player, chest.Position, content, randomCheck);
                            NewItem.UpdateBounds(map.TileWidth, map.TileHeight);
                           // NewItem.Alive = true;
                            World.Items.Add(NewItem);

                        }
                        chest.AssetName  = "Textures\\Characters\\ChestOpen";
                        chest.LoadContent(content, chest.AssetName);
                        chest.Alive = false;
                   }
            }
            foreach (Dynamite   dynamite in World.Player .dynamites  )
            {
                if (World.Player.Bounds.Intersects(dynamite.Explosion .Bounds ) & dynamite .Explosion .Alive )
                {
                    World.Player.TakeDamage (dynamite.Explosion.Damage);
                    break;
                }
            }
        }
     



        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0, 0);

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            GraphicsDevice graphicsDevice = ScreenManager.GraphicsDevice;

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, camera.Matrix); ///BlendState.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.None, camera.Matrix);

				SetVisible(true, true, false);
				map.Draw(spriteBatch, camera.VisibleArea);

				//spriteBatch.Draw(whitePixel, World.Player.Bounds, new Color(255, 0, 0, 128));

                if (World.Enemies.Count > 0)
                {
                    foreach (Enemy enemy in World.Enemies)
                    {
                        enemy.Draw(spriteBatch, ScreenManager.Font);
                    }
                }
                foreach (Chest  chest in World.Chests )
                {
                    chest.Draw(spriteBatch);
                }
                foreach (Item item in World.Items)
                {
                    item.Draw(spriteBatch);
                }
                
            

                //World.Player.Draw(spriteBatch);
                // Implement statement to draw Shoe and enemies on proper layers so neither improperly
                //      overlap one another -- Robert
                World.Player.Draw(spriteBatch);

				SetVisible(false, false, true);
				map.Draw(spriteBatch, camera.VisibleArea);
					
            spriteBatch.End();

			spriteBatch.Begin();
            //if (ShowCollisions)
            //{
            //    //DrawClipBounds(spriteBatch);
            //    //DrawPortals(spriteBatch);
            //    spriteBatch.Draw(whitePixel, World.Player.Bounds, new Color(255, 0, 0, 128));
            //    foreach (Enemy enemy in World.Enemies)
            //    {
            //        spriteBatch.Draw(whitePixel, enemy.Bounds, new Color(255, 0, 0, 128));
            //    }
            //}

          

            spriteBatch.End();

            // Draw Overlay Tile Layers
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, camera.Matrix); ///BlendState.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.None, camera.Matrix);

            SetVisible(false, false, true);
            map.Draw(spriteBatch, camera.VisibleArea);

            spriteBatch.End();

            // Draw UI
            spriteBatch.Begin();

            hud.Draw(spriteBatch, graphicsDevice);

            //spriteBatch.End();

            //spriteBatch.DrawString(ScreenManager.Font, "Location: " + World.Player.Position.ToString(), new Vector2(0f, 60f), Color.Yellow);

            //  spriteBatch.DrawString(ScreenManager.Font, "Experiance: " + World.Player.Experiance.ToString(), new Vector2(0f, 20f), Color.Yellow);
            //  spriteBatch.DrawString(ScreenManager.Font, "Needed experiance: " + World.Player.NeededExperiance.ToString(), new Vector2(0f, 50f), Color.Yellow);
            // spriteBatch.DrawString(ScreenManager.Font, "Level: " + (World.Player.Level+1) .ToString(), new Vector2(0f, 80f), Color.Yellow);
                     
            
            //spriteBatch.DrawString(ScreenManager.Font, "Current Weapon: " + World.Player.currentGun.ToString(), new Vector2(0f, 120f), Color.Yellow);
            //spriteBatch.DrawString(ScreenManager.Font, "Pistol ammo: " + World.Player.PistolAmmo .ToString(), new Vector2(0f, 150), Color.Yellow);
            //spriteBatch.DrawString(ScreenManager.Font, "Shotgun ammo: " + World.Player.ShotgunAmmo .ToString(), new Vector2(0f, 180), Color.Yellow);
            //spriteBatch.DrawString(ScreenManager.Font, "Dynamite ammo: " + World.Player.DynamiteAmmo .ToString(), new Vector2(0f, 210), Color.Yellow);
            //spriteBatch.DrawString(ScreenManager.Font, "Hitpoints: " + InputState., new Vector2(0f, 240), Color.Yellow);
            //spriteBatch.DrawString(ScreenManager.Font, "Armor: " + World.Player.Armor .ToString(), new Vector2(0f, 270), Color.Yellow);
           
            // spriteBatch.DrawString(ScreenManager.Font, "Speed: " + World.Player.Speed .ToString(), new Vector2(0f,300), Color.Yellow);

            //  spriteBatch.DrawString(ScreenManager.Font, "Gametime: " + gameTime.TotalGameTime.ToString (), new Vector2(0f, 340), Color.Yellow);

			spriteBatch.End();


            if (TransitionPosition > 0)
			{
				ScreenManager.FadeBackBufferToBlack(255 - TransitionAlpha);
			}
        }

		public void SetVisible(bool ground, bool details, bool overlay)
		{
			map.GetLayer("Ground").Visible = ground;
			map.GetLayer("Detail").Visible = details;
			map.GetLayer("Overlay").Visible = overlay;
		}

        #endregion
    }
}
