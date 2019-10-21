using UnityEngine;
using System.Collections.Generic;
using Photon;

public class TCP_SkillsObjects : PunBehaviour
{
    public PunTeams Team { get; set; }
    public PhotonPlayer PlayerOwner { get; set; }


    protected virtual void DestroyMe()
    {
        if (this.gameObject != null)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

}
