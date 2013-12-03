using UnityEngine;
using System.Collections;
using UnitySchool;
using System.Runtime.InteropServices;

public class PresenterBGUI : MonoBehaviour {
	
	//Variavel de controle do rolamento da tela de Apresentacao
	private Vector2 scrollView = Vector2.zero;
			
	//Screen Size
	private float MaxWidth;
	private float MaxWHeight;
	
	//Cameras
	public Camera cameraM;
	public Camera cameraA;
	public Camera cameraB;
	
	//Browser Variables   
	public int 							m_TextureID;
	public int 							width;
	public int 							height;
	public bool 						m_bForceOpenGL = true;
	public bool 						m_bLoading = false;
    public bool 						m_bInitialized = false;
    private Color[] 					m_pixels = null;
    private GCHandle 					m_pixelsHandler;   
    public GameObject 					textureGui; 
	private Texture2D 					m_texture;
    private PresenterBWebViewEvents 	browserEventHandler; 
	
	
	private UnityWebCore.BeginNavigationDelFunc 	beginNavigationDelFunc;
	private UnityWebCore.BeginLoadingDelFunc		beginLoadingDelFunc;
	private UnityWebCore.FinishLoadingDelFunc 		finishLoadingDelFunc;
	private UnityWebCore.ReceiveTitleDelFunc 		receiveTitleDelFunc;
	private UnityWebCore.ChangeTooltipDelFunc 		changeTooltipDelFunc;
	private UnityWebCore.ChangeTargetURLDelFunc 	changeTargetURLDelFunc;
	private UnityWebCore.ChangeCursorDelFunc 		changeCursorDelFunc;
	private UnityWebCore.JSCallbackDelFunc 			showControlWindowFunc;	
		
	//Classe de Controle e Modelo
	PresenterBControl presBCrtl;
	PresenterModel presModel;
	
	//Page
	int pag;
	
	// Use this for initialization
	void Start () {
		presBCrtl = GameObject.FindObjectOfType(typeof(PresenterBControl)) as PresenterBControl;
		presModel = new PresenterModel();
		Debug.Log("Presenter: Request");
		presBCrtl.setModel(presModel);	
		browserEventHandler = GetComponent<PresenterBWebViewEvents>();
		Init(512, 512);    
	}
	
	//Inicializa WebBroser
	public void Init(int width, int height)
    {
        this.width = width;
        this.height = height;
        m_texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
		m_TextureID = m_texture.GetNativeTextureID();
		
		if(m_bForceOpenGL && !Application.isEditor)
		{
			UnityWebCore.CreateView(m_TextureID, new System.IntPtr(0), width, height, false, 10);
		}	
		else
		{
			 //Get Color[] (pixels) from texture 
			m_pixels = m_texture.GetPixels(0);
			// Create GCHandle - Allocation of m_pixels in memory. 
			m_pixelsHandler = GCHandle.Alloc(m_pixels, GCHandleType.Pinned);       
			
			UnityWebCore.CreateView(m_TextureID, m_pixelsHandler.AddrOfPinnedObject(), width, height, true, 10);
		}
		
		// assign m_texture to this GUITexture texture        
        textureGui.renderer.material.mainTexture = m_texture;     
		
		beginNavigationDelFunc = new UnityWebCore.BeginNavigationDelFunc(this.onBeginNavigationDelFunc);
		beginLoadingDelFunc = new UnityWebCore.BeginLoadingDelFunc(this.onBeginLoadingDelFunc);
		finishLoadingDelFunc = new UnityWebCore.FinishLoadingDelFunc(this.onFinishLoadingDelFunc);
		receiveTitleDelFunc = new UnityWebCore.ReceiveTitleDelFunc(this.onReceiveTitleDelFunc);
		changeTooltipDelFunc = new UnityWebCore.ChangeTooltipDelFunc(this.onChangeTooltipDelFunc);
		changeTargetURLDelFunc = new UnityWebCore.ChangeTargetURLDelFunc(this.onChangeTargetURLDelFunc);
		changeCursorDelFunc = new UnityWebCore.ChangeCursorDelFunc(this.onChangeCursorDelFunc);
		
		UnityWebCore.SetBeginNavigationFunc(m_TextureID, beginNavigationDelFunc);	
		UnityWebCore.SetBeginLoadingFunc(m_TextureID, beginLoadingDelFunc);	
		UnityWebCore.SetFinishLoadingFunc(m_TextureID, finishLoadingDelFunc);	
		UnityWebCore.SetReceiveTitleFunc(m_TextureID, receiveTitleDelFunc);	
		UnityWebCore.SetChangeTooltipFunc(m_TextureID, changeTooltipDelFunc);	
		UnityWebCore.SetChangeTargetURLFunc(m_TextureID, changeTargetURLDelFunc);	
		UnityWebCore.SetChangeCursorFunc(m_TextureID, changeCursorDelFunc);	
				
        browserEventHandler.setDimensions(width, height);

        m_bInitialized = true;

        browserEventHandler.interactive = true;
    }
	
