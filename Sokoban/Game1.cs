using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;


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
        Vector2 nextPosBox;
        const int levelSize = 20;
        const int width = 6;
        const int height = 6;
        int[,,] gameboard = new int [levelSize, height, width];
        bool[] menu = new bool[levelSize];
        int level = 0;
        int space = 0;
        int box = 1;
        int wall = 2;
        int player = 3;
        int target = 4;
        int corner = 5;
        int levelSelection = 0;
        bool isLeftPressed = false;
        bool isRightPressed = false;
        bool isUpPressed = false;
        bool isDownPressed = false;
        bool winLose;
        bool mainMenu;

        List<SoundEffect> sounds;

        const string winMessage = "WIN!";
        const string gameOverMessage = "GAME OVER!";

        
     
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            sounds = new List<SoundEffect>();
            mainMenu = true;
        }

        protected override void Initialize()
        {
           
            if(level == 0)
            {
                playerPos = new Vector2(50,50);
                nextPos = new Vector2(50,50);
                nextPosBox = new Vector2(50*3, 50*3);
                boxPos = new Vector2(50 * 3, 50 * 3);
            }

            for(int i = 0; i < levelSize; i++)
            {
                menu[i] = false;
            }

            menu[0] = true; //First level can always be played

            winLose = false;

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

            gameboard[0, 2, 4] = target;
            gameboard[0, 1, 1] = corner;
            gameboard[0, height-2, width-2] = corner;
            gameboard[0, height-2, 1] = corner;
            gameboard[0, 1, width-2 ] = corner;

            //Adding the sound effects
            sounds.Add(Content.Load<SoundEffect>("BoxHitsWall")); 
            sounds.Add(Content.Load<SoundEffect>("WinClaps"));
            sounds.Add(Content.Load<SoundEffect>("Footsteps"));
            sounds.Add(Content.Load<SoundEffect>("OnTarget"));
            sounds.Add(Content.Load<SoundEffect>("TaDa"));
            sounds.Add(Content.Load<SoundEffect>("PlayerHitsWall3"));
            sounds.Add(Content.Load<SoundEffect>("BoxSlide"));
            sounds.Add(Content.Load<SoundEffect>("GameOver"));
        }

        public void playSound()
        {
            if (!winLose && !mainMenu)
            {
                //PLayer is not moving
                if (!(nextPos.X == playerPos.X && nextPos.Y == playerPos.Y))
                {
                    //walk
                    if (gameboard[level, (int)(nextPos.Y / 50), (int)(nextPos.X / 50)] == space || gameboard[level, (int)(nextPos.Y / 50), (int)(nextPos.X / 50)] == corner)
                    {
                        sounds[2].CreateInstance().Play();
                    }

                    //player hits the wall
                    if (gameboard[level, (int)(nextPos.Y / 50), (int)(nextPos.X / 50)] == wall)
                    {
                        sounds[5].CreateInstance().Play();
                    }

                    if (nextPos.X == boxPos.X && nextPos.Y == boxPos.Y)
                    {
                        //player slides the box if it is possible for the box to slide in that direction
                        if (gameboard[level, (int)(nextPosBox.Y / 50), (int)(nextPosBox.X / 50)] == space)
                        {
                            sounds[6].CreateInstance().Play();
                        }

                        //box hits the wall
                        if (gameboard[level, (int)(nextPosBox.Y / 50), (int)(nextPosBox.X / 50)] == wall)
                        {
                            sounds[0].CreateInstance().Play();
                        }

                        //box is at the target
                        if (gameboard[level, (int)(nextPosBox.Y / 50), (int)(nextPosBox.X / 50)] == target)
                        {
                            sounds[3].CreateInstance().Play();
                            sounds[1].CreateInstance().Play();
                            winLose = true;
                        }

                        //box is at the corner(game over)
                        if (gameboard[level, (int)(nextPosBox.Y / 50), (int)(nextPosBox.X / 50)] == corner)
                        {
                            sounds[7].CreateInstance().Play();
                            winLose = true;
                        }
                    }
                }
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.M))
            {
                mainMenu = true;
            }
            if(mainMenu)
            {
                if(Keyboard.GetState().IsKeyDown(Keys.Left) && levelSelection >= 0 && !isLeftPressed)
                {
                    isLeftPressed = true;
                    levelSelection = levelSelection - 1;
                    if(levelSelection == -1)
                    {
                        levelSelection = 19;
                    }
                    playSound();
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Right) && levelSelection <= 19 && !isRightPressed)
                {
                    isRightPressed = true;
                    levelSelection = levelSelection + 1;
                    if(levelSelection == 20)
                    {
                        levelSelection = 0;
                    }
                    playSound();
                }
                //Play selected level
                if (levelSelection >= 0 && levelSelection <= 19) 
                {
                    if (menu[levelSelection] && Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        mainMenu = false;
                        playSound();
                    }
                }
                //Level is locked
                else
                {
                    playSound();
                }

            }
            //left
            if (Keyboard.GetState().IsKeyDown(Keys.Left) && isLeftPressed == false && !winLose)
            {
                nextPos.X = playerPos.X  - 50;
                nextPos.Y = playerPos.Y;
                nextPosBox.X = boxPos.X - 50;
                nextPosBox.Y = boxPos.Y;
                playSound();

                if (gameboard[level, (int)(playerPos.Y / 50), (int)(playerPos.X / 50)] != wall && playerPos.X > 50)
                {
                    if (boxPos.X == playerPos.X - 50 && boxPos.Y == playerPos.Y)
                    {
                        if (gameboard[level, (int) (playerPos.Y / 50), (int) (playerPos.X / 50) - 2] == space || gameboard[level, (int)(playerPos.Y / 50), (int)(playerPos.X / 50) - 2] == target || gameboard[level, (int)(playerPos.Y / 50), (int)(playerPos.X / 50) - 2] == corner)
                        {
                            boxPos.X = boxPos.X - 50;
                        }
                        else
                            playerPos.X = playerPos.X + 50; 
                    }
                    playerPos.X = playerPos.X - 50;
                    isLeftPressed = true;
                }
                
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Left))
            {
                if (isLeftPressed)
                {
                    isLeftPressed = false;
                }
            }

            //right
            if (Keyboard.GetState().IsKeyDown(Keys.Right) && isRightPressed == false && !winLose)
            {
                nextPos.X = playerPos.X + 50;
                nextPos.Y = playerPos.Y;
                nextPosBox.X = boxPos.X + 50;
                nextPosBox.Y = boxPos.Y;
                playSound();

                if (gameboard[level, (int)(playerPos.Y / 50), (int) (playerPos.X / 50)] != wall && playerPos.X < (width - 2) * 50)
                {
                    if (boxPos.X == playerPos.X + 50 && boxPos.Y == playerPos.Y)
                    {
                        if (gameboard[level, (int)(playerPos.Y / 50), (int)(playerPos.X / 50) + 2] == space || gameboard[level, (int)(playerPos.Y / 50), (int)(playerPos.X / 50) + 2] == target || gameboard[level, (int)(playerPos.Y / 50), (int)(playerPos.X / 50) + 2] == corner)
                        {
                            boxPos.X = boxPos.X + 50;
                        }
                        else
                            playerPos.X = playerPos.X - 50;
                        
                    }
                    playerPos.X = playerPos.X + 50;
                    isRightPressed = true;
                }
                
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Right))
            {
                if (isRightPressed)
                {
                    isRightPressed = false;
                }
            }

            //down
            if (Keyboard.GetState().IsKeyDown(Keys.Down) &&  isDownPressed == false && !winLose)
            {
                nextPos.X = playerPos.X;
                nextPos.Y = playerPos.Y + 50;
                nextPosBox.X = boxPos.X;
                nextPosBox.Y = boxPos.Y + 50;
                playSound();

                if (gameboard[level, (int)(playerPos.Y / 50), (int)(playerPos.X / 50)] != wall && playerPos.Y < (height - 2) * 50)
                {
                    if (boxPos.Y == playerPos.Y + 50 && boxPos.X == playerPos.X)
                    {
                        if (gameboard[level, (int)(playerPos.Y / 50) + 2, (int)(playerPos.X / 50)] == space || gameboard[level, (int)(playerPos.Y / 50) + 2, (int)(playerPos.X / 50)] == target || gameboard[level, (int)(playerPos.Y / 50) + 2, (int)(playerPos.X / 50)] == corner)
                        {
                            boxPos.Y = boxPos.Y + 50;
                        }
                        else
                            playerPos.Y = playerPos.Y - 50;
                    }
                    playerPos.Y = playerPos.Y + 50;
                    isDownPressed = true;
                }
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Down))
            {
                if (isDownPressed)
                {
                    isDownPressed = false;
                }
            }

            //up
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && isUpPressed == false && !winLose)
            {
                nextPos.X = playerPos.X;
                nextPos.Y = playerPos.Y - 50;
                nextPosBox.X = boxPos.X;
                nextPosBox.Y = boxPos.Y - 50;
                playSound();

                if (gameboard[level, (int)(playerPos.Y / 50), (int)(playerPos.X / 50)] != wall && playerPos.Y > 50)
                {
                    if (boxPos.Y == playerPos.Y - 50 && boxPos.X == playerPos.X)
                    {
                        if (gameboard[level, (int)(playerPos.Y / 50) - 2, (int)(playerPos.X / 50)] == space || gameboard[level, (int)(playerPos.Y / 50) - 2, (int)(playerPos.X / 50)] == target || gameboard[level, (int)(playerPos.Y / 50) - 2, (int)(playerPos.X / 50)] == corner)
                        {
                            boxPos.Y = boxPos.Y - 50;
                        }
                        else
                            playerPos.Y = playerPos.Y + 50;
                    }
                    playerPos.Y = playerPos.Y - 50;
                    isUpPressed = true;
                }
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Up))
            {
                if (isUpPressed)
                {
                    isUpPressed = false;
                }
            }

            if(Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                Initialize();
            }



            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            _spriteBatch.Begin();
            if (!mainMenu)
            {
                texture = Content.Load<Texture2D>("sokobanImages");
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        if (gameboard[level, y, x] > 0)
                        {
                            //win message
                            if (gameboard[level, (int)(boxPos.Y / 50), (int)(boxPos.X / 50)] == target)
                            {
                                SpriteFont font;
                                font = Content.Load<SpriteFont>("File");
                                _spriteBatch.DrawString(font, winMessage, new Vector2(100, 100), Color.Red);
                            }
                            //game over message
                            else if (gameboard[level, (int)(boxPos.Y / 50), (int)(boxPos.X / 50)] == corner)
                            {
                                SpriteFont font;
                                font = Content.Load<SpriteFont>("File");
                                _spriteBatch.DrawString(font, gameOverMessage, new Vector2(100, 100), Color.Red);
                            }
                            _spriteBatch.Draw(texture, playerPos, new Rectangle((gameboard[level, y, x]) * 50, 0, 50, 50), Color.White);
                            _spriteBatch.Draw(texture, boxPos, new Rectangle((gameboard[level, y, x] - 2) * 50, 0, 50, 50), Color.White);
                            _spriteBatch.Draw(texture, new Rectangle(x * 50, y * 50, 50, 50), new Rectangle((gameboard[level, y, x] - 1) * 50, 0, 50, 50), Color.White);
                        }
                    }

                }
            }
            else
            {
                SpriteFont font;
                font = Content.Load<SpriteFont>("File");
                _spriteBatch.DrawString(font, "SOKOBAN", new Vector2(100,100), Color.BlueViolet);
                int x = 100;
                int y = 150;
                for (int i = 0; i < levelSize; i++)
                {
                    if (i <= 9)
                    {
                        //If level to draw is the selected level, draw it different color
                        if (i == levelSelection)
                        {
                            _spriteBatch.DrawString(font, (i + 1).ToString(), new Vector2(x, y), Color.Coral);
                        }
                        else
                        {
                            _spriteBatch.DrawString(font, (i + 1).ToString(), new Vector2(x, y), Color.Aqua);
                        }
                        x = x + 50;
                    }
                    else
                    {
                        //If level to draw is the selected level, draw it different color
                        if (i == levelSelection)
                        {
                            _spriteBatch.DrawString(font, (i + 1).ToString(), new Vector2(x - 50*10, y + 50), Color.Coral);
                        }
                        else
                        {
                            _spriteBatch.DrawString(font, (i + 1).ToString(), new Vector2(x - 50 * 10, y + 50), Color.Aqua);
                        }
                        x = x + 50;
                    }
                }
            }
            _spriteBatch.End();
           
            base.Draw(gameTime);
        }
    }
}
