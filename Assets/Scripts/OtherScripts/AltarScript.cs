using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AltarScript : MonoBehaviour
{
    public bool altarActive;
    private TMP_Text objectiveText;
    // Start is called before the first frame update
    void Start()
    {
        objectiveText = GameObject.FindGameObjectWithTag("ObjectiveText").GetComponent<TMP_Text>();
        objectiveText.text = "Objective: Survive 10 mins and wait for the Altar to charge!";
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
    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.name == "Player" && altarActive)
        {
            print("game won");
        }
    }
}