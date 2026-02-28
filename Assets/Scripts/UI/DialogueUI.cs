using UnityEngine.UI;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;

public class DialogueUI : MonoBehaviour
{
    [Header("UI基本面板")]
    public GameObject dialoguePanel;
    public Image head;
    public TextMeshProUGUI mainText;
    public Button nextButton;
    [Header("选项面板")]
    public RectTransform optionPanel;
    public OptionUI optionPrefab;

    [Header("对话数据")]
    public DialogueData_SO currentDialogue;
    private DialoguePiece currentPiece;

    void Awake()
    {
        nextButton.onClick.AddListener(OnNextButtonClick);
    }


    public void UpdateDialogueData(DialogueData_SO dialogueData)
    {
        currentDialogue = dialogueData;
        UpdateDialogueUI(currentDialogue.dialoguePieces[0]);
    }

    private Text ttt;
    public void UpdateDialogueUI(DialoguePiece piece)
    {
        currentPiece = piece;
        dialoguePanel.SetActive(true);
        UIManager.instance.inputUIManager.playerInput.actions.FindActionMap("Player").Disable();
        if (piece.image != null)
        {
            head.sprite = piece.image;
            head.gameObject.SetActive(true);
        }
        else
            head.gameObject.SetActive(false);

        mainText.text = "";
        float time = piece.text.Length * 0.1f;
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

        CreateOptions(piece);
    }

    private void OnNextButtonClick()
    {
        if (currentDialogue.dialoguePiecesDictionary.TryGetValue(currentPiece.targetID, out DialoguePiece nextPiece))
        {
            UpdateDialogueUI(nextPiece);
        }
        else
        {
            dialoguePanel.SetActive(false);
            UIManager.instance.inputUIManager.playerInput.actions.FindActionMap("Player").Enable();
        }
    }


    void CreateOptions(DialoguePiece piece)
    {
        Debug.Log("创建选项");
        if (optionPanel.childCount > 0)
        {
            foreach (Transform child in optionPanel)
            {
                child.gameObject.SetActive(false);
            }
        }
        Debug.Log(piece.options.Count);

        for (int i = 0; i < piece.options.Count; i++)
        {
            Debug.Log("生成选项" + i);
            OptionUI option = optionPanel.GetChild(3-i).GetComponent<OptionUI>();
            option.gameObject.SetActive(true);
            option.UpdateOption(piece.options[i], piece);
        }
    }

}
