using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PlayerAvoidController : MonoBehaviour
{
    [SerializeField, Tooltip("回避距離")] private float _avoidDistance = 3.0F;

    [SerializeField, Tooltip("その回避距離を何秒で移動するか")]
    private float _avoidDuration = 0.5F;
    
    /// <summary>現在回避中かどうか</summary>
    private bool _isAvoiding = false;

    /// <summary>現在回避中かどうか</summary>
    public bool IsAvoiding
    {
        get => _isAvoiding;
        private set
        {
            if (_isAvoiding != value)
            {
                _onIsAvoidingChanged?.Invoke(value);
                _isAvoiding = value;
            }
        }
    }

    /// <summary>IsAvoidingが変更された際によばれるEvent</summary>
    private Action<bool> _onIsAvoidingChanged = default;
    
    /// <summary>IsAvoidingが変更された際によばれるEvent</summary>
    public event Action<bool> OnIsAvoidingChanged
    {
        add => _onIsAvoidingChanged += value;
        remove => _onIsAvoidingChanged -= value;
    }

    /// <summary>現在Tweeningしているかどうか</summary>
    private bool _isTweening = false;

    private void OnEnable()
    {
        CustomInputManager.Instance.PlayerInputActions.Player.Avoid.started += InputAvoiding;
        CustomInputManager.Instance.PlayerInputActions.Player.Avoid.canceled += CancelInputAvoiding;
    }

    private void OnDisable()
    {
        CustomInputManager.Instance.PlayerInputActions.Player.Avoid.started -= InputAvoiding;
        CustomInputManager.Instance.PlayerInputActions.Player.Avoid.canceled -= CancelInputAvoiding;
    }

    /// <summary>InputActionに登録する関数</summary>
    /// <param name="context">コールバック</param>
    private void InputAvoiding(InputAction.CallbackContext context)
    {
        IsAvoiding = true;
    }

    private void CancelInputAvoiding(InputAction.CallbackContext context)
    {
        if (!_isTweening)
        {
            IsAvoiding = false;
        }
    }

    /// <summary>AvoidStateに入ったら行ってほしい処理</summary>
    public void OnSteteEntry()
    {
        transform.DOMove(transform.position + (transform.forward * _avoidDistance), _avoidDuration)
            .OnStart(() => _isTweening = true)
            .OnComplete(() =>
            {
                _isTweening = false;
                IsAvoiding = false;
            });
    }
}