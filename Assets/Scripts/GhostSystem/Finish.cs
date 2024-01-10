using UnityEngine;

public class Finish : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InputController.Instance.inputCheck = false;
            UIController.SetWindow(Screen.FinishLevelScreen);
            GhostController.This.Finish();
        }
    }
}