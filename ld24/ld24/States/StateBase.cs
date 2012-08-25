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

      public abstract void Init(GraphicsDevice dev);
      public abstract void Uninit();

      public GameStates Update(double dt)
      {
         _curKeys = Keyboard.GetState();
         _curMouse = Mouse.GetState();

         GameStates ret = OnUpdate(dt);

         _prevKeys = _curKeys;
         _prevMouse = _curMouse;

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
   }
}
