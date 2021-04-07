using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainManager : GameSingleton<RainManager>
{
    public GameObject RainPrefab;

    private RainScript2D RainScript;
    private bool _rain_heavy = false;
    public bool rain_heavy
    {
        get { return _rain_heavy; }
        set { _rain_heavy = value; }
    }

    private bool _rain_stop = false;
    public bool rain_stop
    {
        get { return _rain_stop; }
        set { _rain_stop = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        RainScript = RainPrefab.GetComponent<RainScript2D>();
        ////RainScript.RainIntensity = 0.1f;
        RainScript.EnableWind = false;
        RainScript.CollisionMask = 0;
        _rain_heavy = false;
        _rain_stop = true;// false;
    }

    public void EnableWind(bool dir_left)
    {
        RainScript.EnableWind = true;
        RainScript.WindZone.mode = WindZoneMode.Directional;

        if (dir_left)
        {

        }else
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        //RainScript.gameObject.transform.SetAsLastSibling();
        if (!_rain_stop && (RainScript.RainIntensity < 0.7f || _rain_heavy))
        {
            RainScript.RainIntensity = Mathf.Clamp(RainScript.RainIntensity + Time.deltaTime * Consts.RainHeavySpeed, 0, 1);
        }
        if (_rain_stop)
        {
            //Debug.Log(RainScript.RainIntensity);
            RainScript.RainIntensity = Mathf.Clamp(RainScript.RainIntensity - Time.deltaTime * Consts.RainHeavySpeed, 0, 1);
        }
    }
}
