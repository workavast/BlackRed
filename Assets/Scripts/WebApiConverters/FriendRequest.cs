using SharedLibrary.Responses.FriendsController;

namespace WebApiConverters
{
    public class FriendRequestFromMe
    {
        public int Id { get; private set; }
        public string UserName { get; private set; }

        public FriendRequestFromMe(FriendRequestResponse friendPairResponse)
        {
            Id = friendPairResponse.RequestId;
            UserName = friendPairResponse.RecipientName;
        }
        
        public FriendRequestFromMe(int id, string userName)
        {
            Id = id;
            UserName = userName;
        }
    }
    
    public class FriendRequestToMe
    {
        public int Id { get; private set; }
        public string UserName { get; private set; }

        public FriendRequestToMe(FriendRequestResponse friendPairResponse)
        {
            Id = friendPairResponse.RequestId;
            UserName = friendPairResponse.SenderName;
        }
        
        public FriendRequestToMe(int id, string userName)
        {
            Id = id;
            UserName = userName;
        }
    }
}