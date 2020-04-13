using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextSingleton : MonoBehaviour
{
    public static TextSingleton Instance { get; private set; }
    public Text Text;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }
}
