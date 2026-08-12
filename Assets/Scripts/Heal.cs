using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Heal : MonoBehaviour
{
    enum healType { potion, HOT }
    [SerializeField] healType type;

    [SerializeField] int healAmount;
    [SerializeField] float healRate;
    [SerializeField] int healDelay;

    float healTimer;
    int healthOrig;
    bool isHealing;

    void Start()
    {
        
    }

    void Update()
    {
        if(!isHealing)
        {
            healTimer += Time.deltaTime;

            if(healTimer >= healDelay)
            {
                isHealing = true;
            }
        }
    }

    public void damageTaken()
    {
        isHealing = false;

        healTimer = 0;
    }

    public void beginHealing(IHeal h)
    {
        StartCoroutine(startHealing(h));
    }

    IEnumerator startHealing(IHeal h)
    {
        while (isHealing)
        {
            h.heal(healAmount);
            yield return new WaitForSeconds(healRate);
        }
    }
}
