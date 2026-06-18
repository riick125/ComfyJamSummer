using ComfyJamSummer.Entities.Base;

namespace ComfyJamSummer.EventDatas
{
    public class UIEventData
    {
        public Actor Target { get; set; }
    }

    public enum UIEvent
    {
        ReduceBar,
        HealBar
    }
}