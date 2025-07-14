using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIInteface:MonoBehaviour
{
    public Shoot RocketLauncherTurret;
    public Shoot GatlingTurret;
    public Shoot flamerTurret;
    public GameObject turretMenu;
    public TMPro.TMP_Text waveText;
    public TMPro.TMP_Text moneyText;
    public TMPro.TMP_Text livesText;

    public TMPro.TMP_Text upgradeButtonText;

    public Button setSpeedPause;
    public Button setSpeedOne;
    public Button setSpeedTwo;
    public Button setSpeedThree;

    public AudioSource wrongSound;


    GameObject itemPrefab;
    GameObject focusObj;

    private Shoot currentClickedOnTurret;

    private void Start()
    {
        setSpeedPause.onClick.AddListener(SpeedPauseClicked);
        setSpeedOne.onClick.AddListener(SpeedOneClicked);
        setSpeedTwo.onClick.AddListener(SpeedTwoClicked);
        setSpeedThree.onClick.AddListener(SpeedThreeClicked);
    }

    void SpeedOneClicked()
    {
        LevelManager.setGameSpeed(1);
    }

    void SpeedTwoClicked()
    {
        LevelManager.setGameSpeed(2);
    }

    void SpeedThreeClicked()
    {
        LevelManager.setGameSpeed(3);
    }

    void SpeedPauseClicked()
    {
        LevelManager.setGameSpeed(0);
    }

    public void PlayAgain()
    {
        Time.timeScale = 1;
        LevelManager.wavesEmitted = 0;
        LevelManager.totalMoney = 500;
        LevelManager.totalLives = 10;
        LevelManager.totalEnemies = 0;
        LevelManager.numberOfWaves = 4;
        LevelManager.levelOver = false;
        LevelManager.nextWave = false;   
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        
    } 


    public void CreteRocketLauncher()
    {
        if(LevelManager.totalMoney >= RocketLauncherTurret.turretDetails.moneyCost)
        {

            itemPrefab = RocketLauncherTurret.gameObject;
            CreateItemForButton();
            LevelManager.totalMoney -= (int)RocketLauncherTurret.turretDetails.moneyCost;
        }
        else
        {
            wrongSound.Play();
        }
    }

    public void CreateGatling()
    {
        if(LevelManager.totalMoney >= GatlingTurret.turretDetails.moneyCost)
        {
            itemPrefab = GatlingTurret.gameObject;
            CreateItemForButton();
            LevelManager.totalMoney -= (int)GatlingTurret.turretDetails.moneyCost;

        }
        else
        {
            wrongSound.Play();
        }
    }

    public void CreateFlamer()
    {
        if(LevelManager.totalMoney >= flamerTurret.turretDetails.moneyCost)
        {
            itemPrefab = flamerTurret.gameObject;
            CreateItemForButton();
            LevelManager.totalMoney -= (int)flamerTurret.turretDetails.moneyCost;

        }
        else
        {
            wrongSound.Play();
        }
    }

    public void CloseTurretMenu()
    {
        turretMenu.SetActive(false);
    }

    public void SellTower()
    {
        LevelManager.totalMoney += (int)(currentClickedOnTurret.turretDetails.moneyCost * 0.70 + currentClickedOnTurret.turretDetails.upgradeMoneyCost * 0.50);
        Destroy(currentClickedOnTurret.gameObject, 0.1f);
        CloseTurretMenu();
    }

    public void UpgradeTower()
    {

        if(LevelManager.totalMoney >= currentClickedOnTurret.turretDetails.upgradeMoneyCost)
        {
            LevelManager.totalMoney -= (int) currentClickedOnTurret.turretDetails.upgradeMoneyCost;
            currentClickedOnTurret.turretDetails.damage *= 1.2f;
            currentClickedOnTurret.turretDetails.upgradeMoneyCost *= 2f;
            upgradeButtonText.text = "Upgrade (" + (int)currentClickedOnTurret.turretDetails.upgradeMoneyCost + ")";
        }
        else
        {
            wrongSound.Play();
        }

    }

    void CreateItemForButton()
    {
        Ray ray = Camera.main.ScreenPointToRay(GetInputPosition());
        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            focusObj = Instantiate(itemPrefab, hit.point, itemPrefab.transform.rotation);
            foreach(var col in focusObj.GetComponentsInChildren<Collider>())
                col.enabled = false;
        }
    }
    void Update()
    {
        if(LevelManager.wavesEmitted < LevelManager.numberOfWaves)
        {
            waveText.text = "Wave: " + (LevelManager.wavesEmitted + 1) + "/" + LevelManager.numberOfWaves;

        }

        moneyText.text = "Gold: " + LevelManager.totalMoney;
        if(LevelManager.totalLives >= 0)
        {
            livesText.text = "Lives: " + LevelManager.totalLives;
        }


        bool inputBegan = IsInputBegan();
        bool inputHeld = IsInputHeld();
        bool inputEnded = IsInputEnded();
        Vector3 inputPosition = GetInputPosition();

        if(inputBegan)
        {
            if(EventSystem.current.IsPointerOverGameObject()) return;

            Ray ray = Camera.main.ScreenPointToRay(inputPosition);
            if(Physics.Raycast(ray, out RaycastHit hit) &&
                hit.collider.gameObject.CompareTag("Turret"))
            {
                turretMenu.transform.position = inputPosition;
                currentClickedOnTurret = hit.collider.gameObject.GetComponent<Shoot>();
                upgradeButtonText.text = "Upgrade (" + (int)currentClickedOnTurret.turretDetails.upgradeMoneyCost + ")";
                turretMenu.SetActive(true);
            }
        }
        else if(inputHeld && focusObj)
        {
            Ray ray = Camera.main.ScreenPointToRay(inputPosition);
            if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, ~(1 << LayerMask.NameToLayer("Turret"))))
            {

                focusObj.transform.position = hit.point;
            }
        }
        else if(inputEnded && focusObj)
        {
            Ray ray = Camera.main.ScreenPointToRay(inputPosition);
            if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, ~(1 << LayerMask.NameToLayer("Turret"))) &&
                hit.collider.gameObject.CompareTag("Platform") &&
                hit.normal.Equals(new Vector3(0, 1, 0)))
            {
                hit.collider.gameObject.tag = "Occupied";
                focusObj.transform.position = new Vector3(hit.collider.gameObject.transform.position.x, focusObj.transform.position.y, hit.collider.gameObject.transform.position.z);
                foreach(var col in focusObj.GetComponentsInChildren<Collider>())
                    col.enabled = true;
            }
            else
            {
                LevelManager.totalMoney += (int)focusObj.GetComponent<Shoot>().turretDetails.moneyCost;
                Destroy(focusObj);
            }

            focusObj = null;
        }
    }
    bool IsInputBegan()
    {
        if(Input.touchCount == 1)
            return Input.GetTouch(0).phase == TouchPhase.Began;
        return Input.GetMouseButtonDown(0);
    }

    bool IsInputHeld()
    {
        if(Input.touchCount == 1)
            return Input.GetTouch(0).phase == TouchPhase.Moved;
        return Input.GetMouseButton(0);
    }

    bool IsInputEnded()
    {
        if(Input.touchCount == 1)
        {
            var phase = Input.GetTouch(0).phase;
            return phase == TouchPhase.Ended || phase == TouchPhase.Canceled;
        }
        return Input.GetMouseButtonUp(0);
    }

    Vector3 GetInputPosition()
    {
        if(Input.touchCount == 1)
            return Input.GetTouch(0).position;
        return Input.mousePosition;
    }
}