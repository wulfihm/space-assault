﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Entities;
using SpaceAssault.Entities.Weapon;

namespace SpaceAssault.Utils
{
    class FleetBuilder
    {
        public List<AEnemys> _enemyShips;
        public List<Bullet> _bulletList;
        private List<Bullet> _removeBulletList;
        private List<AEnemys> _addList;
        private TimeSpan _lastChunkTime;
        private Random _rand;
        public String _whereTheyCome;

        protected TimeSpan _globalTimeSpan;

        public FleetBuilder()
        {
            _enemyShips = new List<AEnemys>();
            _addList = new List<AEnemys>();
            _bulletList = new List<Bullet>();
            _rand = new Random();
            _removeBulletList = new List<Bullet>();
        }

        public void Update(GameTime gameTime, Vector3 targetPosition)
        {
            // updating bullets
            _globalTimeSpan = gameTime.TotalGameTime;
            foreach (Bullet bullet in _bulletList)
            {
                bullet.Update(gameTime);
                if (bullet._bulletlife < 0)
                {
                    _removeBulletList.Add(bullet);
                }
            }
            foreach (Bullet bullet in _removeBulletList)
            {
                _bulletList.Remove(bullet);
            }
            _removeBulletList.Clear();

            // updating every ship
            foreach (var ship in _enemyShips)
            {
                ship.Intelligence(targetPosition, ref _bulletList);
                ship.Update(gameTime);
            }
            
            // adding fleets
            if (gameTime.TotalGameTime > (_lastChunkTime.Add(TimeSpan.FromMilliseconds(2000))))
            {
                //maximale Anzahl an Flotten
                if(this._enemyShips.Count<6)
                    Formation(targetPosition);
                _lastChunkTime = gameTime.TotalGameTime;
            }
        }
        public void Draw()
        {
            foreach (Bullet bullet in _bulletList)
            {
                bullet.Draw();
            }

            foreach (var ship in _enemyShips)
            {
                ship.Draw();
            }
        }
        //drone position damit alles außerhalb des Bilschirms spawned
        private void Formation(Vector3 DronePosition)
        {
            int zdist;
            int xoffset;
            for (int i = 0; i < 1; i++)
            {
                int Corner = _rand.Next(1, 4);
                Vector3 position = new Vector3();
                //Corner = 3;
                switch (Corner)
                {
                    case 1:
                        //unten rechts
                        zdist = _rand.Next(0, 400);
                        xoffset = _rand.Next(-35, 35);


                        position.X = DronePosition.X + 350 + xoffset;
                        position.Z = DronePosition.Z + zdist;
                        position.Y = 0f;
                        _whereTheyCome = "Neue Flotte von unten rechts";
                        break;
                    case 2:
                        //oben rechts
                        zdist = _rand.Next(-400, 0);
                        xoffset = _rand.Next(-35, 35);


                        position.X = DronePosition.X + 350 + xoffset;
                        position.Z = DronePosition.Z + zdist;
                        position.Y = 0f;
                        _whereTheyCome = "Neue Flotte von oben rechts";
                        break;
                    case 3:
                        //unten links
                        zdist = _rand.Next(0, 400);
                        xoffset = _rand.Next(-35, 35);


                        position.X = DronePosition.X - 350 + xoffset;
                        position.Z = DronePosition.Z + zdist;
                        position.Y = 0f;
                        _whereTheyCome = "Neue Flotte von unten links";
                        break;
                    case 4:
                        //oben links
                        zdist = _rand.Next(-400, 0);
                        xoffset = _rand.Next(-35, 35);


                        position.X = DronePosition.X - 350 + xoffset;
                        position.Z = DronePosition.Z + zdist;
                        position.Y = 0f;
                        _whereTheyCome = "Neue Flotte von oben links";
                        break;
                }
                Console.WriteLine(_whereTheyCome);

                int formationchoice = _rand.Next(1, 4);
                //Console.WriteLine(formationchoice+"<<<<<<<<<<<<<<");
                switch (formationchoice)
                {
                    case 1:
                        //Console.WriteLine("Case 11111111111111111111111");
                        BomberDouble(position);
                        break;
                    case 2:
                        //Console.WriteLine("Case 222222222222222222222222");
                        Arrow(position);
                        break;
                    case 3:
                        //Console.WriteLine("Case 333333333333333333333333");
                        FighterTriangle(position);
                        break;
                }
              
            }


        }

        private void FighterTriangle(Vector3 SpawnPosition)
        {
            int Rand= _rand.Next(40, 60);
            int zRand = _rand.Next(40, 60);
            EnemyFighter ship = new EnemyFighter(SpawnPosition+new Vector3(SpawnPosition.X+Rand, 0, SpawnPosition.Z + Rand));
            EnemyFighter ship2 = new EnemyFighter(SpawnPosition + new Vector3(SpawnPosition.X, 0, SpawnPosition.Z + Rand));
            EnemyFighter ship3 = new EnemyFighter(SpawnPosition + new Vector3(SpawnPosition.X+Rand, 0, SpawnPosition.Z));

            ship.Initialize();
            ship.LoadContent();
            _addList.Add(ship);
            
            ship2.Initialize();
            ship2.LoadContent();
            _addList.Add(ship2);
            //Console.WriteLine("WRONG PLACE");
            ship3.Initialize();
            ship3.LoadContent();
            _addList.Add(ship3);

            _enemyShips.AddRange(_addList);
            _addList.Clear();
        }
        private void Arrow(Vector3 SpawnPosition)
        {
            int Rand = _rand.Next(40, 60);
            int zRand = _rand.Next(40, 60);
            EnemyBomber ship = new EnemyBomber(SpawnPosition + new Vector3(SpawnPosition.X + Rand, 0, SpawnPosition.Z + Rand));
            EnemyFighter ship2 = new EnemyFighter(SpawnPosition + new Vector3(SpawnPosition.X, 0, SpawnPosition.Z + Rand));
            EnemyFighter ship3 = new EnemyFighter(SpawnPosition + new Vector3(SpawnPosition.X + Rand, 0, SpawnPosition.Z));
            EnemyFighter ship4 = new EnemyFighter(SpawnPosition + new Vector3(SpawnPosition.X + Rand, 0, SpawnPosition.Z+2*Rand));
          
            //EnemyFighter ship4 = new EnemyFighter(SpawnPosition+ new Vector3(0,0,0)); //funktioniert damit nicht  WAT
        
            ship.Initialize();
            ship.LoadContent();
            _addList.Add(ship);

            ship2.Initialize();
            ship2.LoadContent();
            _addList.Add(ship2);

            ship3.Initialize();
            ship3.LoadContent();
            _addList.Add(ship3);

            ship4.Initialize();
            ship4.LoadContent();
            _addList.Add(ship4);
      

            _enemyShips.AddRange(_addList);
            _addList.Clear();
        }
        private void BomberDouble(Vector3 SpawnPosition)
        {
            int Rand = _rand.Next(40, 60);
            int zRand = _rand.Next(40, 60);
            EnemyBomber ship = new EnemyBomber(SpawnPosition + new Vector3(SpawnPosition.X + Rand, 0, SpawnPosition.Z + Rand));
            EnemyBomber ship2 = new EnemyBomber(SpawnPosition + new Vector3(SpawnPosition.X, 0, SpawnPosition.Z + Rand));
            

            ship.Initialize();
            ship.LoadContent();
            _addList.Add(ship);

            ship2.Initialize();
            ship2.LoadContent();
            _addList.Add(ship2);


            _enemyShips.AddRange(_addList);
            _addList.Clear();
        }
    }
}
