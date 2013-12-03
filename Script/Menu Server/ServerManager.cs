using UnityEngine;
using System.Collections;

public class ServerManager : MonoBehaviour {
	
	//Screen Size
	private float MaxWidth;
	private float MaxWHeight;
	
	private const string typeName = "UVVUnitySchool";
	private const string gameName = "ClassRoom";
		
	//Classe
	ServerMenu serverMenu;
	
	//Players Conectados
	int numP;
		
	// Use this for initialization
	void Start () {		
		this.serverMenu = GameObject.FindObjectOfType(typeof(ServerMenu)) as ServerMenu;
				
		StartServer();
	}
	
	#region Server
	private void StartServer() {
	    Network.InitializeServer(4, 25000, !Network.HavePublicAddress());			
		//MasterServer.RegisterHost(typeName, gameName);	
		Network.natFacilitatorPort = 25000;
		serverMenu.setMsg(0, "sala registrada.");		
		serverMenu.setMsg(1, "sala registrada.");
	}
	
	void OnServerInitialized() {
		serverMenu.setMsg(0, "Servidor inicializado... registrando salas...");
		serverMenu.setMsg(1, "Servidor inicializado... registrando salas...");
	}
	#endregion
	
	[RPC] void sysMsg(int course, string msg) {
		serverMenu.setMsg(course, msg);
	}
			
	[RPC] void synChat(int course, string msg) {
		serverMenu.setMsg(course, msg);
	}
	
	[RPC] void onQuit(int course, NetworkPlayer player) {		
		serverMenu.setMsg(course, "Player excluido: " + player.ToString());
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
	}		
	
	// Update is called once per frame
	void Update () {
		numP = Network.connections.Length;
	}
	
	
}
