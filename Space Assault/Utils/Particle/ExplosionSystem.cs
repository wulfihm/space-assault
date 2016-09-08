﻿using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace SpaceAssault.Utils.Particle
{
    class ExplosionSystem
    {
        public ParticleSystem _system;
        public ParticleSystem _system2;
        public Vector3 _position;
        public int _state;
        private float _duration;
        private float _count;
        private float _radius;
        private bool _advancedeffects;


        // Random number generator
        Random random = new Random();

        public ExplosionSystem(ParticleSystem particleSettings, Vector3 position, float duration)
        {
            _system = particleSettings;
            _position = position;
            _duration = duration;
            _count = 0;
            _radius = 0;
        }

        public ExplosionSystem(ParticleSystem particleSettings, ParticleSystem particleSettings2, Vector3 position, float duration, float radius)
        {
            _system = particleSettings;
            _system2 = particleSettings2;
            _position = position;
            _duration = duration;
            _radius = radius;
            _count = 0;
        }

        public ExplosionSystem(ParticleSystem particleSettings, ParticleSystem particleSettings2, Vector3 position, float duration, float radius, bool advancedeffects)
        {
            _system = particleSettings;
            _system2 = particleSettings2;
            _position = position;
            _duration = duration;
            _count = 0;
            _radius = radius;
            _advancedeffects = advancedeffects;
        }

        public void Update(GameTime gameTime)
        {
            switch(_state)
            {
                case 0:
                    _system.AddParticle(_position, Vector3.Zero);
                    if (_count > _duration)
                        _state = 1;
                    break;
                case 1:
                    if (_count > _duration + _radius)
                        _state = 2;
                    break;
            }

            if (_state != 2)
            { 
                _system.Update(gameTime);
                if (_system2 != null)
                    CircleExplosion(gameTime);
                if (_advancedeffects)
                    ExplosionField();

                _count++;
            }

        }

        public void Draw()
        {
            if (_state != 2)
            {
                _system.Draw();
                if (_system2 != null)
                    _system2.Draw();
            }
        }


        Vector3 RandomPointOnCircle()
        {
            double angle = random.NextDouble() * Math.PI * 2;

            float x = (float)Math.Cos(angle);
            float y = (float)Math.Sin(angle);

            return new Vector3(_position.X + x * _count, _position.Y, _position.Z + y * _count);
        }

        //#################################
        // Helper Update - Circle
        //#################################
        void CircleExplosion(GameTime gameTime)
        {
            const int borderParticlesPerFrame = 50;

            for (int i = 0; i < borderParticlesPerFrame; i++)
            {
                _system2.AddParticle(RandomPointOnCircle(), Vector3.Zero);
            }
            _system2.Update(gameTime);
        }

        //#################################
        // Helper ExplosionField
        //#################################
        void ExplosionField()
        {
            float angle = 0.0f;
            for (int i = 0; i <= _radius; i += 25)
            {
                while (angle < 2 * Math.PI)
                {
                    _system.AddParticle(_position + new Vector3(i * (float)Math.Cos(angle), 0, i * (float)Math.Sin(angle)), Vector3.Zero);
                    angle += 0.2f;
                }
                angle = 0;
            }
        }

    }
}
