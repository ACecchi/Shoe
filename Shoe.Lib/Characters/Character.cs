using System;
using Shoe.Lib.Sprites;
using Microsoft.Xna.Framework;
using TiledLib;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Shoe.Lib.Characters
{
    public abstract class Character : AnimatedSprite
    {
        #region Fields

        #endregion

        #region Properties
        public bool Alive { get; set; }//daniel
       
        public int MaxFramesPerDirection { get; set; }
        public Vector2 TilePosition { get; set; }
        public Direction Direction { get; set; }
        public float Speed { get; set; }
        public Vector2 LastMovement { get; set; }

        public int Level { get; set; }

        public SoundEffect HitSound;
        public SoundEffect DeathSound;

        public int Hitpoints { get; set; }  
        public float DamageModifier { get; set; } 
        public Map Map { get; set; }
        #endregion

        #region Constructor

        #endregion

        #region Methods

        public virtual void Update(GameTime gameTime, Camera camera)
        {
            if (!IsAnimating) return;

            if (totalFrames == -1) totalFrames = SpritesPerRow * SpritesPerColumn;

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= FrameLength)
            {
                timer = 0f;

                currentFrame = ((currentFrame + 1) % MaxFramesPerDirection) + ((int)Direction * MaxFramesPerDirection);
            }
        }

        public void Collision()
        {
            Position = LastMovement;

        }

        public void UpdateBounds(int tileWidth, int tileHeight)
        {
            //Bounds = new Rectangle((int)(Position.X - (tileWidth / 2)), (int)(Position.Y), tileWidth - (tileWidth / 16), tileHeight - (tileHeight / 16)); ;
            Bounds = new Rectangle((int)(Position.X - (tileWidth / 2)), (int)(Position.Y- (tileHeight / 2)), tileWidth - (tileWidth / 16), tileHeight - (tileHeight / 16)); ;

            //Bounds = new Rectangle((int)(Position.X - (tileWidth / 2)), (int)(Position.Y), tileWidth , tileHeight); ;
        }

        public void TakesDamage(int damage)
        {
            Hitpoints -= damage;
            HitSound.Play();
        }
        #endregion
    }
}
