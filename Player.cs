using SplashKitSDK;

namespace Volley2DGame
{
    public class Player
    {
        public Circle playerCircle;
        private Bitmap _playerBitmap;
        private Color Color { get; }
        public double XSpeed { get; private set; }
        public double YSpeed { get; private set; }
        private bool IsJumping { get; set; }
        private const double _playerWidth = 60;
        public Player(Color color, double x, double y)
        {
            playerCircle = SplashKit.CircleAt(x, y, _playerWidth / 2);
            Color = color;
            XSpeed = 0;
            YSpeed = 0;
            IsJumping = false;
            _playerBitmap = new Bitmap("Player", "Resources/player1.png");
        }

        public void HandleInput()
        {
            // Handle player input for horizontal movement
            if (SplashKit.KeyDown(KeyCode.AKey) || SplashKit.KeyDown(KeyCode.LeftKey))
            {
                XSpeed = -GameConstants.MoveSpeed;
            }
            else if (SplashKit.KeyDown(KeyCode.DKey) || SplashKit.KeyDown(KeyCode.RightKey))
            {
                XSpeed = GameConstants.MoveSpeed;
            }
            else
            {
                XSpeed = 0;
            }

            // Handle player input for jumping
            if ((SplashKit.KeyTyped(KeyCode.WKey) || SplashKit.KeyDown(KeyCode.UpKey)) && !IsJumping)
            {
                YSpeed = GameConstants.JumpStrength;
                IsJumping = true;
            }
        }

        public void Update()
        {

            // Apply gravity to simulate jumping and falling
            YSpeed += GameConstants.Gravity;

            // Update player position
            playerCircle.Center.X += XSpeed;
            playerCircle.Center.Y += YSpeed;

            // Check for collisions with the ground or other objects
            if (playerCircle.Center.Y > GameConstants.GroundLevel) // Adjust the ground level as needed
            {
                playerCircle.Center.Y = GameConstants.GroundLevel;
                IsJumping = false;
            }

            // Set the horizontal limits
            if (playerCircle.Center.X < _playerWidth / 2) playerCircle.Center.X = _playerWidth / 2;
            else if (playerCircle.Center.X > (GameConstants.WindowWidth / 2) - (_playerWidth / 2)) playerCircle.Center.X = (GameConstants.WindowWidth / 2) - (_playerWidth / 2);
        }

        public void Draw()
        {
            SplashKit.DrawBitmap(_playerBitmap, playerCircle.Center.X - _playerBitmap.Width / 2, playerCircle.Center.Y - _playerBitmap.Height / 2);
        }
    }
}