	//Cria a UI e os possiveis Inputs
    void OnGUI() {
		MaxWidth = Screen.width;
		MaxWHeight = Screen.height;
		
		if(cameraB.enabled) {			
			GUILayout.BeginArea(new Rect(MaxWidth*0.1f, MaxWHeight*0.1f, MaxWidth*0.8f, MaxWHeight*0.8f));
	        scrollView = GUILayout.BeginScrollView(scrollView);	
			//Debug.Log("Presenter: SlideType:" + presModel.SloodleSlideType);
			if(presModel.SloodleSlideType.Contains("image")) {
				if(!m_bLoading) {  //presModel.IsLoading
					Debug.Log("Presenter loading IMAGE: " + presModel.SloodleSlideUrl);
					LoadURL(presModel.SloodleSlideUrl);
					//GUI.DrawTexture(new Rect(MaxWidth*0.0f, MaxWHeight*0.0f, presModel.TextureSlide.width, presModel.TextureSlide.height), presModel.TextureSlide);
				}
			} else if(presModel.SloodleSlideType.Contains("video")) {
				//Debug.Log("Presenter loading VIDEO: " + presModel.SloodleSlideUrl);	
				if(!m_bLoading) {
					Debug.Log("Presenter loading URL: " + presModel.SloodleSlideUrl);
					LoadURL(presModel.SloodleSlideUrl);
				}
			} else if (presModel.SloodleSlideType.Contains("html")) {				
				if(!m_bLoading) {
					Debug.Log("Presenter loading URL: " + presModel.SloodleSlideUrl);
					LoadURL(presModel.SloodleSlideUrl);
				}
			}
			GUILayout.EndArea();
	        GUILayout.EndScrollView();
			
			if(GUI.Button(new Rect(MaxWidth*0.66f, MaxWHeight*0.9f, MaxWidth*0.1f, MaxWHeight*0.1f), "Sair")) {
				cameraM.enabled = true;
				cameraA.enabled = false;
				cameraB.enabled = false;
			}
		 	if(GUI.Button(new Rect(MaxWidth*0.78f, MaxWHeight*0.9f, MaxWidth*0.1f, MaxWHeight*0.1f), "Voltar")) {
				pag = int.Parse(presModel.SloodleSlideNum) - 1;
				if(pag < 1) {
					presModel.SloodleSlideNum = "1";
				} else {
					m_bLoading = false;
					presModel.SloodleSlideNum = pag.ToString();
				}
				presBCrtl.onChangeSlide();
				Debug.Log("Presenter Voltar: " + presModel.SloodleSlideNum);
			}	
		 	if(GUI.Button(new Rect(MaxWidth*0.9f, MaxWHeight*0.9f, MaxWidth*0.1f, MaxWHeight*0.1f), "Proximo")) {
				m_bLoading = false;
				pag = int.Parse(presModel.SloodleSlideNum) + 1;
				presModel.SloodleSlideNum = pag.ToString();
				Debug.Log("Presenter Proximo: " + presModel.SloodleSlideNum);
				presBCrtl.onChangeSlide();
			}
		}
    }
	
