using System;
using System.Collections.Generic;
using System.Linq;
using SharedLibrary.Responses;
using SharedLibrary.Responses.FriendsController;
using SharedLibrary.Responses.LevelsController;
using UnityEngine;

namespace SQL_Classes
{
    [Serializable]
    public class Point
    {
        public float x;
        public float y;
        public float z;
        public float time;

        public Point()
        {
            x = 0;
            y = 0;
            z = 0;
            time = 0;
        }
        
        public Point(float xCord, float yCord, float zCord, float timeMoment)
        {
            x = xCord;
            y = yCord;
            z = zCord;
            time = timeMoment;
        }
        
        public Vector3 Position() => new Vector3(x, y, z);
    }

    public class LeaderboardRow
    {
        public int Place;
        public string UserName;
        public float Time;

        public LeaderboardRow(LeaderboardRowResponse rowResponse)
        {
            Place = rowResponse.Place;
            UserName = rowResponse.UserName;
            Time = rowResponse.Time;
            
        }
    }

    public class LeaderboardPage
    {
        private List<LeaderboardRow> _rows;
        public IReadOnlyList<LeaderboardRow> Rows => _rows;

        public LeaderboardPage(List<LeaderboardRowResponse> rows)
        {
            _rows = rows.Select(r => new LeaderboardRow(r)).ToList();
        }
    }
    
    public class PlayerLevelData
    {
        public int Num { get; set; }

        public float Time { get; set; }

        public Way Way { get; set; }
        
        public PlayerLevelData(int num, float time, Way way)
        {
            Num = num;
            Time = time;
            Way = way;
        }
    }
    
    [Serializable]
    public class Way
    {
        public List<Point> points = new List<Point>();
        
        public void Add(float x, float y, float z, float time)
        {
            points.Add(new Point(x,y,z,time));
        }
    }

    public class SomeWays
    {
        public List<Way> Ways;

        public SomeWays(List<Way> ways)
        {
            Ways = ways;
        }
    }

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