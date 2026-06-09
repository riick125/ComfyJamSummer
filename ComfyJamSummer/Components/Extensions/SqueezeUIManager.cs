using System.Collections.Generic;
using System.Linq;
using ComfyJamSummer.Components.Gameplay;
using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Extensions;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.PoolObjects;
using Nez;
using Nez.UI;
using static ComfyJamSummer.Components.Gameplay.SqueezeAnimation;

namespace ComfyJamSummer.Components.Extensions
{
    public class SqueezeUIManager : SceneComponent
    {
        IReadOnlyList<Animated> _entities;

        public SqueezeUIManager()
        {
            Pool<SqueezeAnimation>.WarmCache(500);
            Pool<SqueezeProcess>.WarmCache(1000);
        }

        public SqueezeAnimation Create(SqueezeAnimationPoolConfig config)
        {
            var squeeze = Pool<SqueezeAnimation>.Obtain();

            squeeze.Initialize(config);

            return squeeze;
        }

        public override void Update()
        {
            base.Update();

            if (Scene != null)
            {
                _entities = Scene.EntitiesOfType<Animated>();
            }

            foreach (var entity in _entities)
            {
                if (entity.Canvas == null)
                {
                    continue;
                }

                if (entity.Canvas != null)
                {
                    var allElements = UIHelper.GetAllChildren(entity.Canvas.Stage).Where(x => UtilHelper.HasSqueezeAnimationProperty(x)).ToList();
                    allElements.ForEach(x =>
                    {
                        UpdateSqueeze(x);
                    });
                }
            }
        }

        private static void UpdateSqueeze(Element x)
        {
            var type = x.GetType();

            var prop = type.GetProperty("SqueezeAnimation");
            if (prop == null)
                return;

            var squeezeObj = prop.GetValue(x);
            if (squeezeObj == null)
                return;

            var updateMethod = squeezeObj.GetType().GetMethod("Update");
            if (updateMethod == null)
                return;

            updateMethod.Invoke(squeezeObj, null);
        }

        public void Clear()
        {
            Pool<SqueezeAnimation>.ClearCache();
            Pool<SqueezeProcess>.ClearCache();
        }

        public override void OnRemovedFromScene()
        {
            base.OnRemovedFromScene();

            Clear();
        }
    }
}
