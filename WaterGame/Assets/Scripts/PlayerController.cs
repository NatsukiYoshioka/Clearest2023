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

    [Header("�ړ��̑���"), SerializeField]
    private float _speed = 3;

    [Header("�W�����v����u�Ԃ̑���"), SerializeField]
    private float _jumpSpeed = 7;

    [Header("�d�͉����x"), SerializeField]
    private float _gravity = 15;

    [Header("�������̑��������iInfinity�Ŗ������j"), SerializeField]
    private float _fallSpeed = 10;

    [Header("�����̏���"), SerializeField]
    private float _initFallSpeed = 2;

    [Header("�n�ʂ̈ʒu"),SerializeField]
    private GameObject GroundHeight;

    [Header("�v���C���[�I�u�W�F�N�g"), SerializeField]
    private GameObject myGameObject;

    private Transform _transform;
    private CharacterController _characterController;

    //private Vector2 _inputMove;
    private float _verticalVelocity;
    private float _turnVelocity;
    private float Slow = 1;
    private float RecoverNameTime = 0f;
    private bool _isGroundedPrev;

    /*
    /// <summary>
    /// �ړ�Action(PlayerInput������Ă΂��)
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        // ���͒l��ێ����Ă���
        _inputMove = context.ReadValue<Vector2>();
    }
    */

    /*
    /// <summary>
    /// �W�����vAction(PlayerInput������Ă΂��)
    /// </summary>
    public void OnJump(InputAction.CallbackContext context)
    {
        // �{�^���������ꂽ�u�Ԃ����n���Ă��鎞��������
        if (!context.performed || !_characterController.isGrounded) return;

        // ����������ɑ��x��^����
        _verticalVelocity = _jumpSpeed;
    }
    */

    private void Awake()
    {
        Application.targetFrameRate = 60; // 60fps�ɐݒ�
        _transform = transform;
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        //�n�ʂɗ����Ă��邩�ǂ���
        var isGrounded = _characterController.isGrounded;

        //�ړ�����
        var _inputMove = new Vector3(Input.GetAxis(horizontalString), 0f, -Input.GetAxis(verticalString));

        //�W�����v����
        if(Input.GetButtonDown(jumpString)&&isGrounded)
        {
            _verticalVelocity= _jumpSpeed;
        }

        if (isGrounded && !_isGroundedPrev)
        {
            // ���n����u�Ԃɗ����̏������w�肵�Ă���
            _verticalVelocity = -_initFallSpeed;
        }
        else if (!isGrounded)
        {
            // �󒆂ɂ���Ƃ��́A�������ɏd�͉����x��^���ė���������
            _verticalVelocity -= _gravity * Time.deltaTime;

            // �������鑬���ȏ�ɂȂ�Ȃ��悤�ɕ␳
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
        
        // ������͂Ɖ����������x����A���ݑ��x���v�Z
        var moveVelocity = new Vector3(
            _inputMove.x * _speed/Slow,
            _verticalVelocity/Slow,
            _inputMove.z * _speed/Slow
        );

        // ���݃t���[���̈ړ��ʂ��ړ����x����v�Z
        var moveDelta = moveVelocity * Time.deltaTime;

        // CharacterController�Ɉړ��ʂ��w�肵�A�I�u�W�F�N�g�𓮂���
        _characterController.Move(moveDelta);

        if (_inputMove != Vector3.zero)
        {
            
            // �ړ����͂�����ꍇ�́A�U�����������s��

            // ������͂���y������̖ڕW�p�x[deg]���v�Z
            var targetAngleY = -Mathf.Atan2(_inputMove.z, _inputMove.x)
                * Mathf.Rad2Deg + 90;

            // �C�[�W���O���Ȃ��玟�̉�]�p�x[deg]���v�Z
            var angleY = Mathf.SmoothDampAngle(
                _transform.eulerAngles.y,
                targetAngleY,
                ref _turnVelocity,
                0.1f
            );

            // �I�u�W�F�N�g�̉�]���X�V
            _transform.rotation = Quaternion.Euler(0, angleY, 0);
        }

        if(myGameObject.transform.GetChild(2).gameObject.transform.name!="BeamHitName"&&RecoverNameTime==0)
        {
            RecoverNameTime = Time.time;
        }
        if(Time.time-RecoverNameTime>=3.0f)
        {
            myGameObject.transform.GetChild(2).gameObject.transform.name = "BeamHitName";
            RecoverNameTime = 0f;
        }

        if(GroundHeight.transform.position.y-10>_transform.position.y)
        {
            GameManager gamemanager;
            GameObject manager= GameObject.Find("GameManager");
            gamemanager = manager.GetComponent<GameManager>();
            switch(myGameObject.transform.GetChild(2).gameObject.transform.name)
            {
                case "Player1":
                    gamemanager.Player1Point++;
                    break;
                case "Player2":
                    gamemanager.Player2Point++;
                    break;
                case "Player3":
                    gamemanager.Player3Point++;
                    break;
                case "Player4":
                    gamemanager.Player4Point++;
                    break;
            }
            Destroy(myGameObject);
        }
    }

    //�����𓖂Ă�ꂽ���ǂ���
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Ball")
        {
            switch(collision.gameObject.name)
            {
                case "Player1":
                    myGameObject.transform.GetChild(2).gameObject.transform.name = "Player1";
                    break;
                case "Player2":
                    myGameObject.transform.GetChild(2).gameObject.transform.name = "Player2";
                    break;
                case "Player3":
                    myGameObject.transform.GetChild(2).gameObject.transform.name = "Player3";
                    break;
                case "Player4":
                    myGameObject.transform.GetChild(2).gameObject.transform.name = "Player4";
                    break;
            }
            Slow = 2f;
            Debug.Log("hit");
        }
    }
}
