using System.Collections.Generic;
using System.Linq;
using SharedLibrary.Responses.LevelsController;

namespace WebApiConverters
{
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
}