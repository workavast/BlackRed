using SharedLibrary.Responses.FriendsController;

namespace WebApiConverters
{
    public class FriendPair
    {
        public int Id { get; private set; }
        public string FriendName { get; private set; }

        public FriendPair(FriendPairResponse friendPairResponse)
        {
            Id = friendPairResponse.Id;
            FriendName = friendPairResponse.FriendName;
        }
        
        public FriendPair(int id, string friendName)
        {
            Id = id;
            FriendName = friendName;
        }
    }
}