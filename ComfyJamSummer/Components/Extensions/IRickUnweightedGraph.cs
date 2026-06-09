using System.Collections.Generic;
using ComfyJamSummer.Entities.Creatures;

namespace ComfyJamSummer.Components.Extensions
{
    public interface IRickUnweightedGraph<T>
    {
        IEnumerable<T> GetNeighbors(Creature creature, T node);
    }
}