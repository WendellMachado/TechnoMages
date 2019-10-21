using UnityEngine;
using UnityEngine.UI;
using Photon;

public class TCP_Skills_SepeTackle : TCP_Skill_Core
{
    [SerializeField]
    float sprintBoost, bindTime;

    protected override void UseSkill()
    {
        this.GetComponent<Rigidbody>().velocity = transform.forward * sprintBoost;
    }

    void OnCollisionEnter(Collision c)
    {
        if(timeUsed > 0)
        {
            if (c.gameObject.GetComponent<TCP_Character>() && c.gameObject.GetComponent<PhotonView>().owner.GetTeam() != this.gameObject.GetComponent<PhotonView>().owner.GetTeam())
            {
                c.gameObject.GetComponent<PhotonView>().RPC("Bind", PhotonTargets.All, bindTime);
            }
            StopUsing();
        }
    }
}
