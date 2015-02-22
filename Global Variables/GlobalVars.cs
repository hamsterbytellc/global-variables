using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[System.Serializable]
public static class GlobalVars {

	private static bool _debugEnabled;
	public static Dictionary<string, object> genericVars = new Dictionary<string, object>();

	#region GENERIC METHODS
	public static T Get<T> (string name){
		if(genericVars.ContainsKey(name)){
			try {
				return (T)genericVars[name];
			} catch(InvalidCastException e){
				if(Application.isEditor && _debugEnabled)
					Debug.LogError("GlobalVars: Type Mismatch! " + e.Message);
				return default (T);
			}
		} else {
			if(Application.isEditor && _debugEnabled)
				Debug.LogError("GlobalVars: Variable does not exist! Assign the variable before you try to access it.");
			return default (T);
		}
	}

	public static void Set<T> (string name, T data){
		if(genericVars.ContainsKey(name)){
			genericVars[name] = data;
		} else {
			try {
				genericVars.Add(name, data);
			} catch(InvalidCastException e){
				if(_debugEnabled && Application.isEditor)
					Debug.LogError("Global Vars: " + e.Message + " " + data.GetType().ToString().Split('.')[1] + " -> " + typeof(T).ToString().Split('.')[1]);
			}
		}	
	}

	public static void Remove(string name){
		if(genericVars.ContainsKey(name)){
			genericVars.Remove(name);
		} else {
			if(Application.isEditor && _debugEnabled)
				Debug.LogWarning("GlobalVars: Variable does not exist! Assign the variable before you try to remove it.");
		}
	}

	#endregion

	public static void EnableDebug(){
		_debugEnabled = true;
	}

	public static void DisableDebug(){
		_debugEnabled = false;
	}
}
