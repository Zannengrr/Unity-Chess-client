using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageComponent : MonoBehaviour
{
    public TextMeshProUGUI infoMessage;

    private void Awake()
    {
        Deactivate();
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
    public void SetMessage(string message)
    {
        infoMessage.text = message;
    }
}
