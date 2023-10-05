using SplashKitSDK;

namespace Volley2DGame
{
    public class AIOpponent
    {
        public Circle OpponentCircle;
        private Bitmap _aiOpponentBitmap;
        private Color Color { get; }
        public double XSpeed { get; private set; }
        public double YSpeed { get; private set; }
        private bool IsJumping { get; set; }
        private Ball Ball { get; }
        private const double _aiOpponentWidth = 60;



        public AIOpponent(Color color, double x, double y, Ball ball)
        {
            OpponentCircle = SplashKit.CircleAt(x, y, _aiOpponentWidth / 2);
            Color = color;
            XSpeed = 0;
            YSpeed = 0;
            IsJumping = false;
            Ball = ball;
            _aiOpponentBitmap = new Bitmap("AI", "Resources/player2.png");
        }

        public void Update()
        {
            if (Ball.BallCircle.Center.X > GameConstants.WindowWidth / 2)
            {
                // Calculate the direction to the expected ball landing point
                double ballX = Ball.BallCircle.Center.X;
                double ballY = Ball.BallCircle.Center.Y;
                double ballSpeedX = Ball.XSpeed;
                double ballSpeedY = Ball.YSpeed;
                double expectedLandingX;
                double expectedLandingY;

                // Predict the ball's landing point based on its trajectory
                if (ballSpeedY != 0)
                {
                    int timeToHitGround = 0;
                    double y = ballY;
                    while (y < GameConstants.GroundLevel)
                    {
                        ballSpeedY += GameConstants.Gravity;
                        y += ballSpeedY;
                        timeToHitGround++;
                    }

                    expectedLandingX = ballX + (ballSpeedX * timeToHitGround) + 10;
                    expectedLandingY = GameConstants.GroundLevel;
                }
                else
                {
                    expectedLandingX = ballX + 25;
                    expectedLandingY = GameConstants.GroundLevel;
                }


                // Move towards the expected landing point
                double deltaX = expectedLandingX - OpponentCircle.Center.X;
                double deltaY = expectedLandingY - OpponentCircle.Center.Y;
                double length = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

                if (ballSpeedY == 0 && deltaX < 5) Jump();

                if (length > 0)
                {
                    double moveSpeed = GameConstants.AIOpponentMoveSpeed; // Use the AI opponent's move speed constant
                    deltaX /= length;
                    // deltaY /= length;
                    XSpeed = deltaX * moveSpeed;
                    // YSpeed = deltaY * moveSpeed;
                }
                else
                {
                    XSpeed = 0;
                    YSpeed = 0;
                }

                // Apply gravity to simulate jumping and falling
                YSpeed += GameConstants.Gravity;

                // Update opponent position
                if (expectedLandingX > GameConstants.WindowWidth / 2) OpponentCircle.Center.X += XSpeed;
                OpponentCircle.Center.Y += YSpeed;

                // Set the horizontal limits
                if (OpponentCircle.Center.X < (GameConstants.WindowWidth / 2) + (_aiOpponentWidth / 2)) OpponentCircle.Center.X = (GameConstants.WindowWidth / 2) + (_aiOpponentWidth / 2);
                else if (OpponentCircle.Center.X > GameConstants.WindowWidth - (_aiOpponentWidth / 2)) OpponentCircle.Center.X = GameConstants.WindowWidth - (_aiOpponentWidth / 2);

                // Check for collisions with the ground
                if (OpponentCircle.Center.Y > GameConstants.GroundLevel) // Ball hits the ground
                {
                    OpponentCircle.Center.Y = GameConstants.GroundLevel;
                    IsJumping = false;
                }
            }
        }

        public void Draw()
        {
            SplashKit.DrawBitmap(_aiOpponentBitmap, OpponentCircle.Center.X - _aiOpponentBitmap.Width / 2, OpponentCircle.Center.Y - _aiOpponentBitmap.Height / 2);
        }

        public void Jump()
        {
            if (!IsJumping)
            {
                YSpeed = GameConstants.JumpStrength;
                IsJumping = true;
            }
        }
    }
}