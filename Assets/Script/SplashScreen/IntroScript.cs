using UnityEngine;
using System.Collections;

public class IntroScript : MonoBehaviour {

	enum LOGIN_STATUT{
		VALID,
		CHOOSE,
		PASSWORD,
		INVALID,
		NONE
	}
	//GUI
	public Rect labelConnecting;
	public Rect ButtonProfiles;
	public Rect nextButton;
	public Rect prevButton;
	public Rect labelInfo;
	public Rect ChooseButton;
	public Rect CreateNewButton;
	public Rect BackButton;
	public Rect TextAreaPassword;
	public float offset;
	public float speedLetters;
	
	public GUISkin skin;
	private string labelToDisplay;
	private int posLength;
	//private float time;
	
	
	//choosing profile
	private int startNumber;
	private int profileSelected;
	private string infoToDisplay;
	private int posLengthInfo;
	public float speedLettersInfo;
	private string password;
	
	private LOGIN_STATUT ls;
	
	void Start(){
		TextManager.Instance.LoadTextFile();
		ProfileManager.Instance.CreateBunchTestProfile();
		labelToDisplay = TextManager.Instance.texts["SplashScreen"]["LOADING"];
		posLength = 0;
	//	time = 0;
		ls = LOGIN_STATUT.NONE;
		StartCoroutine(TextLogin());
		startNumber = 0;
		profileSelected = -1;
		password = "";
	}
	
	
	void OnGUI(){
		GUI.skin = skin;
		GUI.Label(new Rect(labelConnecting.x*Screen.width, labelConnecting.y*Screen.height, labelConnecting.width*Screen.width, labelConnecting.height*Screen.height), labelToDisplay.Substring(0, posLength), "CenteredLabel");
	
		
		if(ls == LOGIN_STATUT.PASSWORD){
			
			GUI.SetNextControlName("pass");
			password = GUI.PasswordField(new Rect(TextAreaPassword.x*Screen.width, TextAreaPassword.y*Screen.height, TextAreaPassword.width*Screen.width, TextAreaPassword.height*Screen.height), password, '*');
			
			GUI.FocusControl("pass");
			
			if(Event.current.isKey && Event.current.keyCode == KeyCode.Return){
				if(password == ProfileManager.Instance.profiles[profileSelected].password){
					ls = LOGIN_STATUT.NONE;
					ProfileManager.Instance.setCurrentProfile(ProfileManager.Instance.profiles[profileSelected]);
					labelToDisplay = TextManager.Instance.texts["SplashScreen"]["CONNECTING_SUCCEED"].Replace("USER_NAME", ProfileManager.Instance.currentProfile.name);
					posLength = 0;	
					StartCoroutine(TextDisplay(LOGIN_STATUT.VALID));
				}else{
					labelToDisplay = TextManager.Instance.texts["SplashScreen"]["CONNECTING_PASSWORDFAIL"];
					posLength = 0;	
					StartCoroutine(TextDisplay(LOGIN_STATUT.NONE));
					password = "";
				}
			}
			
			if(GUI.Button(new Rect(BackButton.x*Screen.width, BackButton.y*Screen.height, BackButton.width*Screen.width, BackButton.height*Screen.height), "< back", "ButtonSimple")){
				ls = LOGIN_STATUT.NONE;
				labelToDisplay = TextManager.Instance.texts["SplashScreen"]["CONNECTING_CHOOSE"];
				posLength = 0;
				StartCoroutine(TextDisplay(LOGIN_STATUT.CHOOSE));
			}
		}
		
		
		if(ls == LOGIN_STATUT.CHOOSE){
			for(int i=startNumber; i<startNumber+4 && i<ProfileManager.Instance.profiles.Count;i++){
				if(GUI.Button(new Rect(ButtonProfiles.x*Screen.width, (ButtonProfiles.y + offset*(i-startNumber))*Screen.height, ButtonProfiles.width*Screen.width, ButtonProfiles.height*Screen.height), ProfileManager.Instance.profiles[i].name, profileSelected == i ? "ButtonSimpleSelected" : "ButtonSimple")){
					profileSelected = i;
					infoToDisplay = "Name : " + ProfileManager.Instance.profiles[i].name + "\n\n" +
					"Number of song played : " + ProfileManager.Instance.profiles[i].scoreOnSong.Count + "\n\n" +
					"Game time : " + "00:00h" + "\n\n\n\n" +
					"Story mode progression : " + "0%" + "\n\n" + 
					"Achievement : " + "0/400" + "\n\n";
					StopAllCoroutines();
					StartCoroutine(TextInfo());
				}
				
			}
			if((startNumber+4) < ProfileManager.Instance.profiles.Count){
				if(GUI.Button(new Rect(nextButton.x*Screen.width, nextButton.y*Screen.height, nextButton.width*Screen.width, nextButton.height*Screen.height), "next >", "ButtonSimple")){
					startNumber += 4;
				}
			}
			if(startNumber > 0){
				if(GUI.Button(new Rect(prevButton.x*Screen.width, prevButton.y*Screen.height, prevButton.width*Screen.width, prevButton.height*Screen.height), "< prev", "ButtonSimple")){
					startNumber -= 4;
				}
			}
			
			if(GUI.Button(new Rect(CreateNewButton.x*Screen.width, CreateNewButton.y*Screen.height, CreateNewButton.width*Screen.width, CreateNewButton.height*Screen.height), "NEW PROFILE", "ButtonSimple")){
				labelToDisplay = "";
				posLength = 0;
				ls = LOGIN_STATUT.INVALID;
			}
			
			if(profileSelected != -1){
				GUI.Label(new Rect(labelInfo.x*Screen.width, labelInfo.y*Screen.height, labelInfo.width*Screen.width, labelInfo.height*Screen.height), infoToDisplay.Substring(0, posLengthInfo));
			
				if(GUI.Button(new Rect(ChooseButton.x*Screen.width, ChooseButton.y*Screen.height, ChooseButton.width*Screen.width, ChooseButton.height*Screen.height), "VALID", "ButtonSimple")){
					ls = LOGIN_STATUT.NONE;
					posLength = 0;
					labelToDisplay = TextManager.Instance.texts["SplashScreen"]["CONNECTING_PASSWORD"] + " : ";
					StartCoroutine(TextDisplay(LOGIN_STATUT.PASSWORD));
				}
			}
		}
		
		
		
	
	}
	
	
	void Update(){
		
	}
	
	
	IEnumerator TextInfo(){
		posLengthInfo = 0;
		while(posLengthInfo < infoToDisplay.Length){
			posLengthInfo++;
			yield return new WaitForSeconds(speedLettersInfo);
		}
	}
	
