using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TeleportPoint : MonoBehaviour
{
    [SerializeField] Transform targetTeleportPoint;

    public GameObject player;
    private CharacterController characterController;  //注意：传送前一定要关掉这个控制，不然传不了
    private bool playerInRange = false;

    void Update()
    {
        if(player == null) return;
        if(playerInRange && GameInputManager.Instance.Interact)
        {
            StartCoroutine(TeleportPlayer());
        }
    }

    IEnumerator TeleportPlayer()
    {
        player.GetComponent<PlayerInput>().enabled = false;
        yield return UIManager.instance.fadeManager.Fade(1);

        Debug.Log($"当前传送点: {gameObject.name}, 目标点: {targetTeleportPoint?.name}");

        characterController.enabled = false;

        player.transform.position = targetTeleportPoint.position;

        float delayTime = 1f;
        while(delayTime > 0)
        {
            delayTime -= Time.deltaTime;
            yield return null;
        }

        yield return UIManager.instance.fadeManager.Fade(0);
        player.GetComponent<PlayerInput>().enabled = true;
        characterController.enabled = true;

        //传送无法触发OnTriggerExit，所以要手动设为false
        UIManager.instance.interactPrompt.HidePrompt();
        player = null;
        characterController = null;
        playerInRange = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            player = other.gameObject;
            characterController = player.GetComponent<CharacterController>();
            UIManager.instance.interactPrompt.ShowPrompt(InteractType.Use);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
            characterController = null;
            UIManager.instance.interactPrompt.HidePrompt();
        }
    }
}
