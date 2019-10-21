using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon;

public class TCP_SpawnManager : PunBehaviour
{
    [SerializeField]
    Transform[] spawnPoints;

    [SerializeField]
    Camera lobbyCamera;

    [SerializeField]
    GameObject deathFeedback, characterSelectScreen, generalCanvas;

    [SerializeField]
    TCP_MessagingSystem messageManager;

    [SerializeField]
    TCP_TdmManager tdmManager;

    string characterSelected;

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        generalCanvas.SetActive(true);
        JoinTeam();
        messageManager.AddMessage(PhotonNetwork.player.NickName.ToString() + " entrou na partida pelo time " + PhotonNetwork.player.GetTeam().ToString());//ao entrar na sala, mando uma mensagem pelo sistema de mensagens, indicando nome e time do novo jogador
        characterSelectScreen.SetActive(true);//quando o jogador entra na sala, a seleção de personagens é ativada.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void JoinTeam()
    {
        if(PunTeams.PlayersPerTeam[PunTeams.Team.red].Count >= PunTeams.PlayersPerTeam[PunTeams.Team.blue].Count)
        {
            PhotonNetwork.player.SetTeam(PunTeams.Team.blue);
        }
        else
        {
            PhotonNetwork.player.SetTeam(PunTeams.Team.red);
        }
        PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "Kills", 0 } });
        PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "Deaths", 0 } });
    }

    public void SelectCharacter(string character)//essa função é chamada no evento de click dos botões da interface.
    {
        characterSelected = character;
        SpawnPlayer();
    }

   void PreparePlayerRespawn(float respawnTime, int killerId, int killedId)//quando o jogador morre, se depara com o feedback de morte.
    {
        messageManager.AddMessage(PhotonPlayer.Find(killerId).NickName + " matou " + PhotonPlayer.Find(killedId).NickName);

        tdmManager.UpdatePlayerKda(PhotonPlayer.Find(killerId), KillOrDeath.Kill);
        tdmManager.UpdatePlayerKda(PhotonPlayer.Find(killedId), KillOrDeath.Death);//adicionando e removendo pontos do assassino e assassinado, respectivamente.

        lobbyCamera.enabled = true;
        deathFeedback.SetActive(true);
        Invoke("RespawnPlayer", respawnTime);
    }

    void RespawnPlayer()
    {
        deathFeedback.SetActive(false);//ao "renascer", o jogador escolhe novamente o personagem que irá usar.
        characterSelectScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    void SpawnPlayer()//aqui o player é (re)spawnado de fato, sendo instanciado no cenario.
    {
        characterSelectScreen.SetActive(false);
        GameObject player;

        if (PhotonNetwork.player.GetTeam() == PunTeams.Team.blue)
        {
            player = PhotonNetwork.Instantiate(characterSelected, spawnPoints[0].position, Quaternion.identity, 0);
        }
        else
        {
            player = PhotonNetwork.Instantiate(characterSelected, spawnPoints[1].position, Quaternion.identity, 0);
        }
        
        player.GetComponent<TCP_CharactersHealth>().RespawnMe += PreparePlayerRespawn;// atribui a função startspawnprocess ao evento respawme do script do player.
        lobbyCamera.enabled = false;
    }
}
