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

        private Station _station;
        List<Label> Labels = new List<Label>();
        List<Bar> Bars = new List<Bar>();
        private UIItem _shields = new UIItem();
        private Dialog _upgradeDialog;
        private Dialog _upgradeVincinityDialog;
        private Dialog _scoreDialog;

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
            //UI

            //Shield
            _shields.LoadContent("Images/UI/shield_ui");

            //Bars
            Bars.Add(new Bar(new Rectangle(new Point(50, Global.GraphicsManager.GraphicsDevice.Viewport.Height - 80), new Point(300, 60)), Color.Red, droneFleet.GetActiveDrone()._maxHealth));
            Bars.Add(new Bar(new Rectangle(new Point(50, Global.GraphicsManager.GraphicsDevice.Viewport.Height - 90), new Point(300, 60)), Color.Blue, droneFleet.GetActiveDrone()._maxShield));
            Bars.Add(new Bar(new Rectangle(new Point(Global.GraphicsManager.GraphicsDevice.Viewport.Width - 400, Global.GraphicsManager.GraphicsDevice.Viewport.Height - 750), new Point(300, 60)), Color.Green, _station._maxhealth));
            Bars.Add(new Bar(new Rectangle(new Point(Global.GraphicsManager.GraphicsDevice.Viewport.Width - 400, Global.GraphicsManager.GraphicsDevice.Viewport.Height - 740), new Point(300, 60)), Color.White,_station._maxShield));
            foreach (var bar in Bars)
            {
                bar.LoadContent();
            }

            //Dialogs
            _upgradeDialog = new Dialog(Global.GraphicsManager.GraphicsDevice.Viewport.Width / 2-150, Global.GraphicsManager.GraphicsDevice.Viewport.Height - 750, 32, 336, 8, false);
            _upgradeVincinityDialog = new Dialog(Global.GraphicsManager.GraphicsDevice.Viewport.Width / 2 - 150, Global.GraphicsManager.GraphicsDevice.Viewport.Height - 702, 32, 336, 8, false);
            _scoreDialog = new Dialog(50, Global.GraphicsManager.GraphicsDevice.Viewport.Height - 750, 32, 192, 8, false);
            _upgradeDialog.LoadContent();
            _scoreDialog.LoadContent();
            _upgradeVincinityDialog.LoadContent();
        }
        //#################################
        // Draw
        //#################################
        public void Draw(DroneBuilder droneFleet)
        {
            _shields.Draw(new Point(50, Global.GraphicsManager.GraphicsDevice.Viewport.Height - 130), droneFleet._armor, new Color(1f, 1f, 1f, 0.5f));

            Bars[0].Draw(droneFleet.GetActiveDrone()._health, droneFleet.GetActiveDrone()._maxHealth);
            Bars[1].Draw(droneFleet.GetActiveDrone()._shield, droneFleet.GetActiveDrone()._maxShield);
            Bars[2].Draw(_station._health, _station._maxhealth);
            Bars[3].Draw(_station._shield,_station._maxShield);

            _scoreDialog.Draw("Score: " + Global.HighScorePoints.ToString());

            if (droneFleet._updatePoints > 0)
            {
                _upgradeDialog.Draw("Upgrade available: " + droneFleet._updatePoints.ToString());

                if ((Vector3.Distance(this._station.Position, droneFleet.GetActiveDrone().Position) - GameplayScreen._stationHeight) < 150)
                    _upgradeVincinityDialog.Draw("Press B for Shop!");
            }         
        }
    }
}
