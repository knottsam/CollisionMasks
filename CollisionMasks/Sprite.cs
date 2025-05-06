using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CollisionMasks
{
    public class Sprite
    {
        public Vector2 Position { get; set; } // The position of the sprite
        public Texture2D Texture { get; set; } // The texture of the sprite
        public CollisionMask Mask { get; private set; } // The collision mask for pixel-perfect collision detection

        // Constructor
        public Sprite(Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
            Mask = new CollisionMask(texture); // Create a collision mask from the texture
        }

        // Draw the sprite using the sprite batch
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }

        // Check for pixel-perfect collision with a point
        public bool IsColliding(Vector2 point)
        {
            // Calculate the relative position of the point within the sprite
            int relativeX = (int)(point.X - Position.X);
            int relativeY = (int)(point.Y - Position.Y);

            // Check if the point collides with the sprite's mask
            return Mask.IsSolid(relativeX, relativeY);
        }
    }
}
