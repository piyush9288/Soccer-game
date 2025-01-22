using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Player scriptPlayer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Ball"))
        {
            if (name.Equals("GoalDetector1"))
            {
                scriptPlayer.IncreaseMyScore();
            }
            else
            {
                scriptPlayer.IncreaseOtherScore();
            }
        }
    }

}
