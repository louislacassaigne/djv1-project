using UnityEngine;

public class EnfantScript : MonoBehaviour
{
    public void StartRolling()
    {
        // 1. Récupérer le GameObject parent
        Transform parentTransform = transform.parent;

        // 2. Récupérer le script du parent (remplace "CharacterScript" par le nom de ton script)
        PlayerCharacter characterScript = parentTransform.GetComponent<PlayerCharacter>();

        // 3. Appeler la fonction
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
        // 1. Récupérer le GameObject parent
        Transform parentTransform = transform.parent;

        // 2. Récupérer le script du parent (remplace "CharacterScript" par le nom de ton script)
        PlayerCharacter characterScript = parentTransform.GetComponent<PlayerCharacter>();

        // 3. Appeler la fonction
        if (characterScript != null)
        {
            characterScript.StopRolling();
        }
        else
        {
            Debug.LogError("Le script PlayerCharacter n'a pas été trouvé sur le parent !");
        }
    }
    
}
