using UnityEngine;
using UnityEngine.UI;
using Photon;

public abstract class TCP_Skill_Core : PunBehaviour
{
    [SerializeField]
    protected float cooldownTime, useTime; //floats do tempo de cooldown e o tempo maximo para castar continuamente a skill.

    [SerializeField]
    protected string skillButton; //string que identifica o botão configurado no input do unity para ativar a skill

    [SerializeField]
    protected CastTypes castType; //o tipo de cast da skill.

    [SerializeField]
    protected Image skillFeedback; //objeto que receberá a imagem da HUD a ser atualizada na função UpdateCooldownFeedback.

    [SerializeField]
    protected TCP_Character character;

    [SerializeField]
    protected PhotonView pView;

    protected bool putOnCooldown = false;

    protected enum CastTypes { SingleCast, ContinuousCast, ToggleCast }; //enumerado dos tipos de cast das skills.

    protected float timeUsed; //float que informa por quanto tempo o jgoador está castando uma skill. 

    protected bool canUse = true; //bool que marca se a skill está em cooldown ou não.

    protected bool silenced, toggled = false; //bool que determina se o personagem tem o efeito de status silence, impedindo que ele use skills.

    public bool Silenced
    {
        get { return silenced; }
        set { silenced = value; }
    } //get set da variavel silenced.

    /// <summary>
    /// Metodo abstrato que será o funcionamento central de cada skill. Basta dar um override para usar.
    /// </summary>
    protected abstract void UseSkill();

    protected virtual void StopUsing()
    {
        this.canUse = false;
        timeUsed = 0;
        this.skillFeedback.fillAmount = 1.0f;
        Invoke("ResetCooldown", this.cooldownTime);
    } //função usada caso a skill precise ser cancelada durante o cast.

    protected virtual void ResetCooldown()
    {
        canUse = true;
        putOnCooldown = false;
    } //função usada no invoke para resetar o cooldown.

    protected virtual void CheckInputSingleCast()
    {
        if(!photonView.isMine)
        {
            return;
        }//testa se o input é do jogador local, se não for ele retorna sem executar o resto do código.

        if(Input.GetButtonDown(this.skillButton) && this.canUse && !silenced)
        {
            UseSkill();
            if (!putOnCooldown) { return; }
            this.canUse = false;
            this.skillFeedback.GetComponent<Image>().fillAmount = 1.0f;
            Invoke("ResetCooldown", this.cooldownTime);
        }
    } //check input que será chamado caso a skill possa ser ativada somente uma vez.

    protected virtual void CheckInputContinuousCast()
    {
        if (!photonView.isMine)
        {
            return;
        }//testa se o input é do jogador local, se não for ele retorna sem executar o resto do código.

        if (Input.GetButton(this.skillButton) && this.canUse && !silenced)
        {
            timeUsed += Time.deltaTime;//não uso o invoke, pois ele acumula.
            UseSkill();

            if (timeUsed >= useTime) { StopUsing(); }
        }
        if (Input.GetButtonUp(this.skillButton) && this.canUse)
        {
            StopUsing();
        }
    }//check input que será chamado caso a skill possa ser ativada continuamente.

    protected virtual void CheckInputToggleCast()
    {
        if (!photonView.isMine)
        {
            return;
        }//testa se o input é do jogador local, se não for ele retorna sem executar o resto do código.

        if(Input.GetButtonDown(this.skillButton) && !silenced && !toggled && canUse)
        {
            UseSkill();
            toggled = true;
        }
        else if (Input.GetButtonDown(this.skillButton) && toggled)
        {
            toggled = false;
            DisableToggleSkill();
        }
    } //check input que será chamado caso a skill seja do tipo toggle

    protected virtual void DisableToggleSkill()
    {

    } //função chamada ao desativar as skills do tipo toggle

    protected virtual void Update()
    {
        if(castType == CastTypes.SingleCast) { CheckInputSingleCast(); }
        else if (castType == CastTypes.ContinuousCast) { CheckInputContinuousCast(); }
        else { CheckInputToggleCast(); }

        UpdateCooldownFeedback();
    }

    protected virtual void UpdateCooldownFeedback()
    {
        if(!canUse)
        {
            this.skillFeedback.fillAmount -= 1.0f / cooldownTime * Time.deltaTime;
        }
    } //atualiza a hud de skills do jogador.
}
