using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMarker : MonoBehaviour
{
    public string questToMark;
    public bool markComplete;

    public bool markOnEnter;
    private bool canMark;

    public bool deactivateOnMarking;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(canMark && Input.GetButtonDown("Fire1"))
        {
            canMark = false;
            MarkQuest();
        }       

        gameObject.SetActive(!deactivateOnMarking); //!deactive yapmamızın sebebi aktif etmeye "!" eklersek aktif et deriz
    }                                               //ve başlangıçta aktif olarak başlatmak istiyoruz burda demek istediğimiz
                                                    //deactive'in değili  = active
    public void MarkQuest()
    {
        if(markComplete)
        {
            QuestManager.instance.MarkQuestComplete(questToMark);
        }else
        {
             QuestManager.instance.MarkQuestIncomplete(questToMark);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            if(markOnEnter)
            {
                MarkQuest();
            }else
            {
                canMark = true;
            }

        }    
    }
    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            canMark = false;
        }    
    }
}
