using System.Linq;
using ComfyJamSummer.Entities;
using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Helpers;
using Nez;
using Nez.Sprites;

namespace ComfyJamSummer.Components.Visuals
{
    public class DepthComponent : SceneComponent
    {
        public override void Update()
        {
            base.Update();

            var island = this.Scene.EntitiesOfType<Island>().FirstOrDefault();

            if (island == null)
            {
                return;
            }

            var entities = this.Scene.EntitiesOfType<Animated>();

            foreach (var x in entities)
            {
                var height = 0f;

                SpriteAnimator animator = null;
                SpriteRenderer renderer = null;

                renderer = x.GetComponent<SpriteRenderer>();

                if (renderer != null)
                {
                    if (x.DepthHeight != 0)
                    {

                    }
                    height = x.DepthHeight != 0 ? x.DepthHeight : renderer.Height / 2;
                }
                else
                {
                    animator = x.GetComponent<SpriteAnimator>();

                    if (animator != null)
                    {
                        height = animator.Height / 3;
                    }
                }

                UtilHelper.ProcessLayerDepth(island, x, animator != null ? animator : null, renderer != null ? renderer : animator, height);
            }
        }
    }
}