	IEnumerator TextDisplay(LOGIN_STATUT lst){
		while(posLength < labelToDisplay.Length){
			posLength++;
			yield return new WaitForSeconds(speedLetters);
		}
		if(lst != LOGIN_STATUT.NONE){
			ls = lst;	
		}
	}
	
	
	IEnumerator TextLogin(){
		yield return new WaitForFixedUpdate();
		while(posLength < labelToDisplay.Length){
			posLength++;
			yield return new WaitForSeconds(speedLetters);
		}
		LoadManager.Instance.Loading();
		labelToDisplay = TextManager.Instance.texts["SplashScreen"]["CONNECTING"];
		posLength = 0;
		while(posLength < labelToDisplay.Length){
			posLength++;
			yield return new WaitForSeconds(speedLetters);
		}
		yield return new WaitForSeconds(1f);
		ProfileManager.Instance.LoadProfiles();
		var verif = ProfileManager.Instance.verifyCurrentProfile();
		
		if(verif){
			labelToDisplay = TextManager.Instance.texts["SplashScreen"]["CONNECTING_SUCCEED"].Replace("USER_NAME", ProfileManager.Instance.currentProfile.name);
			posLength = 0;
			while(posLength < labelToDisplay.Length){
				posLength++;
				yield return new WaitForSeconds(speedLetters);
				
			}
			ls = LOGIN_STATUT.VALID;
		}else if(ProfileManager.Instance.profiles.Count > 0){
			labelToDisplay = TextManager.Instance.texts["SplashScreen"]["CONNECTING_CHOOSE"];
			posLength = 0;
			while(posLength < labelToDisplay.Length){
				posLength++;
				yield return new WaitForSeconds(speedLetters);
			}
			ls = LOGIN_STATUT.CHOOSE;
		}else{
			labelToDisplay = TextManager.Instance.texts["SplashScreen"]["CONNECTING_FAIL"];
			posLength = 0;
			while(posLength < labelToDisplay.Length){
				posLength++;
				yield return new WaitForSeconds(speedLetters);
				
			}
			yield return new WaitForSeconds(1f);
			labelToDisplay = "";
			posLength = 0;
			ls = LOGIN_STATUT.INVALID;
		}
		
		
	
	}
	
	
	
}