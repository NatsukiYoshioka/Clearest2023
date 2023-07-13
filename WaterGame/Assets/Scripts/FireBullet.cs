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
    [Tooltip("弾の発射場所")]
    private GameObject firingPoint;

    [SerializeField]
    [Tooltip("弾")]
    public GameObject WaterSmall;

    private Slider waterTank;
    public string TankName;

    public Vector3 effectNormal;

    public float waterFeed;
    [SerializeField]
    [Tooltip("弾の速さ")]
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
        //水球の発射
        if (Input.GetButtonDown(fire1String) && waterTank.value >= 10.0f && !_beam)
        {
            // 弾を発射する
            BulletShot();
        }

        //ビームが発射されているかの処理
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
	/// 弾の発射
	/// </summary>
    private void BulletShot()
    {
        waterTank.value -= 10.0f;
        // 弾を発射する場所を取得
        Vector3 bulletPosition = firingPoint.transform.position;
        // 上で取得した場所に、"bullet"のPrefabを出現させる
        GameObject newBall = Instantiate(WaterSmall, bulletPosition, transform.rotation);
        //出した弾を出したプレイヤーの名前に変更
        WaterSmall.gameObject.name = _Fire1String;
        // 出現させたボールのforward(z軸方向)
        Vector3 direction = newBall.transform.forward;
        // 弾の発射方向にnewBallのz方向(ローカル座標)を入れ、弾オブジェクトのrigidbodyに衝撃力を加える
        newBall.GetComponent<Rigidbody>().AddForce(direction * speed, ForceMode.Impulse);
        // 出現させたボールの名前を"bullet"に変更
        newBall.name = WaterSmall.name;
        // 出現させたボールを0.8秒後に消す
        Destroy(newBall, 2.0f);
    }
}
