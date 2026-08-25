using System.Collections;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && GameManager.Instance.spawnPosition.transform.position != transform.position)
        {
            GameManager.Instance.spawnPosition.transform.position = transform.position;
            StartCoroutine(DisplayPopup());
        }

        IEnumerator DisplayPopup()
        {
            GameManager.Instance.checkPointPopup.SetActive(true);
            yield return new WaitForSeconds(2);
            GameManager.Instance.checkPointPopup.SetActive(false);
        }
    }
}
