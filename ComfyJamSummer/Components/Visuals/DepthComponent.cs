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

            //var arena = this.Scene.EntitiesOfType<BattleArena>().FirstOrDefault();

            //if (arena == null)
            //{
            //    return;
            //}

            //var entities = this.Scene.EntitiesOfType<Animated>();

            //foreach (var x in entities)
            //{
            //    var height = 0f;

            //    var spriteRenderer = x.GetComponent<SpriteRenderer>();

            //    if (spriteRenderer != null)
            //    {
            //        height = spriteRenderer.Height / 4;
            //    }
            //    else
            //    {
            //        var animator = x.GetComponent<SpriteAnimator>();

            //        if (animator != null)
            //        {
            //            height = animator.Height / 4;
            //        }
            //    }

            //    UtilHelper.SetLayerDepthForMovingObject(x, height);
            //}
        }
    }
}