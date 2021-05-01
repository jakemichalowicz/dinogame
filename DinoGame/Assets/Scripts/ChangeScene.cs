using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public Collider2D playerCollider;
    public int linkedSceneIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision == playerCollider)
        {
            SceneManager.LoadScene(linkedSceneIndex);
        }
    }
}
