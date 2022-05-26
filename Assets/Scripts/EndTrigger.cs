using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    public GameController gameController;

    void OnTriggerEnter()
    {
        gameController.completelevel();
    }


}
