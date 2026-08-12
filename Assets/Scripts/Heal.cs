using System.Collections;
using UnityEngine;

public class Heal : MonoBehaviour
{
    enum healType { potion, HOT }
    [SerializeField] healType type;

    [SerializeField] int healAmount;
    [SerializeField] float healRate;
    [SerializeField] int healDelay;

    IHeal healTarget;
    float healTimer;
    bool isHealing;

    void Start()
    {
        healTarget = GetComponent<IHeal>();
    }

    void Update()
    {
        if(!isHealing)
        {
            healTimer += Time.deltaTime;

            if(healTimer >= healDelay)
            {
                isHealing = true;
                StartCoroutine(startHealing());
            }
        }
    }

    public void damageTaken()
    {
        isHealing = false;

        healTimer = 0;
    }
    IEnumerator startHealing()
    {
        while (!healTarget.isFullHealth() && isHealing)
        {
            healTarget.heal(healAmount);
            yield return new WaitForSeconds(healRate);
        }
        isHealing = false;
    }
}
