using GhostSystem;

namespace DataStorages
{
    public class PlayerLevelData
    {
        public int Num { get; set; }

        public float Time { get; set; }

        public Way Way { get; set; }
        
        public PlayerLevelData(int num, float time, Way way)
        {
            Num = num;
            Time = time;
            Way = way;
        }
    }
}