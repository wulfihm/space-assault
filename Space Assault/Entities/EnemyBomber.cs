﻿using IrrKlang;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Utils;
using SpaceAssault.Utils.Particle;
using SpaceAssault.Utils.Particle.Settings;

namespace SpaceAssault.Entities
{
    class EnemyBomber : AEnemys
    {
        public EnemyBomber(Vector3 position)
        {
            SpawnPos = position;
            Position = position;

            _trail = new Trail(new EnemyBomberTrailSettings());

            RotationMatrix = Matrix.Identity;

            MoveSpeedForward = 0.8f;
            TurnSpeed = 2.0f;

            //_moveSpeedBackward = -0.5f;
            KillMoney = 100;
            Health = 40;

            Gun = new Weapon(3000d);
            gunMakeDmg = 500;
        }

        public override void LoadContent()
        {
            Model = Global.ContentManager.Load<Model>("Models/enemy_bomber");
            Spheres = Collider3D.UpdateBoundingSphere(this);
            Gun.LoadContent();
            HitSound = Engine.AddSoundSourceFromFile("Content/Media/Effects/Objects/GetHitShips.wav", StreamMode.AutoDetect, true);
        }

        public override void Update(GameTime gameTime)
        {
            if (Health <= 0) IsDead = true;

            Spheres = Collider3D.UpdateBoundingSphere(this);
            //TODO: health, armor update
        }
    }
}

