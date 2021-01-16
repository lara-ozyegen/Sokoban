using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sokoban
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Texture2D texture;
        const int levelSize = 20;
        const int width = 5;
        const int height = 5;
        int[,,] gameboard = new int [levelSize, height, width];
        int level = 0;
        int space = 0;
        int box = 1;
        int wall = 2;
        int player = 3;
        int target = 4;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

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
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

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
                        _spriteBatch.Draw(texture, new Rectangle(x * 50, y * 50, 50, 50), new Rectangle((gameboard[level, y, x] - 1) * 50, 0, 50, 50), Color.White);
                    }
                }
                
            }
            
            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
