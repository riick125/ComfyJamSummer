using ComfyJamSummer.Entities.Base;

namespace ComfyJamSummer.EventDatas
{
    public class UIEventData
    {
        public Actor Target { get; set; }

        public float FloatingPoints { get; set; }
    }

    public enum UIEvent
    {
        ReduceBar,
        HealBar,
        UpdatePatienceBar,
        UpdateSatiationBar,
        UpdateBuildBar,
        SendFloatingPoints
    }
}