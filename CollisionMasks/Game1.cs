using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CollisionMasks
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics; // Manages graphics settings and rendering
        private SpriteBatch _spriteBatch; // Used to draw textures to the screen

        // Level + mask
        private Texture2D _levelTex; // The texture representing the level
        private CollisionMask _mask; // Collision mask for the level texture
        private int _levelXPos = 500;
        private int _levelYPos = 0;
        private Vector2 texturePosition = Vector2.Zero; // The position of the level in the world

        // Simple 1×1 white pixel texture used for drawing (e.g., the bullet)
        private Texture2D _pixel;

        // Bullet state
        private Vector2 _bulletPos; // Current position of the bullet
        private bool _bulletActive; // Whether the bullet is currently active
        private bool _bulletHit; // Whether the bullet has hit a solid object
        private const float BulletSpeed = 300; // Speed of the bullet in pixels per second

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this); // Initialize graphics settings
            Content.RootDirectory = "Content"; // Set the content directory
            IsMouseVisible = true; // Show the mouse cursor
            _graphics.PreferredBackBufferWidth = 1920; // Set the screen width
            _graphics.PreferredBackBufferHeight = 1080; // Set the screen height
        }

        protected override void Initialize()
        {
            // Perform any necessary initialization logic here
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice); // Initialize the sprite batch

            // Load the level texture and create a collision mask for it
            _levelTex = Content.Load<Texture2D>("TestLevel");
            _mask = new CollisionMask(_levelTex);

            // Create a 1×1 white pixel texture for drawing (e.g., the bullet)
            _pixel = new Texture2D(GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White }); // Set the pixel color to white
        }

        protected override void Update(GameTime gameTime)
        {
            // Exit the game if the Escape key is pressed
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Update the level's position in the world
            texturePosition = new Vector2(_levelXPos, _levelYPos);

            // Fire a bullet when the Space key is pressed (if no bullet is active)
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && !_bulletActive)
            {
                _bulletActive = true; // Activate the bullet
                _bulletHit = false; // Reset the hit state
                _bulletPos = new Vector2(0, 100); // Set the initial position of the bullet
            }

            // Move the level up or down when the Up or Down arrow keys are pressed
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                _levelYPos -= 10; // Move the level up
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                _levelYPos += 10; // Move the level down
            }

            // Update the bullet's position and check for collisions
            if (_bulletActive && !_bulletHit)
            {
                // Move the bullet to the right
                _bulletPos.X += BulletSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                // Check if the bullet collides with the level (adjusting for the level's position)
                if (_mask.IsSolid((int)(_bulletPos.X - texturePosition.X), (int)(_bulletPos.Y - texturePosition.Y)))
                    _bulletHit = true; // Mark the bullet as having hit something
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black); // Clear the screen with a black color

            _spriteBatch.Begin();

            // Draw the level texture at its current position
            _spriteBatch.Draw(_levelTex, texturePosition, Color.White);

            // Draw the bullet (as a 4×4 square) if it is active
            if (_bulletActive)
            {
                // Use red if the bullet hit something, otherwise use lime green
                Color col = _bulletHit ? Color.Red : Color.LimeGreen;
                _spriteBatch.Draw(_pixel, new Rectangle((int)_bulletPos.X - 2, (int)_bulletPos.Y - 2, 8, 8), col);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
