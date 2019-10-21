using UnityEngine;
using Photon;
using UnityEngine.UI;

public class TCP_Skills_SepeLighting : TCP_Skill_Core
{
    [SerializeField]
    Camera cam;
    [SerializeField]
    float range;

    protected override void UseSkill()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
        {
            if(hit.collider.gameObject.GetComponent<TCP_Character>())
            {
                GameObject lighting = PhotonNetwork.Instantiate("SepeLighting", hit.point, Quaternion.identity, 0);
                lighting.GetComponent<TCP_SkillsObjects>().PlayerOwner = PhotonNetwork.player;
                putOnCooldown = true;
            }
        }
    }
}
