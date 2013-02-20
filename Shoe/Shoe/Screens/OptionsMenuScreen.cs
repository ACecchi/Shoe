#region File Description
//-----------------------------------------------------------------------------
// OptionsMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Shoe.Lib;
using System;
#endregion

namespace Shoe.Screens
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class OptionsMenuScreen : MenuScreen
    {
        #region Fields

		ContentManager content;
		Texture2D background;

		MenuEntry mainVolume;
		MenuEntry sfxVolume;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen() : base("Options")
        {
			// Create our menu entries.
			mainVolume = new MenuEntry(string.Empty);
			sfxVolume = new MenuEntry(string.Empty);
           
            SetMenuEntryText();

            MenuEntry backMenuEntry = new MenuEntry("Back");

            // Hook up menu event handlers.
			mainVolume.Selected += MainVolumeEntrySelected;
			sfxVolume.Selected += SFXVolumeEntrySelected;
            backMenuEntry.Selected += OnCancel;
            
            // Add entries to the menu.
			MenuEntries.Add(mainVolume);
			MenuEntries.Add(sfxVolume);
            MenuEntries.Add(backMenuEntry);
        }


        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        void SetMenuEntryText()
        {
			mainVolume.Text = String.Format("Main Volume: {0} %", Settings.MainVolume);
			sfxVolume.Text = String.Format("SFX Volume: {0} %", Settings.SFXVolume);
        }


        #endregion

		#region Load Content

		public override void LoadContent()
		{
			if (content == null) content = new ContentManager(ScreenManager.Game.Services, "Content");
			background = content.Load<Texture2D>("Textures\\background");
			CenterTitle();
		}

		#endregion

        #region Handle Input

		/// <summary>
		/// Event handler for when the Main Volume menu entry is selected.
		/// </summary>
		void MainVolumeEntrySelected(object sender, PlayerIndexEventArgs e)
		{
			Settings.MainVolume += 5;
			if (Settings.MainVolume > 100) Settings.MainVolume = 0;

			SetMenuEntryText();
		}

		/// <summary>
		/// Event handler for when the SFX Volume menu entry is selected.
		/// </summary>
		void SFXVolumeEntrySelected(object sender, PlayerIndexEventArgs e)
		{
			Settings.SFXVolume += 5;
			if (Settings.SFXVolume > 100) Settings.SFXVolume = 0;

			SetMenuEntryText();
		}


        #endregion

		#region Drawing

		public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            byte fade = TransitionAlpha;

            Color transitionColor = new Color(fade, fade, fade);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend); /// SpriteBlendMode.AlphaBlend);

                spriteBatch.Draw(background, fullscreen, transitionColor);                

            spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion
    }
}
