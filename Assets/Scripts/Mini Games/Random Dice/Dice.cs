using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour {

    private PlayerManager player;
    public FadeCrewMembers fadeController;
    private GameObject dicePanel;
    public GameObject miniMap;
    public GameObject leftHudBlock;
    public string crewMemberName;
    public int timeAfterDesactive;
    //public int diceGamePrice;
    private bool fadeOut;
    private Sprite[] diceSides;

    // Precios de la apuesta
    private int firstBet = 2;
    private int secondBet = 5;

    // PUNTUACIONES
    public TextMeshProUGUI player_punctuation;
    public TextMeshProUGUI npc_punctuation;

    // SPRITES DEL NPC: Para hacer caraccontenta y enfadada
    public Image npc_face;
    public Image player_face;
    public string npcHappyFaceImageSourceName;
    public string npcAngryFaceImageSourceName;
    public string playerHappyFaceImageSourceName;
    public string playerAngryFaceImageSourceName;

    // Imagenes de los dados
    public Image dice_one;
    public Image dice_two;
    public Image dice_three;
    public Image dice_four;

    // Sonido
    public AudioManager audioManager;
    public AudioSource audioSource; // AudioSource local para el sonido en 3D
    public AudioClip[] winClips;
    public AudioClip[] loseClips;
    public AudioClip[] tieClips;
    public AudioClip[] noMoneyClips;

    public Dialogue dialogue;
    // Array con frases de victoria
    public string[] winPhrases = new string[] {
        "Otro botín para mí, grumete.",
        "Te lo dije, ¡la suerte es toda mía!",
        "¡Ganancia fácil, como saquear un barco varado!",
        "¡Vaya, qué sencillo fue ganarte!",
        "Perdiste, amigo, ¡vuelve cuando aprendas!"
    };
    // Array con frases de derrota
    public string[] losePhrases = new string[] {
        "¡Por el Kraken! Me has vencido.",
        "Hoy no tengo la fortuna de mi lado.",
        "¡Maldita sea! Hasta el mejor pierde a veces.",
        "Parece que los dados te quieren más.",
        "¡Maldita sea, esta ronda es tuya!"
    };
    // Array con frases de empate
    public string[] tiePhrases = new string[] {
        "Parece que estamos igualados, amigo.",
        "Ni ganancia ni pérdida, ¡otra ronda quizás!",
        "¡Empate! La suerte nos abandona a ambos.",
        "Empatados, como dos barcos sin viento.",
        "No ganaste, pero tampoco yo. ¡Interesante!"
    };
    // Array con frases de sin dinero
    public string[] noMoneyPhrases = new string[] {
    "No tienes ni una moneda.",
    "Tus bolsillos están vacíos.",
    "¡Vaya! Sin dinero, sin juego.",
    "Parece que te has quedado seco.",
    "Sin oro, no hay apuesta."
};


	// Use this for initialization
	private void Start () {
        if (player == null)
        {
            //player = FindObjectOfType<PlayerManager>();
            player = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
        }

        if(dicePanel == null)
        {
            dicePanel = transform.parent.gameObject;
        }
        if(audioManager == null) // Encuentra el DataManager de forma dinamica si no esta declarado
        {
            audioManager = FindObjectOfType<AudioManager>();
        }
        // Assign Renderer component
        //dice_one = GetComponent<Image>();

        // Load dice sides sprites to array from DiceSides subfolder of Resources folder
        diceSides = Resources.LoadAll<Sprite>("Images/Dice/");

        audioSource.minDistance = 5.0f; // Ajusta este valor al radio en el que quieres que el sonido se escuche alto
        audioSource.maxDistance = 20.0f; // Ajusta este valor a la distancia máxima en la que se escuche el sonido
        audioSource.rolloffMode = AudioRolloffMode.Linear; // Da mas precision al 3D y asi evitamos que si te alejes mucho se siga escuchando
        audioSource.spatialBlend = 1.0f; // Configura el audio en modo 3D

	}
	
    // If you left click over the dice then RollTheDice coroutine is started
    private void OnMouseDown()
    {
        StartCoroutine("RollTheDice");
    }

    public void PlayDiceFirstBet()
    {
        StartCoroutine(RollTheDice(firstBet));
    }

    public void PlayDiceSecondBet()
    {
        StartCoroutine(RollTheDice(secondBet));
    }

    public void CloseDicePanel()
    {
        miniMap.SetActive(true);
        leftHudBlock.SetActive(true);
        dicePanel.SetActive(false);
        Sprite playerLoadedSprite = Resources.Load<Sprite>("Images/HUD/CarasPersonajes/" + npcHappyFaceImageSourceName);
        npc_face.sprite = playerLoadedSprite;
        Sprite playerLoadedSprite_two = Resources.Load<Sprite>("Images/HUD/CarasPersonajes/" + playerHappyFaceImageSourceName);
        player_face.sprite = playerLoadedSprite_two;
        player_punctuation.text = "0";
        npc_punctuation.text = "0";
    }

    // Coroutine that rolls the dice
    private IEnumerator RollTheDice(int betMoney)
    {
        if(player.coins.actualCoins >= betMoney)
        {
            player.coins.spendCoins(betMoney);

            int randomDiceOneSide = 0;
            int randomDiceTwoSide = 0;
            int randomDiceThreeSide = 0;
            int randomDiceFourSide = 0;

            int finalSide_player = 0;
            int finalSide_npc = 0;

            for (int i = 0; i <= 20; i++)
            {
                randomDiceOneSide = Random.Range(0, 5);
                randomDiceTwoSide = Random.Range(0, 5);
                randomDiceThreeSide = Random.Range(0, 5);
                randomDiceFourSide = Random.Range(0, 5);

                dice_one.sprite = diceSides[randomDiceOneSide];
                dice_two.sprite = diceSides[randomDiceTwoSide];
                dice_three.sprite = diceSides[randomDiceThreeSide];
                dice_four.sprite = diceSides[randomDiceFourSide];

                yield return new WaitForSeconds(0.05f);
            }

            // Guardamos la puntuacion de cada uno
            finalSide_player = randomDiceOneSide + randomDiceTwoSide + 2;
            finalSide_npc = randomDiceThreeSide + randomDiceFourSide + 2;
            // Actualizamos el texto que muestra la puntuacion de cada uno
            player_punctuation.text = finalSide_player.ToString();
            npc_punctuation.text = finalSide_npc.ToString();
            
            if(finalSide_player > finalSide_npc)
            {
                Sprite playerLoadedSprite = Resources.Load<Sprite>("Images/HUD/CarasPersonajes/" + npcAngryFaceImageSourceName);
                npc_face.sprite = playerLoadedSprite;
                Sprite playerLoadedSprite_two = Resources.Load<Sprite>("Images/HUD/CarasPersonajes/" + playerHappyFaceImageSourceName);
                player_face.sprite = playerLoadedSprite_two;
                dialogue.panel.SetActive(true);
                dialogue.StartDialogue(RandomPhrase(losePhrases, loseClips), crewMemberName);
                StartCoroutine(DeactivatePanelAfterDelay());
                player.coins.obtainCoins(betMoney * 2);
                Debug.Log("-----> VICTORIA");
            }
            else if(finalSide_player == finalSide_npc)
            {
                Sprite playerLoadedSprite = Resources.Load<Sprite>("Images/HUD/CarasPersonajes/" + npcAngryFaceImageSourceName);
                npc_face.sprite = playerLoadedSprite;
                Sprite playerLoadedSprite_two = Resources.Load<Sprite>("Images/HUD/CarasPersonajes/" + playerAngryFaceImageSourceName);
                player_face.sprite = playerLoadedSprite_two;
                dialogue.panel.SetActive(true);
                dialogue.StartDialogue(RandomPhrase(tiePhrases, tieClips), crewMemberName);
                StartCoroutine(DeactivatePanelAfterDelay());
                player.coins.obtainCoins(betMoney);
                Debug.Log("-----> EMPATE");
            }
            else 
            {
                Sprite playerLoadedSprite = Resources.Load<Sprite>("Images/HUD/CarasPersonajes/" + npcHappyFaceImageSourceName);
                npc_face.sprite = playerLoadedSprite;
                Sprite playerLoadedSprite_two = Resources.Load<Sprite>("Images/HUD/CarasPersonajes/" + playerAngryFaceImageSourceName);
                player_face.sprite = playerLoadedSprite_two;
                dialogue.panel.SetActive(true);
                dialogue.StartDialogue(RandomPhrase(winPhrases, winClips), crewMemberName);
                StartCoroutine(DeactivatePanelAfterDelay());
                Debug.Log("-----> DERROTA");
            }
        }
        else
        {
            dialogue.panel.SetActive(true);
            dialogue.StartDialogue(RandomPhrase(noMoneyPhrases, noMoneyClips), crewMemberName);
            StartCoroutine(DeactivatePanelAfterDelay());
        }
    }

    public string RandomPhrase(string[] phrases, AudioClip[] audio)
    {
        int num = Random.Range(0, phrases.Length);
        PlayDialogueClip(num, audio);
        return phrases[num];
        
    }

    private IEnumerator DeactivatePanelAfterDelay()
    {
        yield return new WaitForSeconds(2);
        dialogue.panel.SetActive(false); // Desactiva el panel de diálogo a los dos segundos de salir del collider del CrewMember
        dialogue.ClearButtons();
    }

    // Corrutina para esperar 5 segundos antes de desactivar el gameObject
    private IEnumerator DeactivateAfterDelay()
    {
        yield return new WaitForSeconds(timeAfterDesactive);
        fadeController.enabled = true;
        fadeOut = true;
        yield return new WaitForSeconds(6);
        gameObject.SetActive(false);
        
    }
    private void PlayDialogueClip(int index, AudioClip[] audio)
    {
        if (index >= 0 && index < audio.Length)
        {
            audioSource.clip = audio[index];
            audioSource.Play();
        }
    }
}
