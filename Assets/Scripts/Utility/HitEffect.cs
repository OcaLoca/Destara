using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{

    public static HitEffect Instance {get; private set;}

    [SerializeField] Animator anim;

     private void Awake() {

        if(Instance == null){
            Instance = this;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit(){
        anim.SetTrigger("Hit");
    }

}
