using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MarketUpdate : MonoBehaviour
{
    private static MarketUpdate instance;
    public static MarketUpdate Instance { get { return instance; } }

    void Awake()
    {
        if (instance == null)
            instance = new MarketUpdate();
    }
}
