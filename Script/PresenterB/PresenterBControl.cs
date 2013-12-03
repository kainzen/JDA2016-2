using UnityEngine;
using System.Collections;
using UnitySchool;

public class PresenterBControl : MonoBehaviour {
	
	private PresenterModel presModel;
	
	// Use this for initialization
	void Start () {
	
	}
	
	public void setModel(PresenterModel presModel) {
		this.presModel = presModel;
		presModel.SloodleControllerId = "6";
		presModel.SloodlePwd = "6fb5ad63-3a27-4360-baa1-13ea45859ab9|758442534";
		presModel.SloodleModuleId = "17";	
		presModel.SloodleSlideNum = "1";
		presModel.SloodleServerRoot = "http://192.168.25.50:8080";
		presModel.PresenterLinker = "/mod/sloodle/mod/presenter-2.0/linker.php";
		onChangeSlide();
	}
	
	//Metodo de Controle, retorna estrutura de um Form e envia pro webserver gerando uma nova thread
	public void onChangeSlide() {		
		WWWForm form = new WWWForm();
        form.AddField("sloodlecontrollerid", presModel.SloodleControllerId);
        form.AddField("sloodlepwd", presModel.SloodlePwd);
        form.AddField("sloodlemoduleid", presModel.SloodleModuleId);
        form.AddField("sloodleslidenum", presModel.SloodleSlideNum);
		WWW www = new WWW(presModel.SloodleServerRoot + presModel.PresenterLinker, form);
		
        StartCoroutine(WaitForRequest(www));
	}
	
	//Recebe a resposta do servidor e atualiza a url da imagem
	public void UpdatePresenterData(WWW www) {
		string[] fullMsg = www.text.Split('\n');		
		string[] msg = fullMsg[2].Split('|');
		if(msg[0] == "1")
			presModel.SloodleSlideNum = "1";
		presModel.SloodleSlideUrl = msg[2];
		presModel.SloodleSlideType = msg[1];
		presModel.IsLoading = true;
		if(presModel.SloodleSlideType.Contains("image")) {
			presModel.SloodleSlideUrl = presModel.SloodleSlideUrl.Replace("localhost", "192.168.25.50");
			//loadImage();
		} else if(presModel.SloodleSlideType.Contains("video")) {
			presModel.SloodleSlideUrl = presModel.SloodleSlideUrl.Replace("localhost", "192.168.25.50");			
		} else if (presModel.SloodleSlideType.Contains("html")) {
			presModel.SloodleSlideUrl = presModel.SloodleSlideUrl.Replace("localhost", "192.168.25.50");			
		}
	}
	
	public void loadImage() {
		presModel.IsLoading = true;
		WWW www = new WWW(presModel.SloodleSlideUrl);
		StartCoroutine(WaitForRequestImage(www));
	}  
	
	//Espera do Request terminar para retornar o resultado do envio de request
	IEnumerator WaitForRequest(WWW www) {		
        yield return www;
 
        if (www.error == null) {
            Debug.Log("WWW Ok!: " + www.text);
			UpdatePresenterData(www);			
        } else {
            Debug.Log("WWW Error: "+ www.error);
			onChangeSlide();
        }    
    }  	
	
	//Conjunto de Metodos que faz download da imagem ou carrega a pagina e renderiza no Presenter
	IEnumerator WaitForRequestImage(WWW www) {		
        yield return www;
 
        if (www.error == null) {
            Debug.Log("WWW Ok!: " + www.text);			
			presModel.TextureSlide = www.texture; 
			presModel.IsLoading = false;
        } else {
            Debug.Log("WWW Error: "+ www.error);
        }    
    } 
	
	
	 
	
	// Update is called once per frame
	void Update () {}
}
