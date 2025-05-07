using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BlinkEffect : MonoBehaviour
{

    Animator anim;

    [SerializeField] private float activationTimerMax;
    [SerializeField] private float activationTimer;
    float timer;
    [SerializeField] private bool isActive;

    // Start is called before the first frame update
    void Start()
    {
         anim = this.GetComponent<Animator>();
         activationTimer = Random.Range(0.5f, activationTimerMax);
         timer = activationTimer;
       
    }

    // Update is called once per frame
    void Update()
    {
        if(!isActive && timer <= 0){
            ActivateEffect();
        }

        if(!isActive && timer > 0){
            timer -= Time.deltaTime;
        }
    }

    void ActivateEffect(){
        
        isActive = true;
        timer = activationTimer;
        anim.SetTrigger("Blink");

    }
    public void DisactivateEffect(){
        isActive = false;
    }
}
