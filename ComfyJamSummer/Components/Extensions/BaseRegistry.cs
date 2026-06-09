using System.Collections.Generic;
using ComfyJamSummer.Entities.Base;
using Nez;

namespace ComfyJamSummer.Components.Extensions
{
    public class BaseRegistry<T> : SceneComponent where T : Animated
    {
        protected readonly List<T> _entities = new();

        protected readonly Dictionary<uint, Animated> _animatedById = new();

        public override void OnEnabled()
        {
            base.OnEnabled();
            _entities.Clear();
        }

        public void Register(T entity)
        {
            if (entity == null)
                return;

            if (!_entities.Contains(entity))
            {
                _entities.Add(entity);

                _animatedById[entity.Id] = entity;
            }
        }

        public void Unregister(T entity)
        {
            if (entity == null)
                return;

            switch (entity)
            {
                default:
                    _entities.Remove(entity);

                    _animatedById.Remove(entity.Id);
                    break;
            }
        }
    }
}