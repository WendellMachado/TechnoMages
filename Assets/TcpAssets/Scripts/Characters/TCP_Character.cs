using UnityEngine;
using System.Collections.Generic;
using Photon;
using UnityEngine.UI;

public class TCP_Character : RigidbodyFirstPersonController
{

    [SerializeField]
    Text fragText, ticketsText;

    void UpdateFragHud()
    {
        fragText.text = "Your Frag: " + PhotonNetwork.player.CustomProperties["Kills"].ToString() + "/" + PhotonNetwork.player.CustomProperties["Deaths"].ToString();
        ticketsText.text = "Blue team tickets: " + PhotonNetwork.room.CustomProperties["BlueTickets"].ToString() + "\n" +
        "Red team tickets: " + PhotonNetwork.room.CustomProperties["RedTickets"];
    }

    protected override void Start()
    {
        base.Start();
        ticketsText = GameObject.Find("TicketsText").GetComponent<Text>();
    }

    protected override void Update()
    {
        if (!photonView.isMine) { return; }

        base.Update();
        if (this.transform.position.y <= -10)
        {
            this.gameObject.GetComponent<PhotonView>().RPC("ChangeHealth", PhotonTargets.All, -9999f, false, this.GetComponent<PhotonView>().owner.ID);
        }
        UpdateFragHud();
    }

    protected override void FixedUpdate()
    {
        if (!photonView.isMine) { return; }

        base.FixedUpdate();
    }
}
