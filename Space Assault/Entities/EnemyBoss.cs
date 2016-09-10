﻿using IrrKlang;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Utils;
using SpaceAssault.Utils.Particle;
using SpaceAssault.Utils.Particle.Settings;

namespace SpaceAssault.Entities
{
    class EnemyBoss : AEnemys
    {
        //The Boss has 4 Seperate AttackTowers who need to be destroyed seperatly
        internal class AttackTower : AEnemys
        {
            public AttackTower(Vector3 position, int health, int damage, Weapon gun)
            {
                Health = health;
                SpawnPos = position;
                Position = position;
                Gun = gun;
            }

            public override void Update(GameTime gameTime)
            {
                if (Health <= 0)
                    IsDead = true;
                Spheres = Collider3D.UpdateBoundingSphere(this);
            }

            public override void LoadContent()
            {
                Model = Global.ContentManager.Load<Model>("Models/enemy_bomber");
                Spheres = Collider3D.UpdateBoundingSphere(this);
                Gun.LoadContent();
            }
        }
        internal struct Vec3Rectangle
        {
            private Vector3 _center;
            private float _width;
            private float _height;
           
            public Vec3Rectangle(Vector3 center, float width, float height)
            {
                _center = center;
                _height = height;
                _width = width;
            }
            public Vector3 EdgeTopRight
            {
                get { return new Vector3(_center.X + _width/2, _center.Y, _center.Z + _height / 2); }
            }
            public Vector3 EdgeTopLeft
            {
                get { return new Vector3(_center.X - _width / 2, _center.Y, _center.Z + _height / 2); }
            }
            public Vector3 EdgeBottomRight
            {
                get { return new Vector3(_center.X + _width / 2, _center.Y, _center.Z - _height / 2); }
            }
            public Vector3 EdgeBottomLeft
            {
                get { return new Vector3(_center.X - _width / 2, _center.Y, _center.Z - _height / 2); }
            }
            public Vector3 Center
            {
                get { return _center; }
                set { _center = value; }
            }
        }

        private AttackTower _tower1;
        private AttackTower _tower2;
        private AttackTower _tower3;
        private AttackTower _tower4;
        private Vec3Rectangle _compositionRec;

        public EnemyBoss(Vector3 spawnposition,int shipHealth, int damage )
        {
            SpawnPos = spawnposition;
            Position = spawnposition;
            _trail = new Trail(new EnemyTrailSettings());
            RotationMatrix = Matrix.Identity;
            MoveSpeedForward = 0.5f;
            TurnSpeed = 2.0f;

            //BIGJOE Rocket for Body
            Gun = new Weapon(1000);
            _compositionRec = new Vec3Rectangle(spawnposition, 100, 100);
            Position = _compositionRec.Center;
            Health = shipHealth;
            _tower1 = new AttackTower(_compositionRec.EdgeBottomLeft, 600, 100, new Weapon(400));
            _tower2 = new AttackTower(_compositionRec.EdgeBottomRight, 600, 100, new Weapon(400));
            _tower3 = new AttackTower(_compositionRec.EdgeTopLeft, 600, 100, new Weapon(2000));
            _tower4 = new AttackTower(_compositionRec.EdgeTopRight, 600, 100, new Weapon(2000));
        }


        public override void Update(GameTime gameTime)
        {
            if (Health <= 0)
                IsDead = true;
            Spheres = Collider3D.UpdateBoundingSphere(this);

            _compositionRec.Center = Position;
            _tower1.Position = _compositionRec.EdgeBottomLeft;
            _tower2.Position = _compositionRec.EdgeBottomRight;
            _tower3.Position = _compositionRec.EdgeTopLeft;
            _tower4.Position = _compositionRec.EdgeTopRight;
        }

        public override void LoadContent()
        {
            Model = Global.ContentManager.Load<Model>("Models/enemy_bomber");           
            Spheres = Collider3D.UpdateBoundingSphere(this);

            _tower1.LoadContent();
            _tower2.LoadContent();
            _tower3.LoadContent();
            _tower4.LoadContent();
        }
    }
}
