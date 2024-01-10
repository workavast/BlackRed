using System;
using DataStorages;
using SharedLibrary.Paths;
using SharedLibrary.Requests.AuthenticationController;
using SharedLibrary.Responses.AuthenticationController;

namespace Apis
{
    public class AuthenticationApi : ApiBase
    {
        protected override string ControllerPath => "Authentication";

        public AuthenticationApi(PlayerDataStorage playerDataStorage, 
            CurrentLevelData currentLevelData, FriendsDataStorage friendsDataStorage) 
            : base(playerDataStorage, currentLevelData, friendsDataStorage) { }
        
        public async void UserRegistration(Action onSuccess, Action<string> onError, string playerName, string playerPassword)
        {
            var req = new AuthenticationRequest(playerName, playerPassword);
            
            var res = await TryPost<AuthenticationResponse>
                (AuthenticationControllerPaths.Register, req, onError);
            if(!res.Item1) return;
            
            PlayerDataStorage.SetMainData(playerName, playerPassword, res.Item2.Toke);
            
            onSuccess?.Invoke();
        }
        
        public async void UserLogin(Action onSuccess, Action<string> onError, string playerName, string playerPassword)
        {
            var req = new AuthenticationRequest(playerName, playerPassword);
            
            var res = await TryPost<AuthenticationResponse>
                (AuthenticationControllerPaths.Login, req, onError);
            if (!res.Item1) return;

            PlayerDataStorage.SetMainData(playerName, playerPassword, res.Item2.Toke);
            
            onSuccess?.Invoke();
        }
    }
}