using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CollisionMasks
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics; // Manages graphics settings and rendering
        private SpriteBatch _spriteBatch; // Used to draw textures to the screen

        // Level sprites
        private Sprite[] _levelSprites;

        // Bullet state
        private Vector2 _bulletPos; // Current position of the bullet
        private bool _bulletActive; // Whether the bullet is currently active
        private bool _bulletHit; // Whether the bullet has hit a solid object
        private const float BulletSpeed = 100; // Speed of the bullet in pixels per second
        private Vector2 _bulletDirection = Vector2.UnitX; // Default bullet direction (right)
        private Texture2D _bulletTexture; // Texture for the bullet

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load level textures and initialize sprites
            Texture2D levelTexture1 = Content.Load<Texture2D>("TestLevel");
            Texture2D levelTexture2 = Content.Load<Texture2D>("TestLevel");

            _levelSprites = new Sprite[]
            {
                new Sprite(levelTexture1, new Vector2(200, 0)),
                new Sprite(levelTexture2, new Vector2(1000, 0))
            };

            // Load bullet texture
            _bulletTexture = new Texture2D(GraphicsDevice, 1, 1);
            _bulletTexture.SetData(new[] { Color.White });
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Handle bullet firing (Space key)
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && !_bulletActive)
            {
                _bulletActive = true;
                _bulletHit = false;
                _bulletPos = new Vector2(0, 100); // Set the initial position of the bullet
                _bulletDirection = Vector2.UnitX; // Default direction: right
            }

            // Handle bullet direction changes (WASD keys)
            if (_bulletActive)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.W))
                    _bulletDirection = Vector2.UnitY * -1; // Up
                if (Keyboard.GetState().IsKeyDown(Keys.S))
                    _bulletDirection = Vector2.UnitY; // Down
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                    _bulletDirection = Vector2.UnitX * -1; // Left
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                    _bulletDirection = Vector2.UnitX; // Right
            }

            // Update the bullet's position and check for collisions
            if (_bulletActive && !_bulletHit)
            {
                MoveBullet(gameTime);
            }

            base.Update(gameTime);
        }

        private void MoveBullet(GameTime gameTime)
        {
            float moveDistance = BulletSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _bulletDirection.Normalize();

            // Stepwise movement
            float stepSize = 1.0f;
            int steps = (int)(moveDistance / stepSize);
            for (int i = 0; i < steps; i++)
            {
                Vector2 previousPosition = _bulletPos;
                _bulletPos += _bulletDirection * stepSize;

                // Check for collision with all level sprites
                foreach (Sprite sprite in _levelSprites)
                {
                    if (sprite.IsColliding(_bulletPos))
                    {
                        _bulletHit = true;
                        _bulletPos = previousPosition;
                        return;
                    }
                }
            }

            // Handle remaining distance
            float remainingDistance = moveDistance % stepSize;
            if (!_bulletHit)
            {
                Vector2 previousPosition = _bulletPos;
                _bulletPos += _bulletDirection * remainingDistance;

                foreach (Sprite sprite in _levelSprites)
                {
                    if (sprite.IsColliding(_bulletPos))
                    {
                        _bulletHit = true;
                        _bulletPos = previousPosition;
                        return;
                    }
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            // Draw all level sprites
            foreach (var sprite in _levelSprites)
            {
                sprite.Draw(_spriteBatch);
            }

            // Draw the bullet
            if (_bulletActive)
            {
                Color col = _bulletHit ? Color.Red : Color.LimeGreen;
                _spriteBatch.Draw(_bulletTexture, new Rectangle((int)_bulletPos.X - 2, (int)_bulletPos.Y - 2, 8, 8), col);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
