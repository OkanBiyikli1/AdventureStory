using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{

    public GameObject theMenu;
    public GameObject[] windows;

    private CharStats[] playerstats;

    public TextMeshProUGUI[] nameText, hpText, mpText, lvlText, expText;
    public Slider[] expSlider;
    public Image[] charImage;
   public GameObject[] charStatHolder;
   public GameObject[] statusButtons;

   public TextMeshProUGUI statusName, statusHP, statusMP, statusStr, statusDef, statusWpnEqpd, statusWpnPwr, statusArmrEqpd, statusArmrPwr, statusExp;
   public Image statusImage;

   public ItemButton[] itemButtons;
   public string selectedItem;
   public Item activeItem;
   public Text itemName, itemDescription, useButtonText;

   public GameObject itemCharChoiceMenu;
   public Text[] itemCharChoiceNames;

   public static GameMenu instance;
   public TextMeshProUGUI goldText;

   public string mainMenuName;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire2"))
        {
            if(theMenu.activeInHierarchy)
            {
                //theMenu.SetActive(false);
                //GameManager.instance.gameMenuOpen = false;

                CloseMenu(); // burayı aşağıya daha düzgün bi şekilde taşıdık..
            }
            else
            {
                theMenu.SetActive(true);
                UpdateMainStats();
                GameManager.instance.gameMenuOpen = true;
            }

            AudioManager.instance.PlaySFX(5);
        }
    }

    public void UpdateMainStats()
    {
        playerstats = GameManager.instance.playerStats;

        for(int i = 0; i < playerstats.Length; i++)
        {
            if(playerstats[i].gameObject.activeInHierarchy)
            {
                charStatHolder[i].SetActive(true);

                nameText[i].text = playerstats[i].charName;
                hpText[i].text = "HP: " + playerstats[i].currentHP + "/" + playerstats[i].maxHP;
                mpText[i].text = "MP: " + playerstats[i].currentMP + "/" + playerstats[i].maxMP;
                lvlText[i].text = "Lvl: " + playerstats[i].playerLevel;
                expText[i].text = "" + playerstats[i].currentEXP + "/" + playerstats[i].expToNextLevel[playerstats[i].playerLevel];
                expSlider[i].maxValue = playerstats[i].expToNextLevel[playerstats[i].playerLevel];
                expSlider[i].value = playerstats[i].currentEXP;
                charImage[i].sprite = playerstats[i].charImage;
            }else
            {
                charStatHolder[i].SetActive(false);
            }
        }

        goldText.text = GameManager.instance.currentGold.ToString() + "g";
    }
    
    public void ToggleWindow(int windowNumber)
    {
        UpdateMainStats();

        for(int i = 0; i < windows.Length; i++)
        {
            if(i == windowNumber)
            {
                windows[i].SetActive(!windows[i].activeInHierarchy); // burda ünlem koyarak tersten yapıyoruz nedenini anlayamadım
            }else
            {
                windows[i].SetActive(false);
            }
        }

        itemCharChoiceMenu.SetActive(false);
    }

    public void CloseMenu()
    {
        for(int i = 0; i < windows.Length; i++)
        {
            windows[i].SetActive(false);
        }
        
        theMenu.SetActive(false);

        GameManager.instance.gameMenuOpen = false;
        
        itemCharChoiceMenu.SetActive(false);
    }

    public void OpenStatus()
    {
        UpdateMainStats();

        // update the information that is shown
        StatusChar(0);

        for(int i= 0; i < statusButtons.Length; i++)
        {
            statusButtons[i].SetActive(playerstats[i].gameObject.activeInHierarchy);
            statusButtons[i].GetComponentInChildren<Text>().text = playerstats[i].charName;
        }
    }
    
    public void StatusChar(int selected)
    {
        statusName.text = playerstats[selected].charName;
        statusHP.text = "" + playerstats[selected].currentHP + "/" + playerstats[selected].maxHP;
        statusMP.text = "" + playerstats[selected].currentMP + "/" +playerstats[selected].maxMP;
        statusStr.text = "" + playerstats[selected].strength; // ya da playerstats[selected].strenth.ToString(); tam sayı olacağı için böyle yazabiliriz.
        statusDef.text = playerstats[selected].defence.ToString();
        if(playerstats[selected].equippedWeapon != "")
        {
            statusWpnEqpd.text = playerstats[selected].equippedWeapon;
        }
        statusWpnPwr.text = playerstats[selected].weaponPwr.ToString();
        if(playerstats[selected].equippedArmor != "")
        {
            statusArmrEqpd.text = playerstats[selected].equippedArmor;
        }
        statusArmrPwr.text = playerstats[selected].armorPwr.ToString();
        statusExp.text = (playerstats[selected].expToNextLevel[playerstats[selected].playerLevel] - playerstats[selected].currentEXP).ToString();
        statusImage.sprite = playerstats[selected].charImage;
    }

    public void ShowItems()
    {
        GameManager.instance.SortItems();

        for(int i = 0; i < itemButtons.Length; i++)
        {
            itemButtons[i].buttonValue = i;

            if(GameManager.instance.itemsHeld[i] != "")
            {
                itemButtons[i].buttonImage.gameObject.SetActive(true);
                itemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;
                itemButtons[i].amountText.text = GameManager.instance.numberOfItems[i].ToString();
            }else
            {
                itemButtons[i].buttonImage.gameObject.SetActive(false);
                itemButtons[i].amountText.text = "";
            }
        }
    }

    public void SelectItem(Item newItem) //itembutton cs inde kullanıyoruz
    {
        activeItem = newItem;

        if(activeItem.isItem)
        {
            useButtonText.text = "Use";
        }

        if(activeItem.isWeapon || activeItem.isArmour)
        {
            useButtonText.text = "Equip";
        }

        itemName.text = activeItem.itemName;
        itemDescription.text = activeItem.description;
    }

    public void DiscardItem()
    {
        if(activeItem != null)
        {
            GameManager.instance.RemoveItem(activeItem.itemName);
        }
    }

    public void OpenItemCharChoice()
    {
        itemCharChoiceMenu.SetActive(true);

        for(int i = 0; i < itemCharChoiceNames.Length; i++)
        {
            itemCharChoiceNames[i].text = GameManager.instance.playerStats[i].charName;
            itemCharChoiceNames[i].transform.parent.gameObject.SetActive(GameManager.instance.playerStats[i].gameObject.activeInHierarchy);

        }
    }

    public void CloseItemCharChoice()
    {
        itemCharChoiceMenu.SetActive(false);
    }

    public void UseItem(int selectChar)
    {
        activeItem.Use(selectChar);
        CloseItemCharChoice();
    }

    public void SaveGame()
    {
        GameManager.instance.SaveData();
        QuestManager.instance.SaveQuestData();
    }

    public void PlayButtonSound()
    {
        AudioManager.instance.PlaySFX(4);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(mainMenuName);

        Destroy(GameManager.instance.gameObject);
        Destroy(PlayerController.instance.gameObject);
        Destroy(AudioManager.instance.gameObject);
        Destroy(gameObject);
    }
}
