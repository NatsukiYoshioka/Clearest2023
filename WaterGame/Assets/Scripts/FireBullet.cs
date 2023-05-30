using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireBullet : MonoBehaviour
{
    [SerializeField]
    [Tooltip("弾の発射場所")]
    private GameObject firingPoint;

    [SerializeField]
    [Tooltip("弾")]
    public GameObject WaterSmall;

    [SerializeField]
    [Tooltip("レーザー")]
    public GameObject WaterBeam;

    public Vector3 effectNormal;

    [SerializeField]
    [Tooltip("弾の速さ")]
    private float speed = 30f;

    //Actionをインスペクターから編集できるようにする
    private bool _fire = false;

    // Update is called once per frame
    void Update()
    {
        if(_fire)
        {
            _fire = false;
            // 弾を発射する
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
	/// 弾の発射
	/// </summary>
    private void BulletShot()
    {        
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

    private void BeamShot()
    {

    }
}