	//Carrega URL
	public void LoadURL(string url)
    {
		m_bLoading = true;
        if (m_bInitialized) {
            UnityWebCore.LoadURL(m_TextureID, url);
		}
    }
	
	// Update is called once per frame
	void Update () {
		if (m_bInitialized) {           
            UnityWebCore.Update();           
            if (UnityWebCore.IsDirty(m_TextureID))
            {                
                m_texture.SetPixels(m_pixels, 0);
                m_texture.Apply();
            }
        }
		if (browserEventHandler.focused && Input.inputString.Length > 0)
        {
            // Send WM_CHAR message
            for(int i = 0; i < Input.inputString.Length; ++i)
            {
                // Backspace - Remove the last character
                if (Input.inputString[i] == '\b') 
                {
					//Debug.Log("backspace");
					UnityWebCore.InjectKeyboard(m_TextureID, 0x0100, 0x08, 0);
					UnityWebCore.InjectKeyboard(m_TextureID, 0x0101, 0x08, 0);
				}
				else if(Input.inputString[i] == '\r' || Input.inputString[i] == '\n')
				{
					//Debug.Log("enter");
					UnityWebCore.InjectKeyboard(m_TextureID, 0x0100, 0x0D, 0);
					UnityWebCore.InjectKeyboard(m_TextureID, 0x0101, 0x0D, 0);
				}
                else
                    UnityWebCore.InjectKeyboard(m_TextureID, 0x0102, Input.inputString[i], 0);
            }
        }
		
		int dyScroll = (int)Input.GetAxis("Mouse ScrollWheel");
        if (dyScroll != 0)
        {
			//Debug.Log("Mouse Scroll" + dyScroll);
            UnityWebCore.MouseWheel(m_TextureID, dyScroll);
        }
		
		if(browserEventHandler.hovered)
		{
			if(Input.GetMouseButtonDown (2))
				browserEventHandler.handleMouseDown(1);
			else if(Input.GetMouseButtonDown (1))
				browserEventHandler.handleMouseDown(2);
			else if(Input.GetMouseButtonUp (2))
				browserEventHandler.handleMouseUp(1);
			else if(Input.GetMouseButtonUp (1))
				browserEventHandler.handleMouseUp(2);
		}
	}
	
	public void Destroy()
    {
        try
        {
            if (m_TextureID != 0)
            {
                UnityWebCore.DestroyView(m_TextureID);
				if(m_pixels != null)
					m_pixelsHandler.Free();
				Destroy(m_texture);
                GetComponent<PresenterBWebViewEvents>().interactive = false;
                m_TextureID = 0;

                m_bInitialized = false;
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }       
    }
	
	// Called once on leaving app
	void OnApplicationQuit()
    {
         UnityWebCore.DestroyView(m_TextureID);
    }
	
	public void onBeginNavigationDelFunc(string frameName, string url)
	{
		//Debug.Log("BeginNavigation to " + url);
	}
	
    public void onBeginLoadingDelFunc(string frameName, string url, int statusCode, string mimeType)
	{
		//Debug.Log("BeginLoading: " + url);
	}
    public void onFinishLoadingDelFunc()
	{
		//Debug.Log("FinishLoading");
	}
	
	public void onReceiveTitleDelFunc(string frameName, string title)
	{
		//Debug.Log("ReceiveTitle: " + title);
	}
	
    public void onChangeTooltipDelFunc(string tooltip)
	{
		//Debug.Log("Change tootip: " + tooltip);
	}
	
	public void onChangeCursorDelFunc(int cursorID)
	{
		//Debug.Log("Change cursor: " + cursorID);
	}
	
    public void onChangeTargetURLDelFunc(string url)
	{
		//Debug.Log("ChangeTargetURL: " + url);
	}
}
