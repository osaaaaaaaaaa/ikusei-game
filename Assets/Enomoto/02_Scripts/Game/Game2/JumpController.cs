using KanKikuchi.AudioManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpController : MonoBehaviour
{
    [SerializeField] MiniGameManager2 gameManager;
    [SerializeField] LayerMask layer;
    GameObject playerObj;
    Rigidbody2D rb2D;
    bool isInit = false;
    public int currrentJumpCnt;
    public float jumpPower;

    const int jumpCntMax = 2;

    private void Start()
    {
        currrentJumpCnt = 0;
    }

    void Update()
    {
        if (!isInit || gameManager.isGameOver || gameManager.isGameClear) return;
        if (IsGround()) currrentJumpCnt = 0;

        if (Input.GetMouseButtonDown(0)
            && currrentJumpCnt < jumpCntMax)
        {
            currrentJumpCnt++;
            Jump();
        }
    }

    public void Init(GameObject _player,Rigidbody2D _rigidbody2D)
    {
        playerObj = _player;
        rb2D = _rigidbody2D;
        isInit = true;
    }

    public void Jump()
    {
        SEManager.Instance.Play(SEPath.JUMP);
        rb2D.velocity = new Vector2(rb2D.velocity.x, 0f);
        rb2D.AddForce(Vector2.up * jumpPower,ForceMode2D.Impulse);
        playerObj.transform.localPosition += new Vector3(0.0f, 0.2f);
    }

    bool IsGround()
    {
        Vector3 basePosition = playerObj.transform.position;    // モンスターのピボットが中心にあるため調整する
        Vector3 leftStartPosition = basePosition - Vector3.right * 0.5f;      // 左側の始点
        Vector3 rightStartPosition = basePosition + Vector3.right * 0.5f;     // 右側の始点
        Vector3 endPosition = basePosition - Vector3.up * 0.2f;               // 終点(下)

        Debug.DrawLine(leftStartPosition, endPosition,Color.red); 
        Debug.DrawLine(rightStartPosition, endPosition, Color.red);

        return Physics2D.Linecast(leftStartPosition, endPosition, layer)
            || Physics2D.Linecast(rightStartPosition, endPosition, layer);
    }
}