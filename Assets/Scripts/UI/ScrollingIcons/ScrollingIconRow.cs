using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollingIconRow : MonoBehaviour
{
    public ScrollingIcon[] Icons { get; private set; }
    private readonly Vector3 movedirection = new Vector3(60f, -60f, 0);
    private readonly Vector3 startPoint = new Vector3(-80.38f, 987.38f, 0);
    private Sprite[] sprites;
    private List<Sprite> shuffledSprites;
    // Start is called before the first frame update
    private void Start()
    {
        Icons = GetComponentsInChildren<ScrollingIcon>();
        sprites = Resources.LoadAll<Sprite>("Sprites/Modules");
        shuffledSprites = new List<Sprite>();
        shuffledSprites = sprites.OrderBy(x => Random.Range(0, float.MaxValue)).ToList();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -90.91f)
        {
            transform.position = startPoint;
            foreach (ScrollingIcon icon in Icons)
            {
                icon.SetImage(shuffledSprites[Random.Range(0, shuffledSprites.Count)]);
            }
        }
        transform.position += 3 * Time.deltaTime * movedirection;
        Debug.Log(transform.position);
    }
}
