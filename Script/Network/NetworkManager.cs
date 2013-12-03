using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
	
	//Server
	private bool visible = false;
	private int type;
	
	//Screen Size
	private float MaxWidth;
	private float MaxWHeight;
	
	private const string typeName = "UVVUnitySchool";
	private const string gameName = "ClassRoom";
	
	//Client
	private HostData[] hostData; 
	
	public GameObject playerPrefab;
	public Camera camera;
	WebIntercomModel WebInModel;
		
	// Use this for initialization
	void Start () {		
		WebInModel = GameObject.FindObjectOfType(typeof(WebIntercomModel)) as WebIntercomModel;		
		RefreshHostList();
	}
	
	#region Server
	//OLD METHOD SERVER METHODS ARE NOW IN SERVERMANAGER
	private void StartServer() {
	    Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
    	MasterServer.RegisterHost(typeName, gameName);			
	}
	
	void OnServerInitialized() {
	    Debug.Log("Server Initializied");
		SpawnPlayer();
	}
	
	public void OnServerStarted() {
		StartServer();
	}
	
	#endregion
	
	#region	Client
	private void RefreshHostList() {
	    MasterServer.RequestHostList(typeName);
	}
	 
	void OnMasterServerEvent(MasterServerEvent msEvent)	{
	    if (msEvent == MasterServerEvent.HostListReceived) {
	        hostData = MasterServer.PollHostList();
			Debug.Log("Server List Received: " + hostData.ToString());
			JoinServer(hostData[0]);
		}
	}
	
	private void JoinServer(HostData hostData) {
	    Network.Connect(hostData);
		Debug.Log("Server Joined");
	}
	
	void OnConnectedToServer() {		
	    Debug.Log("Server Connected");
		SpawnPlayer();
	}
	 
	private void SpawnPlayer()
	{		
	    Network.Instantiate(playerPrefab, new Vector3(630.7527f, 23.69808f, 1227.302f), Quaternion.identity, PlayerPrefs.GetInt("Course"));
		Network.sendRate = 1f;
		Debug.Log("Spawn!");
	} 
	
	public void OnServerSelected() {
		//RefreshHostList();
		Network.Connect("192.168.25.50", 25000);
		Debug.Log("Server Joined");
	}
	#endregion
	
	void OnGUI() {
		MaxWidth = Screen.width;
		MaxWHeight = Screen.height;
		
		//OLD METHOD --- NEW ON OnServerSelected AND OnServerStarted
	    if (!Network.isClient && !Network.isServer && visible && type == 0)
	    {
	        if (GUI.Button(new Rect(MaxWidth*0.9f, MaxWHeight*0.9f, MaxWidth*0.1f, MaxWHeight*0.1f), "Start"))
	            StartServer();
	    } else if (!Network.isClient && !Network.isServer && visible && type == 1)
	    {
	        if (GUI.Button(new Rect(MaxWidth*0.9f, MaxWHeight*0.9f, MaxWidth*0.1f, MaxWHeight*0.1f), "Entrar"))
	            JoinServer(hostData[0]);
	    }
	}
	
	public void broadCastChat(string msg) {
		networkView.RPC("synChat", RPCMode.OthersBuffered, PlayerPrefs.GetInt("Course"), msg);
		//networkView.RPC("sysMsg", RPCMode.Server, PlayerPrefs.GetInt("Course"), msg);
	}
	
	[RPC] void synChat(int course, string msg) {
		if(course == PlayerPrefs.GetInt("Course"))
			WebInModel.Messages.Add(msg);			
	}
	
	[RPC] void sysMsg(int course, string msg) {
		//Server Only
	}
	
	[RPC] void onQuit(int course, NetworkPlayer player) {
		//Server Only
	}
	
	public void configVisible(int serverType) { 
		visible = true;
		type = serverType;
		if(type == 1) {
			Debug.Log("Refresh Server List");
			RefreshHostList();
		}
	}
	
	// Update is called once per frame
	void Update () {}
	
	// Called once on leaving app
	void OnApplicationQuit()
    {
		Debug.Log("Disconnecting....");
		networkView.RPC("onQuit", RPCMode.OthersBuffered, PlayerPrefs.GetInt("Course"), Network.player);
		Network.Disconnect();
    }
}
