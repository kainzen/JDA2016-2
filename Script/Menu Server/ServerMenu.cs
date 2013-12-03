using UnityEngine;
using System.Collections;

public class ServerMenu : MonoBehaviour {
	
	//Screen Size
	private float MaxWidth;
	private float MaxHeight;
	
	//Variavel de controle do rolamento da tela de Chat
	private Vector2 scrollViewA = Vector2.zero;
	private Vector2 scrollViewB = Vector2.zero;
	
	//Server Logs
	private ArrayList serverLogA = new ArrayList();	
	private ArrayList serverLogB = new ArrayList();	
		
	//Estilo
	private GUIStyle textStyle;
	
	// Use this for initialization
	void Start () {
		textStyle = new GUIStyle();
		textStyle.normal.textColor = Color.white;
		textStyle.fontSize = 18;
		serverLogA.Add("Inicializando servidor...");
		serverLogB.Add("Inicializando servidor...");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {			
		Input.eatKeyPressOnTextFieldFocus = false;
			
		MaxWidth = Screen.width;
		MaxHeight = Screen.height;
		
		GUI.Label(new Rect(MaxWidth*0.04f, MaxHeight*0.04f, MaxWidth*0.2f, MaxHeight*0.05f), "Curso de Teste Sloodle 01", textStyle);	 
		GUILayout.BeginArea(new Rect(MaxWidth*0.04f , MaxHeight*0.09f, MaxWidth*0.92f, MaxHeight*0.37f));
        scrollViewA = GUILayout.BeginScrollView(scrollViewA);
        foreach(string c in serverLogA) {
            GUILayout.Label(c);
        }
        GUILayout.EndArea();
        GUILayout.EndScrollView();
        scrollViewA.y++;
		
		GUI.Label(new Rect(MaxWidth*0.04f, MaxHeight*0.51f, MaxWidth*0.2f, MaxHeight*0.05f), "Curso de Teste Sloodle 02", textStyle);	 
		GUILayout.BeginArea(new Rect(MaxWidth*0.04f , MaxHeight*0.55f, MaxWidth*0.92f, MaxHeight*0.37f));
        scrollViewB = GUILayout.BeginScrollView(scrollViewB);
        foreach(string c in serverLogB) {
            GUILayout.Label(c);
        }
        GUILayout.EndArea();
        GUILayout.EndScrollView();
        scrollViewB.y++;
	}
	
	public void setMsg(int course, string msg){
		if(course == 0) {
			serverLogA.Add(msg);		
		} else if(course == 1) {
			serverLogB.Add(msg);			
		}
	}
}
