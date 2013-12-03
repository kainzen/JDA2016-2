using UnityEngine;
using System.Collections;


public class WebIntercomControl : MonoBehaviour {
	
	//Lista de Cursos
	private ArrayList Cursos = new ArrayList();
	//Variavel de Controle das mensagens vindas do Moodle
	private int chatID; 
	//Classe Modelo
	WebIntercomModel WebInModel;
	UnityUserModel UUModel;
	
	//Construtor
	void Start() {
		chatID = 0;	
		this.WebInModel = GameObject.FindObjectOfType(typeof(WebIntercomModel)) as WebIntercomModel;
		this.UUModel = GameObject.FindObjectOfType(typeof(UnityUserModel)) as UnityUserModel;
		//StartCourseListener(0);
		//StartCourseListener(1);
	}
	
	public void StartCourseListener(int curso) {
		Debug.Log("Listener: " + curso);
		Cursos.Add(curso);
		UpdateChatLoop(curso);
	}
	
	//Metodo de Controle, retorna mensagem customizada com tag de usuario e horário
	public string MessageTemplate(string message) {
		return System.DateTime.Now.ToString("HH:mm") + WebInModel.UnityTag + UUModel.SloodleUnityName + ": " + message;
	}
	
	//Metodo de Controle, retorna estrutura de um POST Request pro WebServer Sloodle com a mensagem entrada no chat
	public void SloodleChatRequest(string message) {	
		WWWForm form;
		Debug.Log("Sending Msg...");
		foreach(int i in Cursos) {
			Debug.Log("Enviando a curso: " + i);
			form = new WWWForm();
	        form.AddField("sloodlecontrollerid", WebInModel.SloodleControllerId(i));
	        form.AddField("sloodlepwd", WebInModel.SloodlePwd(i));
	        form.AddField("sloodlemoduleid", WebInModel.SloodleModuleId(i));
	        form.AddField("sloodleuuid", WebInModel.SloodleUuid(i));
	        form.AddField("sloodleavname", UUModel.SloodleUnityName);
	        form.AddField("sloodleserveraccesslevel", WebInModel.SloodleSloodleAcessLevel(i));
	        form.AddField("message", WebInModel.UnityTag + UUModel.SloodleUnityName + ": " + message);
	        WWW www = new WWW(WebInModel.SloodleServerRoot(i) + WebInModel.ChatLinker(i), form); 		
		
			StartCoroutine(WaitForRequest(www));	
		}        
	}
	
	//Metodo de Controle, retorna lista de atualização a caixa de chat com a nova mensagem, é possivel vir texto de sucesso, exemplo 1|OK....
	public void UpdateChatBox(WWW www, int curso) {
		string[] msgList = www.text.Split('\n');
		for(int i = 0; i < msgList.Length; i++) {
			string[] msg = msgList[i].Split('|');
			if ((int.Parse(msg[0]) > chatID) && (msg.Length == 3) && !VerifyUnityChatLoopBack(msg[2])) {
				chatID = int.Parse(msg[0]);
				WebInModel.MessageReceived = System.DateTime.Now.ToString("HH:mm") + "(Moodle)" + msg[1] + ": " + msg[2];
				WebInModel.AddMessagesMoodle(WebInModel.MessageReceived);
			}
		}			
		foreach(string msgs in WebInModel.MessagesMoodle) {
			WebInModel.Messages.Add(msgs);
		}
		UpdateChatLoop(curso);
	}
	
	//Verifica se a mensagem veio do Prório Unity
	bool VerifyUnityChatLoopBack(string msg) {
		if(msg.Contains(WebInModel.UnityTag))
			return true;
		else
			return false;
	}
	
	//Metodo de Controle, retorna estrutura de um Form e envia pro webserver gerando uma nova thread
	public void UpdateChatLoop(int curso) {
		WebInModel.CleanMessagesMoodle();
		
		WWWForm form = new WWWForm();
        form.AddField("sloodlecontrollerid", WebInModel.SloodleControllerId(curso));
        form.AddField("sloodlepwd", WebInModel.SloodlePwd(curso));
        form.AddField("sloodlemoduleid", WebInModel.SloodleModuleId(curso));
		WWW www = new WWW(WebInModel.SloodleServerRoot(curso) + WebInModel.ChatLinker(curso), form);
		
        StartCoroutine(WaitForRequestChat(www, curso));
	}
	
	//OLD METHOD
	//Metodo de Login no ambiente Moodle 
	public void LoginRequest(string username, string password) {	
		Debug.Log("Gerando Login Form para " + username + "|" + password + " ...");
		
		UUModel.SloodleUnityName = username;
		UUModel.Password = password;
		WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);
        form.AddField("service", "moodle_mobile_app");
		WWW www = new WWW(UUModel.ServerRoot + UUModel.LoginURL, form);
		
		Debug.Log("...enviando: " + www.url);
        StartCoroutine(WaitForRequestLogin(www));
	}
	
	//NEW METHOD
	//Atualizando Nome de Usuário e Token
	public void setUser(string username, string token) {			
		UUModel.SloodleUnityName = username;
		UUModel.Token = token;
	}
	
	//OLD METHOD
	//Metodo de Login no ambiente Moodle
	public void ReceiveToken (WWW www) {	
		Debug.Log("Salvando token...");
		
		string[] msgList = www.text.Split('\n');
		string[] tokenList = msgList[0].Split(':');
		string token = tokenList[1];
		token.Remove(0, 1);
		token.Remove(token.Length - 2, 1);
		UUModel.Token = token;
		
		Debug.Log("...salvo: " + UUModel.Token);
	}
	
	//Espera do Request terminar para retornar o resultado do envio de mensagem
	IEnumerator WaitForRequest(WWW www) {		
        yield return www;
 
        if (www.error == null) {
            Debug.Log("WWW Ok!: " + www.text);
        } else {
            Debug.Log("WWW Error: "+ www.error);
        }    
    }  
	
	//Espera do Request terminar para retornar o resultado da verificação de novas mensagem e chama devolta a função de verificação
	IEnumerator WaitForRequestChat(WWW www, int curso) {		
        yield return www;
 
        if (www.error == null) {
            Debug.Log("WWW Ok!: " + www.text);
			UpdateChatBox(www, curso);
        } else {
            Debug.Log("WWW Error: "+ www.error);			
			UpdateChatBox(www, curso);
        }    
    }
	
	//Espera do Request terminar para retornar o resultado da verificação de novas mensagem e chama devolta a função de verificação
	IEnumerator WaitForRequestLogin(WWW www) {		
        yield return www;
 
        if (www.error == null) {
            Debug.Log("WWW Ok!: " + www.text);
			ReceiveToken(www);
        } else {
            Debug.Log("WWW Error: "+ www.error);
        }    
    }
}


