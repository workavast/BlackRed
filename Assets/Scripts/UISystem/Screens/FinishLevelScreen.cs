using UnityEngine;
using TMPro;

public class FinishLevelScreen : UIScreenBase
{
    [SerializeField] private TMP_Text fullLevelTime;
    [SerializeField] private TMP_Text timesDifference;
    private GhostController _ghostRecord;
    
    private void OnEnable()
    {
        _ghostRecord = GhostController.This;

        fullLevelTime.text = _ghostRecord.CurrentFullTime.ToString();
        
        float difference = _ghostRecord.CurrentFullTime - _ghostRecord.PreviousFullTime;
        
        if(difference < 0)
            timesDifference.color = Color.green;
        else
            timesDifference.color = Color.red;

        if (_ghostRecord.PreviousFullTime == 0 || difference == 0)
        {
            timesDifference.text = "";
        }
        else
        {
            if (difference > 0)
                timesDifference.text = "+" + difference.ToString();
            else
                timesDifference.text = difference.ToString();  
        }
    }
    
    public void _LoadScene(int sceneNum)
    {
        UIController.LoadScene(sceneNum);
    }

    public void _Quit()
    {
        UIController.Quit();
    }
}