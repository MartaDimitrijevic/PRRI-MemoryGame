using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public const int gridRows = 2;
    public const int gridColumns = 7;
    public const float offsetX = 3f;
    public const float offsetY = 5f;

    [SerializeField] private MainCard originalCard;
    [SerializeField] private Sprite[] images;
    [SerializeField] private TextMesh scoreLabel;
    [SerializeField] private TextMesh attemptsLabel;
    [SerializeField] private TextMesh timePassedLabel;

    private int _score = 0;
    private int _attempts = 0;
    private float _timePassed = 0f;
    private MainCard _firstRevealedCard;
    private MainCard _secondRevealedCard;


    public bool canReveal
    {
        get
        {
            return _secondRevealedCard == null;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        SetCardStartLayout();
    }
    private void Update()
    {
        TimePassed();
    }

    private void SetCardStartLayout()
    {
        Vector3 startPosition = originalCard.transform.position;

        int[] cardNumbers = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6 };
        cardNumbers = ShuffleCards(cardNumbers);

        MainCard card;

        for (int i = 0; i < gridColumns; i++)
        {
            for (int j = 0; j < gridRows; j++)
            {
                if (i == 0 && j == 0)
                    card = originalCard;
                else
                    card = Instantiate(originalCard);

                int index = j * gridColumns + i;
                int id = cardNumbers[index];
                card.ChangeSprite(id, images[id]);

                float xPos = startPosition.x + (i * offsetX);
                float yPos = startPosition.y + (j * offsetY);
                card.transform.position = new Vector3(xPos, yPos, startPosition.z);
            }
        }
    }

    private int[] ShuffleCards(int[] cardNumbers)
    {
        int[] shuffledCarNumbers = cardNumbers.Clone() as int[];

        for (int i = 0; i < shuffledCarNumbers.Length; i++)
        {
            int temp = shuffledCarNumbers[i];
            int r = UnityEngine.Random.Range(0, shuffledCarNumbers.Length);
            shuffledCarNumbers[i] = shuffledCarNumbers[r];
            shuffledCarNumbers[r] = temp;
        }

        return shuffledCarNumbers;
    }

    public void RevealCard(MainCard card)
    {
        if(_firstRevealedCard == null)
        {
            _firstRevealedCard = card;
        }
        else
        {
            _secondRevealedCard = card;
            StartCoroutine(CheckCardMatchCoroutine());
        }
    }

    private IEnumerator CheckCardMatchCoroutine()
    {
        if(_firstRevealedCard.Id == _secondRevealedCard.Id)
        {
            _score++;
            scoreLabel.text = "Score: " + _score;
        }
        else
        {
            yield return new WaitForSeconds(1f);

            _firstRevealedCard.Unreveal();
            _secondRevealedCard.Unreveal();
        }

        _attempts++;
        attemptsLabel.text = "Attempts: " + _attempts;
        
        _firstRevealedCard = null;
        _secondRevealedCard = null;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void TimePassed()
    {
        _timePassed += Time.deltaTime;
        timePassedLabel.text = "Time: " + _timePassed.ToString("F0") + "s";
    }
}
