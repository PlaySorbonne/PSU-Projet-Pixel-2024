using UnityEngine;
using System.Collections;

public class OnScreenDebug : MonoBehaviour{
	string myLog;
	Queue myLogQueue = new Queue();

	void OnEnable () {
		Application.logMessageReceived += HandleLog;
	}
	
	void OnDisable () {
		Application.logMessageReceived -= HandleLog;
	}

	void HandleLog(string logString, string stackTrace, LogType type){
		myLog = logString;
		string newString = "[" + type + "] : " + myLog + "\n";
        myLogQueue.Enqueue(newString);
        if (type == LogType.Exception) {
            newString = "" + stackTrace;
            myLogQueue.Enqueue(newString);
        }
        myLog = string.Empty;
        foreach(string mylog in myLogQueue){
            myLog += mylog;
        }
    }

	void OnGUI () {
		GUILayout.Label(myLog);
	}
}