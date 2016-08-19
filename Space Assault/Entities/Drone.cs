﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using SpaceAssault.Utils;
using SpaceAssault.Utils.Particle;
using SpaceAssault.Utils.Particle.Settings;

/// <summary>
///  Movement, Schießen, Health, Sterben, neu Spawnen.
/// </summary>
namespace SpaceAssault.Entities
{
    class Drone : AEntity
    {
        private Vector3 _spawnPos;
        private float _turnSpeed;
        private float _moveSpeedForward;
        private float _moveSpeedBackward;
        private float _moveSpeedLeft;
        private float _moveSpeedRight;
        private float _moveSpeedModifier;
        private float _moveSpeedModifierSideways;

        public int _maxHealth;
        public int _health;
        public int _armor;
        public int _makeDmg;

        //shieldLogic
        public int _maxShield;
        public int _shield;
        private int _shieldpast;
        private TimeSpan _shieldrefreshdelay;
        private bool _wasDamaged = false;


        private bool _alternatingGunLogic = false;
        private bool _isNotDead;


        public Weapon Gun;

        //used for scaling all speed values beside turnSpeed;
        private float _speedScaling = 2f;

        public Drone(Vector3 position, int makeDmg, int maxHealth, int armor, int maxShield)
        {
            _spawnPos = position;
            Position = position;
            _makeDmg = makeDmg;
            _maxHealth = maxHealth;
            _armor = armor;
            _maxShield = maxShield;
            trail = new List<Trail>();
            TrailParticles = new TrailParticleSystem();
            trail.Add(new Trail(TrailParticles));

        }

        public override void Initialize()
        {
            RotationMatrix = Matrix.Identity;
            _turnSpeed = 10.0f;
            _moveSpeedForward = 1.0f * _speedScaling;
            _moveSpeedBackward = -1.0f * _speedScaling;
            _moveSpeedLeft = 1.0f * _speedScaling;
            _moveSpeedRight = -1.0f * _speedScaling;
            _shieldpast = _maxShield;
            _shield = _maxShield;
            _health = _maxHealth;
            _isNotDead = true;

            Gun = new Weapon(200d);
            Gun.Initialize();
        }

        public void Reset()
        {
            RotationMatrix = Matrix.Identity;
            _turnSpeed = 10.0f;
            _moveSpeedForward = 1.0f * _speedScaling;
            _moveSpeedBackward = -1.0f * _speedScaling;
            _moveSpeedLeft = 1.0f * _speedScaling;
            _moveSpeedRight = -1.0f * _speedScaling;
            _health = _maxHealth;
            _shield = _maxShield;

            _isNotDead = true;
            _moveSpeedModifier = 0;
            _moveSpeedModifierSideways = 0;
            Position = _spawnPos;
        }

        public override void LoadContent()
        {
            Model = Global.ContentManager.Load<Model>("Models/drone");
            Spheres = Collider3D.UpdateBoundingSphere(this);
            //RotationMatrix = Matrix.CreateRotationX();
        }

        public override void Update(GameTime gameTime)
        {
            //Console.WriteLine("Test:"+MaxHealth+" "+Health+" "+Armor);
            Spheres = Collider3D.UpdateBoundingSphere(this);

            if (gameTime.TotalGameTime > (_shieldrefreshdelay.Add(TimeSpan.FromSeconds(3))))
            {

                if (_shield == _shieldpast)
                    _wasDamaged = false;
                _shieldpast = _shield;
                _shieldrefreshdelay = gameTime.TotalGameTime;
            }
            if (_wasDamaged == false && _shield < _maxShield)
                _shield += 1;

            if (_health <= 0) IsNotDead = false;
        }

        public bool IsNotDead
        {
            get { return _isNotDead; }
            protected set { _isNotDead = value; }
        }

