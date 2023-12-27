using UnityEngine;

namespace WEB_API
{
    public class GlobalData : MonoBehaviour
    {
        private readonly PlayerDataStorage _playerDataStorage = new PlayerDataStorage();
        private readonly CurrentLevelData _currentLevelData = new CurrentLevelData();
        
        public IReadOnlyPlayerDataStorage PlayerDataStorage => _playerDataStorage;
        public IReadOnlyCurrentLevelData CurrentLevelData => _currentLevelData;
        public NetworkController NetworkController { get; private set; }

        public static GlobalData Instance { get; private set;} 
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }
        
            DontDestroyOnLoad(gameObject);
            Instance = this;

            NetworkController = new NetworkController(_playerDataStorage, _currentLevelData);
        }
    }
}