using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireBullet : MonoBehaviour
{
    [SerializeField]
    [Tooltip("�e�̔��ˏꏊ")]
    private GameObject firingPoint;

    [SerializeField]
    [Tooltip("�e")]
    public GameObject WaterSmall;

    [SerializeField]
    [Tooltip("���[�U�[")]
    public GameObject WaterBeam;

    public Vector3 effectNormal;

    [SerializeField]
    [Tooltip("�e�̑���")]
    private float speed = 30f;

    //Action���C���X�y�N�^�[����ҏW�ł���悤�ɂ���
    private bool _fire = false;

    // Update is called once per frame
    void Update()
    {
        if(_fire)
        {
            _fire = false;
            // �e�𔭎˂���
            BulletShot();
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        _fire = true;
    }

    public void OnBeam(InputAction.CallbackContext context)
    {
        BeamShot();
    }

    /// <summary>
	/// �e�̔���
	/// </summary>
    private void BulletShot()
    {        
        // �e�𔭎˂���ꏊ���擾
        Vector3 bulletPosition = firingPoint.transform.position;
        // ��Ŏ擾�����ꏊ�ɁA"bullet"��Prefab���o��������
        GameObject newBall = Instantiate(WaterSmall, bulletPosition, transform.rotation);
        // �o���������{�[����forward(z������)
        Vector3 direction = newBall.transform.forward;
        // �e�̔��˕�����newBall��z����(���[�J�����W)�����A�e�I�u�W�F�N�g��rigidbody�ɏՌ��͂�������
        newBall.GetComponent<Rigidbody>().AddForce(direction * speed, ForceMode.Impulse);
        // �o���������{�[���̖��O��"bullet"�ɕύX
        newBall.name = WaterSmall.name;
        // �o���������{�[����0.8�b��ɏ���
        Destroy(newBall, 2.0f);
    }

    private void BeamShot()
    {

    }
}
