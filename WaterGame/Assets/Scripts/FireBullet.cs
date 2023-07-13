using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FireBullet : MonoBehaviour
{
    [SerializeField]
    private string fire1String;

    [SerializeField]
    private string fire2String;

    [SerializeField]
    [Tooltip("�e�̔��ˏꏊ")]
    private GameObject firingPoint;

    [SerializeField]
    [Tooltip("�e")]
    public GameObject WaterSmall;

    private Slider waterTank;
    public string TankName;

    public Vector3 effectNormal;

    public float waterFeed;
    [SerializeField]
    [Tooltip("�e�̑���")]
    private float speed = 30f;

    [SerializeField]
    private string _Fire1String;

    private bool _beam = false;

    void Start()
    {
        waterTank = GameObject.Find(TankName).GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(waterTank.value<=2.0f)
        {
            waterTank.value = 2.0f;
        }
        if (!_beam)
        {
            waterTank.value += waterFeed;
        }
        else if(waterTank.value<=10)
        {
            waterTank.value += waterFeed;
        }
        //�����̔���
        if (Input.GetButtonDown(fire1String) && waterTank.value >= 10.0f && !_beam)
        {
            // �e�𔭎˂���
            BulletShot();
        }

        //�r�[�������˂���Ă��邩�̏���
        if(Input.GetButton(fire2String)&& waterTank.value > 10.0f)
        {
            _beam = true;
        }
        if(Input.GetButtonUp(fire2String))
        {
            _beam = false;
        }
    }

    /*
    public void OnBeam(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                if(waterTank.value>10.0f)
                {
                    _beam = true;
                }
                else if(waterTank.value<=5.0f)
                {
                    _beam = false;
                }
                break;
            case InputActionPhase.Canceled:
                _beam = false;
                break;
        }
    }
    */

    /*
    public void OnFire(InputAction.CallbackContext context)
    {
        if (!context.performed||_beam||waterTank.value<10.0f) return;
        _fire = true;
    }
    */

    /// <summary>
	/// �e�̔���
	/// </summary>
    private void BulletShot()
    {
        waterTank.value -= 10.0f;
        // �e�𔭎˂���ꏊ���擾
        Vector3 bulletPosition = firingPoint.transform.position;
        // ��Ŏ擾�����ꏊ�ɁA"bullet"��Prefab���o��������
        GameObject newBall = Instantiate(WaterSmall, bulletPosition, transform.rotation);
        //�o�����e���o�����v���C���[�̖��O�ɕύX
        WaterSmall.gameObject.name = _Fire1String;
        // �o���������{�[����forward(z������)
        Vector3 direction = newBall.transform.forward;
        // �e�̔��˕�����newBall��z����(���[�J�����W)�����A�e�I�u�W�F�N�g��rigidbody�ɏՌ��͂�������
        newBall.GetComponent<Rigidbody>().AddForce(direction * speed, ForceMode.Impulse);
        // �o���������{�[���̖��O��"bullet"�ɕύX
        newBall.name = WaterSmall.name;
        // �o���������{�[����0.8�b��ɏ���
        Destroy(newBall, 2.0f);
    }
}
