using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shoe.Lib;
using Shoe.Lib.Characters;
using Shoe.Lib.Screens;
using System.Collections.Generic;

namespace Shoe.Lib.Hud
{
    public class GraphicBar
    {

        private Vector2 position;
        private Vector2 dimension;

        private float valueMax;
        private float valueCurrent;

        private bool enabled;

        /// <summary>
        /// Creates a new Bar Component for the HUD.
        /// </summary>
        /// <param name="position">Component position on the screen.</param>
        /// <param name="dimension">Component dimensions.</param>
        /// <param name="valueMax">Maximum value to be displayed.</param>
        /// <param name="spriteBatch">SpriteBatch that is required to draw the sprite.</param>
        /// <param name="graphicsDevice">Graphicsdevice that is required to create the semi transparent background texture.</param>

        public GraphicBar()
        {

        }
        
        public void getValues(Vector2 position, Vector2 dimension)
        {
            this.position = position;
            this.dimension = dimension;
        }
        
        public GraphicBar(Vector2 position, Vector2 dimension)
        {
            this.position = position;
            this.dimension = dimension;
            this.enabled = true;
        }

        /// <summary>
        /// Updates the text that is displayed after ":".
        /// </summary>
        /// <param name="valueCurrent">Text to be displayed.</param>
        public void update(float valueCurrent, float valueMax)
        {
            this.valueCurrent = valueCurrent;
            this.valueMax = valueMax;
        }

        /// <summary>
        /// Draws the BarComponent with the values set before.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            if (enabled)
            {
                float percent = valueCurrent / valueMax;

                Color backgroundColor = new Color(0, 0, 0, 128);
                Color barColor = new Color(0, 255, 0, 200);
                if (percent < 0.50)
                    barColor = new Color(255, 255, 0, 200);
                if (percent < 0.20)
                    barColor = new Color(255, 0, 0, 200);

                Rectangle backgroundRectangle = new Rectangle();
                Texture2D dummyTexture;

                backgroundRectangle.Width = (int)(dimension.X * percent); //0.9 * 
                backgroundRectangle.Height = (int)(dimension.Y); // * 0.5
                backgroundRectangle.X = (int)position.X + (int)(dimension.X * 0.05);
                backgroundRectangle.Y = (int)position.Y + (int)(dimension.Y * 0.25);

                dummyTexture = new Texture2D(graphicsDevice, 1, 1);
                dummyTexture.SetData(new Color[] { barColor });

                spriteBatch.Draw(dummyTexture, backgroundRectangle, barColor);
            }
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Color color)
        {
            if (enabled)
            {
                float percent = valueCurrent / valueMax;

                Rectangle backgroundRectangle = new Rectangle();
                Texture2D dummyTexture;

                backgroundRectangle.Width = (int)(dimension.X * percent); //0.9 * 
                backgroundRectangle.Height = (int)(dimension.Y); // * 0.5
                backgroundRectangle.X = (int)position.X + (int)(dimension.X * 0.05);
                backgroundRectangle.Y = (int)position.Y + (int)(dimension.Y * 0.25);

                dummyTexture = new Texture2D(graphicsDevice, 1, 1);
                dummyTexture.SetData(new Color[] { color });

                spriteBatch.Draw(dummyTexture, backgroundRectangle, color);
            }
        }

    }
}
