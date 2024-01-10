using GhostSystem;

namespace DataStorages
{
    public class CurrentLevelData : IReadOnlyCurrentLevelData
    {
        public bool LevelChosen { get; set; }
        public int LevelNum { get; set; }
        public SomeWays OtherPlayersWays { get; set; }
    }
    
    public interface IReadOnlyCurrentLevelData
    {
        public bool LevelChosen { get; }
        public int LevelNum { get; }
        public SomeWays OtherPlayersWays { get; }
    }
}