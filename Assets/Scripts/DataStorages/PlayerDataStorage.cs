using System.Collections.Generic;

namespace DataStorages
{
    public class PlayerDataStorage : IReadOnlyPlayerDataStorage
    {
        public string Name { get; private set; }
        public string Password { get; private set; }
        public string JwtToken { get; private set; }
        public IReadOnlyList<PlayerLevelData> Levels => _levels;

        private List<PlayerLevelData> _levels;
        
        public void AuthenticatePlayer(string name, string password, string jwtToken, List<PlayerLevelData> levels)
        {
            Name = name;
            Password = password;
            JwtToken = $"Bearer {jwtToken}";
            _levels = levels;
        }

        public void SetMainData(string name, string password, string jwtToken)
        {
            Name = name;
            Password = password;
            JwtToken = $"Bearer {jwtToken}";
        }
        
        public void SetLevelsDatas(List<PlayerLevelData> levels)
        {
            _levels = levels;
        }
        
        public void RegisterNewLevelResult(PlayerLevelData newPlayerLevelData)
        {
            _levels.Add(newPlayerLevelData);
        }
        
        public void UpdateLevelResult(PlayerLevelData newPlayerLevelData)
        {
            var res = _levels.FindIndex(pld => pld.Num == newPlayerLevelData.Num);
            _levels[res] = newPlayerLevelData;
        }
    }

    public interface IReadOnlyPlayerDataStorage
    {
        public string Name { get; }
        public string Password { get; }
        public string JwtToken { get; }
        public IReadOnlyList<PlayerLevelData> Levels { get; }
    }
}