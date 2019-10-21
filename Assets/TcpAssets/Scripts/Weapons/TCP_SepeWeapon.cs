using UnityEngine;
using Photon;
using System;

public class TCP_SepeWeapon : TCP_Weapon_Core
{
    //esse script está acoplado ao modelo da arma e não ao personagem(fugindo do padrão do projeto), por ser uma arma melee. Foi o modo encontrado de detectar a colisão em um único script.

    [SerializeField]
    GameObject owner;

    [PunRPC]
    protected override void PrimaryFire()
    {
        animator.SetTrigger("BasicAttack");
    }

    void OnTriggerEnter(Collider c)
    {
        if(c.gameObject.GetComponent<TCP_CharactersHealth>() && c.gameObject != owner && animator.GetCurrentAnimatorStateInfo(0).IsName("SepeBasicAttackAnimation")
            && c.gameObject.GetComponent<PhotonView>().owner.GetTeam() != owner.GetComponent<PhotonView>().owner.GetTeam())
        {//me certifico que o dano só será aplicado caso a colisão não seja com o próprio jogador E o estado de animação seja o de ataque.
         //agora checo se o time do jogador atingido é diferente do time do agressor

            crosshairHit.enabled = true;
            Invoke("DisableHitFeedback", fireDelay / 2);
            c.gameObject.GetComponent<PhotonView>().RPC("ChangeHealth", PhotonTargets.AllBuffered, damage/2, false, PhotonNetwork.player.ID);
        }
    }
}
