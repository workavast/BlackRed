using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIScreenRepository : MonoBehaviour
{
    private Dictionary<Type, UIScreenBase> _screens;

    private static UIScreenRepository _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);
            return;
        }

        _instance = this;

        _screens = new Dictionary<Type, UIScreenBase>();

        UIScreenBase[] uiScreens = FindObjectsOfType<UIScreenBase>(true);
        foreach(UIScreenBase screen in uiScreens)
            _screens.Add(screen.GetType(), screen);
    }

    public static TScreen GetScreen<TScreen>() where TScreen : UIScreenBase
    {
        if (_instance == null)
            throw new NullReferenceException($"Instance is null");
        
        if(_instance._screens.TryGetValue(typeof(TScreen), out UIScreenBase screen))
            return (TScreen)screen;

        return default;
    }
}