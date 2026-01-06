using UnityEngine;

public class EnfantScript : MonoBehaviour
{
    public void StartRolling()
    {
        Transform parentTransform = transform.parent;
        PlayerCharacter characterScript = parentTransform.GetComponent<PlayerCharacter>();
        if (characterScript != null)
        {
            characterScript.StartRolling();
        }
        else
        {
            Debug.LogError("Le script PlayerCharacter n'a pas été trouvé sur le parent !");
        }
    }

    public void StopRolling()
    {
        Transform parentTransform = transform.parent;
        PlayerCharacter characterScript = parentTransform.GetComponent<PlayerCharacter>();
        if (characterScript != null)
        {
            characterScript.StopRolling();
        }
        else
        {
            Debug.LogError("Le script PlayerCharacter n'a pas été trouvé sur le parent !");
        }
    }

    public void ToggleJump()
    {
        Transform parentTransform = transform.parent;
        PlayerCharacter characterScript = parentTransform.GetComponent<PlayerCharacter>();

        if (characterScript != null)
        {
            characterScript._isJumping = !characterScript._isJumping;
        }
        else
        {
            Debug.LogError("Le script PlayerCharacter n'a pas été trouvé sur le parent !");
        }
    }
    
}
