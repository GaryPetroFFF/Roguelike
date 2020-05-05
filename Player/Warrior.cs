using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Warrior : Character
{
    public Text HPText_;

    //Work variables
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        characterSight_ = CharacterSightEnum.RIGHT;

        health_ = 100;
        armor_ = 0;
        damage_ = 10;
        speed_ = 5;
    }

    // Update is called once per frame
    new void Update()
    {
       base.Update();
       HPText_.text = health_.ToString();
    }

    protected override void FlipCharacterSight()
    {
        base.FlipCharacterSight();
        Vector2[] points = GetComponent<PolygonCollider2D>().GetPath(0);
        switch(characterSight_)
        {
            case CharacterSightEnum.LEFT:
                points.SetValue(new Vector2(-1f, -0.3f), 0);
                points.SetValue(new Vector2(-1f, 0.3f), 1);
                GetComponent<PolygonCollider2D>().SetPath(0, points);
                break;
            case CharacterSightEnum.RIGHT:
                points.SetValue(new Vector2(1f, -0.3f), 0);
                points.SetValue(new Vector2(1f, 0.3f), 1);
                GetComponent<PolygonCollider2D>().SetPath(0, points);
                break;
        }
    }
}
