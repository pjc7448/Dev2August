using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    [SerializeField] GameObject MenuActive;
    [SerializeField] GameObject MenuPause;
    [SerializeField] GameObject MenuWin;
    [SerializeField] GameObject MenuLose;

    [SerializeField] TMP_Text EnemyCount;

    public bool isPaused;
    public GameObject player;
    public PlayerController playerScript;
    public Image playerHPBar;
    public GameObject damageFlashPanel;

    float timeScaleOrig;

    
    int GameGoalCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        timeScaleOrig = Time.timeScale;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (MenuActive == null)
            {
                // pause the game
                StatePause();
                MenuActive = MenuPause;
                MenuActive.SetActive(true);

            }
            else if (MenuActive == MenuPause)
            {
                // unpauses the game and returns to the game
                StateUnpaused();
            }
        }
    }

    public void StatePause()
    {
        isPaused = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

    }

    public void StateUnpaused()
    {
        isPaused = false;
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        MenuActive.SetActive(false);
        MenuActive = null;
    }

    public void UpdateGameGoal(int amount)
    {
        GameGoalCount += amount;

        EnemyCount.text = "Enemies Left: " + GameGoalCount;

        // win condition check
        if (GameGoalCount <= 0)
        {
            // win the game
            StatePause();
            MenuActive = MenuWin;
            MenuActive.SetActive(true);
        }
    }

    public void youLose()
    {
        StatePause();
        MenuActive = MenuLose;
        MenuActive.SetActive(true);
    }


}
