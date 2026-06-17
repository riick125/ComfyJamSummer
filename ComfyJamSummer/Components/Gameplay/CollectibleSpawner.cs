using ComfyJamSummer.Entities;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using Microsoft.Xna.Framework;
using Nez;
using System;

namespace ComfyJamSummer.Components.Gameplay
{
    public class CollectibleSpawner : BaseComponent
    {
        public CollectibleSpawner(GameManager manager, Prefabs prefabs) : base(manager, prefabs)
        {
        }

        public void Spawn(CollectibleType type, int quantity, Vector2 position, Vector2 fallDestination)
        {
            if (!Validate(this.Entity))
                return;

            var island = UtilHelper.GetEntity<Island>();

            if (island == null)
                return;

            for (int i = 0; i < quantity; i++)
            {
                var chaos = (float)Nez.Random.RNG.NextDouble() * 4f - 2f;
                var spiralFactor = MathF.Sin(i * 0.5f) * 15f;
                var wiggleX = Mathf.Sin(i * 0.8f) * 5f;
                var wiggleY = Mathf.Cos(i * 0.3f) * 8f;

                var radius = (float)Nez.Random.RNG.NextDouble() * 5f;
                var angle = (float)Nez.Random.RNG.NextDouble() * MathHelper.TwoPi;

                var offset = new Vector2(
                    Mathf.Cos(angle + spiralFactor) * radius + wiggleX + chaos * 4f,
                    Mathf.Sin(angle * 1.3f) * (radius * 0.7f) + wiggleY + chaos * 2f
                );

                offset.Y -= Math.Abs(Mathf.Sin(i)) * 8f;

                _scene.AddEntity(_prefabs.GetCollectible(_prefabs.InteractableConfig.CloneNpc(island.Id, position + offset), type, fallDestination + offset));

                offset = default;
            }
        }
    }
}