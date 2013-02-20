using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Shoe.Lib.Screens;

namespace Shoe.Screens
{
	class CreditsScreen: MenuScreen
    {
        #region Fields

		ContentManager content;
		Texture2D background;
				
        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
		public CreditsScreen() : base("Credits")
        {
			MenuEntry backMenuEntry = new MenuEntry("Back");
            backMenuEntry.Selected += OnCancel;
            MenuEntries.Add(backMenuEntry);
        }
		
        #endregion

		#region Load Content

		public override void LoadContent()
		{
			if (content == null) content = new ContentManager(ScreenManager.Game.Services, "Content");
			background = content.Load<Texture2D>("Textures\\Background001");
			CenterTitle();
			if (ScreenManager != null)
			{
				MenuPosition = new Vector2(100f, ScreenManager.GraphicsDevice.Viewport.Height - (ScreenManager.GraphicsDevice.Viewport.Height * 0.1f));
			}
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

			Vector2 position = new Vector2(100, 150);

			float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

			if (ScreenState == ScreenState.TransitionOn)
				position.X -= transitionOffset * 256;
			else
				position.X += transitionOffset * 512;

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend); ///SpriteBlendMode.AlphaBlend);

                spriteBatch.Draw(background, fullscreen, transitionColor);

				spriteBatch.DrawString(ScreenManager.Font, "[ Design | Programming ] - Anthony Checci, Robert Hertenstein, Daniel Schmidt", position, Color.White);
				spriteBatch.DrawString(ScreenManager.Font, "[ Concept | Design | Testing ] - 'Sprite Reapers' ", position + new Vector2(0f, ScreenManager.Font.LineSpacing * 1), Color.White);
				spriteBatch.DrawString(ScreenManager.Font, "[ Music ] - Mary Houser", position + new Vector2(0f, ScreenManager.Font.LineSpacing * 2), Color.White);
                spriteBatch.DrawString(ScreenManager.Font, "[ Sound ] - Anthony Checci, Mary Houser", position + new Vector2(0f, ScreenManager.Font.LineSpacing * 3), Color.White);
                spriteBatch.DrawString(ScreenManager.Font, "[ Concept Art ] - Mary Houser, Daniel Schmidt", position + new Vector2(0f, ScreenManager.Font.LineSpacing * 4), Color.White);
                spriteBatch.DrawString(ScreenManager.Font, "[ Level Art | Design ] - Daniel Schmidt", position + new Vector2(0f, ScreenManager.Font.LineSpacing * 5), Color.White);
                spriteBatch.DrawString(ScreenManager.Font, "[ HUD Art | Design ] - Robert Hertenstein", position + new Vector2(0f, ScreenManager.Font.LineSpacing * 6), Color.White);
                spriteBatch.DrawString(ScreenManager.Font, "[ Menu Art ] - Greg Hedges, Daniel Schmidt", position + new Vector2(0f, ScreenManager.Font.LineSpacing * 7), Color.White);
                spriteBatch.DrawString(ScreenManager.Font, "[ Sprite Art ] - Adam Mortell", position + new Vector2(0f, ScreenManager.Font.LineSpacing * 8), Color.White);
                spriteBatch.DrawString(ScreenManager.Font, "[ Pick-up | Power-Up Art ] - Robert Hertenstein", position + new Vector2(0f, ScreenManager.Font.LineSpacing * 9), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion
    }
}
