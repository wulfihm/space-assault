﻿
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Space_Assault.Utils;

namespace Space_Assault.Entities
{
    public abstract  class AEntity
    {
        protected Model Model;
        public Vector3 Position;
        protected Matrix RotationMatrix;

        public abstract void Update(GameTime gameTime);

        public abstract void LoadContent(ContentManager cm);

        public abstract void Initialize();

        public void Draw(Camera camera)
        {
            foreach (var mesh in Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.World = RotationMatrix*Matrix.CreateWorld(Position, Vector3.Forward, Vector3.Up);
                    effect.View = camera.ViewMatrix;
                    effect.Projection = camera.ProjectionMatrix;
                }
                mesh.Draw();
            }
        }
    }
}
