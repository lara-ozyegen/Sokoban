using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System;

namespace Sokoban
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Texture2D texture;
        Vector2 playerPos;
        Vector2 boxPos;
        Vector2 nextPos;
        const int levelSize = 20;
        const int width = 8;
        const int height = 8;
        int[,,] gameboard = new int [levelSize, height, width];
        int level = 0;
        int space = 0;
        int box = 1;
        int wall = 2;
        int player = 3;
        int target = 4;
        bool isLeftPressed = false;
        bool isRightPressed = false;
        bool isUpPressed = false;
        bool isDownPressed = false;

        List<SoundEffect> sounds;

        const string winMessage = "WIN!";
        const string gameOverMessage = "GAME OVER!";


     
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            sounds = new List<SoundEffect>();
        }

        protected override void Initialize()
        {
           
            if(level == 0)
            {
                playerPos = new Vector2(50,50);
                nextPos = new Vector2(0,0);
                boxPos = new Vector2(50 * 3, 50 * 3);
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            for (int i = 0; i < width; i++)
            {
                gameboard[0, 0, i] = wall;
                gameboard[0, height - 1, i] = wall;

            }

            for (int i = 0; i < height; i++)
            {
                gameboard[0, i, 0] = wall;
                gameboard[0, i, width - 1] = wall;
            }

            gameboard[0, 1, 6] = target;

            //Adding the sound effects
            sounds.Add(Content.Load<SoundEffect>("BoxHitsWall"));
            //sounds.Add(Content.Load<SoundEffect>("BoxSlide"));
            sounds.Add(Content.Load<SoundEffect>("WinClaps"));
            sounds.Add(Content.Load<SoundEffect>("Footsteps"));
            sounds.Add(Content.Load<SoundEffect>("OnTarget"));
            sounds.Add(Content.Load<SoundEffect>("GameOver"));
            sounds.Add(Content.Load<SoundEffect>("TaDa"));
            sounds.Add(Content.Load<SoundEffect>("PlayerHitsWall1"));
            sounds.Add(Content.Load<SoundEffect>("PlayerHitsWall2"));
            sounds.Add(Content.Load<SoundEffect>("PlayerHitsWall3"));
            sounds.Add(Content.Load<SoundEffect>("PlayerHitsWall4"));
            sounds.Add(Content.Load<SoundEffect>("PlayerHitsWall5"));
            sounds.Add(Content.Load<SoundEffect>("PlayerHitsWall6"));

        }

        public void playSound(Vector2 nextPos)
        {
            if(gameboard[level, (int)nextPos.Y, (int)(nextPos.X)] == wall)
            {
                Random rand = new Random();
                int num = rand.Next(1, 7);
                sounds[num+5].CreateInstance().Play();
            }
            if (gameboard[level, (int)nextPos.Y, (int)(nextPos.X)] == target)
            {
                sounds[3].CreateInstance().Play();
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
           
            if (Keyboard.GetState().IsKeyDown(Keys.Left) && isLeftPressed == false && playerPos.X > 50)
            {
                if (gameboard[level, (int)(playerPos.Y / 50), (int)(playerPos.X / 50)] != wall)
                {
                    if (boxPos.X == playerPos.X - 50 && boxPos.Y == playerPos.Y)
                    {
                        if (gameboard[level, (int)(playerPos.Y / 50), (int)(playerPos.X / 50) - 2] == space || gameboard[level, (int)(playerPos.Y / 50), (int)(playerPos.X / 50) - 2] == target)
                        {
                            boxPos.X = boxPos.X - 50;
                        }
                        else
                            playerPos.X = playerPos.X + 50;
                    }
                    playerPos.X = playerPos.X - 50;
                    isLeftPressed = true;
                    sounds[2].CreateInstance().Play();
                }
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Left))
            {
                if (isLeftPressed)
                {
                    isLeftPressed = false;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right) && playerPos.X < (width - 2) * 50 && gameboard[level, (int)(playerPos.Y / 50), (int)(playerPos.X / 50)] != wall && isRightPressed == false)
            {
                if (boxPos.X == playerPos.X + 50 && boxPos.Y == playerPos.Y)
                {
                    if (gameboard[level, (int)(playerPos.Y / 50), (int)(playerPos.X / 50) + 2] == space || gameboard[level, (int)(playerPos.Y / 50), (int)(playerPos.X / 50) + 2] == target)
                    {
                        boxPos.X = boxPos.X + 50;
                    }
                    else
                        playerPos.X = playerPos.X - 50;
                }
                playerPos.X = playerPos.X + 50;
                isRightPressed = true;
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Right))
            {
                if (isRightPressed)
                {
                    isRightPressed = false;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down) && playerPos.Y < (height - 2) * 50 && gameboard[level, (int)(playerPos.Y / 50), (int)(playerPos.X / 50)] != wall && isDownPressed == false)
            {
                if (boxPos.Y == playerPos.Y + 50 && boxPos.X == playerPos.X)
                {
                    if (gameboard[level, (int)(playerPos.Y / 50) + 2, (int)(playerPos.X / 50)] == space || gameboard[level, (int)(playerPos.Y / 50) + 2, (int)(playerPos.X / 50)] == target)
                    {
                        boxPos.Y = boxPos.Y + 50;
                    }
                    else
                        playerPos.Y = playerPos.Y - 50;
                }
                playerPos.Y = playerPos.Y + 50;
                isDownPressed = true;
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Down))
            {
                if (isDownPressed)
                {
                    isDownPressed = false;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up) && playerPos.Y > 50 && gameboard[level, (int)(playerPos.Y / 50), (int)(playerPos.X / 50)] != wall && isUpPressed == false)
            {
                if (boxPos.Y == playerPos.Y - 50 && boxPos.X == playerPos.X)
                {
                    if (gameboard[level, (int)(playerPos.Y / 50) - 2, (int)(playerPos.X / 50)] == space || gameboard[level, (int)(playerPos.Y / 50) - 2, (int)(playerPos.X / 50)] == target)
                    {
                        boxPos.Y = boxPos.Y - 50;
                    }
                    else
                        playerPos.Y = playerPos.Y + 50;
                }
                playerPos.Y = playerPos.Y - 50;
                isUpPressed = true;
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Up))
            {
                if (isUpPressed)
                {
                    isUpPressed = false;
                }
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            _spriteBatch.Begin();
            texture = Content.Load<Texture2D>("sokobanImages");
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (gameboard[level, y, x] > 0)
                    {
                        if(gameboard[level, (int)(boxPos.Y / 50), (int)(boxPos.X / 50)] == target)
                        {
                            SpriteFont font;
                            font = Content.Load<SpriteFont>("File");
                            _spriteBatch.DrawString(font, winMessage, new Vector2(100,100), Color.Red);
                            break;
                        }
                        _spriteBatch.Draw(texture, playerPos, new Rectangle((gameboard[level, y, x]) * 50, 0, 50, 50), Color.White);
                        _spriteBatch.Draw(texture, boxPos, new Rectangle((gameboard[level, y, x] - 2) * 50, 0, 50, 50), Color.White);
                        _spriteBatch.Draw(texture, new Rectangle(x * 50, y * 50, 50, 50), new Rectangle((gameboard[level, y, x] - 1) * 50, 0, 50, 50), Color.White);
                    }
                }
                
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
