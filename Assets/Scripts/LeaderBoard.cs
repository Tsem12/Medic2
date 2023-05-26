using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Dan.Main;
using NaughtyAttributes;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] leaderBoard;
    [SerializeField] TMP_InputField input;
    [SerializeField] TMP_InputField scoreInput;
    [SerializeField] string inputName;
    [SerializeField] int score;

    string publicKey = "be5d39a610e697568baaf723155180febd2e8fc1ff14f9f71c0acb5ea97ce010";

    private void Start()
    {
        GetLeaderBoard();
    }


    public void GetLeaderBoard()
    {
        LeaderboardCreator.GetLeaderboard(publicKey, ((msg) =>
        {
            for (int i = 0; i < msg.Length; i++)
            {
                leaderBoard[i].text = msg[i].Username + " " + msg[i].Score.ToString();
            }

        }));
    }


    public void SetLeaderBoard(string name, int score)
    {
        LeaderboardCreator.UploadNewEntry(publicKey, name, score, ((msg) =>
        {
            GetLeaderBoard();
        }));

    }

    public void AddToLeaderBoard()
    {
        SetLeaderBoard(input.text,int.Parse(scoreInput.text));
    }
}
