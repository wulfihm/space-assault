﻿using Microsoft.Xna.Framework;
using SpaceAssault.Utils;
using System.Collections.Generic;
using SpaceAssault.Entities;
using SpaceAssault.Screens.UI;

namespace SpaceAssault.Screens
{
    class InGameOverlay
    {

        //#################################
        // Variables
        //#################################
        List<Label> Labels = new List<Label>();
        private Drone _drone;
        private Station _station;
        List<Bar> Bars = new List<Bar>();
        private UIItem _shields = new UIItem();

        //#################################
        // Constructor
        //#################################
        public InGameOverlay(Station _station)
        {
            this._station = _station;
        }

        //#################################
        // LoadContent - Function
        //#################################
        public void LoadContent(DroneBuilder droneFleet)
        {
            /*//UI
            Labels.Add(new Label("gamefont", "Health: ", 50, Global.GraphicsManager.PreferredBackBufferHeight - 50, Color.White));
            Labels.Add(new Label("gamefont", "StationHealth: ", Global.GraphicsManager.PreferredBackBufferWidth - 200, Global.GraphicsManager.PreferredBackBufferHeight - 50, Color.White));
            */

            _shields.LoadContent("Images/UI/shield_ui");
            Labels.Add(new Label("gamefont", "Upgrade available: ", Global.GraphicsManager.PreferredBackBufferWidth/2, 50, Color.GreenYellow));
            Labels.Add(new Label("gamefont", "Score: ", 50, Global.GraphicsManager.PreferredBackBufferHeight - 750, Color.DarkSalmon));
            Labels.Add(new Label("gamefont", "Press B for Shop!", Global.GraphicsManager.PreferredBackBufferWidth/2, 70, Color.DarkSalmon));
            Bars.Add(new Bar(new Rectangle(new Point(50, Global.GraphicsManager.PreferredBackBufferHeight - 80), new Point(300, 60)), Color.Red, droneFleet.GetActiveDrone()._maxHealth));
            Bars.Add(new Bar(new Rectangle(new Point(50, Global.GraphicsManager.PreferredBackBufferHeight - 90), new Point(300, 60)), Color.Blue, droneFleet.GetActiveDrone()._maxShield));
            Bars.Add(new Bar(new Rectangle(new Point(Global.GraphicsManager.PreferredBackBufferWidth-400, Global.GraphicsManager.PreferredBackBufferHeight - 750), new Point(300, 60)), Color.Green, 10000));

            foreach (var bar in Bars)
            {
                bar.LoadContent();
            }
        }
        //#################################
        // Draw
        //#################################
        public void Draw(DroneBuilder droneFleet)
        {
            
            Labels[1].Draw(Global.HighScorePoints);

            if (droneFleet._updatePoints > 0)
            {
                Labels[0].Draw(droneFleet._updatePoints);

                if ((Vector3.Distance(this._station.Position, this._drone.Position) - GameplayScreen._stationHeight) < 150)
                    Labels[2].Draw();
            }
            

            Bars[0].Draw(droneFleet.GetActiveDrone()._health, droneFleet.GetActiveDrone()._maxHealth);
            Bars[1].Draw(droneFleet.GetActiveDrone()._shield, droneFleet.GetActiveDrone()._maxShield);
            Bars[2].Draw(_station._health, 10000);
            _shields.Draw(new Point(50, Global.GraphicsManager.PreferredBackBufferHeight - 130), droneFleet._armor,new Color(1f,1f,1f,0.5f));
        }

    }
}
