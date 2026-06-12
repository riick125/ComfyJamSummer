using ComfyJamSummer.Enums;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using Microsoft.Xna.Framework;

namespace ComfyJamSummer.Components.Gameplay
{
    public class CollectibleSpawner : BaseComponent
    {
        public CollectibleSpawner(GameManager manager, Prefabs prefabs) : base(manager, prefabs)
        {
        }

        public void Spawn(CollectibleType type, int quantity, Vector2 position)
        {
            if (!Validate(this.Entity))
                return;

            for (int i = 0; i < quantity; i++)
            {
                
            }
        }
    }
}