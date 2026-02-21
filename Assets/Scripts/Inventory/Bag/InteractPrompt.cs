using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InteractPrompt : MonoBehaviour
{
    public GameObject prompt;
    public Text interactText;
    public void ShowPrompt(InteractType interactType)
    {
        if(interactType == InteractType.PickUp)
            interactText.text = "F 拾取";
        else if(interactType == InteractType.Use)
            interactText.text = "F 使用";
        prompt.SetActive(true);
    }
    public void HidePrompt()
    {
        prompt.SetActive(false);
    }
}
