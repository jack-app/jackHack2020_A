using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cloud : MonoBehaviour
{
    private Vector2 chiledWind = Vector2.zero;
    [NonSerialized]
    public CloudManager manager = default;
    [NonSerialized]
    public bool isTouching = false;
    [NonSerialized]
    public bool isTouched = false;

    public void Init()
    {
        var x = Random.Range(0, 0.25f);
        if(Random.Range(0, 2) == 0)
        {
            x *= -1;
        }
        var y = Random.Range(-0.25f, 0.25f);

        chiledWind = new Vector2(x, y);
    }

    public double IsTouched(Vector3 touchPosition)
    {
        var distance = (touchPosition - transform.position).magnitude;
        if(distance > 1)
        {
            return double.NaN;
        }

        return distance;
    }

    private void Update()
    {
        if (isTouching)
        {
            return;
        }

        var wind = manager.worldWind + chiledWind;
        if (isTouched)
        {
            wind *= 10;
        }

        transform.Translate(wind * Time.deltaTime);
    }
}
