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
namespace Shoe.Lib.Objects
{
    public class Chest : Character 
    {
       public Vector2 Size { get; set; }
        public int numberOfItems { get; set; }
    
        public override void LoadContent(ContentManager content, string assetName)
        {
            base.LoadContent(content, assetName);
            //LayerDepth = 1.0f;
        }
       // public bool Alive { get; set; }
        public override Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
            }
        }
        

       


    }
}
