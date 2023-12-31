using UnityEngine;
using UniRx;

/// <summary>
/// MVPのPresenter
/// </summary>
public class IngameUIPresenter : MonoBehaviour
{
    [SerializeField]
    private IngameUIView _ingameUIView;

    private InGameSystem _inGameSystem;

    private PlayerHPController _playerHPController;

    private void Awake()
    {
        // PlayerHPControllerを取得する
        _playerHPController =
            FindObjectOfType<PlayerHPController>().GetComponent<PlayerHPController>();
        // InGameSystemを取得する
        _inGameSystem =
            FindObjectOfType<InGameSystem>().GetComponent<InGameSystem>();

        if (_inGameSystem == null)
        {
            Debug.LogError("InGameSystemが見つかりませんでした");
            return;
        }
        else if (_playerHPController == null)
        {
            Debug.LogError("PlayerHPControllerが見つかりませんでした");
            return;
        }
        _ingameUIView.Intialized();
        SubscribeUIEvent();
    }

    /// <summary>
    /// UIのイベントを購読する
    /// </summary>
    private void SubscribeUIEvent()
    {
        // PlayerHPControllerのOnCurrentHpChangedにPlayerLifeUIのSetLifeViewを登録する
        _playerHPController.OnCurrentHpChanged +=
            _ingameUIView.PlayerLifeUI.SetLifeView;

        // InGameSystemのScoreSystemのItemCountにItemCountUIのSetItemCountViewを登録する
        _inGameSystem
            .ScoreSystem
            .ItemCount
            .Subscribe(
                _ingameUIView.ItemCountUI.SetItemCountView)
            .AddTo(gameObject);
    }

    private void Update()
    {
        // InGameSystemのTimerSystemのTimerにGameTimerUIのSetTimeViewを登録する
        _ingameUIView
            .GameTimerUI
            .SetTimerView(TimerSystem.Timer);
    }

    private void OnDestroy()
    {
        // PlayerHPControllerのOnCurrentHpChangedからPlayerLifeUIのSetLifeViewを削除する
        _playerHPController.OnCurrentHpChanged -= 
            _ingameUIView.PlayerLifeUI.SetLifeView;
    }
}
