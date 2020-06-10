using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameText : MonoBehaviour
{
    
    private Text nameText;
    // Start is called before the first frame update
    void Start()
    {
        nameText = GetComponent<Text>();

        if(UserDataManager.Instance.User != null)
        {
            nameText.text = $"Hi! {UserDataManager.Instance.User.Email}";
        }

        else
        {
            nameText.text = "ERROR : AuthManager.User == null";
        }
    }

}
