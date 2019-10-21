using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TCP_StatusEffects : MonoBehaviour
{
    TCP_Character character;

    void Start()
    {
        character = this.GetComponent<TCP_Character>();
    }

    #region SlowEffect;
    float lastSlowValue;

    public void Slow(float slowValue, float slowTime)
    {
        lastSlowValue = slowValue;

        character.movementSettings.ForwardSpeed /= lastSlowValue;
        character.movementSettings.BackwardSpeed /= lastSlowValue;
        character.movementSettings.StrafeSpeed /= lastSlowValue;

        Invoke("DeSlow", slowTime);
    }

    void DeSlow()
    {

        character.movementSettings.ForwardSpeed *= lastSlowValue;
        character.movementSettings.BackwardSpeed *= lastSlowValue;
        character.movementSettings.StrafeSpeed *= lastSlowValue;

    } //metodo que irá desfazer o efeito do slow
    #endregion

    #region SilenceEffect

    public void Silence(float silenceTime)
    {
        TCP_Skill_Core[] skills;

        skills = this.GetComponents<TCP_Skill_Core>();

        for(int i = 0; i < skills.Length; i++)
        {
            skills[i].Silenced = true;
        }

        Invoke("DeSilence", silenceTime);
    }

    void DeSilence()
    {
        TCP_Skill_Core[] skills;

        skills = this.GetComponents<TCP_Skill_Core>();

        for (int i = 0; i < skills.Length; i++)
        {
            skills[i].Silenced = false;
        }
    }

    #endregion

    #region BindEffect

    [PunRPC]//precisei usar um rpc por não estar instanciando nada, o bind é aplicado de player para player. (idealmente, qualquer coisa que passe de um player para outro deveria ser um rpc)
    public void Bind(float bindTime)
    {
        character.movementSettings.ForwardSpeed /= 10000;
        character.movementSettings.BackwardSpeed /= 10000;
        character.movementSettings.StrafeSpeed /= 10000;

        Invoke("DeBind", bindTime);
    }

    void DeBind()
    {
        character.movementSettings.ForwardSpeed *= 10000;
        character.movementSettings.BackwardSpeed *= 10000;
        character.movementSettings.StrafeSpeed *= 10000;
    }

    #endregion
}
