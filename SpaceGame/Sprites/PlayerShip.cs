﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using SpaceGame.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceGame.Sprites
{
    public class PlayerShip : Spaceship
    {
        private bool _holdingInfoToggle = false;
        private bool _showInfo = false;

        public PlayerShip(Vector2 position, Texture2D texture, Texture2D wingTexture) 
            : base(position, texture, wingTexture)
        {
        }

        public void SetAccelerations(float t)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            bool moving = false;

            if (keyboardState.IsKeyDown(Keys.A)) 
            {
                angularThrust = -maxAngularThrust;
                moving = true;
            }
            else if (keyboardState.IsKeyDown(Keys.D))
            {
                angularThrust = maxAngularThrust;
                moving = true;
            }
            if (keyboardState.IsKeyDown(Keys.W))
            {
                RotateWings(t);
                linearThrust = maxLinearThrust;
                moving = true;
            }
            else if (keyboardState.IsKeyDown(Keys.S))
            {
                RotateWings(t);
                linearThrust = -maxLinearThrust;
                moving = true;
            }
            if (!moving)
            {
                angularThrust = 0f;
                linearThrust = 0f;
            }
            if (keyboardState.IsKeyDown(Keys.Tab))
            {
                if (!_holdingInfoToggle)
                    _showInfo = !_showInfo;
                _holdingInfoToggle = true;
            }
            else
                _holdingInfoToggle = false;
        }

        public void RotateWings(float t)
        {
            var deltaAngle = Helper.SimplifyRadians(wingRotation - rotation);
            if (deltaAngle < 0.1f) return;
            if (deltaAngle >= Math.PI) wingRotation = Helper.SimplifyRadians(wingRotation + wingAngularSpeed * t);
            else wingRotation = Helper.SimplifyRadians(wingRotation - wingAngularSpeed * t);
        }

        public override void Update(GameTime gameTime)
        {
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;
            SetAccelerations(t);
            Move(t);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_showInfo)
            {
                spriteBatch.DrawLine(position + facing * 20, position + facing * 40, Color.Green);
                spriteBatch.DrawLine(position + direction * 20, position + direction * (20 + (linearVelocity.Length()) * 20 / maxLinearVelocity), Color.Blue);
            }                                                   
            base.Draw(spriteBatch);
        }
    }
}
