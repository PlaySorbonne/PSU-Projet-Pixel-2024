using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class OnScreenDebug : MonoBehaviour{
    float DEBUG_PRINT_TIME = 6.0f;
    int MAX_NUMBER_OF_MESSAGES = 25;

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
        if (logMessages.Count > MAX_NUMBER_OF_MESSAGES)
        {
            logMessages.RemoveAt(0);
            logRemainingTimes.RemoveAt(0);
        }
    }

	void OnGUI () {
		GUILayout.Label(myLog);
	}
}