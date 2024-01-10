using DataStorages;
using UnityEngine;
using WebApiConverters;

namespace UISystem.Elements
{
    public class LeaderboardPage_UI : TableBase<LeaderboardPageRow, LeaderboardRow>
    {
        [SerializeField] private GameObject playerFrame;
        [SerializeField] private GameObject closeScreen;
        
        protected override void SetRowData(LeaderboardPageRow row, LeaderboardRow newData)
        {
            if (newData.UserName == GlobalData.Instance.PlayerDataStorage.Name)
            {
                playerFrame.SetActive(true);
                Vector3 position = playerFrame.transform.position;
                position.y = row.transform.position.y;
                playerFrame.transform.position = position;
            }
                    
            row.SetData(newData);
            row.SwitchVisible(true);
        }
        
        public void SwitchCloseScreenVisible(bool show) => closeScreen.SetActive(show);

        public void SwitchVisible(bool show) => gameObject.SetActive(show);
    }
}