using UnityEngine;
using System.Collections;

public class WebIntercomGUI : MonoBehaviour {
	
	//Variavel de controle do rolamento da tela de Chat
	private Vector2 scrollView = Vector2.zero;
	//Classe de Controle e Modelo
	WebIntercomControl WebInControl;
	WebIntercomModel WebInModel;
	NetworkManager NetManager;
	
	//Screen Size
	private float MaxWidth;
	private float MaxHeight;
	
	//Zera a variavel de nova mensagem, chama a função loop de verificação do chat do Moodle
    void Start() {
		this.WebInModel = GameObject.FindObjectOfType(typeof(WebIntercomModel)) as WebIntercomModel;
		this.WebInControl = GameObject.FindObjectOfType(typeof(WebIntercomControl)) as WebIntercomControl;
		this.NetManager = GameObject.FindObjectOfType(typeof(NetworkManager)) as NetworkManager;
		WebInModel.AddMessages("Bem Vindo a UVV virtual " + PlayerPrefs.GetString("Name") + ": www.uvv.br");
		
		init();
    }
	
	void init(){ 
		Debug.Log("User: " + PlayerPrefs.GetString("Name") + " | Course: " + PlayerPrefs.GetInt("Course").ToString() + " | Token" + PlayerPrefs.GetString("Token"));
		WebInControl.StartCourseListener(PlayerPrefs.GetInt("Course"));
		WebInControl.setUser(PlayerPrefs.GetString("Name"), PlayerPrefs.GetString("Token"));
		NetManager.OnServerSelected();
	}
	
	//Cria a UI e os possiveis Inputs
    void OnGUI() {		
		Input.eatKeyPressOnTextFieldFocus = false;
		
		MaxWidth = Screen.width;
		MaxHeight = Screen.height;
		
        GUILayout.BeginArea(new Rect(0 , MaxHeight*0.552f, MaxWidth*0.35f, MaxHeight*0.4f));
        scrollView = GUILayout.BeginScrollView(scrollView);
        foreach(string c in WebInModel.Messages) {
            GUILayout.Label(c);
        }
        GUILayout.EndArea();
        GUILayout.EndScrollView();
        scrollView.y++;
		
		GUI.SetNextControlName("chatWindow");
        WebInModel.Message = GUI.TextField(new Rect(0, MaxHeight*0.97f, MaxWidth*0.3f, MaxHeight*0.03f), WebInModel.Message);
        if (GUI.Button(new Rect(MaxWidth*0.3f, MaxHeight*0.97f, MaxWidth*0.05f, MaxHeight*0.03f), "Enviar")) {
			if (WebInModel.Message.Length > 0) {	
				if(WebInModel.Message.StartsWith("/")) {
					commandVerify(WebInModel.Message);
				} else {
					WebInModel.MessageToSend = WebInModel.Message;	
					WebInModel.Messages.Add(WebInControl.MessageTemplate(WebInModel.MessageToSend));		
					WebInControl.SloodleChatRequest(WebInModel.MessageToSend);	
					NetManager.broadCastChat(WebInControl.MessageTemplate(WebInModel.MessageToSend));
				}		
	            WebInModel.Message = "";	
			}		
        }		
		if (Input.GetKeyDown("return")) {
			if (WebInModel.Message.Length > 0) {	
				if(WebInModel.Message.StartsWith("/")) {
					commandVerify(WebInModel.Message);
				} else {
					WebInModel.MessageToSend = WebInModel.Message;	
					WebInModel.Messages.Add(WebInControl.MessageTemplate(WebInModel.MessageToSend));		
					WebInControl.SloodleChatRequest(WebInModel.MessageToSend);
					NetManager.broadCastChat(WebInControl.MessageTemplate(WebInModel.MessageToSend));
				}				
	            WebInModel.Message = "";	
				GUI.UnfocusWindow();	
			} else {
				GUI.FocusControl("chatWindow");
			}			
		}		
    }
	
	public void SystemMsg(string smsg) {
		WebInModel.Messages.Add(smsg);
	}
	
	// Update is called once per frame
	void Update () {}	
	
	void commandVerify(string cmd) { 
		try {
			Debug.Log("Starting course chat: " + cmd);
			cmd = cmd.Remove(0, 1);
			string[] msg = cmd.Split(' ');
			Debug.Log(msg.ToString());
			if(msg[0].Equals("curso")) {
				if((msg[1].Equals("0")) || (msg[1].Equals("1"))) {
					WebInModel.Messages.Add("Inicializando WebIntercom, curso unity ID: " + msg[1]);
					WebInControl.StartCourseListener(int.Parse(msg[1]));
				}
			} else if(msg[0].Equals("server")) {
				if(msg[1].Equals("0")) {				
					WebInModel.Messages.Add("Ativacao do Servidor habilitado...");
					NetManager.configVisible(0);
				} else if(msg[1].Equals("1")) {				
					WebInModel.Messages.Add("Habilitado a entrar no servidor...");
					NetManager.configVisible(1);
				} 
			} else if(msg[0].Equals("login")) {
				WebInModel.Messages.Add("Logando no Moodle...");
				WebInControl.LoginRequest(msg[1], msg[2]);
			} 
		} catch(System.Exception ex) {
			WebInModel.Messages.Add("Erro ocorrido...");
			WebInModel.Messages.Add(ex.Message);
			WebInModel.Messages.Add("...favor reiniciar a aplicativo.");
		}
	}
}
