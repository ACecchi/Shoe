using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shoe.Lib.Sprites
{
    public class AnimatedSprite : Sprite
    {
        #region Fields

        protected float timer = 0;
        protected int currentFrame = 0;
        protected int totalFrames = -1;
        private Rectangle? _SourceRectangle = null;

        #endregion

        #region Properties

        public Vector2 FrameSize { get; set; }
        public bool IsAnimating { get; set; }
        public float FrameLength { get; set; }

        protected int SpritesPerRow
        {
            get
            {
                return (int)(Texture.Width / FrameSize.X);
            }
        }

        protected int SpritesPerColumn
        {
            get
            {
                return (int)(Texture.Height / FrameSize.Y);
            }
        }

        public override Rectangle? SourceRectangle
        {
            get
            {
                if (currentFrame >= 0)
                {
                    int x = (int)((currentFrame % SpritesPerRow) * FrameSize.X);
                    int y = (int)((currentFrame / SpritesPerRow) * FrameSize.Y);
                    int w = (int)FrameSize.X;
                    int h = (int)FrameSize.Y;

                    _SourceRectangle = new Rectangle(x, y, w, h);
                }

                return _SourceRectangle;
            }
            set
            {
                _SourceRectangle = value;
            }
        }

        public override Vector2 Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                base.Position = value;
                //Bounds = new Rectangle((int)value.X, (int)value.Y, (int)FrameSize.X, (int)FrameSize.Y);
            }
        }

        #endregion

        #region Constructor

        public AnimatedSprite()
        {
            FrameSize = new Vector2(64f, 64f);
            IsAnimating = true;
            FrameLength = 0.05f;
        }

        #endregion

        #region Methods

        public virtual void Update(GameTime gameTime)
        {
            if (!IsAnimating) return;

            if (totalFrames == -1) totalFrames = SpritesPerRow * SpritesPerColumn;

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= FrameLength)
            {
                timer = 0f;

                currentFrame = (currentFrame + 1) % totalFrames;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible && Texture != null)
            {
                Color tint = new Color(Tint.R, Tint.G, Tint.B, Convert.ToByte(Opacity * Tint.A));
                Rectangle destRect = new Rectangle((int)Position.X, (int)Position.Y, (int)FrameSize.X, (int)FrameSize.Y);
                spriteBatch.Draw(Texture, destRect, SourceRectangle, tint, MathHelper.ToRadians(RotationAngle), Origin, Orientation, 0f);
            }
        }

        #endregion
    }
}
