using UnityEngine;
using Photon;
using UnityEngine.UI;

public class TCP_LobbyManager : PunBehaviour
{
    [SerializeField]
    Text connectionStatusText;
    [SerializeField]
    InputField nameField;
    [SerializeField]
    GameObject lobbyPanel;
    [SerializeField]
    Button btJoinRoom;
    [SerializeField]
    TCP_TdmManager tdmManager;

    void Start ()
    {
        PhotonNetwork.ConnectUsingSettings("0.1");
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        btJoinRoom.interactable = true;
    }

    public void JoinMatch()//função será chamada ao apertar o botão na UI
    {
        if(nameField.text != string.Empty)
        {

            PhotonNetwork.player.NickName = nameField.text;

            PhotonNetwork.JoinOrCreateRoom("Partida1", tdmManager.DeathMatchConfiguration(), TypedLobby.Default);
        }
        else
        {
            nameField.placeholder.GetComponent<Text>().text = "Digite seu nick antes de entrar na sala!";
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        lobbyPanel.SetActive(false);
    }

    void Update ()
    {
        connectionStatusText.text = PhotonNetwork.connectionStateDetailed.ToString();
	}
}
