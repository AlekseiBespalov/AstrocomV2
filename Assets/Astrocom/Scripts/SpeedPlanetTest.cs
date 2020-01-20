using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPlanetTest : MonoBehaviour
{
    public GameObject nBody;
    public float speed;
    public float scaling;
    private void FixedUpdate()
    {
        scaling = GravityEngine.Instance().GetVelocityScale();
        speed = (float)Vector3.Magnitude(GravityEngine.Instance().GetScaledVelocity(nBody)) / scaling;
    }
}
