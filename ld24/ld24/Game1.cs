using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using ld24.Data;

namespace ld24
{
   /// <summary>
   /// This is the main type for your game
   /// </summary>
   public class Game1 : Microsoft.Xna.Framework.Game
   {
      public const int TILE_SIZE = 32;

      GraphicsDeviceManager graphics;
      SpriteBatch spriteBatch;

      private States.GameStates _state;
      private States.StateBase _gameState;

      internal static Player Player = new Player();

      public Game1()
      {
         graphics = new GraphicsDeviceManager(this);
         Content.RootDirectory = "Content";
      }

      /// <summary>
      /// Allows the game to perform any initialization it needs to before starting to run.
      /// This is where it can query for any required services and load any non-graphic
      /// related content.  Calling base.Initialize will enumerate through any components
      /// and initialize them as well.
      /// </summary>
      protected override void Initialize()
      {
         _state = States.GameStates.Menu;
         SwitchStates(_state);

         base.Initialize();
      }

      /// <summary>
      /// LoadContent will be called once per game and is the place to load
      /// all of your content.
      /// </summary>
      protected override void LoadContent()
      {
         // Create a new SpriteBatch, which can be used to draw textures.
         spriteBatch = new SpriteBatch(GraphicsDevice);

         // TODO: use this.Content to load your game content here
      }

      /// <summary>
      /// UnloadContent will be called once per game and is the place to unload
      /// all content.
      /// </summary>
      protected override void UnloadContent()
      {
         // TODO: Unload any non ContentManager content here
      }

      /// <summary>
      /// Allows the game to run logic such as updating the world,
      /// checking for collisions, gathering input, and playing audio.
      /// </summary>
      /// <param name="gameTime">Provides a snapshot of timing values.</param>
      protected override void Update(GameTime gameTime)
      {
         // Allows the game to exit
         if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            this.Exit();

         double dt = gameTime.ElapsedGameTime.TotalSeconds;
         States.GameStates next = _gameState.Update(dt);
         if (next != _state)
            SwitchStates(next);
         
         base.Update(gameTime);
      }

      /// <summary>
      /// This is called when the game should draw itself.
      /// </summary>
      /// <param name="gameTime">Provides a snapshot of timing values.</param>
      protected override void Draw(GameTime gameTime)
      {
         GraphicsDevice.Clear(Color.CornflowerBlue);

         _gameState.Draw(GraphicsDevice);

         base.Draw(gameTime);
      }

      private void SwitchStates(States.GameStates newState)
      {
         States.StateBase cur = _gameState;
         if (cur != null)
            cur.Uninit();

         switch (newState)
         {
            default:
            case States.GameStates.Quit:
               this.Exit();
               break;
            case States.GameStates.Menu:
               _gameState = new States.MenuState();
               break;
            case States.GameStates.Instruct:
               _gameState = new States.InstructState();
               break;
            case States.GameStates.InGame:
               _gameState = new States.InGame();
               break;
         };

         _state = newState;
         if (_gameState != null)
            _gameState.Init(this);
      }
   }
}
