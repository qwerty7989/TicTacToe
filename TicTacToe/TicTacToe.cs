using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace TicTacToe
{
	public class TicTacToe : Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;
		private Texture2D _line, _cross, _circle;
		private SpriteFont _fontTerminal;

		private int[,] _gameTable;
		private int _gameTableCount;
		private Boolean _isCircleTurn;
		private Boolean _isGameOver;
		private Boolean _isGameDraw;

		public TicTacToe()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			// TODO: Add your initialization logic here

			// Set the width and height of your window.
			_graphics.PreferredBackBufferWidth = 600;
			_graphics.PreferredBackBufferHeight = 600;
			_graphics.ApplyChanges();

			_gameTable = new int[3, 3];
			_isGameOver = false;

			base.Initialize();
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			// TODO: use this.Content to load your game content here

			// Load a Texture2D from a file through Content.mgcb.
			_line = this.Content.Load<Texture2D>("Line");
			_cross = this.Content.Load<Texture2D>("Cross");
			_circle = this.Content.Load<Texture2D>("Circle");
			_fontTerminal = this.Content.Load<SpriteFont>("Fonts/Terminal");
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			MouseState state = Mouse.GetState();

			// TODO: Add your update logic here

			// Check winning condition.
			if ((_gameTable[0, 0] == _gameTable[0, 1] && _gameTable[0, 1] == _gameTable[0, 2] && _gameTable[0, 0] != 0)
				|| (_gameTable[1, 0] == _gameTable[1, 1] && _gameTable[1, 1] == _gameTable[1, 2] && _gameTable[1, 0] != 0)
				|| (_gameTable[2, 0] == _gameTable[2, 1] && _gameTable[2, 1] == _gameTable[2, 2] && _gameTable[2, 0] != 0)
				|| (_gameTable[0, 0] == _gameTable[1, 0] && _gameTable[1, 0] == _gameTable[2, 0] && _gameTable[0, 0] != 0)
				|| (_gameTable[0, 1] == _gameTable[1, 1] && _gameTable[1, 1] == _gameTable[2, 1] && _gameTable[0, 1] != 0)
				|| (_gameTable[0, 2] == _gameTable[1, 2] && _gameTable[1, 2] == _gameTable[2, 2] && _gameTable[0, 2] != 0)
				|| (_gameTable[0, 0] == _gameTable[1, 1] && _gameTable[1, 1] == _gameTable[2, 2] && _gameTable[0, 0] != 0)
				|| (_gameTable[0, 2] == _gameTable[1, 1] && _gameTable[1, 1] == _gameTable[2, 0] && _gameTable[0, 2] != 0))
			{
				_isGameOver = true;
			}
			else if (_gameTableCount == 9)
			{
				_isGameDraw = true;
			}

			// Skip the turn process if there's winner.
			if (_isGameOver || _isGameDraw) {
				// Reset the game.
				if (Keyboard.GetState().IsKeyDown(Keys.X))
				{
					_isGameOver = false;
					_isGameDraw = false;
					_isCircleTurn = false;
					_gameTable = new int[3, 3];
					_gameTableCount = 0;
				}

				return;
			}

			// Check if the mouse was clicked.
			if (state.LeftButton == ButtonState.Pressed)
			{
				int iPos = state.X / 200;
				int jPos = state.Y / 200;

				if (iPos >= 0 && iPos < 3 && jPos >= 0 && jPos < 3)
				{
					// Check if cell was empty or not.
					if (_gameTable[jPos, iPos] == 0)
					{
						// Check whom turn is.
						if (_isCircleTurn)
						{
							// O turn.
							_gameTable[jPos, iPos] = 1;
							_gameTableCount++;
							_isCircleTurn = false;
						}
						else
						{
							// X turn.
							_gameTable[jPos, iPos] = -1;
							_gameTableCount++;
							_isCircleTurn = true;
						}
					}
				}
			}

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);

			_spriteBatch.Begin();

			// TODO: Add your drawing code here

			// draw the X and O on the table.
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					if (_gameTable[i, j] == 1)
					{
						// circle
						_spriteBatch.Draw(_circle, new Vector2(200 * j, 200 * i), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					}
					else if (_gameTable[i, j] == -1)
					{
						// cross
						_spriteBatch.Draw(_cross, new Vector2(200 * j, 200 * i), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					}
				}
			}

			// Draw horizontal line for the table.
			_spriteBatch.Draw(_line, new Vector2(0, 200), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			_spriteBatch.Draw(_line, new Vector2(0, 400), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			// Draw vertical line for the table.
			_spriteBatch.Draw(_line, new Vector2(200, 0), null, Color.White, MathHelper.Pi / 2, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			_spriteBatch.Draw(_line, new Vector2(400, 0), null, Color.White, MathHelper.Pi / 2, Vector2.Zero, 1f, SpriteEffects.None, 0f);


			// If there's a winner, announce the winner
			if (_isGameOver)
			{
				{
					var winnerName = _isCircleTurn ? "X" : "O";
					_spriteBatch.DrawString(_fontTerminal, winnerName + " is the Winner!", new Vector2(200, 300), Color.CornflowerBlue);
					_spriteBatch.DrawString(_fontTerminal, "Press X to reset", new Vector2(200, 400), Color.CornflowerBlue);
				}
			}
			else if (_isGameDraw)
			{
				_spriteBatch.DrawString(_fontTerminal, "The Game ends in Draw!", new Vector2(150, 300), Color.CornflowerBlue);
				_spriteBatch.DrawString(_fontTerminal, "Press X to reset", new Vector2(200, 400), Color.CornflowerBlue);
			}

			_spriteBatch.End();

			_graphics.BeginDraw();

			base.Draw(gameTime);
		}
	}
}