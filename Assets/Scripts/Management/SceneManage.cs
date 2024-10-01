using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SceneManage : MonoBehaviour
{
    public static SceneManage Instance;
    public enum STATE{OVER,HOME,PAUSE,SHOP,PLAY}
    public Animator homeAnimator,pauseAnimator,overAnimator,shopAnimator;
    public STATE currentState;
    public AudioClip gameStart, gameOver, gameMusic;
    private AudioSource gameAudioSource;
    public CanvasGroup tutorials;

    void Awake()
    {
        SceneManage.Instance = this;
        currentState = STATE.HOME;
        timeStill();
        gameAudioSource = transform.GetComponent<AudioSource>();
    }
    public void homeUp(){
        homeAnimator.SetTrigger("Hup");
        StartCoroutine(timeMove(.5f));
        gameAudioSource.PlayOneShot(gameStart);
    }

    public void homeDown(){
        homeAnimator.SetTrigger("Hown");
        currentState = STATE.HOME;
        timeStill();
        resetGame();
    }

    public void shopUp(){
        currentState = STATE.SHOP;
        StartCoroutine(massAttract());
        StartCoroutine(toShop());
    }
    IEnumerator massAttract(){
        yield return new WaitForSecondsRealtime(.5f);
        globalVariables.Instance.paddleHealth.softReset();
        SquareCoin[] coins = FindObjectsOfType<SquareCoin>();
        foreach(SquareCoin j in coins){
            j.massAttract = true;
        }
    }
    IEnumerator toShop(){
        deleteAllInstances("EnemyBullet");
        ShopManage.Instance.generateShop();
        yield return new WaitForSecondsRealtime(2);
        shopAnimator.SetTrigger("Sup");
        timeStill();
    }

    public void shopDown(){
        shopAnimator.SetTrigger("Sown");
        tutorials.alpha = 0;
        globalVariables.Instance.paddleHealth.softReset();
        StartCoroutine(timeMove(0.5f));
        ShopManage.Instance.deGenerate();
        StartCoroutine(LevelManage.Instance.GenerateLevel(2));
    }
    public void overIn(){
        overAnimator.SetTrigger("Gin");
        LevelManage.Instance.setScore();
        currentState = STATE.OVER;
        timeStill();
        gameAudioSource.PlayOneShot(gameOver);
    }

    public void overOut(){
        overAnimator.SetTrigger("Gout");
        StartCoroutine(timeMove(0.5f));
    }

    public void pauseIn(){
        pauseAnimator.SetTrigger("Pin");
        currentState = STATE.PAUSE;
        timeStill();
    }

    public void pauseOut(){
        pauseAnimator.SetTrigger("Pout");
        StartCoroutine(timeMove(0.4f));
    }

    private void timeStill(){
        Time.timeScale = 0;
    }
    public void restart(){
        overOut();
        resetGame();
    }

    public void OTH(){
        StartCoroutine(overToHome());
    }
    public void PTH(){
        StartCoroutine(pauseToHome());
    }
    public IEnumerator overToHome(){
        homeDown();
        currentState = STATE.HOME;
        yield return new WaitForSecondsRealtime(.3f);
        overAnimator.SetTrigger("Gout");
    }

    public IEnumerator pauseToHome(){
        homeDown();
        currentState = STATE.HOME;
        yield return new WaitForSecondsRealtime(.3f);
        pauseAnimator.SetTrigger("Pout");
    }
    public void pauseRestart(){
        pauseOut();
        resetGame();
    }

    public void unPause(){
        pauseOut();
    }

    private void resetGame(){
        LevelManage.Instance.setScore();
        globalVariables.Instance.paddleHealth.reset();
        LevelManage.Instance.reset();
        UpgradeManage.Instance.reset();
        deleteAllInstances("Coin");
        deleteAllInstances("BallBullet");
        deleteAllInstances("EnemyBullet");
        deleteAllInstances("FloatingUi");
        deleteAllInstances("Enemy");
    }

    private void deleteAllInstances(string thing){
        GameObject[] things = GameObject.FindGameObjectsWithTag(thing);
        foreach(var j in things){
            Destroy(j.gameObject);
        }
    }

    IEnumerator timeMove(float seconds){
        yield return new WaitForSecondsRealtime(seconds);
        Time.timeScale = 1;
        currentState = STATE.PLAY;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(currentState == STATE.PLAY){
                pauseIn();
            }
            else if(currentState == STATE.PAUSE){
                pauseOut();
            }
        }
    }
    public void resetHighScore(){
        PlayerPrefs.SetInt("HighScore",0);
    }
    public void quitGame(){
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
