using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;

public class TCP_Skills_GurungTurret : TCP_Skill_Core {

    [SerializeField]
    private TCP_BasicGun gunFire;
    [SerializeField]
    private TCP_Character player;
    [SerializeField]
    private GameObject gunModel, turretModel;

    [SerializeField]
    private float boostDamage;
    [SerializeField]
    private float boostFireDelay;

    private int oMagazine;

    void Start() {
        player = this.GetComponent<TCP_Character>();
        gunFire = this.GetComponent<TCP_BasicGun>();
        canUse = false;
        this.skillFeedback.GetComponent<Image>().fillAmount = 1.0f;
        Invoke("ResetCooldown", this.cooldownTime);
    }

    [PunRPC]
    void EnableTurret()
    {
        gunModel.SetActive(false);
        turretModel.SetActive(true);
    }

    [PunRPC]
    void DisableTurret()
    {
        gunModel.SetActive(true);
        turretModel.SetActive(false);
    }

	protected override void UseSkill()
    {
		oMagazine = gunFire.GetMagSize;

		player.movementSettings.ForwardSpeed /= 10000;
		player.movementSettings.BackwardSpeed /= 10000;
		player.movementSettings.StrafeSpeed /= 10000;

		gunFire.GetMagSize = 0;
		gunFire.GetDamage -= boostDamage;
		gunFire.GetFireDelay /= boostFireDelay;

        pView.RPC("EnableTurret", PhotonTargets.All);

		TCP_Skill_Core[] skills;

		skills = this.GetComponents<TCP_Skill_Core>();

		for(int i = 0; i < skills.Length; i++)
		{
			skills[i].Silenced = true;
		}

	}

    protected override void DisableToggleSkill()
    {
        player.movementSettings.ForwardSpeed *= 10000;
        player.movementSettings.BackwardSpeed *= 10000;
        player.movementSettings.StrafeSpeed *= 10000;

        gunFire.GetDamage += boostDamage;
        gunFire.GetFireDelay *= boostFireDelay;
        gunFire.GetMagSize = oMagazine;

        pView.RPC("DisableTurret", PhotonTargets.All);

        TCP_Skill_Core[] skills;

        skills = this.GetComponents<TCP_Skill_Core>();

        for (int i = 0; i < skills.Length; i++)
        {
            skills[i].Silenced = false;
        }

        this.canUse = false;
        this.skillFeedback.GetComponent<Image>().fillAmount = 1.0f;
        Invoke("ResetCooldown", this.cooldownTime);

    }
}
