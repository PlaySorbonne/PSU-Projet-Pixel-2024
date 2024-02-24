using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class OnScreenDebug : MonoBehaviour{
    float DEBUG_PRINT_TIME = 2.0f;

	string myLog;
    List<string> logMessages = new List<string>();
    List<float> logRemainingTimes = new List<float>();
	Queue myLogQueue = new Queue();

    private void Update()
    {
        for (int i = 0; i < logRemainingTimes.Count; i++)
        {
            logRemainingTimes[i] -= Time.deltaTime;
            if (logRemainingTimes[i] <= 0.0f)
            {
                logMessages.RemoveAt(i);
                logRemainingTimes.RemoveAt(i);
            }
        }
        myLog = string.Empty;
        for (int i=0; i<logMessages.Count; i++)
        {
            myLog += logMessages[i] + "\n";
        }
    }

	void OnEnable () {
		Application.logMessageReceived += HandleLog;
	}
	
	void OnDisable () {
		Application.logMessageReceived -= HandleLog;
	}

	void HandleLog(string logString, string stackTrace, LogType type){
        logMessages.Add(logString);
        logRemainingTimes.Add(DEBUG_PRINT_TIME);
    }

	void OnGUI () {
		GUILayout.Label(myLog);
	}
}