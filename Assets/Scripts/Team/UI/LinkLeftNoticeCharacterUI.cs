using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;

public class LinkLeftNoticeCharacterUI : MonoBehaviour
{
    public static LinkLeftNoticeCharacterUI Instance;
    [SerializeField] private RectTransform characterIcon;
    [SerializeField] private Image image;
    [SerializeField] private float enterDuration = 0.18f;
    [SerializeField] private float stayDuration = 1f;
    [SerializeField] private float outsideOffsetX = 900f;
    private Vector2 _targetPosition;
    private Sequence _showSequence;

    private void Awake()
    {
        Instance = this;
        _targetPosition = characterIcon.anchoredPosition;
        characterIcon.gameObject.SetActive(false);
    }
    public void SetCharacterIcon(Sprite sprite)
    {
        image.sprite = sprite;
    }

    public void Show()
    {
        // DOTween 的 Tween/Sequence 不会因为重新播放自动停掉，先 Kill 防止多个动画同时控制同一个 UI。
        _showSequence?.Kill();

        Vector2 startPosition = _targetPosition + Vector2.left * outsideOffsetX;
        characterIcon.anchoredPosition = startPosition;
        characterIcon.gameObject.SetActive(true);

        // Sequence 表示一串按顺序执行的动画：先滑入，再等待，最后隐藏。
        // SetUpdate(true) 表示使用真实时间，不受 Time.timeScale 慢动作影响。
        _showSequence = DOTween.Sequence()
            .SetUpdate(true)
            // DOAnchorPos 是 RectTransform 的 UI 位移动画，Ease.OutCubic 让滑入先快后慢。
            .Append(characterIcon.DOAnchorPos(_targetPosition, enterDuration).SetEase(Ease.OutCubic))
            // AppendInterval 是等待时间，这里让角色提示停留一秒。
            .AppendInterval(stayDuration)
            .AppendCallback(() => characterIcon.gameObject.SetActive(false))
            .OnComplete(() => _showSequence = null);
    }
    public void Hide()
    {
        _showSequence?.Kill();
        _showSequence = null;

        characterIcon.gameObject.SetActive(false);
    }
}
