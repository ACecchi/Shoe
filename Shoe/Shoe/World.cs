using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shoe.Lib.Characters;
using Microsoft.Xna.Framework;
using Shoe.Lib.Objects;
namespace Shoe
{
    public static class World
    {
        public static Player Player { get; set; }
        public static List<Enemy> Enemies { get; set; }
        public static List<Item> Items { get; set; }
        public static List<Portal> Portals { get; set; }
        public static List<Chest> Chests { get; set; }
        public static List<Spawn> Spawns { get; set; }
	
		public static Dictionary<Vector2, Rectangle> ClipMap { get; set; }
        public static Dictionary<Vector2, Rectangle> AmmoClipMap { get; set; }

		
    }
}
