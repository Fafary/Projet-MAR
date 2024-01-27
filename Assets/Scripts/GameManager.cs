using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private PlayerController playerController;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    GameObject manager = new GameObject("GameManager");
                    instance = manager.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    private bool gameStarted = false;
    private bool gameOver = false;

    public string objectiveTag = "Objective"; // Objectif pour finir la partie
    public GameObject deathScreen; // Ecran de mort (non affiché de base)
    public GameObject winScreen; // Ecran de victoire (non affiché de base)

     void Awake()
    {
        // Pour garder le GameManager dans les scènes
        DontDestroyOnLoad(gameObject);

        // Démarrer la partie automatiquement dès que la scène de jeu est chargée
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Méthode appelée lorsqu'une scène est chargée
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Niveau1")
        {
            StartGame();
        }

        if (scene.name == "Niveau2")
        {
            StartGame();
        }
    }


    void Update()
    {

        // Vérifie si la touche ECHAP est enfoncée
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Charge la scène MainMenu
            SceneManager.LoadScene("MainMenu");
            // Déverrouille le curseur
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Vérifie si la partie est terminée
        if (gameStarted && !gameOver)
        {
            CheckGameOver();
        }
    }

    void StartGame()
    {
        // Obtient la référence au PlayerController
        playerController = FindObjectOfType<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("GameManager: PlayerController non trouvé !");
        }

        gameStarted = true;
        Debug.Log("La partie a commencé !");
        // Cacher le curseur de la souris
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void CheckGameOver()
    {
        // Vérifie si le joueur est mort
        if (PlayerIsDead())
        {
            gameOver = true;
            Debug.Log("Vous êtes mort ! La partie est terminée.");
            EndGame();
        }

        // Vérifie si le joueur est dans la zone objectif
        if (PlayerIsInObjectiveZone())
        {
            Debug.Log("C'est gagné !");
            EndGame();
        }
    }

    void EndGame()
    {
        if(gameOver && deathScreen != null)
        {
            // Affiche l'écran de mort
            deathScreen.SetActive(true);
        } else if (winScreen != null){
            winScreen.SetActive(true);
        }
        // Lancer la coroutine pour revenir à l'écran principal après un délai
        StartCoroutine(RestartAfterDelay());
    }

    IEnumerator RestartAfterDelay()
    {
        // Attendre 5 secondes
        yield return new WaitForSeconds(5f);

        // Revenir à l'écran principal
        SceneManager.LoadScene("MainMenu");
    }

    bool PlayerIsDead()
    {
        // Vérifie le niveau de santé du joueur
        if (playerController != null && playerController.PlayerHealth <= 0f)
        {
            // Le joueur est mort
            return true;
        }
        // Le joueur est en vie
        return false;
    }

    bool PlayerIsInObjectiveZone()
    {
        // Vérifie si le joueur est dans la zone objectif en fonction du tag
        GameObject[] objectives = GameObject.FindGameObjectsWithTag(objectiveTag);

        foreach (GameObject objective in objectives)
        {
            if (objective != null && playerController != null &&
                Vector3.Distance(playerController.transform.position, objective.transform.position) < 1.0f)
            {
                return true;
            }
        }

        return false;
    }
}
