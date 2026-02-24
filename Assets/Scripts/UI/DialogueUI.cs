using UnityEngine.UI;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DialogueUI : MonoBehaviour
{
    [Header("UI基本面板")]
    public GameObject dialoguePanel;
    public Image head;
    public TextMeshProUGUI mainText;
    public Button nextButton;
    public Button[] optionButtons;

    public DialogueData_SO currentDialogue;
    private int currentIndex = 0;

    void Awake()
    {
        nextButton.onClick.AddListener(OnNextButtonClick);
    }


    public void UpdateDialogueData(DialogueData_SO dialogueData)
    {
        currentDialogue = dialogueData;
        currentIndex = 0;
        UpdateDialogueUI(currentDialogue.dialoguePieces[currentIndex]);
    }

    private Text ttt;
    public void UpdateDialogueUI(DialoguePiece piece)
    {
        currentIndex++;
        dialoguePanel.SetActive(true);
        if (piece.image != null)
        {
            head.sprite = piece.image;
            head.gameObject.SetActive(true);
        }
        else
            head.gameObject.SetActive(false);

        mainText.text = "";
        float time = piece.text.Length *0.1f;
        DOTween.To(() => string.Empty, value => mainText.text = value, piece.text, time)
               .SetEase(Ease.Linear);

        if (piece.options.Count == 0)
        {
            nextButton.gameObject.SetActive(true);
            
        }
        else
        {
            nextButton.gameObject.SetActive(false);
        }
    }

    private void OnNextButtonClick()
    {
        if (currentIndex < currentDialogue.dialoguePieces.Count)
            UpdateDialogueUI(currentDialogue.dialoguePieces[currentIndex]);
        else
            dialoguePanel.SetActive(false);
    }

}
