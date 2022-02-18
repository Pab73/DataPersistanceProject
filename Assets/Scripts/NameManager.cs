using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NameManager : MonoBehaviour
{
    public static NameManager Instance;
    
    public InputField playersNameInputField;

    public string plyrName;
    
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
        plyrName = playersNameInputField.text;
    }
}
