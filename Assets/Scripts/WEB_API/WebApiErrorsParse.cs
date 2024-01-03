using System;
using SharedLibrary;

namespace WEB_API
{
    public static class WebApiParse
    {
        public static string ErrorResult(string error)
        {
            if (error == "") return "Server error"; 
            
            if (!Enum.TryParse(error, out ErrorType errorType))
                throw new Exception("Invalid error string");
            
            return errorType switch
            {
                ErrorType.None => "Server error",
                
                ErrorType.NoNameIdentifier => "Sorry, some error, try re enter in your account",
                
                ErrorType.InvalidLogin => "Invalid login",
                ErrorType.InvalidPassword => "Invalid password",
                ErrorType.UserWithTisNameIsExist => "User with this name is exist",
                
                ErrorType.InvalidLevelData => "Sorry, some error",
                ErrorType.LevelDataDontFound => "Level data don't found",
                
                ErrorType.UserDontFound => "User don't found",
                ErrorType.InvalidUserId => "Some error, try update page and try again",
                ErrorType.FriendRequestToSelf => "You can't send request to self",
                ErrorType.FriendRequestExist => "Request already send to this player",
                ErrorType.FriendRequestDontFound => "Request don't found, try update page and try again",
                ErrorType.FriendPairDontFound => "Request don't found, try update page and try again",
                
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}