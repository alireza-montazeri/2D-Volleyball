using System;
using SplashKitSDK;

namespace Volley2DGame
{
    class Program
    {
        private static Bitmap bgBitmap = new Bitmap("bg", "Resources/background.jpg");
        private static Font _font = SplashKit.LoadFont("openSans-Bold", "Resources/OpenSans-Bold.ttf");
        static void Main()
        {
            // Create a window
            Window gameWindow = new Window("Volleyball Game", GameConstants.WindowWidth, GameConstants.WindowHeight);

            // Initialize game objects (players, ball, AI opponent)
            Ball ball = new Ball(GameConstants.WindowWidth * 0.25, 400);
            Player player = new Player(Color.Blue, GameConstants.WindowWidth * 0.25, GameConstants.GroundLevel);
            AIOpponent aiOpponent = new AIOpponent(Color.Green, GameConstants.WindowWidth * 0.75, GameConstants.GroundLevel, ball);

            // Main game loop
            while (!gameWindow.CloseRequested)
            {
                // Tell SplashKit to process events
                SplashKit.ProcessEvents();

                // Handle user input
                player.HandleInput();

                // Update game objects
                player.Update();
                aiOpponent.Update();
                ball.Update(player, aiOpponent);

                // Draw game objects
                DrawBackground(gameWindow);
                DrawScore(ball);
                player.Draw();
                ball.Draw();
                aiOpponent.Draw();

                gameWindow.Refresh();
            }

            // Cleanup and close the game
            SplashKit.CloseWindow("Volleyball Game");
        }

        static private void DrawBackground(Window gameWindow)
        {
            gameWindow.Clear(Color.White);
            SplashKit.DrawBitmap(bgBitmap, 0, 0);
            // SplashKit.FillRectangle(Color.SandyBrown, 0, GameConstants.GroundLevel - 200, GameConstants.WindowWidth, GameConstants.GroundLevel + 20);
            SplashKit.FillRectangle(Color.SaddleBrown, 0, GameConstants.GroundLevel + 20, GameConstants.WindowWidth, GameConstants.WindowHeight);
        }

        static private void DrawScore(Ball ball)
        {
            SplashKit.DrawText($"{ball.PlayerScore} : {ball.AIScore}", Color.Black, _font!, 25, GameConstants.WindowWidth / 2 - 20, 20);
        }
    }

    public static class GameConstants
    {
        public const int WindowWidth = 1080;
        public const int WindowHeight = 720;
        // Define gravity constant
        public const double Gravity = 0.004;
        public const double GroundLevel = 0.8 * WindowHeight;

        public const double MoveSpeed = 0.3;    // Adjust the player's horizontal speed
        public const double JumpStrength = -1.2; // Adjust the player's jump strength
        public const double AIOpponentMoveSpeed = 0.3; // Adjust the AI opponent's move speed
        public const double HitPower = 1.8;
    }
}
