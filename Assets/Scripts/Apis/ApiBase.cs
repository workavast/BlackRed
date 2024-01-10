using System;
using System.Threading.Tasks;
using DataStorages;
using WEB_API;

namespace Apis
{
    public abstract class ApiBase
    {
        private const string MainPartOfPath = "http://localhost:5159";
        
        protected abstract string ControllerPath { get; }
        
        protected readonly PlayerDataStorage PlayerDataStorage;
        protected readonly CurrentLevelData CurrentLevelData;
        protected readonly FriendsDataStorage FriendsDataStorage;
        
        protected string JwtToken => PlayerDataStorage.JwtToken;

        protected ApiBase(PlayerDataStorage playerDataStorage, 
            CurrentLevelData currentLevelData, FriendsDataStorage friendsDataStorage) 
        {
            PlayerDataStorage = playerDataStorage;
            CurrentLevelData = currentLevelData;
            FriendsDataStorage = friendsDataStorage;
        }
        
        /// <summary>
        /// Send request to web api. 
        /// </summary>
        /// <param name="onError"> Invoke it, if request un success </param>
        /// <returns>
        /// return two values. <br/>
        /// bool - if request success then it true, else false. <br/>
        /// T - if request success then it T-response data, else default. <br/>
        /// </returns>
        protected async Task<(bool, T)> TryGet<T>(string methodPath, Action<string> onError, string jwt = null)
        {
            string fullPath = $"{MainPartOfPath}/{ControllerPath}/{methodPath}";
            
            var res = await WebApi.Get<T>(fullPath, jwt);
            if (!res.Item1)
                onError?.Invoke(WebApiParse.ErrorResult(res.Item2));

            return (res.Item1, res.Item3);
        }
        
        /// <summary>
        /// Send request to web api. 
        /// </summary>
        /// <param name="onError"> Invoke it, if request un success </param>
        /// <returns>
        /// return two values. <br/>
        /// bool - if request success then it true, else false. <br/>
        /// T - if request success then it T-response data, else default. <br/>
        /// </returns>
        protected async Task<(bool, T)> TryPost<T>(string methodPath, object data, Action<string> onError, string jwt = null)
        {
            string fullPath = $"{MainPartOfPath}/{ControllerPath}/{methodPath}";
            
            var res = await WebApi.Post<T>(fullPath, data, jwt);
            if (!res.Item1)
                onError?.Invoke(WebApiParse.ErrorResult(res.Item2));

            return (res.Item1, res.Item3);
        }
        
        /// <summary>
        /// Send request to web api. 
        /// </summary>
        /// <param name="onError"> Invoke it, if request un success </param>
        /// <returns>
        /// return true if request success, else false.
        /// </returns>
        protected async Task<bool> TryPost(string methodPath, object data, Action<string> onError, string jwt = null)
        {
            string fullPath = $"{MainPartOfPath}/{ControllerPath}/{methodPath}";
            
            var res = await WebApi.Post(fullPath, data, jwt);
            if (!res.Item1)
                onError?.Invoke(WebApiParse.ErrorResult(res.Item2));

            return res.Item1;
        }
    }
}