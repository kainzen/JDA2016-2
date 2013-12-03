using UnityEngine;
using System.Collections;

public class WebIntercomModel : MonoBehaviour {
		
	private ArrayList messages = new ArrayList();	
	private string message = "";		
	private string messageToSend = "";	
	private ArrayList messagesMoodle = new ArrayList();		
	private string messageReceived = "";
	private string sloodleControllerId = "6";	
	private string sloodleServerRoot = "http://192.168.25.2:8080";
	private string chatLinker = "/mod/sloodle/mod/chat-1.0/linker.php";
	private string sloodlePwd = "83c50dce-52fb-40bc-ae1d-f497ac192cf4|67756731";
	private string sloodleModuleId = "7";
	private string sloodleUuid = "6ad3ad74-e00f-4a23-8ff8-8b4fa6b1818b";
	private string sloodleSloodleAcessLevel = "0";
	private string unityTag = "(Unity)";
	
	//Contrustor
	void Start() {}
	
	#region GET/SET
	//GET & SET da lista messages
	public ArrayList Messages {
		get { return messages;  }
		set { messages = value; }
	}	
	//Add messages
	public void AddMessages(string Msg) 
	{
		messages.Add(Msg);
	}
	//Limpa messages
	public  void CleanMessages() {
		messages.Clear();
	}
	//GET & SET da variavel message
	public string Message {
		get { return message;  }
		set { message = value; }
	}
	
	//GET & SET da variavel messageToSend
	public string MessageToSend {
		get { return messageToSend;  }
		set { messageToSend = value; }
	}
	
	//GET & SET da lista messagesMoodle
	public ArrayList MessagesMoodle {
		get { return messagesMoodle;  }
		set { messagesMoodle = value; }
	}	
	//Add messagesMoodle
	public void AddMessagesMoodle(string Msg) 
	{
		messagesMoodle.Add(Msg);
	}
	//Limpa messagesMoodle
	public  void CleanMessagesMoodle() {
		messagesMoodle.Clear();
	}
	
	//GET & SET da variavel messageReceived
	public string MessageReceived {
		get { return messageReceived;  }
		set { messageReceived = value; }
	}	
	
	//GET da variavel unityTag
	public string UnityTag {
		get { return unityTag;  }
	}	
	#endregion
	
	#region Cursos
	//GET da variavel sloodleControllerId
	public string SloodleControllerId (int curso) {
		switch(curso) {
		case 0:
			return "6"; 
		break;
		case 1:
			return "15"; 
		break;
		default:
			return "6"; 
		break;
		}  
	}
	
	//GET da variavel sloodleServerRoot	
	public string SloodleServerRoot (int curso) {
		switch(curso) {
		case 0:
			return "http://192.168.25.50:8080";
		break;
		case 1:
			return "http://192.168.25.50:8080";
		break;
		default:
			return "http://192.168.25.50:8080";
		break;
		}  
	}
	
	//GET da variavel chatLinker
	public string ChatLinker (int curso) {
		switch(curso) {
		case 0:
			return "/mod/sloodle/mod/chat-1.0/linker.php";
		break;
		case 1:
			return "/mod/sloodle/mod/chat-1.0/linker.php";
		break;
		default:
			return "/mod/sloodle/mod/chat-1.0/linker.php";
		break;
		}  
	}
	
	//GET da variavel sloodlePwd
	public string SloodlePwd (int curso) {
		switch(curso) {
		case 0:
			return "83c50dce-52fb-40bc-ae1d-f497ac192cf4|67756731";
		break;
		case 1:
			return "4e93b6fd-2191-4fb3-85ad-720d8b21c554|172992528"; 
		break;
		default:
			return "83c50dce-52fb-40bc-ae1d-f497ac192cf4|67756731";
		break;
		}  
	}
	
	//GET da variavel sloodleModuleId
	public string SloodleModuleId (int curso) {
		switch(curso) {
		case 0:
			return "7";
		break;
		case 1:
			return "16";
		break;
		default:
			return "7";
		break;
		}  
	}
	
	//GET da variavel sloodleUuid
	public string SloodleUuid (int curso) {
		switch(curso) {
		case 0:
			return "6ad3ad74-e00f-4a23-8ff8-8b4fa6b1818b";
		break;
		case 1:
			return "8f97e2c1-3194-4f5b-bdcb-71c715c05e99"; 
		break;
		default:
			return "6ad3ad74-e00f-4a23-8ff8-8b4fa6b1818b";
		break;
		}  
	}
	
	//GET da variavel sloodleSloodleAcessLevel
	public string SloodleSloodleAcessLevel (int curso) {
		switch(curso) {
		case 0:
			return "0";
		break;
		case 1:
			return "0";
		break;
		default:
			return "0";
		break;
		}  
	}
	#endregion
}



