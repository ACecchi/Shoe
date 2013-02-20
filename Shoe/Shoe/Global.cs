using System;
using Shoe.Data;
using System.Collections.Generic;
using TiledLib;
using Microsoft.Xna.Framework.Graphics;

namespace Shoe
{
	public static class Global
	{

		public static List<SaveGameDescription> SaveGameDescriptions { get; set; }
        //public static Texture2D WhitePixel { get; set; }
		
		static Global()
		{
			SaveGameDescriptions = new List<SaveGameDescription>();
		}

		#region Extension Methods
        //public static void LoadContent(GraphicsDevice graphicsDevice)
        //{
        //    WhitePixel = new Texture2D(graphicsDevice, 1, 1);
        //    Color[] pixels = new Color[1 * 1];
        //    pixels[0] = Color.White;
        //    WhitePixel.SetData<Color>(pixels);
        //}

		public static int GetTileIndex(this Tile tile)
		{
			int ret = 0;

			int width = tile.Source.Width;
			int height = tile.Source.Height;
			int tilesPerRow = tile.Texture.Width / width;
			int x = tile.Source.X / width;
			int y = tile.Source.Y / height;
			ret = (y * tilesPerRow) + x;
			return ret;
		}

		#endregion
	}
}
