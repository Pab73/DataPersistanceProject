using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NameManager : MonoBehaviour
{
    public static NameManager Instance;
    
    public InputField playersNameInputField;

    string _plyrName;

    public string PlyrName { get => _plyrName; set => _plyrName = value; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }   

    public void SetPlayerName()
    {
        PlyrName = playersNameInputField.text;
    }
}
