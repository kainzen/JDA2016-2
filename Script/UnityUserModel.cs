using UnityEngine;
using System.Collections;

public class UnityUserModel : MonoBehaviour {
	
	private string sloodleUnityName = "UnityPlayer";
	private string password = "K@in190388";
	private string serverRoot = "http://192.168.25.2:8080";	
	private string loginURL = "/login/token.php";
	private string token = "";
	
	//Construtor
	void Start() {}
	
	// Update is called once per frame
	void Update () {}
	
	//GET & SET da variavel sloodleUnityName
	public string SloodleUnityName {
		get { return sloodleUnityName;  }
		set { sloodleUnityName = value; }
	}
	
	//GET & SET da variavel sloodleUnityName
	public string Password {
		get { return password;  }
		set { password = value; }
	}
	
	//GET & SET da variavel sloodleUnityName
	public string LoginURL {
		get { return loginURL;  }
	}
	
	//GET & SET da variavel sloodleUnityName
	public string ServerRoot {
		get { return serverRoot;  }
	}
	
	//GET & SET da variavel sloodleUnityName
	public string Token {
		get { return token;  }
		set { token = value; }
	}
}


