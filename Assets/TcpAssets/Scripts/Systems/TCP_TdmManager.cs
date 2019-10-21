using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;

public enum KillOrDeath { Kill, Death }

public class TCP_TdmManager : PunBehaviour
{
    [SerializeField]
    int teamTickets;

    public void UpdatePlayerKda(PhotonPlayer player, KillOrDeath killOrDeath)
    {
        if(killOrDeath == KillOrDeath.Kill)
        {
            ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable() { { "Kills", (int)player.CustomProperties["Kills"] + 1 } };
            player.SetCustomProperties(hashtable);
        }

        else if(killOrDeath == KillOrDeath.Death)
        {
            DeduceTickets(player);
            ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable() { { "Deaths", (int)player.CustomProperties["Deaths"] + 1 } };
            player.SetCustomProperties(hashtable);
        }
    }

    public RoomOptions DeathMatchConfiguration()
    {
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
        props.Add("Ended", false);
        props.Add("BlueTickets", teamTickets);
        props.Add("RedTickets", teamTickets);
        props.Add("VictorTeam", 4);
        props.Add("LoserTeam", 4);

        RoomOptions roomConfig;
        roomConfig = new RoomOptions();
        roomConfig.IsOpen = true;
        roomConfig.IsVisible = true;
        roomConfig.MaxPlayers = 10;
        roomConfig.CustomRoomProperties = props;

        return roomConfig;
    }

    void DeduceTickets(PhotonPlayer player)
    {
        if (player.GetTeam() == PunTeams.Team.blue)
        {
            ExitGames.Client.Photon.Hashtable hashtableRoom = new ExitGames.Client.Photon.Hashtable() { { "BlueTickets", (int)PhotonNetwork.room.CustomProperties["BlueTickets"] - 1 } };
            PhotonNetwork.room.SetCustomProperties(hashtableRoom);
        }
        else
        {
            ExitGames.Client.Photon.Hashtable hashtableRoom = new ExitGames.Client.Photon.Hashtable() { { "RedTickets", (int)PhotonNetwork.room.CustomProperties["RedTickets"] - 1 } };//copiar a linha de cima pra drenar os tickets da equipe cujo membro morreu
            PhotonNetwork.room.SetCustomProperties(hashtableRoom);
        }

        if ((int)PhotonNetwork.room.CustomProperties["BlueTickets"] <= 0)
        {
            ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
            props.Add("VictorTeam", PunTeams.Team.blue);
            props.Add("LoserTeam", PunTeams.Team.red);
            props.Add("Ended", true);
            PhotonNetwork.room.SetCustomProperties(props);
        }
        else if((int)PhotonNetwork.room.CustomProperties["RedTickets"] <= 0)
        {
            ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
            props.Add("VictorTeam", PunTeams.Team.red);
            props.Add("LoserTeam", PunTeams.Team.blue);
            props.Add("Ended", true);
            PhotonNetwork.room.SetCustomProperties(props);
        }
    }

    void Update()
    {
        
    }
}
