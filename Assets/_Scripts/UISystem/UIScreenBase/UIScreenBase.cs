using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScreenBase : MonoBehaviour
{
    public void _SetWindow(int screen)
    {
        UIController.SetWindow((Screen)screen);
    }
}