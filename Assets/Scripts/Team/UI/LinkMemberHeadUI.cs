using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LinkMemberHeadUI : MonoBehaviour
{
    [SerializeField] private Image _headImage;

    public void UpdateHeadImage(Sprite sprite)
    {
        _headImage.sprite = sprite;
    }
}
