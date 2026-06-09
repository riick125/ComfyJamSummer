using System;

namespace ComfyJamSummer.UI.Data
{
    public class ElementUserData
    {
        public Type ElementClassType { get; set; }

        public int Id { get; set; }

        public ElementUserData(Type elementClassType, int id)
        {
            ElementClassType = elementClassType;
            Id = id;
        }
    }
}