using UnityEngine;
using Photon;

public class TCP_SepeLighting : TCP_SkillsObjects
{
    [SerializeField]
    float silenceTime, damage;

    void OnCollisionEnter(Collision c)
    {
        if(c.gameObject.GetComponent<TCP_Character>()
        && c.gameObject.GetComponent<PhotonView>().owner.GetTeam() != this.PlayerOwner.GetTeam())
        {
            c.gameObject.GetComponent<TCP_StatusEffects>().Silence(silenceTime);
            c.gameObject.GetComponent<PhotonView>().RPC("ChangeHealth", PhotonTargets.AllBuffered, damage/2, false, PlayerOwner.ID);
        }
        Invoke("DestroyMe", 0.5f);
    }
}
