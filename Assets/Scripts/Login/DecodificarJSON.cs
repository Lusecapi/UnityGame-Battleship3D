using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DecodificarJSON  {

	public string error;

	public static DecodificarJSON CreateFromJSON(string jsonString)
	{
		return JsonUtility.FromJson<DecodificarJSON>(jsonString);
	}

}
