using System;
using Microsoft.Xna.Framework;
using Shoe.Lib.Screens;
using Shoe.Screens;
//using Shoe.Lib.Components;
using TiledLib;

namespace Shoe
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        ScreenManager screenManager;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1280,
                PreferredBackBufferHeight = 720
            };
            
            Content.RootDirectory = "Content";

            screenManager = new ScreenManager(this);

            Components.Add(screenManager);

            screenManager.AddScreen(new BackgroundScreen(), null);
            screenManager.AddScreen(new MainMenuScreen(), null);

			//Components.Add(new FramerateComponent(this));
        }
    }
}
