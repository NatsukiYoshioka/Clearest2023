using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FireBullet : MonoBehaviour
{
    [SerializeField]
    [Tooltip("弾の発射場所")]
    private GameObject firingPoint;

    [SerializeField]
    [Tooltip("弾")]
    public GameObject WaterSmall;

    public Slider waterTank;

    public Vector3 effectNormal;

    public float waterFeed;
    [SerializeField]
    [Tooltip("弾の速さ")]
    private float speed = 30f;

    //Actionをインスペクターから編集できるようにする
    private bool _fire = false;
    private bool _beam = false;

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
        if (_fire && waterTank.value >= 10.0f)
        {
            _fire = false;
            // 弾を発射する
            BulletShot();
        }
    }

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
    public void OnFire(InputAction.CallbackContext context)
    {
        if (!context.performed||_beam) return;
        _fire = true;
    }

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
