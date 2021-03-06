﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Utils;
using SpaceAssault.Utils.Particle;
using SpaceAssault.Utils.Particle.Settings;

namespace SpaceAssault.Entities
{

    public class Bullet : AEntity
    {
        private Vector3 _moveDirection;
        private float _moveSpeed;
        public int _bulletLifeTime;
        protected int _makeDmg;
        protected bool _canDmgStation;
        public BulletType _bulletType;

        public enum BulletType
        {
            YellowLazer,
            BlueLazer,
            PhotonBomb,
            BigJoe,
            EnemyLazer,
            BossGun
        }

        public Bullet(BulletType bulletType, Model model, float moveSpeed, bool canDamageStation, int damage, Vector3 position, Vector3 direction)
        {
            Model = model;
            Position = position;
            Spheres = Collider3D.UpdateBoundingSphere(this);
            RotationMatrix = Matrix.CreateWorld(position, direction, Vector3.Up);

            _bulletType = bulletType;
            _makeDmg = damage;
            _canDmgStation = canDamageStation;
            _bulletLifeTime = 25000;
            _moveDirection = direction;
            _moveSpeed = moveSpeed;
            if (bulletType == BulletType.BigJoe)
                _trail = new Trail(new BombTrailSettings());
            if (bulletType == BulletType.PhotonBomb)
                _trail = new Trail(new BomberBombTrailSettings());
            if (bulletType == BulletType.BossGun)
                _trail = new Trail(new BomberBombTrailSettings());

        }


        public override void Update(GameTime gameTime)
        {
            Spheres = Collider3D.UpdateBoundingSphere(this);
            Position -= _moveDirection * _moveSpeed;
            _bulletLifeTime -= gameTime.ElapsedGameTime.Milliseconds;
        }

        public override void LoadContent()
        {
            throw new NotImplementedException();
        }

        public void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public int makeDmg
        {
            get { return _makeDmg; }
        }

        public bool CanDamageStation
        {
            get { return _canDmgStation; }
        }
    }
}
