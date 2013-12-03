using UnityEngine;
using System.Collections;

public class ClientMenu : MonoBehaviour {
	
	//Screen Size
	private float MaxWidth;
	private float MaxHeight;
		
	//Usuario e Senha
	public string Username;
	public string Password;
	
	//Texture
	public Texture2D uvvTexture;
	
	//Estilo
	private GUIStyle textStyle;
	
	//Classe
	UnityUserModel UUModel;
	
	//Flag de login
	private bool isWrongPass = false;
	private bool isCourseSelect = false;
	
	// Use this for initialization
	void Start () {
		textStyle = new GUIStyle();
		textStyle.normal.textColor = Color.black;
		textStyle.fontSize = 18;
		
		this.UUModel = GameObject.FindObjectOfType(typeof(UnityUserModel)) as UnityUserModel;
		
		isCourseSelect = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	//Cria a UI e os possiveis Inputs
    void OnGUI() {			
		Input.eatKeyPressOnTextFieldFocus = false;
			
		MaxWidth = Screen.width;
		MaxHeight = Screen.height;
		
		GUILayout.BeginArea(new Rect(0 , 0, 0, 0));
        GUILayout.EndArea();
		
		if(!isCourseSelect) {	
			if(!uvvTexture){Debug.LogError("Assign a Texture in the inspector."); return;}
			GUI.DrawTexture(new Rect(MaxWidth*0.15f, MaxHeight*0.05f, MaxWidth*0.7f, MaxHeight*0.7f), uvvTexture, ScaleMode.ScaleToFit, true, 1f);
						                
			GUI.Label(new Rect(MaxWidth*0.23f, MaxHeight*0.855f, MaxWidth*0.2f, MaxHeight*0.05f), "Usuario", textStyle);	 
			GUI.Label(new Rect(MaxWidth*0.23f, MaxHeight*0.925f, MaxWidth*0.2f, MaxHeight*0.05f), "Senha", textStyle);	             
					
			GUI.SetNextControlName("Username");
	        Username = GUI.TextField(new Rect(MaxWidth*0.35f, MaxHeight*0.85f, MaxWidth*0.3f, MaxHeight*0.05f), Username);
	        
			GUI.SetNextControlName("Password");
	        Password = GUI.TextField(new Rect(MaxWidth*0.35f, MaxHeight*0.92f, MaxWidth*0.3f, MaxHeight*0.05f), Password);
	        		
			if (GUI.Button(new Rect(MaxWidth*0.67f, MaxHeight*0.85f, MaxWidth*0.1f, MaxHeight*0.12f), "Entrar")) {
				if ((Password.Length > 0) && (Username.Length > 0)) {
					WWWForm form = new WWWForm();
			        form.AddField("username", Username);
			        form.AddField("password", Password);
			        form.AddField("service", "moodle_mobile_app");
					WWW www = new WWW(UUModel.ServerRoot + UUModel.LoginURL, form);
					
					Debug.Log("...enviando: " + www.url);
			        StartCoroutine(WaitForRequestLogin(www));
				}		
	        }		
			if (Input.GetKeyDown("return")) {
				if ((Password.Length > 0) && (Username.Length > 0)) {		
					WWWForm form = new WWWForm();
			        form.AddField("username", Username);
			        form.AddField("password", Password);
			        form.AddField("service", "moodle_mobile_app");
					WWW www = new WWW(UUModel.ServerRoot + UUModel.LoginURL, form);
					
					Debug.Log("...enviando: " + www.url);
			        StartCoroutine(WaitForRequestLogin(www));
					
					GUI.UnfocusWindow();	
				} else {
					GUI.FocusControl("Password");
				}			
			}	
			if(isWrongPass) {
				GUI.Label(new Rect(MaxWidth*0.3f, MaxHeight*0.80f, MaxWidth*0.4f, MaxHeight*0.05f), "Senha ou nome de usuario incorretos!", textStyle);
			}
		} else {
			if (GUI.Button(new Rect(MaxWidth*0.3f, MaxHeight*0.35f, MaxWidth*0.4f, MaxHeight*0.2f), "Curso de Teste Sloodle 01")) {
				PlayerPrefs.SetInt("Course", 0);	
				isCourseSelect = false;
	        }
			if (GUI.Button(new Rect(MaxWidth*0.3f, MaxHeight*0.6f, MaxWidth*0.4f, MaxHeight*0.2f), "Curso de Teste Sloodle 02")) {
				PlayerPrefs.SetInt("Course", 1);	
				isCourseSelect = false;
	        }
		}
    }
	
	//Metodo salva Token de identificacao
	public void ReceiveToken (WWW www) {	
		Debug.Log("Salvando token...");
		
		string[] msgList = www.text.Split('\n');
		string[] tokenList = msgList[0].Split(':');
		string token = tokenList[1];
		token.Remove(0, 1);
		token.Remove(token.Length - 2, 1);
		PlayerPrefs.SetString("Token", token);
		PlayerPrefs.SetString("Name", Username);
		
		Application.LoadLevel("nova_escola");
		
		Debug.Log("...salvo!");		
	}
	
	//Metodo verificacao de Login no ambiente Moodle
	public void LoginVerify (WWW www) {	
		if(!www.text.Contains("error")) {
			ReceiveToken (www);
		} else {
			Debug.Log("Usuario nao identificado!");	
			Password = "";	
			isWrongPass = true;
		}
	}
	
	//Espera do Request terminar para retornar o resultado da verificação de novas mensagem e chama devolta a função de verificação
	IEnumerator WaitForRequestLogin(WWW www) {		
        yield return www;
 
        if (www.error == null) {
            Debug.Log("WWW Ok!: " + www.text);
			LoginVerify(www);
        } else {
            Debug.Log("WWW Error: "+ www.error);
			Password = "";				
        }    
    }
}
