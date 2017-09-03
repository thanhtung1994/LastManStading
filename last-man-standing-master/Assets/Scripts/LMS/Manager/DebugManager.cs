using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public static DebugManager Instance;

    public bool DebugModeEnabled = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

    }
}
