using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharStats : MonoBehaviour
{
    public string charName;
    public int playerLevel = 1;
    public int currentEXP;
    public int[] expToNextLevel;
    public int maxLevel = 100;
    public int baseEXP = 1000;

    
    public int currentHP;
    public int maxHP = 100;
    public int currentMP;
    public int maxMP = 25;
    //public int[] mpLevelBonus;
    public int strength;
    public int defence;
    public int weaponPwr;
    public int armorPwr;
    public string equippedWeapon;
    public string equippedArmor;
    public Sprite charImage;
    
    // Start is called before the first frame update
    void Start()
    {
        expToNextLevel = new int[maxLevel];
        expToNextLevel[1] = baseEXP;

        for(int i = 2; i < expToNextLevel.Length; i++)
        {
            expToNextLevel[i] = Mathf.FloorToInt(expToNextLevel[i - 1] * 1.05f); //buradaki mathf.floortoint sonucu direkt yuvarlıyor mesela 2.7 ise 2 yapıyor
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K)) //TEST !!!!!
        {
            AddExp(500);
        }
    }

    public void AddExp(int expToAdd)
    {
        currentEXP += expToAdd;

        if((playerLevel < maxLevel))
        {
            if(currentEXP > expToNextLevel[playerLevel])
            {
                currentEXP -= expToNextLevel[playerLevel];

                playerLevel++;

                //determine whether to add to str or def based on odd or even
                if(playerLevel%2 == 0)//buradaki "%2" 2 ile bölümünden kalan veya "%3" 3 ile bölümünden kalan demek.
                {
                    strength++;
                }
                else  // burada yaptığımız şey ise 2 ile bölümünden kalan çift ise str eğer çift değilse def artıcak
                {
                    defence++;
                }

                maxHP = Mathf.FloorToInt(maxHP * 1.05f);
                currentHP = maxHP;

                maxMP = Mathf.FloorToInt(maxMP * 1.05f);
                currentMP = maxMP;

                //maxMP += mpLevelBonus[playerLevel];
                //currentMP = maxMP;
                //burada MPde yaptığımız şeyin hpdekinin farklı hali
                //yukarıdaki mplevelbonus eklentisini unitynin içinden dilediğimiz gibi manuel ayarlamalı.
            }
        }

        if(playerLevel >= maxLevel)
        {
            currentEXP = 0;
        }
        
    }
}
