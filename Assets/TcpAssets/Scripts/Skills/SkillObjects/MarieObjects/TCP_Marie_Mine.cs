using UnityEngine;
using System.Collections.Generic;
using Photon;


public class TCP_Marie_Mine : TCP_SkillsObjects
{	
	[SerializeField]
	float bindTime ,lifeTime, damage;

    bool slowed = false;//booleana usada para impedir que o slow seja reaplicado ao msm player.

    TCP_Skills_MarieMine owner;//referência da skill que plantou a mina

    void Start()
    {
        Invoke("DestroyMe", lifeTime);//ao startar, já chamo a função para destruir a mina se o seu tempo de vida acabar
    }

    public TCP_Skills_MarieMine SetOwner
    {
        set { owner = value; }
    }

    protected override void DestroyMe()
    {
        if(this.gameObject != null)
        {
            owner.setMinesPlaced--;//atualizo a variavel minesPlaced do script da skill que plantou a mina

            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.GetComponent<TCP_Character>() && !slowed 
            && c.gameObject.GetComponent<PhotonView>().owner.GetTeam() != this.PlayerOwner.GetTeam())
        {
            c.gameObject.GetComponent<TCP_StatusEffects>().Bind(this.bindTime);
            c.gameObject.GetComponent<PhotonView>().RPC("ChangeHealth", PhotonTargets.AllBuffered, damage, false, PlayerOwner.ID);

            slowed = true; //ao pisar na mina uma vez, o mesmo player n receberá slow da mina novamente.

            DestroyMe();
        }
    }


}