using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //スポーン位置の最大値
    public float SponeMaxUp;
    public float SponeMaxDown;
    public float SponeMaxRight;
    public float SponeMaxLeft;

    [Header("Player1"), SerializeField]
    private GameObject Player1;

    [Header("Player2"), SerializeField]
    private GameObject Player2;

    [Header("Player3"), SerializeField]
    private GameObject Player3;

    [Header("Player4"), SerializeField]
    private GameObject Player4;

    [SerializeField]
    private GameObject SponePoint;

    public GameObject Ground;

    private float Player1Count = 0;

    // Update is called once per frame
    void Update()
    {
        float x = Random.Range(SponeMaxLeft, SponeMaxRight);
        float z = Random.Range(SponeMaxDown, SponeMaxUp);
        SponePoint.transform.position = new Vector3(x, Ground.transform.position.y + 20, z);
        CheckIsExists();
    }

    public void CheckIsExists()
    {
        var Player1Survive = GameObject.Find("Player1(Clone)");
        if (Player1Survive==null && Player1Count == 0)
        {
            Player1Count = Time.time;
        }
        if(Player1Count!=0&&Time.time-Player1Count>=5.0f)
        {
            Instantiate(Player1, SponePoint.transform.position, Quaternion.identity);
            Player1Count = 0;
        }
    }
}
