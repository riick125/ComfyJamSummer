namespace ComfyJamSummer.UI.Data
{
    public class ActorUIData
    {
        public uint EnemyId { get; set; }

        public ActorUIData(uint enemyId)
        {
            EnemyId = enemyId;
        }
    }
}