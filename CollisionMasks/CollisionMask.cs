using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;

namespace CollisionMasks
{
    public class CollisionMask
    {
        private readonly BitArray _bits; // Stores the collision data as a series of bits
        public int Width { get; } // Width of the texture
        public int Height { get; } // Height of the texture

        // Constructor: Creates a collision mask from a texture
        public CollisionMask(Texture2D texture)
        {
            Width = texture.Width;
            Height = texture.Height;

            // Get the color data (pixels) from the texture
            Color[] pixels = new Color[Width * Height];
            texture.GetData(pixels);

            // Initialize the bit array to store collision data
            _bits = new BitArray(Width * Height);

            // Populate the bit array: A pixel is "solid" if its alpha value > 0
            for (int i = 0; i < pixels.Length; i++)
            {
                _bits[i] = pixels[i].A > 0; // Solid if alpha > 0
            }
        }

        // Checks if a specific point (x, y) is solid
        public bool IsSolid(int x, int y)
        {
            // Return false if the point is out of bounds
            if (x < 0 || x >= Width || y < 0 || y >= Height)
            {
                return false;
            }

            // Calculate the index in the bit array for the given (x, y) position
            int index = y * Width + x;

            // Return whether the point is solid
            return _bits[index];
        }
    }
}
