using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;

public class LinkMemberHeadUI : MonoBehaviour
{
    [SerializeField] private Image _headImage;
    [SerializeField] private float _showDuration = 0.18f;
    private Tween _showTween;
    private Vector3 _targetScale = Vector3.one;

    private void Awake()
    {
        _targetScale = transform.localScale;
    }

    public void UpdateHeadImage(Sprite sprite)
    {
        _headImage.sprite = sprite;
    }

    public void PlayShowAnimation()
    {
        // 重新生成队列 UI 时先停掉旧缩放动画，避免新旧 Tween 抢同一个 scale。
        _showTween?.Kill();

        transform.localScale = Vector3.zero;
        // DOScale 会把 localScale 从当前值平滑变到目标值；OutBack 会有一点弹性弹出感。
        // SetUpdate(true) 表示使用真实时间，不受连携技慢动作影响。
        _showTween = transform.DOScale(_targetScale, _showDuration)
            .SetEase(Ease.OutBack)
            .SetUpdate(true);
    }
}
