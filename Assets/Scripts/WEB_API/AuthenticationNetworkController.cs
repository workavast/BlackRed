using System;
using SharedLibrary.Paths;
using SharedLibrary.Requests.AuthenticationController;
using SharedLibrary.Responses.AuthenticationController;

namespace WEB_API
{
    public class AuthenticationNetworkController : NetworkControllerBase
    {
        protected override string ControllerPath => AuthenticationPath;
        private const string AuthenticationPath = "Authentication";

        public AuthenticationNetworkController(PlayerDataStorage playerDataStorage, 
            CurrentLevelData currentLevelData, FriendsDataStorage friendsDataStorage) 
            : base(playerDataStorage, currentLevelData, friendsDataStorage) { }
        
        public async void UserRegistration
            (Action onSuccessDelegate, Action<string> onErrorDelegate, string playerName, string playerPassword)
        {
            var req = new AuthenticationRequest(playerName, playerPassword);
            
            var res = await TryPost<AuthenticationResponse>
                (AuthenticationControllerPaths.Register, req, onErrorDelegate);
            if(!res.Item1) return;
            
            PlayerDataStorage.SetMainData(playerName, playerPassword, res.Item2.Toke);
            
            onSuccessDelegate?.Invoke();
        }
        
        public async void UserLogin
            (Action onSuccessDelegate, Action<string> onErrorDelegate, string playerName, string playerPassword)
        {
            var req = new AuthenticationRequest(playerName, playerPassword);
            
            var res = await TryPost<AuthenticationResponse>
                (AuthenticationControllerPaths.Login, req, onErrorDelegate);
            if (!res.Item1) return;

            PlayerDataStorage.SetMainData(playerName, playerPassword, res.Item2.Toke);
            
            onSuccessDelegate?.Invoke();
        }
    }
}