        public void getHit(int howMuch)
        {
            _wasDamaged = true;
            if (_shield >= 0)
                _shield -= howMuch;
            else
            {
                if (howMuch > _armor)
                    _health -= (howMuch - _armor);
            }
        }
        public void HandleInput(GameTime gameTime, Weapon.BulletType curBullet, ref List<Bullet> bulletList)
        {
            /// <summary>
            /// handling rotation of the drone
            /// </summary>

            //projection of mouse from screen unto the 2d plane in the game http://stackoverflow.com/questions/11503226/c-sharp-xna-mouse-position-projected-to-3d-plane
            Vector3 nearScreenPoint = new Vector3(MouseHandler.Position, 0);
            Vector3 farScreenPoint = new Vector3(MouseHandler.Position, 1);
            Vector3 nearWorldPoint = Global.GraphicsManager.GraphicsDevice.Viewport.Unproject(nearScreenPoint, Global.Camera.ProjectionMatrix, Global.Camera.ViewMatrix, Matrix.Identity);
            Vector3 farWorldPoint = Global.GraphicsManager.GraphicsDevice.Viewport.Unproject(farScreenPoint, Global.Camera.ProjectionMatrix, Global.Camera.ViewMatrix, Matrix.Identity);
            Vector3 direction = farWorldPoint - nearWorldPoint;
            float zFactor = -nearWorldPoint.Y / direction.Y;
            Vector3 zeroWorldPoint = nearWorldPoint + direction * zFactor;

            Vector3 screenDirection = this.Position - zeroWorldPoint;
            screenDirection.Normalize();
            float worldDirection;
            for (float i = 0.5f; i < _turnSpeed; i++)
            {
                worldDirection = RotationMatrix.Forward.Z * screenDirection.X - RotationMatrix.Forward.X * screenDirection.Z;
                if (worldDirection > 0.01)
                {
                    //turn left
                    RotationMatrix *= Matrix.CreateRotationY(MathHelper.ToRadians(0.5f));
                }
                else if (worldDirection < -0.01)
                {
                    //turn right
                    RotationMatrix *= Matrix.CreateRotationY(MathHelper.ToRadians(-0.5f));
                }
            }

            /// <summary>
            /// foward & backward movement
            /// </summary>
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                //forward
                if (_moveSpeedModifier < _moveSpeedForward) _moveSpeedModifier += 0.04f;
                else _moveSpeedModifier = _moveSpeedForward;
                //Position -= new Vector3(0, 0, 1) * _moveSpeedModifier;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                //backward
                if (_moveSpeedModifier > _moveSpeedBackward) _moveSpeedModifier -= 0.04f;
                else _moveSpeedModifier = _moveSpeedBackward;
                //Position -= new Vector3(0, 0, 1) * _moveSpeedModifier;
            }
            else if (_moveSpeedModifier > 0.02f)
            {
                //forward slowing down
                _moveSpeedModifier -= 0.02f;
            }
            else if (_moveSpeedModifier < -0.02f)
            {
                //backward slowing down
                _moveSpeedModifier += 0.02f;
            }
            else
            {
                _moveSpeedModifier = 0.0f;
            }
            Position -= new Vector3(0, 0, 1) * _moveSpeedModifier;

            /// <summary>
            /// left & right movement
            /// </summary>
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                //left
                if (_moveSpeedModifierSideways < _moveSpeedLeft) _moveSpeedModifierSideways += 0.04f;
                else _moveSpeedModifierSideways = _moveSpeedLeft;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                //right
                if (_moveSpeedModifierSideways > _moveSpeedRight) _moveSpeedModifierSideways -= 0.04f;
                else _moveSpeedModifierSideways = _moveSpeedRight;
            }
            else if (_moveSpeedModifierSideways > 0.02f)
            {
                //left slowing down
                _moveSpeedModifierSideways -= 0.02f;
            }
            else if (_moveSpeedModifierSideways < -0.02f)
            {
                //right slowing down
                _moveSpeedModifierSideways += 0.02f;
            }
            else
            {
                _moveSpeedModifierSideways = 0.0f;
            }
            Position -= new Vector3(1, 0, 0) * _moveSpeedModifierSideways;

            /// <summary>
            /// shooting the gun
            /// </summary>
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (_alternatingGunLogic && Gun.Shoot(gameTime, curBullet, 10, Position - RotationMatrix.Left * 3.6f - RotationMatrix.Forward * 11.0f, RotationMatrix, ref bulletList))
                {
                    _alternatingGunLogic = false;
                }
                else if (Gun.Shoot(gameTime, curBullet, 10, Position - RotationMatrix.Right * 3.6f - RotationMatrix.Forward * 11.0f, RotationMatrix, ref bulletList))
                {
                    _alternatingGunLogic = true;
                }
            }

            if (Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                // TODO: New BulletType for Secondary Fire
                Gun.Shoot(gameTime, Weapon.BulletType.PhotonBomb, 10, Position - RotationMatrix.Left * 3.6f - RotationMatrix.Forward * 11.0f, RotationMatrix, ref bulletList);
            }
        }

        public override void Draw()
        {
            if (IsNotDead)
            {
                foreach (var mesh in Model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();
                        effect.PreferPerPixelLighting = true;
                        effect.World = RotationMatrix * Matrix.CreateWorld(Position, Vector3.Forward, Vector3.Up);
                        effect.View = Global.Camera.ViewMatrix;
                        effect.Projection = Global.Camera.ProjectionMatrix;
                    }
                    mesh.Draw();
                }
            }

        }
    }
}
