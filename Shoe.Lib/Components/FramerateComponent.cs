/*using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Shoe.Lib.Components
{
	public class FramerateComponent : DrawableGameComponent
	{
		//private int drawCount;
		//private float drawTimer;
		private string drawString = "FPS: ";

		private SpriteBatch spriteBatch;
		private SpriteFont font;

		public FramerateComponent(Game game) : base(game)
		{
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			font = Game.Content.Load<SpriteFont>("Fonts\\DefaultFont");
		}

		public override void Draw(GameTime gameTime)
		{
			//drawCount++;
			//drawTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
			//if (drawTimer >= 1f)
			//{
				//drawTimer -= 1f;
				//drawString = "FPS: " + drawCount;
				//drawCount = 0;
			//}

			spriteBatch.Begin();
			//spriteBatch.DrawString(font, drawString, new Vector2(10f, 10f), Color.White);
			spriteBatch.End();
		}
	}
}
*/