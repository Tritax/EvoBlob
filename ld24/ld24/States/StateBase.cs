using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ld24.States
{
   enum GameStates : byte
   {
      InGame,
      Quit
   }

   abstract class StateBase
   {
      private KeyboardState _prevKeys;
      private KeyboardState _curKeys;

      private MouseState _prevMouse;
      private MouseState _curMouse;

      private GamePadState _curPad;
      private GamePadState _prevPad;

      public abstract void Init(Game1 g);
      public abstract void Uninit();

      public GameStates Update(double dt)
      {
         _curKeys = Keyboard.GetState();
         _curMouse = Mouse.GetState();
         _curPad = GamePad.GetState(PlayerIndex.One);

         GameStates ret = OnUpdate(dt);

         _prevKeys = _curKeys;
         _prevMouse = _curMouse;
         _prevPad = _curPad;

         return ret;
      }

      protected virtual GameStates OnUpdate(double dt)
      {
         return GameStates.Quit;
      }

      public virtual void Draw(GraphicsDevice dev)
      {
         // Nothing
      }

      protected bool IsKeyPressed(Keys key)
      {
         return _curKeys.IsKeyDown(key) && _prevKeys.IsKeyUp(key);
      }

      protected bool IsKeyDown(Keys key)
      {
         return _curKeys.IsKeyDown(key);
      }

      protected Point GetMouse()
      {
         return new Point(_curMouse.X, _curMouse.Y);
      }

      protected Vector2 GetMoveVector()
      {
         return new Vector2(_curPad.ThumbSticks.Left.X, 0);
      }

      protected bool IsButtonDown(Buttons btn)
      {
         return _curPad.IsButtonDown(btn) && _prevPad.IsButtonUp(btn);
      }
   }
}
