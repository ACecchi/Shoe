using System;
using Microsoft.Xna.Framework;

namespace Shoe.Lib.Objects
{
    public class Portal
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public string DestinationMap { get; set; }
        public Vector2 DestinationTileLocation { get; set; }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
            }
        }
    }
}
