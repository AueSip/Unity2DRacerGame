using UnityEngine;

public class S_CameraFollow : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject player;
    public Transform child;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Mini"); // The player
        child = this.gameObject.transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
        child.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 0);
    }   


}
