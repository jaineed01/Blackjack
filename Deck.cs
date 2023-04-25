using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public Sprite[] faces;
    public GameObject dealer;
    public GameObject player;
    public Button hitButton;
    public Button stickButton;
    public Button playAgainButton;
    public Text finalMessage;
    public Text probMessage;
    public Text playerPointsMessage;
    public Text dealerPointsMessage;
    public Text dealerMessage;
    public Text probabilities;

    public int[] values = new int[52];
    int cardIndex = 0;    
       
    private void Awake()
    {    
        InitCardValues();        

    }

    private void Start()
    {
        ShuffleCards();
        StartGame();        
    }

    private void InitCardValues()
    {
        /*TODO:
         * Asignar un valor a cada una de las 52 cartas del atributo "values".
         * En principio, la posición de cada valor se deberá corresponder con la posición de faces. 
         * Por ejemplo, si en faces[1] hay un 2 de corazones, en values[1] debería haber un 2.
         */

        // Ciclo para asignar los valores correspondientes a las cartas
        
        for (int i = 0; i < values.Length; i++)
        {
            int pp = (i % 13) + 1;
            if (pp > 10)
            {
                pp = 10;

            }
            if (pp == 1)
            {
                pp = 11;

            }
            values[i] = pp;
        }

    }

    private void ShuffleCards()
    {
        /*TODO:
         * Barajar las cartas aleatoriamente.
         * El método Random.Range(0,n), devuelve un valor entre 0 y n-1
         * Si lo necesitas, puedes definir nuevos arrays.
         */
        for (int i = 0; i < values.Length; i++)
        {
            // Generar un índice aleatorio entre 0 y la longitud del array "values"
            var j = Random.Range(0, values.Length);

            // Intercambiar el valor de las cartas
            var temp_value = values[i];
            values[i] = values[j];
            values[j] = temp_value;

            // Intercambiar la cara de las cartas
            var temp_faces = faces[i];
            faces[i] = faces[j];
            faces[j] = temp_faces;
        }

    }

    void StartGame()
    {
        for (int i = 0; i < 2; i++)
        {
            PushPlayer();
            PushDealer();
            playerPointsMessage.text = "Tienes " + player.GetComponent<CardHand>().points + " puntos";
            dealerPointsMessage.text = "Tiene " + dealer.GetComponent<CardHand>().points + " puntos";
            /*TODO:
             * Si alguno de los dos obtiene Blackjack, termina el juego y mostramos mensaje
             */
        }
    }

    private void CalculateProbabilities()
    {
        int playerPoints = player.GetComponent<CardHand>().points;
        if (playerPoints <= 11)
        {
            probMessage.text = "0%";
        }
        else if (playerPoints == 12)
        {
            probMessage.text = "31%";
        }
        else if (playerPoints == 13)
        {
            probMessage.text = "39%";
        }
        else if (playerPoints == 14)
        {
            probMessage.text = "56%";
        }
        else if (playerPoints == 15)
        {
            probMessage.text = "58%";
        }
        else if (playerPoints == 16)
        {
            probMessage.text = "62%";
        }
        else if (playerPoints == 17)
        {
            probMessage.text = "69%";
        }
        else if (playerPoints == 18)
        {
            probMessage.text = "77%";
        }
        else if (playerPoints == 19)
        {
            probMessage.text = "85%";
        }
        else if (playerPoints == 20)
        {
            probMessage.text = "92%";
        }
        else
        {
            probMessage.text = "100%";
        }

        probabilities.text = ">21: " + probMessage.text;
        /*TODO:
         * Calcular las probabilidades de:
         * - Teniendo la carta oculta, probabilidad de que el dealer tenga más puntuación que el jugador
         * - Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una carta
         * - Probabilidad de que el jugador obtenga más de 21 si pide una carta          
         */
    }

    void PushDealer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        dealer.GetComponent<CardHand>().Push(faces[cardIndex],values[cardIndex]);
        cardIndex++;        
    }

    void PushPlayer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        player.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]);
        cardIndex++;
        CalculateProbabilities();
    }       

    public void Hit()
    {
        /*TODO: 
          * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
          */

        //Repartimos carta al jugador
        PushPlayer();
        playerPointsMessage.text = "Tienes " + player.GetComponent<CardHand>().points + " puntos";
        dealerPointsMessage.text = "Tiene " + dealer.GetComponent<CardHand>().points + " puntos";
        /*TODO:
         * Comprobamos si el jugador ya ha perdido y mostramos mensaje
         */

        if (player.GetComponent<CardHand>().points > 21)
        {
            finalMessage.text = "Te has pasado. HAS PERDIDO";
            hitButton.interactable = false;
            stickButton.interactable = false;
        }
        if (player.GetComponent<CardHand>().points == 21)
        {
            finalMessage.text = "Blackjack! HAS GANADO";
            hitButton.interactable = false;
            stickButton.interactable = false;
            
        }
    }

    public void Stand()
    {
        dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
        while (dealer.GetComponent<CardHand>().points < 17)
        {
            PushDealer();
        }
        int dealerPoints = dealer.GetComponent<CardHand>().points;
        int playerPoints = player.GetComponent<CardHand>().points;
        dealerPointsMessage.text = "Tiene " + dealer.GetComponent<CardHand>().points + " puntos";
        playerPointsMessage.text = "Tienes " + player.GetComponent<CardHand>().points + " puntos";
        if (dealerPoints == 21)
        {
            finalMessage.text = "BlackJack del dealer. HAS PERDIDO";
        }
        else if (playerPoints == 21)
        {
            finalMessage.text = "BlackJack!! HAS GANADO!! " ;
           // bankValue += apuesta * 2;
          //  bank.text = "Tienes " + bankValue + " €";
            dealerMessage.text = "El dealer ha conseguido " + dealerPoints + " puntos";
        }
        else if (dealerPoints > 21)
        {
            finalMessage.text = "La banca se ha pasado, has ganado ";
           // bankValue += apuesta * 2;
          //  bank.text = "Tienes " + bankValue + " €";
            dealerMessage.text = "El dealer ha conseguido " + dealerPoints + " puntos";
        }
        else if (dealerPoints == playerPoints)
        {
            finalMessage.text = "Habeis tenido un empate";
          //  bankValue += apuesta;
       //     bank.text = "Tienes " + bankValue + " €";
            dealerMessage.text = "El dealer ha conseguido " + dealerPoints + " puntos";
        }
        else if (dealerPoints > playerPoints)
        {
            
            finalMessage.text = "El dealer te ha superado, has perdido";
            dealerMessage.text = "El dealer ha conseguido " + dealerPoints + " puntos";
        }
        else
        {
            finalMessage.text = "Has ganado  € a la banca. Enhorabuena.";
          //  bankValue += apuesta * 2;
          //  bank.text = "Tienes " + bankValue + " €";
            dealerMessage.text = "El dealer ha conseguido " + dealerPoints + " puntos";
        }
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */

        /*TODO:
         * Repartimos cartas al dealer si tiene 16 puntos o menos
         * El dealer se planta al obtener 17 puntos o más
         * Mostramos el mensaje del que ha ganado
         */

    }

    public void PlayAgain()
    {
        hitButton.interactable = true;
        stickButton.interactable = true;
        finalMessage.text = "";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();    
        dealerPointsMessage.text = "Tiene " + dealer.GetComponent<CardHand>().points + " puntos";
        playerPointsMessage.text = "Tienes " + player.GetComponent<CardHand>().points + " puntos";
        cardIndex = 0;
        ShuffleCards();
        StartGame();
    }
}
