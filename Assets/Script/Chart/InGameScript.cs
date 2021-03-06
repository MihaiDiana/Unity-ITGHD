using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class InGameScript : MonoBehaviour {
	
	#region Declarations
	//Game object song scene
	private GameObject arrow;
	public List<GameObject> typeArrow;
	public GameObject freeze;
	public GameObject mines;
	public Camera MainCamera;
	private Transform TMainCamera;
	public GameObject Background;
	public GameObject CameraBackground;
	private GameObject arrowLeft;
	private GameObject arrowRight;
	private GameObject arrowDown;
	private GameObject arrowUp;
	private Transform[] arrowTarget;
	public Material[] matSkinModel;
	private Material matArrowModel;
	public GameObject lifeBar;
	public GameObject timeBar;
	public GameObject slow;
	public GameObject fast;
	public Camera particleComboCam;
	
	private LifeBar theLifeBar;
	private TimeBar theTimeBar;
	
	private Song thesong;
	
	//USED FOR UPDATE FUNCTION
	private double timebpm; //Temps jou� sur la totalit�, non live, non remise � 0
	private float timechart; //Temps jou� sur le bpm actuel en live avec remise � 0
	private float timestop; //Temps utilis� pour le freeze
	private float totaltimestop; //Temps total � �tre stopp�
	private double timetotalchart; //Temps jou� sur la totalit� en live (timebpm + timechart)
	private float changeBPM; //Position en y depuis un changement de bpm (nouvelle reference)
	private double keybpms;
	private double valuebpms;
	private double keystops;
	private double valuestops;
	
	private int nextSwitchBPM; //Prochain changement de bpm
	private int nextSwitchStop;
	
	private double actualBPM; //Bpm actuel
	
	private double actualstop;
	private float speedmod; //speedmod 
	public float speedmodRate = 2f; //speedmod ajustation 
	private float rateSelected;
	
	//Temps pour le lachement de freeze
	public float unfrozed = 0.350f;
	
	private KeyCode KeyCodeUp;
	private KeyCode KeyCodeDown;
	private KeyCode KeyCodeLeft;
	private KeyCode KeyCodeRight;
	private KeyCode SecondaryKeyCodeUp;
	private KeyCode SecondaryKeyCodeDown;
	private KeyCode SecondaryKeyCodeLeft;
	private KeyCode SecondaryKeyCodeRight;
	
	//DISPLAY PREC
	
	//Temps pour l'affichage des scores
	public float limitDisplayScore = 2f;
	private float timeDisplayScore = 0f;
	public float timeClignotementFantastic = 0.05f;
	public float baseZoom = 20f;
	public float vitesseZoom = 0.1f;
	private bool sensFantastic;
	private float alpha;
	private float zoom;
	private Precision scoreToDisplay;
	public Rect posScore = new Rect(0.38f,0.3f,0.25f,0.25f);
	private Dictionary<Precision, int> countScore;
	
	//SLOWFAST
	public float ClignSpeed;
	public float stateSpeed; //-1 : fast, 0 : rien, 1 : slow
	private bool changeColorSlow = false;
	private bool changeColorFast = false;
	
	//LIFE
	private float life;
	private Dictionary<string, float> lifeBase;
	
	
	//SCORE
	private float score;
	private float scoreInverse;
	private float targetScoreInverse;
	private float fantasticValue;
	private Dictionary<string, float> scoreBase;
	private Dictionary<string, int> scoreCount;
	public Rect posPercent = new Rect(0.39f, 0f, 0.45f, 0.05f);
	
	//PassToDataManager
	private List<double> precAverage;
	private Dictionary<double, int> timeCombo;
	private Dictionary<double, double> lifeGraph;
	public double firstEx;
	public double firstGreat;
	public double firstMisteak;
	
	//FPS
	private long _count;
	private long fps;
	private float _timer;
	
	//CHART
	private List<Arrow> arrowLeftList;
	private List<Arrow> arrowRightList;
	private List<Arrow> arrowDownList;
	private List<Arrow> arrowUpList;
	private List<Arrow> mineLeftList;
	private List<Arrow> mineRightList;
	private List<Arrow> mineDownList;
	private List<Arrow> mineUpList;
	private List<Arrow> keyToRemove;
	private Arrow[] keyToCheck;
	//Dico des arrow prises en freeze
	private Dictionary<Arrow, float> arrowFrozen;
	
	//PARTICLE SYSTEM
	
	private Dictionary<string, ParticleSystem> precLeft;
	private Dictionary<string, ParticleSystem> precRight;
	private Dictionary<string, ParticleSystem> precUp;
	private Dictionary<string, ParticleSystem> precDown;
	private Dictionary<string, ParticleSystem> clearcombo;
	public ParticleSystem[] slowandfast; //0 slow 1 fast
	
	
	
	//GUI
	private Dictionary<string, Texture2D> TextureBase;
	private float wd;
	private float hg;
	private float hgt;
	public float ecart;
	public float ecartCombo;
	private int[] displaying; //score decoup
	private int[] thetab; //combo decoup
	public GUISkin skin;
	public Rect infoDifficulty;
	public Rect infoLevelDifficulty;
	public Rect infoSong;
	public Rect SpeedModTextAera;
	public Rect SpeedModText;
	public Rect SpeedModTextInfo;
	private string speedmodstring;
	private float speedmodSelected;
	private bool speedmodok;
	private int levelToDisplay;
	private string infoToDisplay;
	private Rect infoDifficultyCalc;
	private Rect infoDifficultyLevelCalc;
	private Rect infoSongCalc;
	private Rect infoDifficultyCalcShadow;
	private Rect infoDifficultyLevelCalcShadow;
	private Rect infoSongCalcShadow;
	private Color black = new Color(0f, 0f, 0f, 1f);
	private Color white = new Color(1f, 1f, 1f, 1f);
	
	//DISPLAY
	private Color bumpColor;
	public float bumpfadeSpeed = 0.5f;
	public bool[] displayValue;
	
	//START
	private bool firstUpdate;
	private float oneSecond;
	private float startTheSong; //Time pour d�marrer la chanson
	private bool scenechartfaded;
	
	//BUMP
	private int nextBump;
	private List<double> Bumps;
	public float speedBumps;
	
	
	//Input Bump
	public float speedBumpInput;
	private float scaleBase;
	public float percentScaleInput;
	
	//PROGRESSBAR
	private double firstArrow;
	private double lastArrow;
	
	//COMBO
	private int combo;
	public Rect posCombo = new Rect(0.40f, 0.5f, 0.45f, 0.05f);
	public Rect posdispCombo = new Rect(0.40f, 0.5f, 0.45f, 0.05f);
	private ComboType ct;
	public float speedCombofade;
	private float colorCombo;
	private Color theColorCombo;
	private bool alreadytaged = true;
	private int signCombo = -1;
	private int comboMisses;
	private float alphaCombo;
	public float speedAlphaCombo;
	
	//FAIL OR CLEAR
	private bool fail;
	private bool dead;
	private bool clear;
	private bool fullCombo;
	private bool fullExCombo;
	private bool fullFantCombo;
	private bool perfect;
	private bool isFullComboRace;
	private bool isFullExComboRace;
	private int typeOfDeath; // 0 : Immediatly, 1 : After 30 misses, 2 : Never
	public float timeFailAppear;
	private float timeFailDisappear;
	public float timeClearDisappear;
	private float zoomfail;
	public float speedzoom;
	public float speedziwp;
	private float zwip;
	public Rect posFail;
	public Rect posClear;
	public Rect posRetry;
	public Rect posGiveUp;
	private bool appearFailok;
	private bool disappearFailok;
	public float speedAlphaFailFade;
	private float failalpha;
	private float buttonfailalpha;
	public float speedbuttonfailalpha;
	private bool cacheFailed;
	public float speedFadeAudio;
	private float passalpha;
	public Rect fullComboPos;
	private bool deadAndRetry;
	private bool deadAndGiveUp;
	
	//SOUND
	private AudioClip songLoaded;
	public AudioClip failedSound;
	public AudioClip comboSound;
	public AudioSource secondAudioSource;
	public AudioSource mainAudioSource;
	//DEBUG
	//private int iwashere;
	#endregion
	
	
	//Start
	void Start () {
		
		//Data from option
		speedmod = DataManager.Instance.speedmodSelected*speedmodRate;
		rateSelected = DataManager.Instance.rateSelected;
		DataManager.Instance.LoadScoreJudge(DataManager.Instance.scoreJudgeSelected);
		DataManager.Instance.LoadHitJudge(DataManager.Instance.hitJudgeSelected);
		DataManager.Instance.LoadLifeJudge(DataManager.Instance.lifeJudgeSelected);
		isFullComboRace = DataManager.Instance.raceSelected == 9;
		isFullExComboRace = DataManager.Instance.raceSelected == 10;
		targetScoreInverse = DataManager.Instance.giveTargetScoreOfRace(DataManager.Instance.raceSelected);
		typeOfDeath = DataManager.Instance.deathSelected;
		KeyCodeDown = DataManager.Instance.KeyCodeDown;
		KeyCodeUp = DataManager.Instance.KeyCodeUp;
		KeyCodeLeft = DataManager.Instance.KeyCodeLeft;
		KeyCodeRight = DataManager.Instance.KeyCodeRight;
		SecondaryKeyCodeDown = DataManager.Instance.SecondaryKeyCodeDown;
		SecondaryKeyCodeDown = DataManager.Instance.SecondaryKeyCodeDown;
		SecondaryKeyCodeUp = DataManager.Instance.SecondaryKeyCodeUp;
		SecondaryKeyCodeLeft = DataManager.Instance.SecondaryKeyCodeLeft;
		SecondaryKeyCodeRight = DataManager.Instance.SecondaryKeyCodeRight;
		speedmodSelected = DataManager.Instance.speedmodSelected;
		speedmodstring = DataManager.Instance.speedmodSelected.ToString("0.00");
		speedmodok = true;
		var rand = (int)(UnityEngine.Random.value*DataManager.Instance.skyboxList.Count);
		if(rand == DataManager.Instance.skyboxList.Count){
			rand--;	
		}
		
		
		
		//Arrows
		for(int i=0;i<4;i++){
			if(i != DataManager.Instance.skinSelected){
				var go = GameObject.Find("ArrowSkin" + i);
				Destroy(go);
				var go2 = GameObject.Find("ArrowModelSkin" + i);
				Destroy(go2);
			}
		}
		arrow = typeArrow.ElementAt(DataManager.Instance.skinSelected);
		var modelskin = GameObject.Find("ArrowModelSkin" + DataManager.Instance.skinSelected);
		modelskin.SetActiveRecursively(false);
		arrowLeft = (GameObject) Instantiate(modelskin, new Vector3(0f, 0f, 2f), modelskin.transform.rotation);
		if(DataManager.Instance.skinSelected == 3){
			arrowLeft.transform.FindChild("RotationCenter").Rotate(0f, 0f, 90f);
		}else if(DataManager.Instance.skinSelected != 0){
			arrowLeft.transform.Rotate(0f, 0f, 90f);
			arrowLeft.transform.FindChild("ParticulePrec").Rotate(0f, 0f, -90f);
			arrowLeft.transform.FindChild("ParticulePrec").localPosition = new Vector3(-1f, 0f, 0f);
		}
		
		arrowLeft.transform.parent = MainCamera.gameObject.transform;
		arrowRight = (GameObject) Instantiate(modelskin, new Vector3(6f, 0f, 2f), modelskin.transform.rotation);
		if(DataManager.Instance.skinSelected == 3){
			arrowRight.transform.FindChild("RotationCenter").Rotate(0f, 0f, -90f);
		}else if(DataManager.Instance.skinSelected == 1){
			arrowRight.transform.Rotate(0f, 0f, -90f);
			arrowRight.transform.FindChild("ParticulePrec").Rotate(0f, 0f, 90f);
			arrowRight.transform.FindChild("ParticulePrec").localPosition = new Vector3(1f, 0f, 0f);
		}
		arrowRight.transform.parent = MainCamera.gameObject.transform;
		arrowDown = (GameObject) Instantiate(modelskin, new Vector3(2f, 0f, 2f), modelskin.transform.rotation);
		if(DataManager.Instance.skinSelected == 3){
			arrowDown.transform.FindChild("RotationCenter").Rotate(0f, 0f, 180f);
		}else if(DataManager.Instance.skinSelected == 1){
			arrowDown.transform.Rotate(0f, 0f, 180f);
			arrowDown.transform.FindChild("ParticulePrec").Rotate(0f, 0f, -180f);
			arrowDown.transform.FindChild("ParticulePrec").localPosition = new Vector3(0f, 1f, 0f);
		}
		arrowDown.transform.parent = MainCamera.gameObject.transform;
		arrowUp = (GameObject) Instantiate(modelskin, new Vector3(4f, 0f, 2f), modelskin.transform.rotation);
		arrowUp.transform.parent = MainCamera.gameObject.transform;
		
		arrowTarget = new Transform[4];
		arrowTarget[0] = arrowLeft.transform;
		arrowTarget[1] = arrowDown.transform;
		arrowTarget[2] = arrowUp.transform;
		arrowTarget[3] = arrowRight.transform;
		scaleBase = arrowTarget[0].localScale.x;
		matArrowModel = matSkinModel[DataManager.Instance.skinSelected];
		
		
		RenderSettings.skybox = DataManager.Instance.skyboxList.ElementAt(rand);
		DataManager.Instance.skyboxIndexSelected = rand;
		displayValue = DataManager.Instance.displaySelected;
		
		firstArrow = -10f;
		lastArrow = -10f;
		thesong = (rateSelected == 0f ? DataManager.Instance.songSelected : DataManager.Instance.songSelected.getRatedSong(rateSelected));
		songLoaded = thesong.GetAudioClip();
		mainAudioSource.loop = false;
		createTheChart(thesong);
		Application.targetFrameRate = -1;
		QualitySettings.vSyncCount = 0;
		nextSwitchBPM = 1;
		nextSwitchStop = 0;
		actualBPM = thesong.bpms.First().Value;
		actualstop = (double)0;
		changeBPM = 0;
		
		_count = 0L;
		
		timebpm = (double)0;
		timechart = 0f;//-(float)thesong.offset;
		timetotalchart = (double)0;
		
		
		arrowFrozen = new Dictionary<Arrow, float>();
		keyToRemove = new List<Arrow>();
		keyToCheck = new Arrow[4];
		for(int i=0; i<4; i++) keyToCheck[i] = null;
		
		precRight = new Dictionary<string, ParticleSystem>();
		precLeft = new Dictionary<string, ParticleSystem>();
		precUp = new Dictionary<string, ParticleSystem>();
		precDown = new Dictionary<string, ParticleSystem>();
		clearcombo = new Dictionary<string, ParticleSystem>();
		
		//Prepare the scene
		foreach(var el in Enum.GetValues(typeof(PrecParticle))){
			precLeft.Add( el.ToString(), (ParticleSystem) arrowLeft.transform.FindChild("ParticulePrec").gameObject.transform.FindChild(el.ToString()).particleSystem );
			precDown.Add( el.ToString(), (ParticleSystem) arrowDown.transform.FindChild("ParticulePrec").gameObject.transform.FindChild(el.ToString()).particleSystem );
			precRight.Add( el.ToString(), (ParticleSystem) arrowRight.transform.FindChild("ParticulePrec").gameObject.transform.FindChild(el.ToString()).particleSystem );
			precUp.Add( el.ToString(), (ParticleSystem) arrowUp.transform.FindChild("ParticulePrec").gameObject.transform.FindChild(el.ToString()).particleSystem );
		}
		for(int i=0; i< particleComboCam.transform.GetChildCount(); i++){
			clearcombo.Add(particleComboCam.transform.GetChild(i).name, particleComboCam.transform.GetChild(i).particleSystem);
		}
		
		precAverage = new List<double>();
		timeCombo = new Dictionary<double, int>();
		lifeGraph = new Dictionary<double, double>();
		
		TMainCamera = MainCamera.transform;
		MoveCameraBefore();
		
		//Textures
		TextureBase = new Dictionary<string, Texture2D>();
		TextureBase.Add("FANTASTIC", (Texture2D) Resources.Load("Fantastic"));
		TextureBase.Add("EXCELLENT", (Texture2D) Resources.Load("Excellent"));
		TextureBase.Add("GREAT", (Texture2D) Resources.Load("Great"));
		TextureBase.Add("DECENT", (Texture2D) Resources.Load("Decent"));
		TextureBase.Add("WAYOFF", (Texture2D) Resources.Load("Wayoff"));
		TextureBase.Add("MISS", (Texture2D) Resources.Load("Miss"));
		for(int i=0; i<10; i++){
			TextureBase.Add("S" + i, (Texture2D) Resources.Load("Numbers/S" + i));
			TextureBase.Add("C" + i, (Texture2D) Resources.Load("Numbers/C" + i));
		}
		
		TextureBase.Add("PERCENT", (Texture2D) Resources.Load("Numbers/Percent"));
		TextureBase.Add("DOT", (Texture2D) Resources.Load("Numbers/Dot"));
		TextureBase.Add("COMBODISPLAY", (Texture2D) Resources.Load("DisplayCombo"));
		TextureBase.Add("BLACK", (Texture2D) Resources.Load("black"));
		TextureBase.Add("FAIL", (Texture2D) Resources.Load("Fail"));
		TextureBase.Add("CLEAR", (Texture2D) Resources.Load("Clear"));
		TextureBase.Add("FC", (Texture2D) Resources.Load("FC"));
		TextureBase.Add("FEC", (Texture2D) Resources.Load("FEC"));
		TextureBase.Add("FFC", (Texture2D) Resources.Load("FFC"));
		TextureBase.Add("PERFECT", (Texture2D) Resources.Load("Perfect"));
		TextureBase.Add("DIFFICULTY", (Texture2D) Resources.Load(Enum.GetName(typeof(Difficulty), (thesong.difficulty)).ToLower()));
		
		//stuff
		scoreToDisplay = Precision.NONE;
		timeDisplayScore = Mathf.Infinity;
		sensFantastic = true;
		alpha = 1f;
		scenechartfaded = false;
		
		
		
		//init score and lifebase
		scoreBase = new Dictionary<string, float>();
		scoreCount = new Dictionary<string, int>();
		lifeBase = new Dictionary<string, float>();
		fantasticValue = 100f/(thesong.numberOfStepsWithoutJumps + thesong.numberOfJumps + thesong.numberOfFreezes + thesong.numberOfRolls);
		foreach(Precision el in Enum.GetValues(typeof(Precision))){
			if(el != Precision.NONE){
				scoreBase.Add(el.ToString(), fantasticValue*DataManager.Instance.ScoreWeightValues[el.ToString()]);
				lifeBase.Add(el.ToString(), DataManager.Instance.LifeWeightValues[el.ToString()]);
				
			}
		}
		
		foreach(ScoreCount el2 in Enum.GetValues(typeof(ScoreCount))){
			if(el2 != ScoreCount.NONE){
				scoreCount.Add(el2.ToString(), 0);
			}
		}
		
		
		
		life = 50f;
		lifeGraph.Add(0, life);
		score = 0f;
		scoreInverse = 100f;
		firstEx = -1;
		firstGreat = -1;
		firstMisteak = -1;
		
		theLifeBar = lifeBar.GetComponent<LifeBar>();
		theTimeBar = timeBar.GetComponent<TimeBar>();
		
		//TimeBar
		
		theTimeBar.setDuration((float)firstArrow, (float)lastArrow);
		
		//var bps = thesong.getBPS(actualBPM);
		//changeBPM -= (float)(bps*thesong.offset)*speedmod;
		firstUpdate = true;
		oneSecond = 0f;
		
		startTheSong = (float)thesong.offset + DataManager.Instance.globalOffsetSeconds + DataManager.Instance.userGOS;
		
		//bump
		nextBump = 0;
		
		//combo
		ct = ComboType.FULLFANTASTIC;
		colorCombo = 1f;
		comboMisses = 0;
		alphaCombo = 0.5f;
		theColorCombo = white;
		//GUI
		/*wd = posPercent.width*128;
		hg = posPercent.height*1024;
		hgt = posPercent.height*254;*/
		//ecart = 92f;
		
		displaying = scoreDecoupe();
		thetab = comboDecoupe();
		
		
		//Fail and clear
		fail = false;
		clear = false;
		fullCombo = false;
		fullExCombo = false;
		fullFantCombo = false;
		perfect = false;
		dead = false;
		zwip = 0;
		appearFailok = false;
		disappearFailok = false;
		zoomfail = 0f;
		failalpha = 0f;
		passalpha = 0f;
		cacheFailed = true;
		
		
		//GUI
		var title = thesong.title;
		if(title.Length > 30) title = title.Remove(30, title.Length - 30) + "...";
		var artist = thesong.artist;
		if(artist.Length > 30) artist = artist.Remove(30, artist.Length - 30) + "...";
		var stepartist = thesong.stepartist;
		if(stepartist.Length > 30) stepartist = stepartist.Remove(30, stepartist.Length - 30) + "...";
		infoToDisplay = title + "\n" + artist + "\n"  + stepartist + "\n\n";
		
		var mystat = ProfileManager.Instance.FindTheSongStat(thesong.sip);
		if(mystat != null)
		{
			if(mystat.fail)
			{
				infoToDisplay += "Never cleared\n";
			}else{
				infoToDisplay += "Personnal best : " + mystat.score.ToString("0.00") + "%\n";
			}
		}else{
			infoToDisplay += "First try\n";
		}
		
		var kv = ProfileManager.Instance.FindTheBestScore(thesong.sip);
		if(kv.Key != -1)
		{	
			infoToDisplay += "Friends best :" + kv.Key.ToString("0.00") + "%" + "\n";
			infoToDisplay += "Mastered by " + kv.Value + "\n";
		}else{
			infoToDisplay += "No friends score entry\n";
		}
		levelToDisplay = thesong.level;
		
		
		//Transformation display
		if(displayValue[4]){ //No judge
			limitDisplayScore = -1f;
		}
		
		if(displayValue[5]){ //No background
			RenderSettings.skybox = null;
			Background.GetComponent<MoveBackground>().enabled = false;
			Background.SetActiveRecursively(false);
			
		}
		
		if(displayValue[6]){ //No target
			if(arrowLeft.transform.childCount > 1)
			{
				for(int i=0; i<arrowLeft.transform.childCount; i++)
				{
					if(arrowLeft.transform.GetChild(i).name != "ParticulePrec") 
						arrowLeft.transform.GetChild(i).renderer.enabled = false;
				}
				
				for(int i=0; i<arrowDown.transform.childCount; i++)
				{
					if(arrowDown.transform.GetChild(i).name != "ParticulePrec") 
						arrowDown.transform.GetChild(i).renderer.enabled = false;
				}
				
				for(int i=0; i<arrowUp.transform.childCount; i++)
				{
					if(arrowUp.transform.GetChild(i).name != "ParticulePrec") 
						arrowUp.transform.GetChild(i).renderer.enabled = false;
				}
				
				for(int i=0; i<arrowRight.transform.childCount; i++)
				{
					if(arrowRight.transform.GetChild(i).name != "ParticulePrec") 
						arrowRight.transform.GetChild(i).renderer.enabled = false;
				}
			}else{
				arrowLeft.renderer.enabled = false;
				arrowDown.renderer.enabled = false;
				arrowUp.renderer.enabled = false;
				arrowRight.renderer.enabled = false;
			}
			
		}
		
		//No score : inside the code
		
		//No UI
		if(displayValue[8]){
			for(int i=0;i<lifeBar.transform.childCount; i++){
				if(lifeBar.transform.GetChild(i).childCount > 0){
					for(int j=0;j<lifeBar.transform.GetChild(i).childCount; j++){
						lifeBar.transform.GetChild(i).GetChild(j).renderer.enabled = false;	
					}
				}else{
					lifeBar.transform.GetChild(i).renderer.enabled = false;	
				}
				
			}
			for(int i=0;i<timeBar.transform.childCount; i++){
				timeBar.transform.GetChild(i).renderer.enabled = false;	
			}
			for(int i=0;i<fast.transform.childCount; i++){
				fast.transform.GetChild(i).renderer.enabled = false;	
			}
			for(int i=0;i<slow.transform.childCount; i++){
				slow.transform.GetChild(i).renderer.enabled = false;	
			}
			slow.renderer.enabled = false;
			fast.renderer.enabled = false;
		}
		
		infoSongCalc = new Rect(infoSong.x*Screen.width, infoSong.y*Screen.height, infoSong.width*Screen.width, infoSong.height*Screen.height);
		infoSongCalcShadow = new Rect(infoSong.x*Screen.width + 1, infoSong.y*Screen.height + 1, infoSong.width*Screen.width, infoSong.height*Screen.height);
		infoDifficultyCalc = new Rect(infoDifficulty.x*Screen.width, infoDifficulty.y*Screen.height, infoDifficulty.width*Screen.width, infoDifficulty.height*Screen.height);
		infoDifficultyLevelCalc = new Rect(infoLevelDifficulty.x*Screen.width, infoLevelDifficulty.y*Screen.height, infoLevelDifficulty.width*Screen.width, infoLevelDifficulty.height*Screen.height);
		infoDifficultyLevelCalcShadow = new Rect(infoLevelDifficulty.x*Screen.width + 1, infoLevelDifficulty.y*Screen.height + 1, infoLevelDifficulty.width*Screen.width, infoLevelDifficulty.height*Screen.height);
		
	}
	
	
	void OnApplicationQuit()
	{
		DataManager.Instance.removeRatedSong();
	}
	
	
	//only for FPS
	void OnGUI(){
		
		GUI.skin = skin;
		GUI.depth = 2;
		//fake stuff
		GUI.Label(new Rect(0.9f*Screen.width, 0.05f*Screen.height, 200f, 200f), fps.ToString());		
		//end fake stuff
		GUI.DrawTexture(infoDifficultyCalc, TextureBase["DIFFICULTY"]);
		
		GUI.color = black;
		GUI.Label(infoDifficultyLevelCalcShadow, levelToDisplay.ToString(), "infoDiff");
		GUI.Label(infoSongCalcShadow, infoToDisplay , "infoSong");
		
		GUI.color = DataManager.Instance.diffColor[(int)thesong.difficulty];
		GUI.Label(infoDifficultyLevelCalc, levelToDisplay.ToString(), "infoDiff");
		
		GUI.color = white;
		GUI.Label(infoSongCalc, infoToDisplay , "infoSong");
		
		if(timeDisplayScore < limitDisplayScore && !clear){

			GUI.color = new Color(1f, 1f, 1f, alpha);
			GUI.DrawTexture(new Rect(posScore.x*Screen.width - zoom, posScore.y*Screen.height, posScore.width*Screen.width + zoom*2, posScore.height*Screen.height), TextureBase[scoreToDisplay.ToString()]); 
		}
		
		GUI.color = white;
		
		if(!displayValue[7]){
				
			for(int i=0;i<5;i++){
				if((i == 3 && displaying[3] == 0 && displaying[4] == 0) || (i == 4 && displaying[4] == 0)) break;
				GUI.DrawTexture(new Rect((posPercent.x + ecart*(4-i))*Screen.width, posPercent.y*Screen.height,  posPercent.width*Screen.width,  posPercent.height*Screen.height), TextureBase["S" + displaying[i]]);
				
			}
			GUI.DrawTexture(new Rect((posPercent.x + ((ecart*2)+(ecart/2f)))*Screen.width, posPercent.y*Screen.height,  posPercent.width*Screen.width, posPercent.height*Screen.height), TextureBase["DOT"]);
			GUI.DrawTexture(new Rect((posPercent.x + ecart*5)*Screen.width + posPercent.width, posPercent.y*Screen.height, posPercent.width*Screen.width, posPercent.height*Screen.height), TextureBase["PERCENT"]);
		}
		
		if(!displayValue[8]){
			if(combo >= 5f){
				//var czoom = zoom/4f;
				GUI.color = new Color(theColorCombo.r , theColorCombo.g, theColorCombo.b, alphaCombo);
				for(int i=0; i<thetab.Length; i++){
					GUI.DrawTexture(new Rect((posCombo.x + ((ecartCombo*(thetab.Length-(i+1))/2f) -ecartCombo*((float)i/2f)))*Screen.width, 
					posCombo.y*Screen.height, posCombo.width*Screen.width, posCombo.height*Screen.height), TextureBase["C" + thetab[i]]);
				}
			}
		}
		
		
		
		if(dead){
			if(oneSecond > timeFailAppear){
				
				GUI.color = new Color(1f, 1f, 1f, failalpha);
				GUI.DrawTexture(new Rect(0f,0f, Screen.width*1.2f, Screen.height*1.2f), TextureBase["BLACK"]);
				var ratiow = (float)posFail.width/(float)Mathf.Max (posFail.width, posFail.height);
				var ratioh = (float)posFail.height/(float)Mathf.Max (posFail.width, posFail.height);
				GUI.color = new Color(1f, 1f, 1f, failalpha*alpha);
				if(!cacheFailed) GUI.DrawTexture(new Rect((posFail.x - zwip - ratiow*zoomfail/2f)*Screen.width, (posFail.y + zwip - ratioh*zoomfail/2f)*Screen.height ,
					(posFail.width + zwip*2 + ratiow*zoomfail)*Screen.width, (posFail.height - zwip*2 + ratioh*zoomfail)*Screen.height), TextureBase["FAIL"]);
				if(failalpha >= 1f){
					GUI.color = new Color(1f, 1f, 1f, buttonfailalpha);
					if(GUI.Button(new Rect(posRetry.x*Screen.width, posRetry.y*Screen.height, posRetry.width*Screen.width, posRetry.height*Screen.height), "Retry") && !deadAndRetry && !deadAndGiveUp && buttonfailalpha > 0.5f && speedmodok && GetComponent<DebugOffset>().validValue){
							DataManager.Instance.speedmodSelected = speedmodSelected;
							deadAndRetry = true;
					}
					
					if(GUI.Button(new Rect(posGiveUp.x*Screen.width, posGiveUp.y*Screen.height, posGiveUp.width*Screen.width, posGiveUp.height*Screen.height), "Give up") && !deadAndRetry && !deadAndGiveUp && buttonfailalpha > 0.5f && GetComponent<DebugOffset>().validValue){
							deadAndGiveUp = true;
					}
					
					GUI.color = new Color(0.7f, 0.7f, 0.7f, buttonfailalpha);
					GUI.Label(new Rect(SpeedModText.x*Screen.width, SpeedModText.y*Screen.height, SpeedModText.width*Screen.width, SpeedModText.height*Screen.height), TextManager.Instance.texts["Util"]["SPEEDMOD_TEXT"]);
					speedmodstring = GUI.TextArea (new Rect(SpeedModTextAera.x*Screen.width, SpeedModTextAera.y*Screen.height, SpeedModTextAera.width*Screen.width, SpeedModTextAera.height*Screen.height), speedmodstring.Trim(), 5);
								
					if(!String.IsNullOrEmpty(speedmodstring)){
						double result;
						if(System.Double.TryParse(speedmodstring, out result)){
							if(result >= (double)0.25 && result <= (double)15){
								speedmodSelected = (float)result;
								speedmodok = true;
								var bpmdisplaying = thesong.bpmToDisplay;
								if(bpmdisplaying.Contains("->")){
									bpmdisplaying = (System.Convert.ToDouble(bpmdisplaying.Replace(">", "").Split('-')[0])*speedmodSelected*(1f + (rateSelected/100f))).ToString("0") + "->" + (System.Convert.ToDouble(bpmdisplaying.Replace(">", "").Split('-')[1])*speedmodSelected*(1f + (rateSelected/100f))).ToString("0");
								}else{
									bpmdisplaying = (System.Convert.ToDouble(bpmdisplaying)*speedmodSelected*(1f + (rateSelected/100f))).ToString("0");
								}
								GUI.Label(new Rect((SpeedModTextInfo.x)*Screen.width, SpeedModTextInfo.y*Screen.height, SpeedModTextInfo.width*Screen.width, SpeedModTextInfo.height*Screen.height), "Speedmod : x" + speedmodSelected.ToString("0.00") + " (" + bpmdisplaying + " BPM)");
							}else{
								GUI.color = new Color(0.7f, 0.2f, 0.2f, 1f);
								GUI.Label(new Rect((SpeedModTextInfo.x)*Screen.width, SpeedModTextInfo.y*Screen.height, SpeedModTextInfo.width*Screen.width, SpeedModTextInfo.height*Screen.height), "Speedmod must be between x0.25 and x15");
								GUI.color = new Color(1f, 1f, 1f, 1f);
								speedmodok = false;
							}
						}else{
							GUI.color = new Color(0.7f, 0.2f, 0.2f, 1f);
							GUI.Label(new Rect((SpeedModTextInfo.x)*Screen.width, SpeedModTextInfo.y*Screen.height, SpeedModTextInfo.width*Screen.width, SpeedModTextInfo.height*Screen.height), "Speedmod is not a valid value");
							GUI.color = new Color(1f, 1f, 1f, 1f);
							speedmodok = false;
						}
					}else{
						GUI.color = new Color(0.7f, 0.2f, 0.2f, 1f);
						GUI.Label(new Rect((SpeedModTextInfo.x)*Screen.width, SpeedModTextInfo.y*Screen.height, SpeedModTextInfo.width*Screen.width, SpeedModTextInfo.height*Screen.height), "Empty value");
						GUI.color = new Color(1f, 1f, 1f, 1f);
						speedmodok = false;
					}
				}
			}
			
		}
				
		if(clear){
			//if(oneSecond > timeFailAppear){
				var ratiow = (float)posClear.width/(float)Mathf.Max (posClear.width, posClear.height);
				var ratioh = (float)posClear.height/(float)Mathf.Max (posClear.width, posClear.height);
				if(fullCombo || fullExCombo || fullFantCombo || perfect){
					GUI.color = new Color(1f, 1f, 1f, passalpha*alpha);
					GUI.DrawTexture(new Rect(fullComboPos.x*Screen.width,fullComboPos.y*Screen.width, fullComboPos.width*Screen.width, fullComboPos.height*Screen.height), TextureBase[perfect ? "PERFECT" : (fullFantCombo ? "FFC" : (fullExCombo ? "FEC" : "FC"))]);
				}
				//GUI.DrawTexture(new Rect(0f,0f, Screen.width*1.2f, Screen.height*1.2f), TextureBase["BLACK"]);
				
				GUI.color = new Color(1f, 1f, 1f, 1f);
				if(appearFailok) GUI.DrawTexture(new Rect((posClear.x - zwip - ratiow*zoomfail/2f)*Screen.width, (posClear.y + zwip - ratioh*zoomfail/2f)*Screen.height ,
					(posClear.width + zwip*2 + ratiow*zoomfail)*Screen.width, (posClear.height - zwip*2 + ratioh*zoomfail)*Screen.height), TextureBase["CLEAR"]);	
				GUI.color = new Color(1f, 1f, 1f, failalpha);
				GUI.DrawTexture(new Rect(0f,0f, Screen.width*1.2f, Screen.height*1.2f), TextureBase["BLACK"]);
			//}
			
		}
		
	}
	
	
	
	
	// Update is called once per frame
	void Update () {
		
		
		//FPS
		_count++;
		_timer += Time.deltaTime;
		if(_timer >= 1f){
			fps = _count;
			_count = 0L;
			_timer = 0;
		}
		
		if((!dead && oneSecond >= 1.5f) || clear){
			//timetotal for this frame
			
			timetotalchart = timebpm + timechart + totaltimestop;

			
			//Move Arrows
			//VersionLibre
			MoveArrows();
				
			//Verify inputs
			VerifyValidFrozenArrow();
			VerifyKeysInput();
			VerifyKeysOutput();
			
			//Changement bpm et stops
			VerifyBPMnSTOP();
			
			
			
			//Progress time chart
			if(actualstop != 0){
				
					
					
					if(timestop >= actualstop){
						timechart += timestop - (float)actualstop;
						totaltimestop += (float)actualstop - timestop;
						timetotalchart = timebpm + timechart + totaltimestop;
						actualstop = (double)0;
						timestop = 0f;
						timechart += Time.deltaTime;
				
					}else{
						totaltimestop += Time.deltaTime;
						timestop += Time.deltaTime;
					
					}

			}else{
				timechart += Time.deltaTime;

			}
			
			
			
			//GUI part, because GUI is so slow :(
			//Utile ?
			RefreshGUIPart();
			
			//Start song
			timetotalchart = timebpm + timechart + totaltimestop;
			
			
			if(firstUpdate){
				if(startTheSong <= 0f){
					mainAudioSource.PlayOneShot(songLoaded);
					timechart += startTheSong; 
					timetotalchart = timebpm + timechart + totaltimestop;
					firstUpdate = false;
					MoveArrows();
				}else{
					startTheSong -= Time.deltaTime;	
				}
				
			}
			
			BumpsBPM();
			
			//Fail/Clear part
			
			if(thesong.duration < timetotalchart && !fail && !clear){
				if(life > 0f){
					clear = true;
				}else{
					fail = true;
				}
				if(scoreCount["DECENT"] == 0 && scoreCount["WAYOFF"] == 0 && scoreCount["MISS"] == 0){
					if(score >= 100f || scoreInverse == 100f) perfect = true;
					if(scoreCount["EXCELLENT"] == 0 && scoreCount["GREAT"] == 0) fullFantCombo = true;
					if(scoreCount["GREAT"] == 0) fullExCombo = true;
					fullCombo = true;
					secondAudioSource.PlayOneShot(comboSound); 
				}
				oneSecond = 0f;
				failalpha = 0f;
				buttonfailalpha = 0f;
				passalpha = 1f;
				alpha = 1f;
				sensFantastic = true;
			}
			
			if(fail){
				if((typeOfDeath != 2 && (typeOfDeath == 0 || comboMisses >= 30)) || thesong.duration < timetotalchart){
					dead = true;
					mainAudioSource.Stop ();
					mainAudioSource.volume = 1f;
					mainAudioSource.PlayOneShot(failedSound);
					secondAudioSource.volume = 0f;
					secondAudioSource.loop = true;
					secondAudioSource.Play();
					Background.GetComponent<MoveBackground>().enabled = false;
					CameraBackground.GetComponent<MoveCameraBackground>().enabled = false;
					TMainCamera.GetComponent<GrayscaleEffect>().enabled = true;
					oneSecond = 0f;
					alpha = 1f;
					sensFantastic = true;
					GetComponent<DebugOffset>().enabled = true;
				}
			}
			
			if(clear){
				oneSecond += Time.deltaTime;
				if(mainAudioSource.volume > 0) mainAudioSource.volume -= Time.deltaTime/speedFadeAudio;
					
				if(!appearFailok){
					StartCoroutine(swipTexture(false, posClear.height, 0f));
					var contains = perfect ? "Perf" : (fullFantCombo ? "FFC" : (fullExCombo ? "FEC" : ( fullCombo ? "FBC" : "noPS")) );
					if(!contains.Contains("noPS")){
						particleComboCam.gameObject.active = true;
						foreach(var part in clearcombo.Where(c => c.Key.Contains(contains))){
							part.Value.gameObject.active = true;
							part.Value.Play();
						}
					}
					
					appearFailok = true;
					
				}
				if(oneSecond > timeClearDisappear){
					
				}
				if(oneSecond > timeClearDisappear){
					if(passalpha > 0) passalpha -= Time.deltaTime;
					if(failalpha >= 1){
							SendDataToDatamanager();
							Application.LoadLevel("ScoreScene");
					} 
					failalpha += Time.deltaTime/speedAlphaFailFade;
				}
				ClignCombo();
			}
			
			
			
		}else{
			oneSecond += Time.deltaTime;
			if(!dead) MoveCameraBefore();
			if(dead){
				if(oneSecond > timeFailAppear && !disappearFailok){
					//zoomfail += Time.deltaTime/speedzoom;
					secondAudioSource.volume += Time.deltaTime/speedAlphaFailFade;
					if(failalpha < 1) failalpha += Time.deltaTime/speedAlphaFailFade;
					if(failalpha >= 1 && buttonfailalpha < 1) buttonfailalpha += Time.deltaTime/speedbuttonfailalpha;
					ClignFailed();
				}
				if(!appearFailok && oneSecond > timeFailAppear + 1){
					StartCoroutine(swipTexture(false, posFail.height, 0f));
					appearFailok = true;
					cacheFailed = false;
				}
				if(!disappearFailok && (deadAndRetry || deadAndGiveUp)){
					StartCoroutine(swipTexture(true, posFail.height, 0.5f));
					disappearFailok = true;
					timeFailDisappear = oneSecond;
				}
				if(disappearFailok && oneSecond < (timeFailDisappear + 1f) && buttonfailalpha > 0){
					buttonfailalpha -= Time.deltaTime/speedbuttonfailalpha;
					secondAudioSource.volume -= Time.deltaTime/speedbuttonfailalpha;
				}
				if(disappearFailok && oneSecond > (timeFailDisappear + 1f) ){
					secondAudioSource.Stop();
					SendDataToDatamanager();
					if(deadAndRetry){
						Application.LoadLevel("ChartScene");
					}else if(deadAndGiveUp){
						Application.LoadLevel("ScoreScene");
					}
					
					
				}
			}else if(oneSecond >= 0.5f && !scenechartfaded){
				GetComponent<FadeManager>().FadeOut();
				scenechartfaded = true;
			}
			
		}
		
	}
	
	
	void BumpsBPM(){
		if(nextBump < Bumps.Count && Bumps[nextBump] <= timetotalchart){
			matArrowModel.color = white;
			nextBump++;
		}	
		
		for(int i=0; i<4; i++){
			if(arrowTarget[i].transform.localScale.x < scaleBase){
				arrowTarget[i].transform.localScale += new Vector3(Time.deltaTime*speedBumpInput, Time.deltaTime*speedBumpInput, Time.deltaTime*speedBumpInput);
			}
		}
		
	}
	
	
	void FixedUpdate(){
		
		/*if(oneSecond >= 1f && !dead){
			MoveArrows();
		}*/
		
		if(matArrowModel.color.r > 0.5f){
			var m = 0.5f*Time.deltaTime/speedBumps;
			matArrowModel.color -= new Color(m,m,m, 0f);
		}
		
		/*
		if(timetotalchart >= firstArrow && timetotalchart < lastArrow){
			var div = (float)((timetotalchart - firstArrow)/(lastArrow - firstArrow));
			progressBar.transform.localPosition = new Vector3(progressBar.transform.localPosition.x, - (10f - 10f*div), 8f);
			progressBar.transform.localScale = new Vector3(progressBar.transform.localScale.x, 10f*div, 1f);
		}*/
		theTimeBar.updateTimeBar((float)timetotalchart);
		
		
		if(stateSpeed > 0){
			slow.renderer.material.color = new Color(1f, 0f, 0f, 0.5f);
			if(!slowandfast[0].gameObject.active) slowandfast[0].gameObject.active = true;
			slowandfast[0].Play();
			stateSpeed = 0f;
			changeColorSlow = true;
		}else if(stateSpeed < 0){
			fast.renderer.material.color = new Color(1f, 0f, 0f, 0.5f);
			if(!slowandfast[1].gameObject.active) slowandfast[1].gameObject.active = true;
			slowandfast[1].Play();
			stateSpeed = 0f;
			changeColorFast = true;
		}
		
		if(changeColorFast){
			var div = Time.deltaTime/ClignSpeed;
			var col = fast.renderer.material.color.r - div;
			var colp = fast.renderer.material.color.g + div;
			fast.renderer.material.color = new Color(col, colp, colp, 0.5f);
			if(col <= 0.5f) changeColorFast = false;
		}else if(changeColorSlow){
			var div = Time.deltaTime/ClignSpeed;
			var col = slow.renderer.material.color.r - div;
			var colp = slow.renderer.material.color.g + div;
			slow.renderer.material.color = new Color(col, colp, colp, 0.5f);
			if(col <= 0.5f) changeColorSlow = false;
		}
		
		
		if(ct < ComboType.FULLCOMBO && combo > 100){
			switch (ct){
			case ComboType.FULLEXCELLENT:
				
				if((colorCombo <= 0.3f && signCombo == -1) || (colorCombo >= 1f && signCombo == 1) ){ signCombo *= -1; }
				colorCombo += signCombo*Time.deltaTime/speedCombofade;
				theColorCombo = new Color(1f, 1f, colorCombo, 1f);
				alreadytaged = false;
				break;
			case ComboType.FULLFANTASTIC:
				if((colorCombo <= 0.3f && signCombo == -1) || (colorCombo >= 1f && signCombo == 1) ){ signCombo *= -1; }
				colorCombo += signCombo*Time.deltaTime/speedCombofade;
				theColorCombo = new Color(colorCombo, 1f, 1f, 1f);
				alreadytaged = false;
				break;
			}
		}else if(!alreadytaged){
			theColorCombo = white;
			alreadytaged = true;
		}
	}
	
	
	
	//For moving arrows or do some displaying things
	
	#region Defilement chart
	
	void MoveArrows(){
	
		var bps = thesong.getBPS(actualBPM);
		var move = -((float)(bps*timechart))*speedmod +  changeBPM;
		TMainCamera.position = new Vector3(3f, move - 5, -10f);

		foreach(var el in arrowFrozen.Keys){
			var pos = el.goArrow.transform.position;
			el.goArrow.transform.position = new Vector3(pos.x, move, pos.z);
			pos = el.goArrow.transform.position;
			var div = ((el.posEnding.y - pos.y)/2f);
			el.goFreeze.transform.position = new Vector3(el.goFreeze.transform.position.x, (pos.y + div) , el.goFreeze.transform.position.z);
			el.goFreeze.transform.localScale = new Vector3(1f, -div, 0.1f);
			el.goFreeze.transform.GetChild(0).transform.localScale = new Vector3((el.posEnding.y - pos.y)/(el.posEnding.y - el.posBegining.y), 1f, 0.1f);
			el.changeColorFreeze(arrowFrozen[el], unfrozed);
		}
	}
	
	
	void MoveCameraBefore(){
	
		var bps = thesong.getBPS(actualBPM);
		//var move = -((float)(bps*(-(1.5 - oneSecond - startTheSong)))*speedmod);
		var move = ((float)(bps*((oneSecond - 1.5f + (startTheSong < 0f ? startTheSong : 0f))))*speedmod);
		TMainCamera.position = new Vector3(3f, - 5 - move, -10f);
	}
	
	
	void VerifyBPMnSTOP(){
		//BPM change verify
		if(nextSwitchBPM < thesong.bpms.Count){
			keybpms = thesong.bpms.ElementAt(nextSwitchBPM).Key;
			if(keybpms <= timetotalchart){
				valuebpms = thesong.bpms.ElementAt(nextSwitchBPM).Value;
				//iwashere = 1;
				var bps = thesong.getBPS(actualBPM);
				changeBPM += -((float)(bps*(timechart - (float)(timetotalchart - keybpms))))*speedmod;
				timebpm += (double)timechart - (timetotalchart - keybpms);
				timechart = (float)(timetotalchart - keybpms);
				actualBPM = valuebpms;
				nextSwitchBPM++;
			}
		}
		
		//Stop verif
		if(nextSwitchStop < thesong.stops.Count)
		{
			keystops = thesong.stops.ElementAt(nextSwitchStop).Key;
			if(keystops <= timetotalchart){
				//iwashere = 2;
				valuestops = thesong.stops.ElementAt(nextSwitchStop).Value;
				timetotalchart = timebpm + timechart + totaltimestop;
				timechart -= (float)timetotalchart - (float)keystops;
				timestop += (float)timetotalchart - (float)keystops;
				totaltimestop += (float)timetotalchart - (float)keystops;
				timetotalchart = timebpm + timechart + totaltimestop;
				actualstop = valuestops;
				nextSwitchStop++;
			}
		}
	}
	
	#endregion
	
	#region GUI
	void RefreshGUIPart(){
		if(scoreToDisplay == Precision.FANTASTIC){
			if(sensFantastic){
				alpha -= 0.2f*Time.deltaTime/timeClignotementFantastic;
				sensFantastic = alpha > 0.8f;
			}else{
				alpha += 0.2f*Time.deltaTime/timeClignotementFantastic;
				sensFantastic = alpha >= 1f;
			}
		}else{
			alpha = 1f;
		}
		
		if(zoom > 0 && scoreToDisplay != Precision.DECENT && scoreToDisplay != Precision.WAYOFF){
			zoom -= baseZoom*Time.deltaTime/vitesseZoom;
		}else{
			zoom = 0;
		}
		
		if(alphaCombo > 0.5){
			alphaCombo -= Time.deltaTime/speedAlphaCombo;	
		}
		
		timeDisplayScore += Time.deltaTime;
		
	}
	
	void ClignFailed(){
		if(sensFantastic){
			alpha -= 0.1f*Time.deltaTime/timeClignotementFantastic;
			sensFantastic = alpha > 0.9f;
		}else{
			alpha += 0.1f*Time.deltaTime/timeClignotementFantastic;
			sensFantastic = alpha >= 1f;
		}
		
		
	}
	
	void ClignCombo(){
		if(sensFantastic){
			alpha -= 0.2f*Time.deltaTime/timeClignotementFantastic;
			sensFantastic = alpha > 0.8f;
		}else{
			alpha += 0.2f*Time.deltaTime/timeClignotementFantastic;
			sensFantastic = alpha >= 1f;
		}
		
		
	}
	#endregion
	
	#region Inputs verify
	//Valid or deny the frozen arrow
	void VerifyValidFrozenArrow(){
		if(arrowFrozen.Any()){
			if(keyToRemove.Any()) keyToRemove.Clear();
			
			for(int i=0; i<arrowFrozen.Count;i++){
				var el = arrowFrozen.ElementAt(i);
				arrowFrozen[el.Key] += Time.deltaTime;
				
				if(el.Key.timeEndingFreeze <= timetotalchart){
					switch(el.Key.arrowPos){
					case ArrowPosition.LEFT:
						StartParticleFreezeLeft(false);
						break;
					case ArrowPosition.DOWN:
						StartParticleFreezeDown(false);
						break;
					case ArrowPosition.UP:
						StartParticleFreezeUp(false);
						break;
					case ArrowPosition.RIGHT:
						StartParticleFreezeRight(false);
						break;
					}
					GainScoreAndLife("FREEZE");
					scoreCount[el.Key.arrowType == ArrowType.FREEZE ? "FREEZE" : "ROLL"] += 1;
					desactivateGameObject(el.Key.goArrow);
					desactivateGameObject(el.Key.goFreeze);
					
					keyToRemove.Add(el.Key);
				}
				
				
				if(el.Value >= unfrozed && !keyToRemove.Contains(el.Key)){
					el.Key.goArrow.GetComponent<ArrowScript>().missed = true;
					switch(el.Key.arrowPos){
					case ArrowPosition.LEFT:
						StartParticleFreezeLeft(false);
						break;
					case ArrowPosition.DOWN:
						StartParticleFreezeDown(false);
						break;
					case ArrowPosition.UP:
						StartParticleFreezeUp(false);
						break;
					case ArrowPosition.RIGHT:
						StartParticleFreezeRight(false);
						break;
					}
					GainScoreAndLife("UNFREEZE");
					keyToRemove.Add(el.Key);
				}
				
				
			}
			
			
			
			foreach(var k in keyToRemove){
				arrowFrozen.Remove(k);
			}
			
		}
	}
	
	
	
	//Verify keys Input at this frame
	
	void VerifyKeysInput(){
		
		if((Input.GetKeyDown(KeyCodeLeft) || Input.GetKeyDown(SecondaryKeyCodeLeft) ) && (arrowLeftList.Any())){
			arrowTarget[0].localScale = arrowTarget[0].localScale*percentScaleInput;
			var ar = findNextLeftArrow();
			double realprec = ar.time - (timetotalchart); //Retirer ou non le Time.deltaTime ?
			double prec = Mathf.Abs((float)(realprec));
			
			if(prec < precToTime(Precision.GREAT)){ //great
				
				
				
				if(ar.imJump){
					
					freezeValidateUpdate(ar, prec, realprec);
					
				}else{
					if(ar.arrowType == ArrowType.NORMAL || ar.arrowType == ArrowType.MINE){
						desactivateGameObject(ar.goArrow);
						
					}else{
						ar.goArrow.GetComponent<ArrowScript>().valid = true;
						arrowFrozen.Add(ar,0f);
						ar.displayFrozenBar();
						StartParticleFreezeLeft(true);
						
					}
					precAverage.Add(realprec);
					var ttp = timeToPrec(prec);
					scoreCount[ttp.ToString()] += 1 ;
					if(ar.imHand) scoreCount["HANDS"] += 1 ;
					GainCombo(1, ttp);
					arrowLeftList.Remove(ar);
					StartParticleLeft(ttp);
					GainScoreAndLife(ttp.ToString());
					displayPrec(prec);
				}
				
			}else if(prec < precToTime(Precision.WAYOFF)){ //miss
					if(ar.imJump){
					
						freezeMissedUpdate(ar, prec, realprec);
						
					}else{
						precAverage.Add(realprec);
						var ttp = timeToPrec(prec);
						scoreCount[ttp.ToString()] += 1;
						ar.goArrow.GetComponent<ArrowScript>().missed = true;
						arrowLeftList.Remove(ar);
						StartParticleLeft(ttp);
						displayPrec(prec);
						GainScoreAndLife(ttp.ToString());
					}
					ComboStop(false);
			}
			
			if(prec > precToTime(Precision.FANTASTIC) && prec < precToTime(Precision.WAYOFF)){
				stateSpeed = -1f*Mathf.Sign((float)(ar.time - timetotalchart));
			}else{
				stateSpeed = 0f;	
			}
			
		}
		
		
		if((Input.GetKeyDown(KeyCodeDown)  || Input.GetKeyDown(SecondaryKeyCodeDown))  && (arrowDownList.Any())){
			arrowTarget[1].localScale = arrowTarget[1].localScale*percentScaleInput;
			var ar = findNextDownArrow();
			
			double realprec = ar.time - (timetotalchart);
			double prec = Mathf.Abs((float)(realprec));
			
			if(prec < precToTime(Precision.GREAT)){ //great
				
				
				if(ar.imJump){
					freezeValidateUpdate(ar, prec, realprec);
				}else{
					if(ar.arrowType == ArrowType.NORMAL || ar.arrowType == ArrowType.MINE){
						desactivateGameObject(ar.goArrow);
						
					}else{
						ar.goArrow.GetComponent<ArrowScript>().valid = true;
						arrowFrozen.Add(ar,0f);
						ar.displayFrozenBar();
						StartParticleFreezeDown(true);
						
					}
					precAverage.Add(realprec);
					var ttp = timeToPrec(prec);
					scoreCount[ttp.ToString()] += 1;
					if(ar.imHand) scoreCount["HANDS"] += 1 ;
					GainCombo(1, ttp);
					arrowDownList.Remove(ar);
					StartParticleDown(ttp);
					GainScoreAndLife(ttp.ToString());
					displayPrec(prec);
				}
				
			}else if(prec < precToTime(Precision.WAYOFF)){ //miss
					if(ar.imJump){
					
						freezeMissedUpdate(ar, prec, realprec);
					}else{
						precAverage.Add(realprec);
						var ttp = timeToPrec(prec);
						scoreCount[ttp.ToString()] += 1;
						ar.goArrow.GetComponent<ArrowScript>().missed = true;
						arrowDownList.Remove(ar);
						StartParticleDown(ttp);
						displayPrec(prec);
						GainScoreAndLife(ttp.ToString());
					}
					ComboStop(false);
			}
			
			if(prec > precToTime(Precision.FANTASTIC) && prec < precToTime(Precision.WAYOFF)){
				stateSpeed = -1f*Mathf.Sign((float)(ar.time - timetotalchart));
			}else{
				stateSpeed = 0f;	
			}
		}
		
		
		if((Input.GetKeyDown(KeyCodeUp) || Input.GetKeyDown(SecondaryKeyCodeUp)) && (arrowUpList.Any())){
			arrowTarget[2].localScale = arrowTarget[2].localScale*percentScaleInput;
			var ar = findNextUpArrow();
			double realprec = ar.time - (timetotalchart);
			double prec = Mathf.Abs((float)(realprec));
			
			if(prec < precToTime(Precision.GREAT)){ //great
				
				
				if(ar.imJump){
					freezeValidateUpdate(ar, prec, realprec);
				}else{
					if(ar.arrowType == ArrowType.NORMAL || ar.arrowType == ArrowType.MINE){
						desactivateGameObject(ar.goArrow);
						
					}else{
						ar.goArrow.GetComponent<ArrowScript>().valid = true;
						arrowFrozen.Add(ar,0f);
						ar.displayFrozenBar();
						StartParticleFreezeUp(true);
						
					}
					precAverage.Add(realprec);
					var ttp = timeToPrec(prec);
					scoreCount[ttp.ToString()] += 1;
					if(ar.imHand) scoreCount["HANDS"] += 1 ;
					GainCombo(1, ttp);
					arrowUpList.Remove(ar);
					StartParticleUp(ttp);
					GainScoreAndLife(ttp.ToString());
					displayPrec(prec);
				}
				
			}else if(prec < precToTime(Precision.WAYOFF)){ //miss
					if(ar.imJump){
					
						freezeMissedUpdate(ar, prec, realprec);
					}else{
						precAverage.Add(realprec);
						var ttp = timeToPrec(prec);
						scoreCount[ttp.ToString()] += 1;
						ar.goArrow.GetComponent<ArrowScript>().missed = true;
						arrowUpList.Remove(ar);
						StartParticleUp(ttp);
						displayPrec(prec);
						GainScoreAndLife(ttp.ToString());
					}
					ComboStop(false);
			}
			
			if(prec > precToTime(Precision.FANTASTIC) && prec < precToTime(Precision.WAYOFF)){
				stateSpeed = -1f*Mathf.Sign((float)(ar.time - timetotalchart));
			}else{
				stateSpeed = 0f;	
			}
		
		}
		
		
		if((Input.GetKeyDown(KeyCodeRight) || Input.GetKeyDown(SecondaryKeyCodeRight)) && (arrowRightList.Any())){
			arrowTarget[3].localScale = arrowTarget[3].localScale*percentScaleInput;
			var ar = findNextRightArrow();
			double realprec = ar.time - (timetotalchart);
			double prec = Mathf.Abs((float)(realprec));
			
			if(prec < precToTime(Precision.GREAT)){ //great
				
				
				if(ar.imJump){
					freezeValidateUpdate(ar, prec, realprec);
				}else{
					if(ar.arrowType == ArrowType.NORMAL || ar.arrowType == ArrowType.MINE){
						desactivateGameObject(ar.goArrow);
						
					}else{
						ar.goArrow.GetComponent<ArrowScript>().valid = true;
						arrowFrozen.Add(ar,0f);
						ar.displayFrozenBar();
						StartParticleFreezeRight(true);
						
					}
					precAverage.Add(realprec);
					var ttp = timeToPrec(prec);
					scoreCount[ttp.ToString()] += 1;
					if(ar.imHand) scoreCount["HANDS"] += 1 ;
					GainCombo(1, ttp);
					arrowRightList.Remove(ar);
					StartParticleRight(ttp);
					GainScoreAndLife(ttp.ToString());
					displayPrec(prec);
				}
				
			}else if(prec < precToTime(Precision.WAYOFF)){ //miss
					if(ar.imJump){
					
						freezeMissedUpdate(ar, prec, realprec);
					}else{
						precAverage.Add(realprec);
						var ttp = timeToPrec(prec);
						scoreCount[ttp.ToString()] += 1;
						ar.goArrow.GetComponent<ArrowScript>().missed = true;
						arrowRightList.Remove(ar);
						StartParticleRight(ttp);
						displayPrec(prec);
						GainScoreAndLife(ttp.ToString());
					}
					ComboStop(false);
			}
			
			if(prec > precToTime(Precision.FANTASTIC) && prec < precToTime(Precision.WAYOFF)){
				stateSpeed = -1f*Mathf.Sign((float)(ar.time - timetotalchart));
			}else{
				stateSpeed = 0f;	
			}
		}
		
		
		if(Input.GetKeyDown(KeyCode.Escape) && thesong.duration*0.75f > timetotalchart){
			if(!clear && !fail){
				GetComponent<FadeManager>().FadeIn("solo");
			}
		}
	}
	
	
	
	void freezeValidateUpdate(Arrow ar, double prec, double realprec)
	{
		ar.alreadyValid = true;
		ar.goArrow.GetComponent<ArrowScript>().valid = true;
		
		if(!ar.neighboors.Any(c => c.alreadyValid == false)){
			keyToCheck[0] = ar.neighboors.FirstOrDefault(c => c.arrowPos == ArrowPosition.LEFT);
			keyToCheck[1] = ar.neighboors.FirstOrDefault(c => c.arrowPos == ArrowPosition.DOWN);
			keyToCheck[2] = ar.neighboors.FirstOrDefault(c => c.arrowPos == ArrowPosition.UP);
			keyToCheck[3] = ar.neighboors.FirstOrDefault(c => c.arrowPos == ArrowPosition.RIGHT);
			if(keyToCheck[0] != null){
				if(keyToCheck[0].arrowType == ArrowType.NORMAL || keyToCheck[0].arrowType == ArrowType.MINE){
					desactivateGameObject(keyToCheck[0].goArrow);
					
				}else{
					arrowFrozen.Add(keyToCheck[0],0f);
					keyToCheck[0].displayFrozenBar();
					StartParticleFreezeLeft(true);
					
				}
				
				arrowLeftList.Remove(keyToCheck[0]);
				StartParticleLeft(timeToPrec(prec));
			}
			if(keyToCheck[1] != null){
				if(keyToCheck[1].arrowType == ArrowType.NORMAL || keyToCheck[1].arrowType == ArrowType.MINE){
					desactivateGameObject(keyToCheck[1].goArrow);
					
				}else{
					arrowFrozen.Add(keyToCheck[1],0f);
					keyToCheck[1].displayFrozenBar();
					StartParticleFreezeDown(true);
					
				}
				
				arrowDownList.Remove(keyToCheck[1]);
				StartParticleDown(timeToPrec(prec));
			}
			if(keyToCheck[2] != null){
				if(keyToCheck[2].arrowType == ArrowType.NORMAL || keyToCheck[2].arrowType == ArrowType.MINE){
					desactivateGameObject(keyToCheck[2].goArrow);
					
				}else{
					arrowFrozen.Add(keyToCheck[2],0f);
					keyToCheck[2].displayFrozenBar();
					StartParticleFreezeUp(true);
					
				}
				arrowUpList.Remove(keyToCheck[2]);
				StartParticleUp(timeToPrec(prec));
			}
			if(keyToCheck[3] != null){
				if(keyToCheck[3].arrowType == ArrowType.NORMAL || keyToCheck[3].arrowType == ArrowType.MINE){
					desactivateGameObject(keyToCheck[3].goArrow);
					
				}else{
					arrowFrozen.Add(keyToCheck[3],0f);
					keyToCheck[3].displayFrozenBar();
					StartParticleFreezeRight(true);
					
				}
				arrowRightList.Remove(keyToCheck[3]);
				StartParticleRight(timeToPrec(prec));
			}
			precAverage.Add(realprec);
			var ttp = timeToPrec(prec);
			GainScoreAndLife(ttp.ToString());
			scoreCount[ttp.ToString()] += 1;
			scoreCount[ar.imHand ? "HANDS" : "JUMPS"] += 1;
			if(ar.imHand && ar.neighboors.Count == 2) scoreCount["JUMPS"] += 1;
			displayPrec(prec);
			GainCombo(ar.neighboors.Count, ttp);
			
		}
	}
	
	void freezeMissedUpdate(Arrow ar, double prec, double realprec)
	{
		ar.alreadyValid = true;
		ar.goArrow.GetComponent<ArrowScript>().missed = true;
		if(!ar.neighboors.Any(c => c.alreadyValid == false)){
			keyToCheck[0] = ar.neighboors.FirstOrDefault(c => c.arrowPos == ArrowPosition.LEFT);
			keyToCheck[1] = ar.neighboors.FirstOrDefault(c => c.arrowPos == ArrowPosition.DOWN);
			keyToCheck[2] = ar.neighboors.FirstOrDefault(c => c.arrowPos == ArrowPosition.UP);
			keyToCheck[3] = ar.neighboors.FirstOrDefault(c => c.arrowPos == ArrowPosition.RIGHT);
			if(keyToCheck[0] != null){
				
				arrowLeftList.Remove(keyToCheck[0]);
				StartParticleLeft(timeToPrec(prec));
			}
			if(keyToCheck[1] != null){
				
				
				arrowDownList.Remove(keyToCheck[1]);
				StartParticleDown(timeToPrec(prec));
			}
			if(keyToCheck[2] != null){
				
				arrowUpList.Remove(keyToCheck[2]);
				StartParticleUp(timeToPrec(prec));
			}
			if(keyToCheck[3] != null){
				
				arrowRightList.Remove(keyToCheck[3]);
				StartParticleRight(timeToPrec(prec));
			}
			precAverage.Add(realprec);
			var ttp = timeToPrec(prec);
			scoreCount[ttp.ToString()] += 1;
			GainScoreAndLife(ttp.ToString());
			displayPrec(prec);
		}
	
	}
	
	
	//Verify Key for all frames
	void VerifyKeysOutput(){
		if(arrowFrozen.Any()){
			if(Input.GetKey(KeyCodeLeft) || Input.GetKey(SecondaryKeyCodeLeft)){
				var froz = arrowFrozen.FirstOrDefault(c => c.Key.arrowPos == ArrowPosition.LEFT);
				if(!froz.Equals(default(KeyValuePair<Arrow, float>)))
				{
					if(froz.Key.arrowType == ArrowType.FREEZE || (froz.Key.arrowType == ArrowType.ROLL && (Input.GetKeyDown(KeyCodeLeft) || Input.GetKeyDown(SecondaryKeyCodeLeft))))
					{
						arrowFrozen[froz.Key] = 0f;
					}
				}
			}
			
			
			if(Input.GetKey(KeyCodeDown) || Input.GetKey(SecondaryKeyCodeDown)){
				var froz = arrowFrozen.FirstOrDefault(c => c.Key.arrowPos == ArrowPosition.DOWN);
				if(!froz.Equals(default(KeyValuePair<Arrow, float>)))
				{
					if(froz.Key.arrowType == ArrowType.FREEZE || (froz.Key.arrowType == ArrowType.ROLL && (Input.GetKeyDown(KeyCodeDown) || Input.GetKeyDown(SecondaryKeyCodeDown))))
					{
						arrowFrozen[froz.Key] = 0f;
					}
				}
			}
			
			
			if(Input.GetKey(KeyCodeUp) || Input.GetKey(SecondaryKeyCodeUp)){
				var froz = arrowFrozen.FirstOrDefault(c => c.Key.arrowPos == ArrowPosition.UP);
				if(!froz.Equals(default(KeyValuePair<Arrow, float>)))
				{
					if(froz.Key.arrowType == ArrowType.FREEZE || (froz.Key.arrowType == ArrowType.ROLL && (Input.GetKeyDown(KeyCodeUp) || Input.GetKeyDown(SecondaryKeyCodeUp))))
					{
						arrowFrozen[froz.Key] = 0f;
					}
				}
			}
			
			
			if(Input.GetKey(KeyCodeRight) || Input.GetKey(SecondaryKeyCodeRight)){
				var froz = arrowFrozen.FirstOrDefault(c => c.Key.arrowPos == ArrowPosition.RIGHT);
				if(!froz.Equals(default(KeyValuePair<Arrow, float>)))
				{
					if(froz.Key.arrowType == ArrowType.FREEZE || (froz.Key.arrowType == ArrowType.ROLL && (Input.GetKeyDown(KeyCodeRight) || Input.GetKeyDown(SecondaryKeyCodeRight))))
					{
						arrowFrozen[froz.Key] = 0f;
					}
				}
			}
			
		}
		
	}
	
	#endregion
	
	#region Particules	
	void StartParticleLeft(Precision prec){
		var displayPrec = prec.ToString();
		if(prec < Precision.DECENT && combo >= 100) displayPrec += "C";
		var ps = precLeft[displayPrec];
		if(!ps.gameObject.active) ps.gameObject.active = true;
		ps.Play();
	}
	
	void StartParticleDown(Precision prec){
		var displayPrec = prec.ToString();
		if(prec < Precision.DECENT && combo >= 100) displayPrec += "C";
		var ps = precDown[displayPrec];
		if(!ps.gameObject.active) ps.gameObject.active = true;
		ps.Play();
	}
	
	void StartParticleUp(Precision prec){
		var displayPrec = prec.ToString();
		if(prec < Precision.DECENT && combo >= 100) displayPrec += "C";
		var ps = precUp[displayPrec];
		if(!ps.gameObject.active) ps.gameObject.active = true;
		ps.Play();
	}
	
	void StartParticleRight(Precision prec){
		var displayPrec = prec.ToString();
		if(prec < Precision.DECENT && combo >= 100) displayPrec += "C";
		var ps = precRight[displayPrec];
		if(!ps.gameObject.active) ps.gameObject.active = true;
		ps.Play();
	}
	
	void StartParticleFreezeLeft(bool state){
		var ps = precLeft["FREEZE"];
		if(!ps.gameObject.active) ps.gameObject.active = true;
		ps.Play();
		ps.time = 1f;
		ps.loop = state;
	}
	
	void StartParticleFreezeRight(bool state){
		var ps = precRight["FREEZE"];
		if(!ps.gameObject.active) ps.gameObject.active = true;
		ps.Play();
		ps.time = 1f;
		ps.loop = state;
		
	}
	
	void StartParticleFreezeDown(bool state){
		var ps = precDown["FREEZE"];
		if(!ps.gameObject.active) ps.gameObject.active = true;
		ps.Play();
		ps.time = 1f;
		ps.loop = state;
		
	}
	
	void StartParticleFreezeUp(bool state){
		var ps = precUp["FREEZE"];
		if(!ps.gameObject.active) ps.gameObject.active = true;
		ps.Play();
		ps.time = 1f;
		ps.loop = state;
		
	}
	
	public void StartParticleMineLeft(){
		var ps = precLeft["MINE"];
		if(!ps.gameObject.active) ps.gameObject.active = true;
		ps.Play();
		
	}
	
	public void StartParticleMineRight(){
		var ps = precRight["MINE"];
		if(!ps.gameObject.active) ps.gameObject.active = true;
		ps.Play();
		
	}
	
	public void StartParticleMineDown(){
		var ps = precDown["MINE"];
		if(!ps.gameObject.active) ps.gameObject.active = true;
		ps.Play();
		
	}
	
	public void StartParticleMineUp(){
		var ps = precUp["MINE"];
		if(!ps.gameObject.active) ps.gameObject.active = true;
		ps.Play();
		
	}
	
	#endregion
	
	#region util
	public void GainScoreAndLife(string s){
		if(!fail){
			if(lifeBase[s] <= 0 || combo >= DataManager.Instance.regenComboAfterMiss){
				life += lifeBase[s];
				if(life > 100f){
					life = 100f;	
				}else if(life < 0f){
					life = 0f;	
				}
				if(!lifeGraph.ContainsKey(timetotalchart)) lifeGraph.Add(timetotalchart, life);
				theLifeBar.ChangeBar(life);
			}
			score += scoreBase[s];
			scoreInverse -= (fantasticValue-scoreBase[s]);
			//Debug.Log ("si : " + scoreInverse);
			if(score > 100f){
				score = 100f;	
			}else if(score < 0f){
				score = 0f;	
			}
			displaying = scoreDecoupe();
		}
		
		if(life <= 0f || scoreInverse < targetScoreInverse){
			//Debug.Log ("Fail at life");
			fail = true;
		}
	}
	
	public void AddMineToScoreCombo(){
		scoreCount["MINE"] += 1;	
	}
	
	public void AddMissToScoreCombo(){
		scoreCount["MISS"] += 1;	
	}
	
	public void GainCombo(int c, Precision prec){
		combo+= c;
		alphaCombo = 1f;
		if(ct != ComboType.NONE && prec != Precision.FANTASTIC){
			if(ct == ComboType.FULLFANTASTIC){
				switch(prec){
				case Precision.EXCELLENT:
					ct = ComboType.FULLEXCELLENT;
					firstEx = timetotalchart;
					break;
				case Precision.GREAT:
					ct = ComboType.FULLCOMBO; 
					firstEx = timetotalchart;
					firstGreat = timetotalchart;
					break;
				default:
					firstEx = timetotalchart;
					firstGreat = timetotalchart;
					firstMisteak = timetotalchart;
					ct = ComboType.NONE; 
					break;
				}
			}else if(ct == ComboType.FULLEXCELLENT && prec > Precision.EXCELLENT){
				if(	prec == Precision.GREAT){ 
					firstGreat = timetotalchart;
					ct = ComboType.FULLCOMBO; 
				}
				else { 
					firstGreat = timetotalchart;
					firstMisteak = timetotalchart;
					ct = ComboType.NONE; 
				}
			}else if(ct == ComboType.FULLCOMBO && prec > Precision.GREAT){
				firstMisteak = timetotalchart;
				ct = ComboType.NONE;
			}
		}
		thetab = comboDecoupe();
		theTimeBar.updatePS(ct, combo);
		theTimeBar.HitBar(prec);
		if(isFullExComboRace && ct == ComboType.FULLCOMBO){
			//Debug.Log("Fail at FEC race");
			fail = true;
		}
	}
	
	int[] scoreDecoupe(){
		var outtab = new int[5];
		var thescorestring = score.ToString("000.00").Replace(".","");
		for(int i=0;i<5;i++){
			outtab[i] = System.Convert.ToInt32(""+thescorestring[4-i]);
		}
		
		
		return outtab;
		
	}
	
	
	int[] comboDecoupe(){
		var thecombo = combo.ToString();
		var outtab = new int[thecombo.Length];
		for(int i=0;i<thecombo.Length;i++){
			outtab[i] = System.Convert.ToInt32(""+thecombo[thecombo.Length-1-i]);
		}
		return outtab;
	}
	/*
	float roolInt(string i){
		var ic = (float)Convert.ToDouble(i);
		if(ic == 0.9f){
			return 0f;	
		}else{
			return ic+0.1f;		
		}
	}
	*/
	public void ComboStop(bool miss){
		theTimeBar.breakPS();
		if(!timeCombo.ContainsKey(timetotalchart)) timeCombo.Add(timetotalchart, combo);
		combo = 0;	
		if(ct != ComboType.NONE){
			switch(ct){
			case ComboType.FULLFANTASTIC:
				firstEx = timetotalchart;
				firstGreat = timetotalchart;
				firstMisteak = timetotalchart;
				break;
			case ComboType.FULLEXCELLENT:
				firstGreat = timetotalchart;
				firstMisteak = timetotalchart;
				break;
			case ComboType.FULLCOMBO:
				firstMisteak = timetotalchart;
				break;
			}
				
			ct = ComboType.NONE;
		}
		
		thetab = comboDecoupe();
		if(isFullComboRace || isFullExComboRace){
			//Debug.Log ("Fail at combo race");
			fail = true;
		}
		if(miss){
			comboMisses++;
		}else{
			comboMisses = 0;
		}
	}
	
	
	void desactivateGameObject(GameObject go)
	{
		if(go.GetComponent("ArrowScript") != null) go.GetComponent<ArrowScript>().enabled = false;
		if(go.transform.GetChildCount() > 0)
		{
			for(int i=0; i < go.transform.GetChildCount(); i++)
			{
				go.transform.GetChild(i).renderer.enabled = false;	
			}
		}
		go.transform.renderer.enabled = false;
	}
	
	public void desactiveGameObjectMissed(GameObject go, GameObject gofreeze, float distance)
	{
		if((go.transform.position.y - distance) >= arrowTarget[0].position.y)
		{
			desactivateGameObject(go);
			if(gofreeze != null) desactivateGameObject(gofreeze);
		}
	}
	
	
	void SendDataToDatamanager(){
		DataManager.Instance.scoreEarned = score;
		DataManager.Instance.precAverage = precAverage;
		timeCombo.Add(timetotalchart, combo);
		DataManager.Instance.timeCombo = timeCombo;
		DataManager.Instance.lifeGraph = lifeGraph;
		DataManager.Instance.firstEx = firstEx;
		DataManager.Instance.firstGreat = firstGreat;
		DataManager.Instance.firstMisteak = firstMisteak;
		DataManager.Instance.perfect = perfect;
		DataManager.Instance.fullFantCombo = fullFantCombo;
		DataManager.Instance.fullExCombo = fullExCombo;
		DataManager.Instance.fullCombo = fullCombo;
		DataManager.Instance.scoreCount = scoreCount;
		DataManager.Instance.fail = fail;
		DataManager.Instance.firstArrow = firstArrow;
		ProfileManager.Instance.currentProfile.updateGameTime(timetotalchart);
	}
	
	IEnumerator swipTexture(bool reverse, float height, float wait){
		
		if(!reverse){
			zwip = height/2f;
			while(zwip > 0f){
				zwip -= height/2f*Time.deltaTime/speedziwp;
				yield return new WaitForFixedUpdate();
			}
			zwip = 0f;
		}else{
			yield return new WaitForSeconds(wait);
			zwip = 0f;
			while(zwip < height/2f){
				zwip += height/2f*Time.deltaTime/speedziwp;
				yield return new WaitForFixedUpdate();
			}
			zwip = height/2f;
			cacheFailed = true;
			//Lancer la sc�ne de score
		}
	}
	#endregion
	
	#region Precision
	double precToTime(Precision prec){
		if(prec <= Precision.WAYOFF){
			return DataManager.Instance.PrecisionValues[prec];	
		}
		return 0;
	}
	
	
	public void displayPrec(double prec){
		timeDisplayScore = 0f;
		zoom = baseZoom;
		scoreToDisplay = timeToPrec(prec);
	}
	
	Precision timeToPrec(double prec){
	
		var theprec = DataManager.Instance.PrecisionValues.Where(c => (prec <= c.Value));
		if(theprec.Count() > 0) return theprec.First().Key;
		return Precision.MISS;
		
		
	}
	
	
	#endregion
	
	#region Arrow and time
	
	public Arrow findNextUpArrow(){

		return arrowUpList.First();
			
	}
	
	public Arrow findNextDownArrow(){

		return arrowDownList.First();
			
	}
	
	public Arrow findNextLeftArrow(){

		return arrowLeftList.First();
			
	}
	
	public Arrow findNextRightArrow(){

		return arrowRightList.First();
			
	}
	
	public Arrow findNextUpMine(){

		return mineUpList.First();
			
	}
	
	public Arrow findNextDownMine(){

		return mineDownList.First();
			
	}
	
	public Arrow findNextLeftMine(){

		return mineLeftList.First();
			
	}
	
	public Arrow findNextRightMine(){

		return mineRightList.First();
			
	}
	//Remove key from arrow list
	public void removeArrowFromList(Arrow ar, ArrowPosition state){
		
		switch(state){
			case ArrowPosition.LEFT:
				arrowLeftList.Remove(ar);
				break;
			case ArrowPosition.DOWN:
				arrowDownList.Remove(ar);
				break;
			case ArrowPosition.UP:
				arrowUpList.Remove(ar);
				break;
			case ArrowPosition.RIGHT:
				arrowRightList.Remove(ar);
				break;
				
		}
	}
	
	public void removeMineFromList(Arrow ar, ArrowPosition state){
		
		switch(state){
			case ArrowPosition.LEFT:
				mineLeftList.Remove(ar);
				break;
			case ArrowPosition.DOWN:
				mineDownList.Remove(ar);
				break;
			case ArrowPosition.UP:
				mineUpList.Remove(ar);
				break;
			case ArrowPosition.RIGHT:
				mineRightList.Remove(ar);
				break;
				
		}
	}
	
	
	
	
	public double getTotalTimeChart(){
		return timetotalchart;	
	}
	
	#endregion
	
	#region Chart creation
	//Create the chart

	void createTheChart(Song s){
		
		var ypos = 0f;
		arrowLeftList = new List<Arrow>();
		arrowUpList = new List<Arrow>();
		arrowDownList = new List<Arrow>();
		arrowRightList = new List<Arrow>();
		mineLeftList = new List<Arrow>();
		mineUpList = new List<Arrow>();
		mineDownList = new List<Arrow>();
		mineRightList = new List<Arrow>();
		
		var theBPMCounter = 1;
		var theSTOPCounter = 0;
		double mesurecount = 0;
		double prevmesure = 0;
		double timecounter = 0;
		double timeBPM = 0;
		double timestop = 0;
		double timetotal = 0;
		float prec = 0.001f;
		var ArrowFreezed = new Arrow[4];
		Bumps = new List<double>();
		foreach(var mesure in s.stepchart){
			
			for(int beat=0;beat<mesure.Count;beat++){
			
			
				var bps = s.getBPS(s.bpms.ElementAt(theBPMCounter-1).Value);
				if(theBPMCounter < s.bpms.Count && theSTOPCounter < s.stops.Count){
					while((theBPMCounter < s.bpms.Count && theSTOPCounter < s.stops.Count) 
						&& (s.mesureBPMS.ElementAt(theBPMCounter) < mesurecount - prec || s.mesureSTOPS.ElementAt(theSTOPCounter) < mesurecount - prec)){
					
						if(s.mesureBPMS.ElementAt(theBPMCounter) < s.mesureSTOPS.ElementAt(theSTOPCounter)){
							timecounter += (s.mesureBPMS.ElementAt(theBPMCounter) - prevmesure)/bps;
							
							timeBPM += timecounter;
							timecounter = 0;
							prevmesure = s.mesureBPMS.ElementAt(theBPMCounter);
							theBPMCounter++;
							bps = s.getBPS(s.bpms.ElementAt(theBPMCounter-1).Value);
							//Debug.Log("And bpm change before / bpm");
						}else if(s.mesureBPMS.ElementAt(theBPMCounter) > s.mesureSTOPS.ElementAt(theSTOPCounter)){
							timestop += s.stops.ElementAt(theSTOPCounter).Value;
							theSTOPCounter++;
							//Debug.Log("And stop change before");
						}else{
							timecounter += (s.mesureBPMS.ElementAt(theBPMCounter) - prevmesure)/bps;
							timeBPM += timecounter;
							timecounter = 0;
							prevmesure = s.mesureBPMS.ElementAt(theBPMCounter);
							theBPMCounter++;
							bps = s.getBPS(s.bpms.ElementAt(theBPMCounter-1).Value);
							
							timestop += s.stops.ElementAt(theSTOPCounter).Value;
							theSTOPCounter++;
							//Debug.Log("And bpm change before");
							//Debug.Log("And stop change before");
						}
						
					}
				}else if(theBPMCounter < s.bpms.Count){
					while((theBPMCounter < s.bpms.Count) && s.mesureBPMS.ElementAt(theBPMCounter) < mesurecount - prec){
						
						timecounter += (s.mesureBPMS.ElementAt(theBPMCounter) - prevmesure)/bps;
							
						timeBPM += timecounter;
						timecounter = 0;
						prevmesure = s.mesureBPMS.ElementAt(theBPMCounter);
						theBPMCounter++;
						bps = s.getBPS(s.bpms.ElementAt(theBPMCounter-1).Value);
					
					}
				}else if(theSTOPCounter < s.stops.Count){
					while((theSTOPCounter < s.stops.Count) && s.mesureSTOPS.ElementAt(theSTOPCounter) < mesurecount - prec){
						
						timestop += s.stops.ElementAt(theSTOPCounter).Value;
						theSTOPCounter++;
					
					}
				}
				
				
				timecounter += (mesurecount - prevmesure)/bps;
				
				
				timetotal = timecounter + timeBPM + timestop;
				if((beat)%(mesure.Count/4) == 0) Bumps.Add(timetotal);
				
				var previousnote = mesure.ElementAt(beat).Trim();
				
				
				// Traitement
				
				if(displayValue[0]){ //No mine
					previousnote = previousnote.Replace('M', '0');
				}
				if(displayValue[1]){ //No jump
					if(previousnote.Where( c => c == '1' || c == '2' || c == '4').Count() == 2){
						var done = false;
						for(int i=0; i<4; i++){
							if(previousnote.ElementAt(i) == '1' || 	previousnote.ElementAt(i) == '2' || previousnote.ElementAt(i) == '4'){
								if(done){
									previousnote = previousnote.Remove(i, 1);
									previousnote = previousnote.Insert(i,"0");
								}else{
									done = true;	
								}
							}
						}
					}
				}
				if(displayValue[2]){ //No hands
					if(previousnote.Where( c => c == '1' || c == '2' || c == '4').Count() >= 3){
						var done = false;
						for(int i=0; i<4; i++){
							if(previousnote.ElementAt(i) == '1' || 	previousnote.ElementAt(i) == '2' || previousnote.ElementAt(i) == '4'){
								if(done){
									previousnote = previousnote.Remove(i, 1);
									previousnote = previousnote.Insert(i,"0");
								}else{
									done = true;	
								}
							}
						}
					}
				}
				
				if(displayValue[3]){ //No freeze
					previousnote = previousnote.Replace('2', '1');
					previousnote = previousnote.Replace('4', '1');
					previousnote = previousnote.Replace('3', '0');
				}
				
				char[] note = previousnote.ToCharArray();
				
				var listNeighboors = new List<Arrow>();
				var barr = false;
				var tripleselect = 0;
				
				for(int i =0;i<4; i++){
					if(note[i] == '1'){
						
						barr = true;
						tripleselect++;
						var goArrow = (GameObject) Instantiate(arrow, new Vector3(i*2, -ypos, 0f), arrow.transform.rotation);
						if(DataManager.Instance.skinSelected == 1 || DataManager.Instance.skinSelected == 3){
							for(int j=0; j<goArrow.transform.childCount;j++){
								if(goArrow.transform.GetChild(j).name.Contains("Deco")){
									goArrow.transform.GetChild(j).renderer.material.color = chooseColor(beat + 1, mesure.Count);	
								}
							}
						}else{
							goArrow.renderer.material.color = chooseColor(beat + 1, mesure.Count);
						}
						var theArrow = new Arrow(goArrow, ArrowType.NORMAL, timetotal);
						
						switch(i){
						case 0:
							if(DataManager.Instance.skinSelected == 1){
								theArrow.goArrow.transform.Rotate(0f, 0f, 90f);
							}else if(DataManager.Instance.skinSelected == 3){
								theArrow.goArrow.transform.FindChild("RotationCenter").Rotate(0f, 0f, 90f);
							}
							theArrow.arrowPos = ArrowPosition.LEFT;
							arrowLeftList.Add(theArrow);
							break;
						case 1:
							if(DataManager.Instance.skinSelected == 1){
								theArrow.goArrow.transform.Rotate(0f, 0f, 180f);
							}else if(DataManager.Instance.skinSelected == 3){
								theArrow.goArrow.transform.FindChild("RotationCenter").Rotate(0f, 0f, 180f);
							}
							theArrow.arrowPos = ArrowPosition.DOWN;
							arrowDownList.Add(theArrow);
							break;
						case 2:
							theArrow.arrowPos = ArrowPosition.UP;
							arrowUpList.Add(theArrow);
							break;
						case 3:
							if(DataManager.Instance.skinSelected == 1){
								theArrow.goArrow.transform.Rotate(0f, 0f, -90f);
							}else if(DataManager.Instance.skinSelected == 3){
								theArrow.goArrow.transform.FindChild("RotationCenter").Rotate(0f, 0f, -90f);
							}
							theArrow.arrowPos = ArrowPosition.RIGHT;
							arrowRightList.Add(theArrow);
							break;
						}
						listNeighboors.Add(theArrow);
						//goArrow.SetActiveRecursively(false);
						GetComponent<ManageGameObject>().Add(goArrow);
						//barrow = true;
					}else if(note[i] == '2'){
						barr = true;
						var goArrow = (GameObject) Instantiate(arrow, new Vector3(i*2, -ypos, 0f), arrow.transform.rotation);
						if(DataManager.Instance.skinSelected == 1 || DataManager.Instance.skinSelected == 3){
							for(int j=0; j<goArrow.transform.childCount;j++){
								if(goArrow.transform.GetChild(j).name.Contains("Deco")){
									goArrow.transform.GetChild(j).renderer.material.color = chooseColor(beat + 1, mesure.Count);	
								}
							}
						}else{
							goArrow.renderer.material.color = chooseColor(beat + 1, mesure.Count);
						}
						var theArrow = new Arrow(goArrow, ArrowType.FREEZE, timetotal);
						ArrowFreezed[i] = theArrow;
						switch(i){
						case 0:
							if(DataManager.Instance.skinSelected == 1){
								theArrow.goArrow.transform.Rotate(0f, 0f, 90f);
							}else if(DataManager.Instance.skinSelected == 3){
								theArrow.goArrow.transform.FindChild("RotationCenter").Rotate(0f, 0f, 90f);
							}
							theArrow.arrowPos = ArrowPosition.LEFT;
							arrowLeftList.Add(theArrow);
							break;
						case 1:
							if(DataManager.Instance.skinSelected == 1){
								theArrow.goArrow.transform.Rotate(0f, 0f, 180f);
							}else if(DataManager.Instance.skinSelected == 3){
								theArrow.goArrow.transform.FindChild("RotationCenter").Rotate(0f, 0f, 180f);
							}
							theArrow.arrowPos = ArrowPosition.DOWN;
							arrowDownList.Add(theArrow);
							break;
						case 2:
							theArrow.arrowPos = ArrowPosition.UP;
							arrowUpList.Add(theArrow);
							break;
						case 3:
							if(DataManager.Instance.skinSelected == 1){
								theArrow.goArrow.transform.Rotate(0f, 0f, -90f);
							}else if(DataManager.Instance.skinSelected == 3){
								theArrow.goArrow.transform.FindChild("RotationCenter").Rotate(0f, 0f, -90f);
							}
							theArrow.arrowPos = ArrowPosition.RIGHT;
							arrowRightList.Add(theArrow);
							break;
						}
						listNeighboors.Add(theArrow);
						//goArrow.SetActiveRecursively(false);
						GetComponent<ManageGameObject>().Add(goArrow);
					}else if(note[i] == '3'){
						barr = true;
						var theArrow = ArrowFreezed[i];
						if(ArrowFreezed[i] != null){
							var goFreeze = (GameObject) Instantiate(freeze, new Vector3(i*2, (theArrow.goArrow.transform.position.y + ((-ypos - theArrow.goArrow.transform.position.y)/2f)) , 1f), freeze.transform.rotation);
							goFreeze.transform.localScale = new Vector3(1f, -((-ypos - theArrow.goArrow.transform.position.y)/2f), 0.1f);
							if(DataManager.Instance.skinSelected == 1 || DataManager.Instance.skinSelected == 3){
							for(int j=0; j<theArrow.goArrow.transform.childCount;j++){
								if(theArrow.goArrow.transform.GetChild(j).name.Contains("Deco")){
									goFreeze.transform.GetChild(0).renderer.material.color = theArrow.goArrow.transform.GetChild(j).renderer.material.color;
									j = theArrow.goArrow.transform.childCount;
								}
							}
							}else{
								goFreeze.transform.GetChild(0).renderer.material.color = theArrow.goArrow.renderer.material.color;
							}
							theArrow.setArrowFreeze(timetotal, new Vector3(i*2,-ypos, 0f), goFreeze, null);
						}
						
						ArrowFreezed[i] = null;
					
					}else if(note[i] == '4'){
						barr = true;
						var goArrow = (GameObject) Instantiate(arrow, new Vector3(i*2, -ypos, 0f), arrow.transform.rotation);
						if(DataManager.Instance.skinSelected == 1 || DataManager.Instance.skinSelected == 3){
							for(int j=0; j<goArrow.transform.childCount;j++){
								if(goArrow.transform.GetChild(j).name.Contains("Deco")){
									goArrow.transform.GetChild(j).renderer.material.color = chooseColor(beat + 1, mesure.Count);	
								}
							}
						}else{
							goArrow.renderer.material.color = chooseColor(beat + 1, mesure.Count);
						}
						var theArrow = new Arrow(goArrow, ArrowType.ROLL, timetotal);
						ArrowFreezed[i] = theArrow;
						switch(i){
						case 0:
							if(DataManager.Instance.skinSelected == 1){
								theArrow.goArrow.transform.Rotate(0f, 0f, 90f);
							}else if(DataManager.Instance.skinSelected == 3){
								theArrow.goArrow.transform.FindChild("RotationCenter").Rotate(0f, 0f, 90f);
							}
							theArrow.arrowPos = ArrowPosition.LEFT;
							arrowLeftList.Add(theArrow);
							break;
						case 1:
							if(DataManager.Instance.skinSelected == 1){
								theArrow.goArrow.transform.Rotate(0f, 0f, 180f);
							}else if(DataManager.Instance.skinSelected == 3){
								theArrow.goArrow.transform.FindChild("RotationCenter").Rotate(0f, 0f, 180f);
							}
							theArrow.arrowPos = ArrowPosition.DOWN;
							arrowDownList.Add(theArrow);
							break;
						case 2:
							theArrow.arrowPos = ArrowPosition.UP;
							arrowUpList.Add(theArrow);
							break;
						case 3:
							if(DataManager.Instance.skinSelected == 1){
								theArrow.goArrow.transform.Rotate(0f, 0f, -90f);
							}else if(DataManager.Instance.skinSelected == 3){
								theArrow.goArrow.transform.FindChild("RotationCenter").Rotate(0f, 0f, -90f);
							}
							theArrow.arrowPos = ArrowPosition.RIGHT;
							arrowRightList.Add(theArrow);
							break;
						}
						listNeighboors.Add(theArrow);
						//goArrow.SetActiveRecursively(false);
						GetComponent<ManageGameObject>().Add(goArrow);
					}else if(note[i] == 'M'){
						var goArrow = (GameObject) Instantiate(mines, new Vector3(i*2, -ypos, 0f), mines.transform.rotation);
						var theArrow = new Arrow(goArrow, ArrowType.MINE, timetotal);
						switch(i){
						case 0:
							theArrow.arrowPos = ArrowPosition.LEFT;
							mineLeftList.Add(theArrow);
							break;
						case 1:
							theArrow.arrowPos = ArrowPosition.DOWN;
							mineDownList.Add(theArrow);
							break;
						case 2:
							theArrow.arrowPos = ArrowPosition.UP;
							mineUpList.Add(theArrow);
							break;
						case 3:
							theArrow.arrowPos = ArrowPosition.RIGHT;
							mineRightList.Add(theArrow);
							break;
						}
						
					
					}
					
				}
				
				if(barr){
					if(firstArrow <= 0f) firstArrow = timetotal;
					lastArrow = timetotal;	
				}
				
				for(int i=0;i<4;i++){
					if(ArrowFreezed[i] != null && barr) tripleselect++;
				}
				
				if(tripleselect >= 3f){
					foreach(var el in listNeighboors){
						el.imHand = true;
					}
				}
				
				if(listNeighboors.Count > 1){
					foreach(var el in listNeighboors){
						el.neighboors = listNeighboors;
						el.imJump = true;
					}
				}
	
				if(theBPMCounter < s.bpms.Count){
					if(Mathf.Abs((float)(s.mesureBPMS.ElementAt(theBPMCounter) - mesurecount)) < prec){
							timeBPM += timecounter;
							timecounter = 0;
							theBPMCounter++;
							//Debug.Log("And bpm change");
					}
				}
				
				if(theSTOPCounter < s.stops.Count){
					if(Mathf.Abs((float)(s.mesureSTOPS.ElementAt(theSTOPCounter) - mesurecount)) < prec){
							timestop += s.stops.ElementAt(theSTOPCounter).Value;
							theSTOPCounter++;
							//Debug.Log("And stop");
					}
				}
				prevmesure = mesurecount;
				mesurecount += (4f/(float)mesure.Count);
				
				
				ypos += (4f/(float)mesure.Count)*speedmod;
				
			}
		}
		
		//Sort
		arrowUpList = arrowUpList.OrderBy(c => c.time).ToList();
		arrowDownList = arrowDownList.OrderBy(c => c.time).ToList();
		arrowRightList = arrowRightList.OrderBy(c => c.time).ToList();
		arrowLeftList = arrowLeftList.OrderBy(c => c.time).ToList();
		
		mineUpList = mineUpList.OrderBy(c => c.time).ToList();
		mineDownList = mineDownList.OrderBy(c => c.time).ToList();
		mineRightList = mineRightList.OrderBy(c => c.time).ToList();
		mineLeftList = mineLeftList.OrderBy(c => c.time).ToList();
		
		GetComponent<ManageGameObject>().DoTheStartSort();
	}
	
	
	
	Color chooseColor(int posmesure, int mesuretot){
		
		
		switch(mesuretot){
		case 4:	
			return new Color(1f, 0f, 0f, 1f); //rouge
		
			
		case 8:
			if(posmesure%2 == 0){
				return new Color(0f, 0f, 1f, 1f); // bleu
			}
			return new Color(1f, 0f, 0f, 1f); //rouge
			
			
		case 12:
			if((posmesure-1)%3 == 0){
				return new Color(1f, 0f, 0f, 1f); //rouge
			}
			return new Color(1f, 0f, 1f, 1f); //violet
			
		case 16:
			if((posmesure-1)%4 == 0){
				return new Color(1f, 0f, 0f, 1f); //rouge
			}else if((posmesure+1)%4 == 0){
				return new Color(0f, 0f, 1f, 1f); //bleu
			}
			return new Color(0f, 1f, 0f, 1f); //vert
		
			
		case 24:
			if((posmesure+2)%6 == 0){
				return new Color(0f, 0f, 1f, 1f); //bleu
			}else if((posmesure-1)%6 == 0){
				return new Color(1f, 0f, 0f, 1f); //rouge
			}
			return new Color(1f, 0f, 1f, 1f); //violet
			
			
		case 32:
			if((posmesure-1)%8 == 0){
				return new Color(1f, 0f, 0f, 1f); //rouge
			}else if((posmesure+3)%8 == 0){
				return new Color(0f, 0f, 1f, 1f); //bleu
			}else if((posmesure+1)%4 == 0){
				return new Color(0f, 1f, 0f, 1f); //vert
			}
			return new Color(1f, 0.8f, 0f, 1f); //jaune
			
		case 48:
			if((posmesure+2)%6 == 0){
				return new Color(0f, 1f, 0f, 1f); //vert
			}else if((posmesure-1)%12 == 0){
				return new Color(1f, 0f, 0f, 1f); //rouge
			}else if((posmesure+5)%12 == 0){
				return new Color(0f, 0f, 1f, 1f); //bleu
			}
			return new Color(1f, 0f, 1f, 1f); // violet
		
			
		case 64:
			if((posmesure-1)%16 == 0){
				return new Color(1f, 0f, 0f, 1f); //rouge
			}else if((posmesure+7)%16 == 0){
				return new Color(0f, 0f, 1f, 1f); //bleu
			}else if((posmesure+3)%8 == 0){
				return new Color(0f, 1f, 0f, 1f); //vert
			}else if((posmesure+1)%4 == 0){
				return new Color(1f, 0.8f, 0f, 1f); //jaune
			}
			return new Color(0f, 1f, 0.8f, 1f); //turquoise
			
		case 192:
			if((posmesure-1)%48 == 0){
				return new Color(1f, 0f, 0f, 1f); //rouge
			}else if((posmesure+23)%48 == 0){
				return new Color(0f, 0f, 1f, 1f); //bleu
			}else if((posmesure+11)%24 == 0){
				return new Color(0f, 1f, 0f, 1f); //vert
			}else if((posmesure-1)%4 == 0){
				return new Color(1f, 0f, 1f, 1f); //violet
			}else if((posmesure+5)%12 == 0){
				return new Color(1f, 0.8f, 0f, 1f); //jaune
			}
			return new Color(0f, 1f, 0.8f, 1f); //turquoise
			
		default:
			return new Color(1f, 1f, 1f, 1f);
			
		}
	
		
	}
	
	
	
	#endregion
}
