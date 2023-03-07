using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UiScreenRepository : MonoBehaviour
{
    private Dictionary<Type, UiScreenBase> _screens;

    private static UiScreenRepository _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);
            return;
        }

        _instance = this;

        _screens = new Dictionary<Type, UiScreenBase>();

        UiScreenBase[] uiScreens = FindObjectsOfType<UiScreenBase>(true);
        foreach(UiScreenBase screen in uiScreens)
            _screens.Add(screen.GetType(), screen);
    }

    public static TScreen GetScreen<TScreen>() where TScreen : UiScreenBase
    {
        if (_instance == null)
            throw new NullReferenceException($"Instance is null");
        
        if(_instance._screens.TryGetValue(typeof(TScreen), out UiScreenBase screen))
            return (TScreen)screen;

        return default;
    }
}
