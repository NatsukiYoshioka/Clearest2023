using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private GameObject Player1SponePoint;

    [SerializeField]
    private GameObject Player2SponePoint;

    [SerializeField]
    private GameObject Player3SponePoint;

    [SerializeField]
    private GameObject Player4SponePoint;

    [SerializeField]
    private GameObject RandamSponePoint;

    [Header("地面の位置"), SerializeField]
    private GameObject GroundHeight;

    private float Player1Count = 0;
    private float Player2Count = 0;
    private float Player3Count = 0;
    private float Player4Count = 0;

    [System.NonSerialized]
    public int Player1Point = 0;

    [System.NonSerialized]
    public int Player2Point = 0;

    [System.NonSerialized]
    public int Player3Point = 0;

    [System.NonSerialized]
    public int Player4Point = 0;
    void Start()
    {
        Instantiate(Player1, Player1SponePoint.transform.position, Quaternion.identity);
        Instantiate(Player2, Player2SponePoint.transform.position, Quaternion.identity);
        Instantiate(Player3, Player3SponePoint.transform.position, Quaternion.identity);
        Instantiate(Player4, Player4SponePoint.transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        float x = Random.Range(SponeMaxLeft, SponeMaxRight);
        float z = Random.Range(SponeMaxDown, SponeMaxUp);
        RandamSponePoint.transform.position = new Vector3(x, GroundHeight.transform.position.y + 20, z);
        CheckIsExists();
    }

    public void CheckIsExists()
    {
        var Player1Survive = GameObject.Find("Player1(Clone)");
        var Player2Survive = GameObject.Find("Player2(Clone)");
        var Player3Survive = GameObject.Find("Player3(Clone)");
        var Player4Survive = GameObject.Find("Player4(Clone)");

        if (Player1Survive==null && Player1Count == 0)
        {
            Player1Count = Time.time;
        }
        if(Player1Count!=0&&Time.time-Player1Count>=5.0f)
        {
            Instantiate(Player1, RandamSponePoint.transform.position, Quaternion.identity);
            Player1Count = 0;
        }

        if (Player2Survive == null && Player2Count == 0)
        {
            Player2Count = Time.time;
            Debug.Log(Player1Point);
        }
        if (Player2Count != 0 && Time.time - Player2Count >= 5.0f)
        {
            Instantiate(Player2, RandamSponePoint.transform.position, Quaternion.identity);
            Player2Count = 0;
        }

        if (Player3Survive == null && Player3Count == 0)
        {
            Player3Count = Time.time;
        }
        if (Player3Count != 0 && Time.time - Player3Count >= 5.0f)
        {
            Instantiate(Player3, RandamSponePoint.transform.position, Quaternion.identity);
            Player3Count = 0;
        }

        if (Player4Survive == null && Player4Count == 0)
        {
            Player4Count = Time.time;
        }
        if (Player4Count != 0 && Time.time - Player4Count >= 5.0f)
        {
            Instantiate(Player4, RandamSponePoint.transform.position, Quaternion.identity);
            Player4Count = 0;
        }
    }
}
