using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WireController : MonoBehaviour
{
    [SerializeField] MiniGameManager3 gameManager;

    [SerializeField] List<GameObject> aliveWires;
    [SerializeField] List<GameObject> deathWires;
    int deathCnt;

    public enum COLOR_TYPE_ID
    {
        Red = 0,
        Blue,
        Green,
    }
    public List<int> colorTypeList { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        deathCnt = 0;
    }

    /// <summary>
    /// ワイヤーの色の抽選を行う
    /// </summary>
    public void DrawRandomColor()
    {
        // COLOR_TYPE_IDを参照して用意
        colorTypeList = new List<int> { 0, 1, 2 };

        // リストの中身をシャッフルする(降順)
        for (int i = colorTypeList.Count - 1; i > 0; i--)
        {
            var j = Random.Range(0, i + 1);
            var temp = colorTypeList[i];
            colorTypeList[i] = colorTypeList[j];
            colorTypeList[j] = temp;
        }

        SetupWires();
    }

    /// <summary>
    /// ワイヤーのセットアップを行う
    /// </summary>
    void SetupWires()
    {
        for (int i = 0; i < aliveWires.Count; i++)
        {
            deathWires[i].SetActive(false);
            aliveWires[i].SetActive(true);
            switch (colorTypeList[i])
            {
                case (int)COLOR_TYPE_ID.Red:
                    aliveWires[i].GetComponent<Image>().color = Color.red;
                    deathWires[i].GetComponent<Image>().color = Color.red;
                    break;
                case (int)COLOR_TYPE_ID.Blue:
                    aliveWires[i].GetComponent<Image>().color = Color.blue;
                    deathWires[i].GetComponent<Image>().color = Color.blue;
                    break;
                case (int)COLOR_TYPE_ID.Green:
                    aliveWires[i].GetComponent<Image>().color = Color.green;
                    deathWires[i].GetComponent<Image>().color = Color.green;
                    break;
            }
        }
    }

    /// <summary>
    /// ワイヤーを切る処理
    /// </summary>
    public void OnToggleWirebutton(int i)
    {
        if (gameManager.isGameEnd || gameManager.isPause) return;

        aliveWires[i].SetActive(false);
        deathWires[i].SetActive(true);

        // 正しい順番でワイヤーを切ったか判定する
        if (!JudgeWireColor(i))
        {
            gameManager.GameOver();
            return;
        }

        deathCnt++;
        if (deathCnt >= deathWires.Count)
        {
            // 正しい順番で全てのワイヤーを切ったら次のラウンドを準備する
            deathCnt = 0;
            gameManager.SetupNextRound();
        }
    }

    /// <summary>
    /// 正しい順番で切れたか判定する
    /// </summary>
    bool JudgeWireColor(int i)
    {
        bool isSuccess = true;
        switch (gameManager.colorIndexOrders[0])
        {
            case (int)COLOR_TYPE_ID.Red:
                if (aliveWires[i].GetComponent<Image>().color != Color.red) isSuccess = false;
                break;
            case (int)COLOR_TYPE_ID.Blue:
                if (aliveWires[i].GetComponent<Image>().color != Color.blue) isSuccess = false;
                break;
            case (int)COLOR_TYPE_ID.Green:
                if (aliveWires[i].GetComponent<Image>().color != Color.green) isSuccess = false;
                break;
        }

        gameManager.colorIndexOrders.RemoveAt(0);

        return isSuccess;
    }
}
