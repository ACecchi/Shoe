using System;
using Microsoft.Xna.Framework;

namespace Shoe.Lib.Objects
{
    public class Spawn
    {
        public Point Position { get; set; }
        public Vector2 Size { get; set; }
        public string Type { get; set; }
        public Vector2 DestinationTileLocation { get; set; }
        public int Rate { get; set; }

        public Spawn(Point  position, string type, int rate)
        {
            Position = position;
            Type = type;
            Rate = rate;
        }
        public Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
            }
        }
    }
}
