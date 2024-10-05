using UnityEngine;
using System.Linq;
using Photon.Pun;
using TMPro;
using Photon.Pun.UtilityScripts;

public class Leaderboard : MonoBehaviour
{
    [Header("Options")]
    [SerializeField] private float refreshRate = 1f;

    [Header("Options")]
    [SerializeField] private GameObject[] slots;
    [SerializeField] private TextMeshProUGUI[] scoreTexts;
    [SerializeField] private TextMeshProUGUI[] nameTexts;
    [SerializeField] private TextMeshProUGUI[] kdTexts;


    private void Start() {
        InvokeRepeating(nameof(Refresh), 1f, refreshRate);
    }

    private void Refresh() {
        foreach (var slot in slots) {
            slot.SetActive(false);
        }

        var sortedPlayerList = (from player in PhotonNetwork.PlayerList orderby player.GetScore() descending select player).ToList();

        int i = 0;
        foreach (var player in sortedPlayerList) {
            slots[i].SetActive(true);

            if (player.NickName == "")
                player.NickName = "?";

            nameTexts[i].text = player.NickName;
            scoreTexts[i].text = player.GetScore().ToString();

            if (player.CustomProperties["kills"] != null)
                kdTexts[i].text = player.CustomProperties["kills"] + "/" + player.CustomProperties["deaths"];
            else
                kdTexts[i].text = "0/0";

            i++;
        }
    }
}
