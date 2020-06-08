using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScript : MonoBehaviour {
    public RectTransform mainIconRT;
    public float timeStep = 0.02f;
    public float oneStepAngle = 12f;
    float startTime;

    void Start() {
        startTime = Time.time;
        DontDestroyOnLoad(gameObject);
        Debug.Log("DONT DESTROY!");
    }

    void Update() {
        if (Time.time - startTime >= timeStep) {
            Vector3 iconAngle = mainIconRT.localEulerAngles;
            iconAngle.z += oneStepAngle;

            mainIconRT.localEulerAngles = iconAngle;

            startTime = Time.time;
        }
    }
}
