using ComfyJamSummer.Components.Gameplay;
using ComfyJamSummer.Entities;
using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using Microsoft.Xna.Framework;
using Nez;
using System;
using System.Collections.Generic;

namespace ComfyJamSummer.Components.Visuals
{
    public class WaterBackground : BaseComponent
    {
        private List<Animated> _tiles;

        public WaterBackground(GameManager manager, Prefabs prefabs) : base(manager, prefabs)
        {
            _tiles = new List<Animated>();

            var island = UtilHelper.GetEntity<Island>();

            if (island == null)
            {
                return;
            }

            var multiplier = 1.5f;

            var tileSize = 16;

            var centerPos = island.CenterPosition();

            var minX = centerPos.X - island.Width * multiplier;
            var maxX = centerPos.X + island.Width * multiplier;

            var minY = centerPos.Y - island.Height * multiplier;
            var maxY = centerPos.Y + island.Height * multiplier;

            var distanceX = maxX - minX;
            var distanceY = maxY - minY;

            var horizontalTilesQuantity = (int)Math.Ceiling(distanceX / tileSize);
            var verticalTilesQuantity = (int)Math.Ceiling(distanceY / tileSize);

            var matrix = new int[verticalTilesQuantity, horizontalTilesQuantity];

            var rows = matrix.GetLength(0);
            var columns = matrix.GetLength(1);

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    var position = new Vector2(col * tileSize, row * tileSize);

                    var tile = Core.Scene.AddEntity(prefabs.GetWaterTile(position));

                    tile.Position += new Vector2(minX, minY);

                    _tiles.Add(tile);

                    tile.Animator.Play("Idle");

                    tile.Animator.DebugRenderEnabled = false;
                }
            }
        }
    }
}
