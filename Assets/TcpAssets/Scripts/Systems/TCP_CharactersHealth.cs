using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Photon;

public class TCP_CharactersHealth : PunBehaviour
{
    [SerializeField]
    Text healthText, shieldText;

    [SerializeField]
    Image healthBar, HealFeedback, damageFeedback, shieldBar, otherPlayerHealthBar, otherPlayerShieldBar;//elementos de feedback da ui: barra de vida, indicador de dano, indicador de heal e barra de escudo.

    [SerializeField]
    float maxHealth = 100;

    float health, shield, shieldReceived;

    void Start()
    {
        shield = 0;
        health = maxHealth;
        UpdateHud();
    }

    private IEnumerator FadeOut(Image feedback)
    {
        Color c = feedback.color;

        if (!feedback.enabled)
        {
            c.a = 1f;
            feedback.color = c;
            feedback.enabled = true;
        }
        else
        {
            c.a = 1f;
            feedback.color = c;
        }
        while (feedback.enabled)
        {
            c.a -= Time.deltaTime;
            feedback.color = c;
            if (feedback.color.a <= 0)
            {
                feedback.enabled = false;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    //criação de uma função delegada a um evento. eventos recebem um metódo de outra classe e a executam na classe onde foram criados.
    public delegate void Respawn(float time, int killerId, int killedId);
    public event Respawn RespawnMe;

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(health);
        }
        else if (stream.isReading)
        {
            health = (float)stream.ReceiveNext();
        }
    }

    void UpdateHud()
    {
        healthBar.fillAmount = health / maxHealth;
        shieldBar.fillAmount = shield / shieldReceived;

        otherPlayerHealthBar.fillAmount = health / maxHealth;
        otherPlayerShieldBar.fillAmount = shield / shieldReceived;

        if (shield <= 0)
        {
            healthText.text = Mathf.FloorToInt(health).ToString();
            shieldText.text = string.Empty;
        }
        else
        {
            healthText.text = string.Empty;
            shieldText.text = Mathf.FloorToInt(shield).ToString();
        }

    }

    /// <summary>
    /// Declaração de função RPC que altera a vida de um jogador, chama o feedback de dano e dá respawn no jogador caso sua vida seja menor ou igual a zero
    /// Uma função RPC é um metódo chamado em um cliente e reproduzido em todos os outros pelo servidor, é usado para manter a sincronia de valores como vida e score entre toda a rede.
    /// </summary>
    /// <param name="damage">float que representa a quantidade a ser acrescentada ou retirada da vida do personagem </param>
    /// <param name="addShield">bool que dirá se o valor a ser alterado é ou não na vida do escudo</param>
    [PunRPC]
    public void ChangeHealth(float damage, bool addShield, int attackerId)
    {
        if (damage > 0 && !addShield)
        {
            StartCoroutine(this.FadeOut(this.HealFeedback));
        }
        else if (damage < 0)
        {
            StartCoroutine(this.FadeOut(this.damageFeedback));
        }

        if (!addShield && shield <= 0)// a vida do jogador só será retirada se a booleana addShield for falsa E a vida do escudo for menor ou igual a zero.
        {
            health += damage;

            if (health > maxHealth) { health = maxHealth; }

            UpdateHud();

            if (health <= 0 && photonView.isMine)
            {
                if (RespawnMe != null)
                {
                    RespawnMe(3, attackerId, PhotonNetwork.player.ID);//se o evento delegado de respawn n for nullo, o chamo para ja iniciar o processo de respawn.
                }
                PhotonNetwork.Destroy(this.gameObject);//se a vida do jogador for menor que zero e o jogador for local, uso o metodo do photon de destruir gameobject para destruir o personagem.
            }
        }
        else
        {
            if (damage > 0) { shieldReceived = damage; }
            shield += damage;
            UpdateHud();
        }
    }
}
