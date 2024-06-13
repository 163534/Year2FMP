using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AltarScript : MonoBehaviour
{
    public bool altarActive;
    private TMP_Text objectiveText;
    public GameObject winMenu;
    // Start is called before the first frame update
    void Start()
    {
        objectiveText = GameObject.FindGameObjectWithTag("ObjectiveText").GetComponent<TMP_Text>();
        objectiveText.text = "Objective: Survive 5 mins and wait for the Altar to charge!";
        altarActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(altarActive)
        {
            objectiveText.text = "Objective: Get to the Altar!";
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        //print("Collision" + col.gameObject.name);
        if (col.name == "Player" && altarActive)
        {
            print("game won");
            winMenu.SetActive(true);

        }
    }
}
