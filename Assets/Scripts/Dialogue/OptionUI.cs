using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public TextMeshProUGUI optionText;
    private Button thisButton;
    private string nextPieceID;
    private DialoguePiece currentPiece;

    void Awake()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(OnOptionClick);
    }

    public void UpdateOption(DialogueOption option, DialoguePiece piece)
    {
        currentPiece = piece;
        optionText.text = option.text;
        nextPieceID = option.targetID;
    }

    public void OnOptionClick()
    {

        if (UIManager.instance.dialogueUI.currentDialogue.dialoguePiecesDictionary.TryGetValue(nextPieceID, out DialoguePiece nextPiece))
        {
            UIManager.instance.dialogueUI.UpdateDialogueUI(nextPiece);
        }
        else
        {
            UIManager.instance.dialogueUI.dialoguePanel.SetActive(false);
            UIManager.instance.inputUIManager.playerInput.actions.FindActionMap("Player").Enable();
        }

    }

}
