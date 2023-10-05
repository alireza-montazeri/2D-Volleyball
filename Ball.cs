using System.Runtime.InteropServices;
using SplashKitSDK;

namespace Volley2DGame
{
    public class Ball
    {
        public Circle BallCircle;
        private Bitmap _ballBitmap;
        public double XSpeed { get; private set; }
        public double YSpeed { get; private set; }
        private bool isInPlay = false;

        public int PlayerScore { get; private set; } = 0;
        public int AIScore { get; private set; } = 0;
        private Rectangle BallRectangle { get; set; }
        private Rectangle NetRectangle { get; }
        private double _ballWith = 40;

        public Ball(double x, double y)
        {
            BallCircle = SplashKit.CircleAt(x, y, _ballWith / 2);
            BallRectangle = SplashKit.RectangleAround(BallCircle);
            NetRectangle = SplashKit.RectangleFrom(GameConstants.WindowWidth / 2 - 1, GameConstants.WindowHeight - 380, 1, 380);
            XSpeed = 0;
            YSpeed = 0;
            isInPlay = false;
            _ballBitmap = new Bitmap("Ball", "Resources/Ball.png");
        }

        public void Update(Player player, AIOpponent aiOpponent)
        {
            if (IsCollisionWith(player.playerCircle)) HitByPlayer(player.playerCircle, player.XSpeed, 0);
            if (IsCollisionWith(aiOpponent.OpponentCircle)) HitByPlayer(aiOpponent.OpponentCircle, aiOpponent.XSpeed, 0);
            if (IsBallHitsNet()) XSpeed = -XSpeed;
            if (IsOutOfScreen()) XSpeed = -XSpeed;
            if (HitTheGround())
            {
                isInPlay = false;
                if (BallCircle.Center.X > GameConstants.WindowWidth / 2)
                {
                    PlayerScore++;
                    BallInServePosition(true);
                }
                else
                {
                    AIScore++;
                    BallInServePosition(false);
                }
            }
            if (isInPlay)
            {
                // Apply gravity to simulate falling
                YSpeed += GameConstants.Gravity;
                // Update the ball's position based on its speed
                BallCircle.Center.X += XSpeed;
                BallCircle.Center.Y += YSpeed;

                BallRectangle = SplashKit.RectangleAround(BallCircle);
            }
        }

        public void Draw()
        {
            SplashKit.DrawBitmap(_ballBitmap, BallCircle.Center.X - _ballBitmap.Width / 2, BallCircle.Center.Y - _ballBitmap.Height / 2);
            SplashKit.FillRectangle(Color.Brown, GameConstants.WindowWidth / 2 - 10, GameConstants.WindowHeight - 380, 20, 380);
        }

        private bool IsCollisionWith(Circle collisionObject)
        {
            // Check for collision with a player's circle
            if (SplashKit.CirclesIntersect(BallCircle, collisionObject))
            {
                isInPlay = true;
                return true;
            }
            return false;
        }

        public void HitByPlayer(Circle circle, double playerSpeedX, double playerSpeedY)
        {
            // Calculate the angle between the ball and the player
            double deltaX = BallCircle.Center.X - circle.Center.X;
            double deltaY = BallCircle.Center.Y - circle.Center.Y;
            double angle = Math.Atan2(deltaY, deltaX);

            // Calculate the new X and Y speeds based on the angle and player's power
            double hitPower = GameConstants.HitPower; // You can define the player's power attribute
            XSpeed = Math.Cos(angle) * hitPower * 0.6 + playerSpeedX * 0.2;
            YSpeed = Math.Sin(angle) * hitPower + playerSpeedY * 0.2;
        }

        private bool IsOutOfScreen()
        {
            // Check for collisions with the game boundaries
            return BallCircle.Center.X < 0 || BallCircle.Center.X > SplashKit.ScreenWidth();
        }

        private bool HitTheGround()
        {
            // Check if the ball has hit the ground
            return BallCircle.Center.Y > GameConstants.GroundLevel;
        }

        private void BallInServePosition(bool isPlayer)
        {
            if (isPlayer) BallCircle.Center.X = GameConstants.WindowWidth * 0.25;
            else BallCircle.Center.X = GameConstants.WindowWidth * 0.75;
            BallCircle.Center.Y = 400;
            XSpeed = 0;
            YSpeed = 0;
            isInPlay = false;
        }

        // Check for collisions with the net
        private bool IsBallHitsNet()
        {
            return SplashKit.RectanglesIntersect(BallRectangle, NetRectangle);
        }

    }
}