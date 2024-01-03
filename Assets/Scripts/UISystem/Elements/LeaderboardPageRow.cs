using SQL_Classes;
using TMPro;
using UnityEngine;

namespace UISystem.Elements
{
    public class LeaderboardPageRow : TablePageRowBase<LeaderboardRow>
    {
        [SerializeField] private TextMeshProUGUI place;
        [SerializeField] private TextMeshProUGUI time;

        public override void SetData(LeaderboardRow newData)
        {
            place.text = newData.Place.ToString();
            playerName.text = newData.UserName;
            time.text = newData.Time.ToString();
        }
    }
}