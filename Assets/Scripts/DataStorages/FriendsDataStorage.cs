using System.Collections.Generic;
using WebApiConverters;

namespace DataStorages
{
    public class FriendsDataStorage : IFriendsDataStorage
    {
        private List<FriendPair> _friendPairs;
        private List<FriendRequestToMe> _friendRequestsToMe;
        private List<FriendRequestFromMe> _friendRequestsFromMe;

        public IReadOnlyList<FriendPair> FriendPairs => _friendPairs;
        public IReadOnlyList<FriendRequestToMe> FriendRequestsToMe => _friendRequestsToMe;
        public IReadOnlyList<FriendRequestFromMe> FriendRequestsFromMe => _friendRequestsFromMe;
        
        public void SetFriendPairsData(List<FriendPair> friendPairs) 
            => _friendPairs = friendPairs;
        
        public void SetFriendRequestsToMe(List<FriendRequestToMe> friendRequestsToMe) 
            => _friendRequestsToMe = friendRequestsToMe;
        
        public void SetFriendRequestsFromMe(List<FriendRequestFromMe> friendRequestsFromMe) 
            => _friendRequestsFromMe = friendRequestsFromMe;
    }
    
    public interface IFriendsDataStorage
    {
        public IReadOnlyList<FriendPair> FriendPairs { get; }
        public IReadOnlyList<FriendRequestToMe> FriendRequestsToMe { get; }
        public IReadOnlyList<FriendRequestFromMe> FriendRequestsFromMe { get; }
    }
}