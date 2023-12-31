using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScrollingIconManager : MonoBehaviour
{
    // Start is called before the first frame update

    private Sprite[] sprites;
    private List<Sprite> shuffledSprites;

    private void Awake()
    {
        sprites = Resources.LoadAll<Sprite>("Sprites/Modules");
        shuffledSprites = new List<Sprite>();
        shuffledSprites = sprites.OrderBy(x => Random.Range(0, float.MaxValue)).ToList();
    }
    public void SetImage(MoreInterestingScrollingIcon icon)
    {
        icon.SetImage(shuffledSprites[Random.Range(0, shuffledSprites.Count)]);
    }

}
