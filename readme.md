# Creating a 2D 2-Player Volleyball Game with SplashKit in C#

In this tutorial, we'll create a simple 2D volleyball game from scratch using the SplashKit game development framework. The game will feature two players, a ball, and an AI-controlled opponent. We'll walk through the code for the game, explaining each component and its functionality.

This is the [video](https://video.deakin.edu.au/media/t/1_1nqa2z4b) of the game running. The player on the left side is playable and on the right side is AI controlled.

### Prerequisites

Before we start, make sure you have the following prerequisites:

- C# development environment.
- SplashKit SDK installed (you can download it from [here](https://www.splashkit.io/))

### Project Setup

1. Create a new C# Console Application project in Visual Studio, and name it "Volley2DGame."

2. Add references to the SplashKit SDK by following the installation instructions provided on the [SplashKit website](https://www.splashkit.io/docs/installation/).

3. Create a "Resources" folder in your project directory. Download the required images and fonts and place them in this folder. You can use any images and fonts you prefer.

4. Create a new class called `Program.cs` and copy the following code into it:

```csharp
using System;
using SplashKitSDK;

namespace Volley2DGame
{
    class Program
    {
        private static Bitmap bgBitmap = new Bitmap("bg", "Resources/background.jpg");
        private static Font _font = SplashKit.LoadFont("openSans-Bold", "Resources/OpenSans-Bold.ttf");
        // ... (Rest of the code)
    }

    // ... (Rest of the code)
}
```

This is the entry point of our game, where we initialize the game window and handle game objects.

### Game Constants

Create a `GameConstants` class to store global constants and settings for the game. Add the following code to `Program.cs`:

```csharp
public static class GameConstants
{
    public const int WindowWidth = 1080;
    public const int WindowHeight = 720;
    public const double Gravity = 0.004;
    public const double GroundLevel = 0.8 * WindowHeight;

    public const double MoveSpeed = 0.3;
    public const double JumpStrength = -1.2;
    public const double AIOpponentMoveSpeed = 0.3;
    public const double HitPower = 1.8;
}
```

These constants define the window dimensions, gravity, ground level, player and AI movement speed, jump strength, and hit power.

### Player Class

Let's dive deeper into the `Player` class, which represents the player character in our 2D volleyball game. We'll go through each aspect of the class in more detail.

```csharp
using SplashKitSDK;

namespace Volley2DGame
{
    public class Player
    {
        public Circle PlayerCircle;
        private Bitmap _playerBitmap;
        public double XSpeed { get; private set; }
        public double YSpeed { get; private set; }
        private bool IsJumping { get; set; }
        private const double _playerWidth = 60;

        public Player(double x, double y)
        {
            PlayerCircle = SplashKit.CircleAt(x, y, _playerWidth / 2);
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
            PlayerCircle.Center.X += XSpeed;
            PlayerCircle.Center.Y += YSpeed;

            // Check for collisions with the ground or other objects
            if (PlayerCircle.Center.Y > GameConstants.GroundLevel)
            {
                PlayerCircle.Center.Y = GameConstants.GroundLevel;
                IsJumping = false;
            }

            // Set the horizontal limits
            if (PlayerCircle.Center.X < _playerWidth / 2)
            {
                PlayerCircle.Center.X = _playerWidth / 2;
            }
            else if (PlayerCircle.Center.X > (GameConstants.WindowWidth / 2) - (_playerWidth / 2))
            {
                PlayerCircle.Center.X = (GameConstants.WindowWidth / 2) - (_playerWidth / 2);
            }
        }

        public void Draw()
        {
            SplashKit.DrawBitmap(_playerBitmap, PlayerCircle.Center.X - _playerBitmap.Width / 2, PlayerCircle.Center.Y - _playerBitmap.Height / 2);
        }
    }
}
```

Now, let's break down the different aspects of the `Player` class:

1. **Fields and Properties:**

   - `PlayerCircle`: This is a `Circle` object representing the player's character. It defines the player's position and size on the screen.
   - `_playerBitmap`: This is a `Bitmap` object that represents the player's image or sprite.
   - `XSpeed` and `YSpeed`: These properties represent the player's horizontal and vertical speeds, respectively.
   - `IsJumping`: A boolean flag that indicates whether the player is currently in a jump state.
   - `_playerWidth`: This constant defines the width of the player's character.

2. **Constructor:**

   - The constructor initializes the player's character. It takes the `x`, and `y` coordinates as parameters.
   - It creates a circular collision shape (`PlayerCircle`) at the specified position.
   - It sets the player's initial speed values, jump state, and loads the player's image from a file.

3. **HandleInput Method:**

   - `HandleInput` processes player input for movement and jumping.
   - It checks whether the player is pressing keys for left or right movement (`A`, `LeftArrow`, `D`, `RightArrow`) and sets the `XSpeed` accordingly.
   - If the player presses the jump keys (`W`, `UpArrow`) and is not currently jumping, it initiates a jump by setting `YSpeed` to a negative value.

4. **Update Method:**

   - `Update` is responsible for updating the player's position based on physics, input, and collision detection.
   - It applies gravity by increasing the `YSpeed` to simulate jumping and falling.
   - The player's position is updated based on `XSpeed` and `YSpeed`.
   - It checks for collisions with the ground (`GroundLevel`) to prevent the player from falling through it.
   - It restricts the player's horizontal movement within the screen boundaries.

5. **Draw Method:**
   - `Draw` is used to render the player's character on the screen.
   - It draws the player's image (`_playerBitmap`) at the current position of the player's `PlayerCircle`.

In summary, the `Player` class handles player input, physics, collision detection, and rendering. It ensures that the player character moves, jumps, and interacts with the game world correctly.

### Ball Class

The `Ball` class encapsulates the behavior of the game ball, including its movement, collision detection, scoring, and rendering. The `Update` method is where most of the game logic for the ball is implemented, making it a crucial part of the game's functionality. Additional methods can be added to this class to implement specific game rules and behaviors as needed.

```csharp
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

        public void Draw()
        {
            SplashKit.DrawBitmap(_ballBitmap, BallCircle.Center.X - _ballBitmap.Width / 2, BallCircle.Center.Y - _ballBitmap.Height / 2);
            SplashKit.FillRectangle(Color.Brown, GameConstants.WindowWidth / 2 - 10, GameConstants.WindowHeight - 380, 20, 380);
        }

        // Rest Of The Code
    }
}
```

Now, let's break down the different fields and methods within the `Ball` class:

1. **Fields:**

   - `BallCircle`: This is a `Circle` object that represents the position and size of the ball on the screen.
   - `_ballBitmap`: A `Bitmap` object used to display the ball's image or sprite.
   - `XSpeed` and `YSpeed`: These properties represent the horizontal and vertical speeds of the ball.
   - `isInPlay`: A boolean flag indicating whether the ball is currently in play (i.e., not scored or out of bounds).
   - `PlayerScore` and `AIScore`: These properties keep track of the player's and AI opponent's scores.
   - `BallRectangle`: A `Rectangle` object used for collision detection with other objects.
   - `NetRectangle`: A `Rectangle` object representing the net in the game.
   - `_ballWidth`: This variable stores the width of the ball.

2. **Constructor:**

   - The constructor initializes the ball object with a specified `x` and `y` position.
   - It creates a circular collision shape (`BallCircle`) at the given position.
   - The ball's speed is set to zero, and it is initially not in play.
   - The `_ballBitmap` is loaded with the ball's image from a file.

3. **Draw Method:**

   - The `Draw` method is responsible for rendering the ball and the net on the screen.
   - It draws the ball's image (`_ballBitmap`) at the current position of the `BallCircle`.

4. **IsCollisionWith**

   This method checks if the ball collides with a given circular object (usually a player or the AI opponent). It returns `true` if there is a collision and `false` otherwise. This method is used to detect whether the ball has been hit by a player or the AI opponent.

   ```csharp
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
   ```

   Here's a breakdown of the logic:

   - It uses `SplashKit.CirclesIntersect` to check if the ball's circular shape (`BallCircle`) intersects with the provided circular object (`collisionObject`).
   - If a collision is detected, it sets `isInPlay` to `true` to indicate that the ball is now in play.
   - It returns `true` if there is a collision, otherwise, it returns `false`.

5. **HitByPlayer**

   This method calculates the ball's new speed after being hit by a player or AI opponent. It takes the circle of the hitting object (`circle`), as well as the horizontal and vertical speeds of that object.

   ```csharp
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
   ```

   Here's a breakdown of the logic:

   - It calculates the angle between the ball's current position and the hitting object's center using `Math.Atan2`.
   - Based on the angle and the hitting object's power (represented by `hitPower`), it computes the new horizontal and vertical speeds (`XSpeed` and `YSpeed`) for the ball.
   - The addition of `playerSpeedX` and `playerSpeedY` allows some influence on the ball's direction based on the player's movement.

6. **IsOutOfScreen**

   This method checks if the ball has moved out of the screen boundaries. If so, it returns `true`, indicating that the ball is out of bounds.

   ```csharp
   private bool IsOutOfScreen()
   {
       // Check for collisions with the game boundaries
       return BallCircle.Center.X < 0 || BallCircle.Center.X > SplashKit.ScreenWidth();
   }
   ```

   Here's a breakdown of the logic:

   - It checks whether the ball's center `X` coordinate is less than 0 (left screen boundary) or greater than the screen width (right screen boundary).
   - If either condition is met, it returns `true` to indicate that the ball is out of bounds.

7. **HitTheGround**

   This method checks if the ball has hit the ground. If the ball's `Y` coordinate is greater than the ground level, it returns `true`.

   ```csharp
   private bool HitTheGround()
   {
       // Check if the ball has hit the ground
       return BallCircle.Center.Y > GameConstants.GroundLevel;
   }
   ```

   Here's a breakdown of the logic:

   - It compares the `Y` coordinate of the ball's center (`BallCircle.Center.Y`) with the predefined `GroundLevel` from the `GameConstants` class.
   - If the `Y` coordinate is greater than the ground level, it returns `true` to indicate that the ball has hit the ground.

8. **BallInServePosition**

   This method resets the ball to the serve position based on whether the player or the AI opponent scored. It takes a boolean parameter, `isPlayer`, to determine which side scored.

   ```csharp
   private void BallInServePosition(bool isPlayer)
   {
       if (isPlayer)
       {
           BallCircle.Center.X = GameConstants.WindowWidth * 0.25;
       }
       else
       {
           BallCircle.Center.X = GameConstants.WindowWidth * 0.75;
       }
       BallCircle.Center.Y = 400;
       XSpeed = 0;
       YSpeed = 0;
       isInPlay = false;
   }
   ```

   Here's a breakdown of the logic:

   - If `isPlayer` is `true`, it sets the ball's `X` coordinate to the player's side (left side), and if `false`, it sets it to the AI opponent's side (right side).
   - It sets the `Y` coordinate to 400 (a typical starting height for the ball in volleyball).
   - The `XSpeed` and `YSpeed` are reset to zero to stop any ongoing movement.
   - Finally, it sets `isInPlay` to `false` to indicate that the ball is not in play (waiting for the next serve).

9. **IsBallHitsNet**

   This method checks if the ball collides with the net. It returns `true` if there is a collision.

   ```csharp
   private bool IsBallHitsNet()
   {
       return SplashKit.RectanglesIntersect(BallRectangle, NetRectangle);
   }
   ```

   Here's a breakdown of the logic:

   - It uses `SplashKit.RectanglesIntersect` to check if the ball's rectangular collision shape (`BallRectangle`) intersects with the net's rectangular shape (`NetRectangle`).
   - If there is a collision, it returns `true`, indicating that the ball has hit the net.

10. **Update**

    The `Update` method is the heart of the `Ball` class and handles most of the game logic for the ball's movement and interactions. Let's break down its logic:

    ```csharp
    public void Update(Player player, AIOpponent aiOpponent)
    {
        if (IsCollisionWith(player.PlayerCircle)) HitByPlayer(player.PlayerCircle, player.XSpeed, 0);
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
            BallCircle.Center.Y +=

    YSpeed;
            BallRectangle = SplashKit.RectangleAround(BallCircle);
        }
    }
    ```

    Here's a breakdown of the logic:

    - It first checks for collisions with the player and the AI opponent using `IsCollisionWith`. If a collision is detected, it calls `HitByPlayer` to calculate the new ball speed based on the hitting object's speed and angle.
    - It checks if the ball hits the net using `IsBallHitsNet()` and changes its horizontal direction if necessary.
    - It checks if the ball is out of the screen using `IsOutOfScreen()` and changes its horizontal direction if needed.
    - It checks if the ball hits the ground using `HitTheGround()`. If it does, it handles scoring, updates the score, and positions the ball for the next serve using `BallInServePosition`.
    - If `isInPlay` is `true`, it simulates gravity by adding the gravity constant (`GameConstants.Gravity`) to the vertical speed (`YSpeed`).
    - It updates the ball's position by adding its horizontal and vertical speeds to its center coordinates.
    - It updates the `BallRectangle`, the rectangular collision shape around the ball, to reflect its new position.

    In summary, the `Update` method is responsible for updating the ball's position, handling collisions, scoring, and applying gravity to simulate its movement. It's the core method that drives the behavior of the ball in the game.

### AIOpponent Class

The key logic for the AI opponent's behavior is typically implemented in the `Update` method. This logic can include decision-making based on the game state (e.g., predicting the ball's movement) and moving the AI opponent accordingly.

```csharp
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
            // Constructor
        }

        public void Update()
        {
            // AI opponent logic
            // ... (Code for AI opponent logic)
        }

        public void Draw()
        {
            // Draw the AI opponent
            // ... (Code for drawing the AI opponent)
        }

        public void Jump()
        {
            // Perform a jump action
            // ... (Code for jumping logic)
        }
    }
}
```

In the provided code, the AI opponent's logic is placed within the `Update` method. However, the specific behavior and decision-making process for the AI opponent are not fully detailed in the code comments. You can further elaborate on the AI opponent's behavior by implementing decision-making based on the ball's position, movement, and game state, as well as controlling its movements and jumps accordingly.

Additional logic can be added to the `AIOpponent` class to make the AI opponent more responsive and challenging to play against in your volleyball game.

1. **AIOpponent**

   This is the constructor of the `AIOpponent` class. It initializes the AI opponent object with a specified position, color, and a reference to the game ball.

   ```csharp
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
   ```

   Explanation:

   - `color`: The color of the AI opponent (although it's not used in the provided code).
   - `x` and `y`: The initial position of the AI opponent.
   - `Ball ball`: A reference to the game ball, allowing the AI opponent to interact with it.
   - `OpponentCircle`: A circular collision shape representing the AI opponent's position.
   - `XSpeed` and `YSpeed`: Initialize the AI opponent's horizontal and vertical speeds to zero.
   - `IsJumping`: Set to `false` initially, indicating that the AI opponent is not jumping.
   - `_aiOpponentBitmap`: Load the AI opponent's image from a file.

2. **Update**

   The `Update` method is responsible for updating the AI opponent's position and behavior based on the game state.

   ```csharp
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
               XSpeed = deltaX * moveSpeed;
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
           if (OpponentCircle.Center.X < (GameConstants.WindowWidth / 2) + (_aiOpponentWidth / 2))
               OpponentCircle.Center.X = (GameConstants.WindowWidth / 2) + (_aiOpponentWidth / 2);
           else if (OpponentCircle.Center.X > GameConstants.WindowWidth - (_aiOpponentWidth / 2))
               OpponentCircle.Center.X = GameConstants.WindowWidth - (_aiOpponentWidth / 2);

           // Check for collisions with the ground
           if (OpponentCircle.Center.Y > GameConstants.GroundLevel) // Ball hits the ground
           {
               OpponentCircle.Center.Y = GameConstants.GroundLevel;
               IsJumping = false;
           }
       }
   }
   ```

   Explanation:

   - The `Update` method contains the AI opponent's logic for reacting to the game situation and moving accordingly.
   - It checks if the game ball is on the AI opponent's side of the court (`Ball.BallCircle.Center.X > GameConstants.WindowWidth / 2`). If so, the AI opponent calculates the expected landing point of the ball and moves towards it.
   - It predicts the ball's landing point based on its trajectory, taking into account gravity (`GameConstants.Gravity`) and updates in `ballSpeedX` and `ballSpeedY`.
   - The AI opponent calculates the direction (`deltaX` and `deltaY`) to the expected landing point and adjusts its movement speed accordingly.
   - If the ball's vertical speed (`ballSpeedY`) is zero and the ball is close (`deltaX < 5`), the AI opponent may decide to jump.
   - It updates the AI opponent's position (`OpponentCircle.Center.X` and `OpponentCircle.Center.Y`) based on the calculated speeds.
   - The AI opponent's horizontal movement is constrained within the court's bounds.
   - It simulates jumping and falling by applying gravity to the vertical speed (`YSpeed`).
   - If the AI opponent hits the ground (`OpponentCircle.Center.Y > GameConstants.GroundLevel`), it sets `IsJumping` to `false`.

3. **Draw**

   The `Draw` method is responsible for rendering the AI opponent on the screen.

   ```csharp
   public void Draw()
   {
       SplashKit.DrawBitmap(_aiOpponentBitmap, OpponentCircle.Center.X - _aiOpponentBitmap.Width / 2, OpponentCircle.Center.Y - _aiOpponentBitmap.Height / 2);
   }
   ```

   Explanation:

   - The `Draw` method uses SplashKit to draw the AI opponent's image (`_aiOpponentBitmap`) at the current position of the `OpponentCircle`. It centers the image on the AI opponent's position.

   4. **Jump**

   The `Jump` method allows the AI opponent to perform a jump action.

   ```csharp
   public void Jump()
   {
       if (!IsJumping)
       {
           YSpeed = GameConstants.JumpStrength;
           IsJumping = true;
       }
   }
   ```

   Explanation:

   - The `Jump` method is called when the AI opponent decides to jump.
   - It sets the AI opponent's vertical speed (`YSpeed`) to the jump strength constant (`GameConstants.JumpStrength`) to make the AI opponent jump upwards.
   - It sets `IsJumping` to `true` to indicate that the AI opponent is currently jumping.

   These methods together define the behavior of the AI opponent in your 2D volleyball game. The `Update

   `method contains the core logic that makes the AI opponent react to the ball's movement and make decisions regarding its movements and jumps. The`Draw`method handles rendering the AI opponent, and`Jump` allows the AI opponent to perform a jump when necessary.

### Main Program (Program.cs)

The `Main` method is the central part of your game, responsible for setting up the game window, initializing game objects, running the game loop, handling user input, updating game logic, drawing game elements, and finally, cleaning up and closing the game window. It serves as the backbone of your 2D volleyball game and orchestrates the interactions between various game components.

```csharp
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
```

Here's a detailed explanation of the key sections of the `Main` method:

1. **Window Initialization:**

   ```csharp
   // Create a window
   Window gameWindow = new Window("Volleyball Game", GameConstants.WindowWidth, GameConstants.WindowHeight);
   ```

   - The `Window` class from SplashKit is used to create the game window. It specifies the window title and dimensions based on the constants defined in the `GameConstants` class.

2. **Game Object Initialization:**

   ```csharp
   // Initialize game objects (players, ball, AI opponent)
   Ball ball = new Ball(GameConstants.WindowWidth * 0.25, 400);
   Player player = new Player(Color.Blue, GameConstants.WindowWidth * 0.25, GameConstants.GroundLevel);
   AIOpponent aiOpponent = new AIOpponent(Color.Green, GameConstants.WindowWidth * 0.75, GameConstants.GroundLevel, ball);
   ```

   - Game objects such as the `Ball`, `Player`, and `AIOpponent` are initialized with their respective positions and attributes. The `Ball` object is also given a reference to the `AIOpponent` object.

3. **Game Loop:**

   ```csharp
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
   ```

   - The main game loop runs as long as the game window is not requested to be closed (`!gameWindow.CloseRequested`).
   - Inside the loop, SplashKit's `ProcessEvents` function is called to handle user input and events.
   - Player input is handled using the `HandleInput` method of the `Player` object.
   - Game objects are updated by calling their respective `Update` methods. This includes updating the player's position, AI opponent's behavior, and ball's movement and interactions.
   - The game background is drawn using `DrawBackground`, the score is displayed using `DrawScore`, and the game objects (player, ball, AI opponent) are drawn on the window.
   - The game window is refreshed to display the updated frame.

4. **Cleanup and Game Closure:**

   ```csharp
   // Cleanup and close the game
   SplashKit.CloseWindow("Volleyball Game");
   ```

   - After the game loop exits, the code performs cleanup operations. It closes the game window using `SplashKit.CloseWindow` with the window's title as an argument.

### Conclusion

You have now implemented a basic 2D volleyball game using C# and SplashKit. You've created classes for the player, ball, and AI opponent, and implemented game logic for movement, collision detection, and scoring.

Feel free to enhance the game further by adding features like better AI, sound effects, and more advanced game play mechanics. Happy game development!
