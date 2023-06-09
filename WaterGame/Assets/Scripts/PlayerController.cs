using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private string horizontalString;

    [SerializeField]
    private string verticalString;

    [SerializeField]
    private string jumpString;

    [Header("移動の速さ"), SerializeField]
    private float _speed = 3;

    [Header("ジャンプする瞬間の速さ"), SerializeField]
    private float _jumpSpeed = 7;

    [Header("重力加速度"), SerializeField]
    private float _gravity = 15;

    [Header("落下時の速さ制限（Infinityで無制限）"), SerializeField]
    private float _fallSpeed = 10;

    [Header("落下の初速"), SerializeField]
    private float _initFallSpeed = 2;

    [Header("地面の位置"),SerializeField]
    private GameObject GroundHeight;

    [Header("プレイヤーオブジェクト"), SerializeField]
    private GameObject myGameObject;

    private Transform _transform;
    private CharacterController _characterController;

    //private Vector2 _inputMove;
    private float _verticalVelocity;
    private float _turnVelocity;
    private float Slow = 1;
    private Vector3 BeamDirection;
    private Vector3 BeamPush;
    private bool HitBeam = false;
    private bool _isGroundedPrev;

    /*
    /// <summary>
    /// 移動Action(PlayerInput側から呼ばれる)
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        // 入力値を保持しておく
        _inputMove = context.ReadValue<Vector2>();
    }
    */

    /*
    /// <summary>
    /// ジャンプAction(PlayerInput側から呼ばれる)
    /// </summary>
    public void OnJump(InputAction.CallbackContext context)
    {
        // ボタンが押された瞬間かつ着地している時だけ処理
        if (!context.performed || !_characterController.isGrounded) return;

        // 鉛直上向きに速度を与える
        _verticalVelocity = _jumpSpeed;
    }
    */

    private void Awake()
    {
        Application.targetFrameRate = 60; // 60fpsに設定
        _transform = transform;
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        //地面に立っているかどうか
        var isGrounded = _characterController.isGrounded;

        //移動判定
        var _inputMove = new Vector3(Input.GetAxis(horizontalString), 0f, -Input.GetAxis(verticalString));

        //ジャンプ判定
        if(Input.GetButtonDown(jumpString)&&isGrounded)
        {
            _verticalVelocity= _jumpSpeed;
        }

        if (isGrounded && !_isGroundedPrev)
        {
            // 着地する瞬間に落下の初速を指定しておく
            _verticalVelocity = -_initFallSpeed;
        }
        else if (!isGrounded)
        {
            // 空中にいるときは、下向きに重力加速度を与えて落下させる
            _verticalVelocity -= _gravity * Time.deltaTime;

            // 落下する速さ以上にならないように補正
            if (_verticalVelocity < -_fallSpeed)
                _verticalVelocity = -_fallSpeed;
        }

        if(Slow>1.0f)
        {
            Slow -= 1f / 180f;
        }
        if(Slow<=1.0f)
        {
            Slow = 1.0f;
        }

        _isGroundedPrev = isGrounded;
        
        // 操作入力と鉛直方向速度から、現在速度を計算
        var moveVelocity = new Vector3(
            _inputMove.x * _speed/Slow,
            _verticalVelocity/Slow,
            _inputMove.z * _speed/Slow
        );

        // 現在フレームの移動量を移動速度から計算
        var moveDelta = moveVelocity * Time.deltaTime;

        // CharacterControllerに移動量を指定し、オブジェクトを動かす
        _characterController.Move(moveDelta);

        if(HitBeam)
        {
            BeamPush = BeamDirection * _speed * 1.5f * Time.deltaTime;
            _characterController.Move(BeamPush);
        }

        if (_inputMove != Vector3.zero)
        {
            
            // 移動入力がある場合は、振り向き動作も行う

            // 操作入力からy軸周りの目標角度[deg]を計算
            var targetAngleY = -Mathf.Atan2(_inputMove.z, _inputMove.x)
                * Mathf.Rad2Deg + 90;

            // イージングしながら次の回転角度[deg]を計算
            var angleY = Mathf.SmoothDampAngle(
                _transform.eulerAngles.y,
                targetAngleY,
                ref _turnVelocity,
                0.1f
            );

            // オブジェクトの回転を更新
            _transform.rotation = Quaternion.Euler(0, angleY, 0);
        }

        if(GroundHeight.transform.position.y-10>_transform.position.y)
        {
            Destroy(myGameObject);
        }
    }

    //水球を当てられたかどうか
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name=="WaterSmallObj")
        {
            Slow = 2f;
            Debug.Log("hit");
        }
    }
}
