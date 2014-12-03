using UnityEngine;
using System;
using System.Collections;
using System.Text;

[System.Serializable]		
public class DiaryDevOptions		
{		
	public bool allowUnlimitedDiaries;		
}

public class QuestionEntry {
	public string questionText;
	public string[] answerText;
	public int answerOffset;
	public string alertSymptom; 
	public int alertSymptomTimes;
	
	public QuestionEntry(string q, string[] a, int x) {
		questionText = q;
		answerText = a;
		answerOffset = x;
		alertSymptom = null; 
	}
	public QuestionEntry(string q, string[] a, int x, string sym, int n) {
		questionText = q;
		answerText = a;
		answerOffset = x;
		alertSymptom = sym; 
		alertSymptomTimes = n; 
	}
} 

public class DiaryScript2: MonoBehaviour {
	
	//public objects and variables to be assinged value in Inspector
	public GUISkin mySkin, apptSkin; //for diary window style 
	public bool surveyWindow ;
	public GameObject ApplicationController; //This is for Tyler to use this script outside it
	public DiaryDevOptions devOptions;
	
	//Private Objects and Variables
	//GUI 
	Rect surveyGUI ;
	Rect apptGUI;
	int diaryWinLeft; 
	int diaryWinTop; 
	//Diary process    
	int count = 0;
	int maxPerDay;
	int age=9;
	int[] answerArr;
	int partASec1Num;
	int partASec2Num ;
	int partALen;
	int partA0809Len;
	string answers;
	string symptoms = "";	
	String alert;
	string input ="";
	
	string[] answerYN = {"Yes", "No"} ;
	string[] clickToBegin = {"Click to begin"} ;
	string[] cont = {"continue"} ;
	
	QuestionEntry[] apptsec3;
	QuestionEntry[] partAQuestionArr; 
	QuestionEntry[] questionArr;
	
	bool apptWindow;
	Texture apptImage;
	int winID;
	
	//Audio and Animation
	AudioClip[] questionAudio; 
	bool audioSwitch;
	int audioNum;
	//for partB
	bool showSlider = false;
	int painlocation = 0;
	float painLv;
	float[] bod = new float[43];
	GUIStyle apptStyle;


	
	
	// Initialization
	void Start () {
		//QuestionEnties
		
		//		clearKeys (); 
		partASec1Num = 1;
		partASec2Num = 1;
		partA0809Len = 6;
		partAQuestionArr = (age>=10)?populateMSAS1018(partASec1Num, partASec2Num):populateMSAS0809(partA0809Len);
		partALen = partAQuestionArr.Length;
		
		maxPerDay = 20;
		ApplicationController = GameObject.Find ("ApplicationController"); 
		diaryWinLeft = 30; 
		diaryWinTop = 60; 
		age = 10;
		winID = -1;
		
		surveyGUI = new Rect(diaryWinLeft, diaryWinTop, 
		                     Screen.width-diaryWinLeft*2, Screen.height-diaryWinTop*2);
		apptImage = Resources.Load("appt") as Texture2D;
		apptGUI = new Rect(0, 0, 
		                   Screen.width-diaryWinLeft*2, Screen.height-diaryWinTop*2);


		for(int i=0;i<partALen;i++){
			Debug.Log(partAQuestionArr[i].questionText);
		}
		
		//Audio and Animation
		//		audioNum = questionArr.Length-1;
		//		audioSwitch = false; 
		//		questionAudio = loadAudio (audioNum, "female");
		//		apptsec3 = new QuestionEntry[4]; 
		//		   
	}
	void Update(){
	}
	
	public void OnGUI () {
		//ArgumentException, don't know solution
		switch(winID){//if(surveyWindow)
		case 0: GUI.skin = mySkin;
			surveyGUI = GUI.Window(0, surveyGUI, beginSurvey, "");
			break;
		case 1: surveyGUI = GUI.Window(1,surveyGUI, partB,"");
			break;
		default: break;
		}
		
	}
	//Part B process
	void partB(int windowID){
		
		float w0 = 1306;
		float h0 = 1585;
		float w = apptGUI.width;
		float h = apptGUI.height;
		float fm = w*(float)(385/w0);
		float bm = w*(float)(910/w0);
		float f1 = w*(float)(299/w0);
		float f2 = w*(float)(343/w0);
		float f3 = w*(float)(464/w0);
		float f4 = w*(float)(546/w0);
		float b1 = w*(float)(746/w0);
		float b2 = w*(float)(865/w0);
		float b3 = w*(float)(984/w0);
		float b4 = w*(float)(1063/w0);
		float hh = h*(float)(96/h0);
		float hf = h*(float)(176/h0);
		float hn = h*(float)(301/h0);
		float hc = h*(float)(469/h0);
		float ha = h*(float)(709/h0);
		float hha = h*(float)(829/h0);
		float ht = h*(float)(986/h0);
		float hl = h*(float)(1290/h0);
		float hfo = h*(float)(1483/h0);

		float[] sldrValues = new float[43];

		Rect sldrPos = new Rect(w*(float)0.02, h*(float)0.9, 80, 20);
		Rect[] painPos = new Rect[43];
		string painLocation = "";

		GUI.DrawTexture(apptGUI,apptImage, ScaleMode.StretchToFill, true, 10);
//		if(GUI.Button(new Rect(fm, hh, 10, 10), ""))
//			bod[0]=1-bod[0];//front head
	
//		if(GUI.Button(new Rect(bm, hh, 10, 10), "")) bod[1]=1-bod[1]s;
//		if(GUI.Button(new Rect(f2, hf, 10, 10), "")) bod[2]=1-bod[2]; 
//		if(GUI.Button(new Rect(f3, hf, 10, 10), "")) bod[3]=1-bod[3];
//		if(GUI.Button(new Rect(b2, hf, 10, 10), "")) bod[4]=1-bod[4];
//		if(GUI.Button(new Rect(b3, hf, 10, 10), "")) bod[5]=1-bod[5];
//		if(GUI.Button(new Rect(fm, hn, 10, 10), "")) bod[6]=1-bod[6];
//		if(GUI.Button(new Rect(bm, hn, 10, 10), "")) bod[7]=1-bod[7];
//		
//		if(GUI.Button(new Rect(f1, hc, 10, 10), "")) bod[8]=1-bod[8];
//		if(GUI.Button(new Rect(f2, hc, 10, 10), "")) bod[9]=1-bod[9];
//		if(GUI.Button(new Rect(f3, hc, 10, 10), "")) bod[10]=1-bod[10];
//		if(GUI.Button(new Rect(f4, hc, 10, 10), "")) bod[11]=1-bod[11];
//		if(GUI.Button(new Rect(b1, hc, 10, 10), "")) bod[12]=1-bod[12];
//		if(GUI.Button(new Rect(b2, hc, 10, 10), "")) bod[13]=1-bod[13];
//		if(GUI.Button(new Rect(b3, hc, 10, 10), "")) bod[14]=1-bod[14];
//		if(GUI.Button(new Rect(b4, hc, 10, 10), "")) bod[15]=1-bod[15];
//		
//		if(GUI.Button(new Rect(f1, ha, 10, 10), "")) bod[16]=1-bod[16];//front up arm
//		if(GUI.Button(new Rect(f2, ha, 10, 10), "")) bod[17]=1-bod[17];//front left chest
//		if(GUI.Button(new Rect(f3, ha, 10, 10), "")) bod[18]=1-bod[18];//front right chest 
//		if(GUI.Button(new Rect(f4, ha, 10, 10), "")) bod[19]=1-bod[19];//front up arm
//		if(GUI.Button(new Rect(b1, ha, 10, 10), "")) bod[20]=1-bod[20];
//		if(GUI.Button(new Rect(b2, ha, 10, 10), "")) bod[21]=1-bod[21];
//		if(GUI.Button(new Rect(b3, ha, 10, 10), "")) bod[22]=1-bod[22];
//		if(GUI.Button(new Rect(b4, ha, 10, 10), "")) bod[23]=1-bod[23];
//		
//		if(GUI.Button(new Rect(f1, hha, 10, 10), "")) bod[24]=1-bod[24];
//		if(GUI.Button(new Rect(f4, hha, 10, 10), "")) bod[25]=1-bod[25];
//		if(GUI.Button(new Rect(fm, hha, 10, 10), "")) bod[26]=1-bod[26];
//		if(GUI.Button(new Rect(b1, hha, 10, 10), "")) bod[27]=1-bod[27];
//		if(GUI.Button(new Rect(b4, hha, 10, 10), "")) bod[28]=1-bod[28];
//		if(GUI.Button(new Rect(b2, hha, 10, 10), "")) bod[29]=1-bod[29];
//		if(GUI.Button(new Rect(b3, hha, 10, 10), "")) bod[30]=1-bod[30];
//		
//		if(GUI.Button(new Rect(f2, ht, 10, 10), "")) bod[31]=1-bod[31];
//		if(GUI.Button(new Rect(f3, ht, 10, 10), "")) bod[32]=1-bod[32];
//		if(GUI.Button(new Rect(b2, ht, 10, 10), "")) bod[33]=1-bod[33];
//		if(GUI.Button(new Rect(b3, ht, 10, 10), "")) bod[34]=1-bod[34];
//		if(GUI.Button(new Rect(f2, hl, 10, 10), "")) bod[35]=1-bod[35];//front lower leg
//		if(GUI.Button(new Rect(f3, hl, 10, 10), "")) bod[36]=1-bod[36];
//		if(GUI.Button(new Rect(b2, hl, 10, 10), "")) bod[37]=1-bod[37];
//		if(GUI.Button(new Rect(b3, hl, 10, 10), "")) bod[38]=1-bod[38];
//		if(GUI.Button(new Rect(f2, hfo, 10, 10), "")) bod[39]=1-bod[39];
//		if(GUI.Button(new Rect(f3, hfo, 10, 10), "")) bod[40]=1-bod[40];
//		if(GUI.Button(new Rect(b2, hfo, 10, 10), "")) bod[41]=1-bod[41];
//		if(GUI.Button(new Rect(b3, hfo, 10, 10), "")) bod[42]=1-bod[42];

		painPos[0] =  new Rect(fm, hh, 10, 10);//back head
		painPos[1] =  new Rect(bm, hh, 10, 10);//back head
		painPos[2] =  new Rect(f2, hf, 10, 10); 
		painPos[3] =  new Rect(f3, hf, 10, 10);
		painPos[4] =  new Rect(b2, hf, 10, 10);
		painPos[5] =  new Rect(b3, hf, 10, 10);
		painPos[6] =  new Rect(fm, hn, 10, 10);
		painPos[7] =  new Rect(bm, hn, 10, 10);
		
		painPos[8] =  new Rect(f1, hc, 10, 10) ;
		painPos[9] =  new Rect(f2, hc, 10, 10) ;
		painPos[10] =  new Rect(f3, hc, 10, 10) ;
		painPos[11] =  new Rect(f4, hc, 10, 10) ;
		painPos[12] =  new Rect(b1, hc, 10, 10) ;
		painPos[13] =  new Rect(b2, hc, 10, 10) ;
		painPos[14] =  new Rect(b3, hc, 10, 10) ;
		painPos[15] =  new Rect(b4, hc, 10, 10) ;
		
		painPos[16] =  new Rect(f1, ha, 10, 10) ;//front up arm
		painPos[17] =  new Rect(f2, ha, 10, 10) ;//front left chest
		painPos[18] =  new Rect(f3, ha, 10, 10) ;//front right chest 
		painPos[19] =  new Rect(f4, ha, 10, 10) ;//front up arm
		painPos[20] =  new Rect(b1, ha, 10, 10) ;
		painPos[21] =  new Rect(b2, ha, 10, 10) ;
		painPos[22] =  new Rect(b3, ha, 10, 10) ;
		painPos[23] =  new Rect(b4, ha, 10, 10) ;
		
		painPos[24] =  new Rect(f1, hha, 10, 10) ;
		painPos[25] =  new Rect(f4, hha, 10, 10) ;
		painPos[26] =  new Rect(fm, hha, 10, 10) ;
		painPos[27] =  new Rect(b1, hha, 10, 10) ;
		painPos[28] =  new Rect(b4, hha, 10, 10) ;
		painPos[29] =  new Rect(b2, hha, 10, 10) ;
		painPos[30] =  new Rect(b3, hha, 10, 10) ;
		
		painPos[31] =  new Rect(f2, ht, 10, 10) ;
		painPos[32] =  new Rect(f3, ht, 10, 10) ;
		painPos[33] =  new Rect(b2, ht, 10, 10) ;
		painPos[34] =  new Rect(b3, ht, 10, 10) ;
		painPos[35] =  new Rect(f2, hl, 10, 10) ;//front lower leg
		painPos[36] =  new Rect(f3, hl, 10, 10) ;
		painPos[37] =  new Rect(b2, hl, 10, 10) ;
		painPos[38] =  new Rect(b3, hl, 10, 10) ;
		painPos[39] =  new Rect(f2, hfo, 10, 10) ;
		painPos[40] =  new Rect(f3, hfo, 10, 10) ;
		painPos[41] =  new Rect(b2, hfo, 10, 10) ;
		painPos[42] =  new Rect(b3, hfo, 10, 10) ;
	

		for(int i=0;i<painPos.Length;i++){
			if(GUI.Button(painPos[i],"")){
				painlocation = i;
				showSlider = true;

			}
		}

		if(showSlider){
			bod[painlocation] = GUI.HorizontalSlider (sldrPos, bod[painlocation], 0f, 10f);
			GUI.Label(new Rect(w*(float)0.05, h*(float)0.85, 50,20), Convert.ToInt32(bod[painlocation]).ToString());
		}
	
		if(GUI.Button(new Rect(w*(float)0.05,h*(float)0.9,50,20),"continue")) {
			count=0;
			apptWindow = false;
			surveyWindow = true;
			winID =0;
		}   

	}
	//Part A process
	void beginSurvey(int windowID) {
		if(count<partALen){
			partA(partAQuestionArr);
		}
		else if(count==partALen){
			surveyWindow = false;
			apptWindow = true;
			winID=1;
		} 
		else { 
			////		answers = encodeToString(answerArr); 
			GUILayout.Label("You have already taken the survey today!");
			//			Debug.Log("count has exceeded question number" ); 
			ApplicationController.SendMessage("endDiary");
			audio.Stop();
			return;
		}
	}
	
	
	void partA(QuestionEntry[] questionArr){
		bool textOn = false;
		GUILayout.Label(questionArr[count].questionText);
		for (int j = 0; j < questionArr[count].answerText.Length; j++) {
			if (GUILayout.Button (questionArr [count].answerText [j])) {
				// save user's answer to answerArr
				if (questionArr [count].answerText == answerYN) {
					//							answerArr [count] = 1 - j;
					//							answers += (1 - j).ToString ();
				} else if (questionArr[count].answerText!=clickToBegin) {
					//							answerArr [count] = j + questionArr [count].answerOffset;
					//							answers += (j + questionArr [count].answerOffset).ToString ();
				}
				//					PlayerPrefs.SetString ("responses", answers);
				//					//alert = getSymptom(count, answers); 
				//					//symptoms += encodeSymptom(alert);
				//					alert = getSymptomCode (count, answers);
				//					symptoms += alert;
				//					PlayerPrefs.SetString ("symptoms", symptoms);
				
				// if user answers "no" to any symptom, skip the follow up questions
				if (questionArr [count].answerText == answerYN && j == 1) {
					count++;  
					while (questionArr[count].answerText != answerYN&&questionArr[count].answerText.Length!=1) {
						//							answerArr [count] = -1;
						//							answers += "*";
						//							PlayerPrefs.SetString ("responses", answers);
						count++;
						if (count >= questionArr.Length) {
							//								endDiary ();
							return;
						}
					}
					
				} else if(questionArr[count].answerText==cont&&input==""){
					count+=2;
				} else {
					count++;
				}
				if (count >= questionArr.Length) {
					//						Debug.Log("End of survey questions - 1 ");
					//							endDiary ();
					return;
				}
				
			}
			
		} // presented answers
		if(questionArr[count].answerText==cont){
			textOn = true;
		}
		if(textOn){
			input=GUILayout.TextField(input, 20);
		}
		
	} 
	
	
	
	//*** functional methods ***//
	public void startDiary(){
		//check last diary 
		//		int times2day = PlayerPrefs.GetInt("diary"+today+"times");//if date changes clean this key
		//		String preResponse = PlayerPrefs.GetString("responses");
		//		String preDate = PlayerPrefs.GetString("date");
		//		String preTime = PlayerPrefs.GetString("time");
		//		String preState = PlayerPrefs.GetString("diaryLastState");
		//		
		//if last unfishished, upload last time with "#incomplete"
		//		if(preState.Equals("unfinished"))
		//			queueResponsesForUploading(preDate+" "+preTime+":"+preResponse+"#incomplete");
		
		//		Debug.Log("Have done " +times2day+" time(s) today");
		//		Debug.Log("Last time response: " + PlayerPrefs.GetString("responses")); 
		//		if(times2day>=maxPerDay&&!devOptions.allowUnlimitedDiaries){
		//			toast("You've taken diary"+ times2day+ " times today");
		//			Debug.Log("exceeded the daily maximum" );
		//			ApplicationController.SendMessage("endDiary");
		//		}
		//		else{
		//			Debug.Log("last time:"+ preState);
		//			if(preState.Equals("unfishied")){//default/"finished"
		
		//				}
		//				Debug.Log("Let's resume diary from beginning"); 
		//				toast ("Let's resume diary");
		//			}
		//			else{ //"finished"
		//				Debug.Log("Let's start last diary");
		//				PlayerPrefs.SetString("diaryLastState", "unfishied");
		//				toast ("Let's start diary");
		//			}
		//			PlayerPrefs.SetString("date", today); 
		//			PlayerPrefs.SetString("time",System.DateTime.Now.ToShortTimeString()); 
		count = 0; 
		answers = ""; 
		surveyWindow = true;
		winID = 1;
		//			Debug.Log ("this time: "+ PlayerPrefs.GetString("diary"+today));
		//		}
	}
	
	public void endDiary( ){
		//		String time = System.DateTime.Now.ToShortTimeString();
		//		int times = PlayerPrefs.GetInt ("diary" + today + "times");
		//		surveyWindow = false; 
		//		
		//		if(audio.isPlaying){
		//			audio.Stop();
		//		}
		//		PlayerPrefs.SetString("diaryLastState", "finished");
		//		PlayerPrefs.SetInt("diary"+today+"times", ++times);
		//		PlayerPrefs.SetString("diary"+today+time+"responses", 
		//		                      PlayerPrefs.GetString("responses"));
		//		queueResponsesForUploading (answers); 
		//		Debug.Log ("Answers stored (via PlayerPref): " + 
		//		           PlayerPrefs.GetString("responses"));
		//		Debug.Log ("Responses stored : " + answers);
		//		Debug.Log("Symptom alerts: " + symptoms);//or call player
		//testing 
		//			string[] arr2 = symptoms.Split(new char[]{','});
		//			for(int i=0;i<arr2.Length;i++){
		//				decodeSymptomCode(arr2[i]);
		//			}
		//		quit diary
		ApplicationController.SendMessage ("endDiary");
	}
	
	void clearKeys(){
		//		PlayerPrefs.DeleteKey ("diary" + today);//"finished", "unfinishied"
		//		PlayerPrefs.DeleteKey ("diary" + today+ "times");//1,2,..,maxPerDay
		//		PlayerPrefs.DeleteKey ("responses");// the string: "0***0***..."
		//		PlayerPrefs.DeleteAll (); 
	}
	
	void decodeSymptomCode(string str){
		//		int len = str.Length;
		//		Debug.Log ("str  :" + str);
		//		int n = Convert.ToInt32(str);
		//		Debug.Log("Seems you have symptom" + questionArr[n].alertSymptom+" "+questionArr[n].alertSymptomTimes+"consecutive times");
	}
	//not-in-use
	String encodeResponses(int[] input){
		//		if(input==null||input.Length==0) return "no input"; 
		string s = ""; 
		//		for (int i=1; i<answerArr.Length; i++) {//ignore i=0; corresponding button is "click to begin"
		//			int a = new int();
		//			a = input[i];
		//			s += (a==-1)? "*": a.ToString(); 
		//		}
		return s; 
	}
	//encode symptom string to concise string, "lack of energy" to "loe"
	String encodeSymptom(string sym){
		//		if(sym==null||sym.Length==0){
		//			return null;}
		//		string[] arr=sym.ToLower().Split(new char[]{' '});
		//		int len = arr.Length;
		StringBuilder sb = new StringBuilder();
		//		for(int i=0;i<len;i++){
		//			sb.Append(getFirstLetter(arr[i]));
		//		}
		return sb.Append(",").ToString();
	}
	
	string getFirstLetter(string str){
		//		if(str==null||str.Length==0){
		//			Debug.Log("no first letter");
		//			return null;}
		//		char[] arr = str.ToCharArray();
		//		return arr[0].ToString();
		return null;
	}
	//current count and responses, return symptom string triggered
	//	String getSymptom(int idx, string str){
	//		if(idx<3||idx>116) return null;
	//		else {
	//			int sec1length = msas1018sec1.Length*4; 
	//			int n = (idx<=sec1length)?idx-3:idx-2;
	//			//Debug.Log ("n: " + n); 
	//			if(questionArr[n].alertSymptom!=null){
	//				//				if(triggerAlert(str)){
	//				//				 return questionArr[n].alertSymptom;
	//				//				}
	//				double[] ratings = getRatings(n, str);
	//				if(symptomAlgCriteria(questionArr[n].alertSymptom, ratings))
	//					Debug.Log("Seems that you have: " + questionArr[n].alertSymptom); 
	//				return questionArr[n].alertSymptom;
	//			}
	//			return null;
	//		}
	//	}
	//current count and responses string, return symptom code string. "21" for "Pain"
	//	string getSymptomCode(int idx, string str){
	//		if(idx<3||idx>116) return null;
	//		else {
	//			int sec1length = msas1018sec1.Length*4; 
	//			int n = (idx<=sec1length)?idx-3:idx-2;
	//			//Debug.Log ("n: " + n); 
	//			if(questionArr[n].alertSymptom!=null){
	//				double[] ratings = getRatings(n, str);
	//				if(symptomAlgCriteria(questionArr[n].alertSymptom, ratings))
	//					return n.ToString();
	//			}
	//			return null;
	//		}
	//	}
	
	double[] getRatings(int n, string str){//n symptom idx, str current responses
		//		int len = str.Length;
		//		String curQuestionResponse = null; 
		//		double[] ratings = new double[0]; 
		//		if (len <= 93) {//section 1: 3 followUp questions
		//			if (len % 4 != 0) {
		//				Debug.Log ("something wrong w/ response (93)");
		//				return null; 
		//			}
		//			curQuestionResponse = str.Substring (len - 3); // "345", "***"
		//			char[] curQR = curQuestionResponse.ToCharArray(); 
		//			ratings = new double[3];
		//			for (int i=0; i<curQuestionResponse.Length; i++) {
		//				//				Debug.Log ("current resonse: " + curQR[i]); 
		//				ratings[i] = rate(curQR[i]);
		//			}
		//		}  else if (len <= 117) {//section 2: 2 followUp questions
		//			if (len % 3 != 2) {
		//				Debug.Log ("something wrong w/ response (117)");
		//				return null; 
		//			}
		//			curQuestionResponse = str.Substring (len - 2); // "35", "**"
		//			char[] curQR = curQuestionResponse.ToCharArray(); 
		//			ratings= new double[2];
		//			for (int i=0; i<curQuestionResponse.Length; i++) {
		//				//				Debug.Log ("current resonse: " + curQR[i]);
		//				ratings[i] = rate(curQR[i]);
		//			}
		//		}  else {
		//			Debug.Log ("length exceeds 117");
		//		}
		//		return ratings;
		return  null;
	}
	
	bool nthAlertForSec1(double[] arr, double sum, double a, double b, double c, int limit, String sym){
		int n = PlayerPrefs.GetInt(sym);
		if(sum>=7.5||(arr[0]>=a
		              &&arr[1]>=b
		              &&arr[2]>=c)){
			n++;
			if(n==limit) {
				PlayerPrefs.SetInt(sym, 0); 
				return true;}
			PlayerPrefs.SetInt(sym, n);
		}
		else 
			PlayerPrefs.SetInt(sym, 0); 
		return false;
	}
	
	bool nthAlertForSec2(double[] arr, double sum, double a, double b, int limit, String sym){
		int n = PlayerPrefs.GetInt(sym);
		if(sum>=7.5||(arr[0]>=a&&arr[1]>=b)){
			n++;
			if(n==limit){
				PlayerPrefs.SetInt(sym, 0); 
				return true;}
			PlayerPrefs.SetInt(sym, n);
		}
		else {
			PlayerPrefs.SetInt(sym, 0); 
		}
		return false;
	}
	bool onceAlert(double[] arr, double sum, double oft, double sev, double bot){
		return sum>=7.5||(arr[0]>=oft 
		                  && arr[1]>=sev 
		                  && arr[2]>=bot);
	}
	
	void queueResponsesForUploading(string responses)
	{
		string date;//not being used
		string submission;
		
		date = System.DateTime.Now.ToLongDateString();
		Debug.Log ("queueUpdate: "+ date);
		//put all of your responses into a string, values separated by underscores
		//		submission = date + ":" + responses;
		submission = responses+"#"+symptoms; 
		ApplicationController.SendMessage("queueDiarySubmission", submission);
		Debug.Log ("final submission:" +submission);
	}
	
	
	void saveResponse(){
	}
	//	for debugging
	void toast(string message)
	{
		//only include in code for tests on Android Device
		Toast.Instance ().ToastshowMessage (message, ToastLenth.LENGTH_SHORT);
	}
	
	
	//********//Populate and Hard Coding Methods Start Here //********//
	//Merged this method with drawSurvey()
	QuestionEntry[] populateMSAS1018(int num1, int num2) {        
		QuestionEntry[] msas1018sec1 = new QuestionEntry[num1]; //Section1: 23 Questions MSAS 10-18
		QuestionEntry[] msas1018sec2 = new QuestionEntry[num2]; // Section2: 8 Questions MSAS 10-18
		QuestionEntry[] followUp = new QuestionEntry[3]; // 3 follow-up questions
		
		int len1 = num1*4;
		int len2 = num2*3;
		int lenPartA = len1+len2+2;//"2" for introductions
		QuestionEntry[] questionArr = new QuestionEntry[lenPartA];//
		
		string msas1018sec1intro = 
			"We have 23 symptoms to ask you about in this section. Pay attention to each one carefully. " +
				"If you have had the symptom since your LAST DIARY ENTRY, select YES." + Environment.NewLine +
				"If YES, let us know how OFTEN you had it, how SEVERE it was usually and " + 
				"how much it BOTHERED OR DISTRESSed by selecting the appropriate answer." + Environment.NewLine +
				"If you did not have the symptom, select NO";
		string msas1018sec2intro = 
			"We have 8 symptoms to ask you about in this section. Pay attention to each one carefully. " +
				"If you have had the symptom since your LAST DIARY ENTRY, let us know how SEVERE it was usually and " + 
				"how much it BOTHERED OR DISTRESSED by selecting the appropriate answer." + Environment.NewLine +
				"If you did not have the symptom, select NO.";
		string questionBegin = "Since the last entry, have you had any ";
		string[] answerYND = new string[] {"Yes", "No", "Did not try since last entry"} ;
		string[] answerOften = new string[] {"Almost Never", "Sometimes", "A lot", "Almost always"} ;
		string[] answerSev = new string[] {"Slight", "Moderate", "Severe", "Very severe"} ;
		string[] answerBother = new string[] {"Not at all", "A little bit", "Somewhat", "Quite a bit", "Very much"} ;
		
		//section 1: 23 questions - how often? how severe? how much bother or distress
		msas1018sec1[0] = new QuestionEntry ("Q1: " + questionBegin + "difficulty concentrating or paying attention?", answerYN, 3);
		//		msas1018sec1[1] = new QuestionEntry ("Q2: " + questionBegin + "pain?", answerYN, 3, "Pain",1);
		//		msas1018sec1[2] = new QuestionEntry ("Q3: " + questionBegin + "lack of energy?", answerYN, 3, "Lack of Energy", 5);
		//		msas1018sec1[3] = new QuestionEntry ("Q4: " + questionBegin + "COUGH? ", answerYN, 3, "Cough",3); 
		//		msas1018sec1[4] = new QuestionEntry ("Q5: " + questionBegin + "FEELING OF BEING NERVOUS? ", answerYN, 3, "Nervousness",5); 
		//		msas1018sec1[5] = new QuestionEntry ("Q6: " + questionBegin + "DRY MOUTH? ", answerYN, 3); 
		//		msas1018sec1[6] = new QuestionEntry ("Q7: " + questionBegin + "NAUSEA or FEELING LIKE YOU COULD VOMIT? ", answerYN, 3, "Nausea",3); 
		//		msas1018sec1[7] = new QuestionEntry ("Q8: " + questionBegin + "A FEELING OF BEING DROWSY? ", answerYN, 3, "Drowsiness",1); 
		//		msas1018sec1[8] = new QuestionEntry ("Q9: " + questionBegin + "ANY NUMBNESS, TINGLING or PINS AND NEEDLES FEELING IN HANDS or FEET? ", answerYN, 3, "Numbness/Tingling",3); 
		//		msas1018sec1[9] = new QuestionEntry ("Q10: " + questionBegin + "DIFFICULTY SLEEPING? ", answerYN, 3); 
		//		msas1018sec1[10] = new QuestionEntry ("Q11: " + questionBegin + "PROBLEMS WITH URINATION or 'PEEING'?" , answerYN, 3, "Problems with Urination",1); 
		//		msas1018sec1[11] = new QuestionEntry ("Q12: " + questionBegin + "VOMITING or THROWING UP?" , answerYN, 3,"Vomiting",3); 
		//		msas1018sec1[12] = new QuestionEntry ("Q13: " + questionBegin + "SHORTNESS OF BREATH?" , answerYN, 3, "Shortness of breath",1); 
		//		msas1018sec1[13] = new QuestionEntry ("Q14: " + questionBegin + "DIARRHEA OR LOOSE BOWEL MOVEMENT?" , answerYN, 3, "Diarrhea",3); 
		//		msas1018sec1[14] = new QuestionEntry ("Q15: " + questionBegin + "FEELING OF SADNESS?" , answerYN, 3, "Feeling of sadness",5); 
		//		msas1018sec1[15] = new QuestionEntry ("Q16: " + questionBegin + "SWEATS?" , answerYN, 3, "Sweats",5); 
		//		msas1018sec1[16] = new QuestionEntry ("Q17: " + questionBegin + "WORRING?" , answerYN, 3); 
		//		msas1018sec1[17] = new QuestionEntry ("Q18: " + questionBegin + "ITCHING?" , answerYN, 3, "Itching",3); 
		//		msas1018sec1[18] = new QuestionEntry ("Q19: " + questionBegin + "LACK OF APPETITE OR NOT WANTING TO EAT?" , answerYN, 3); 
		//		msas1018sec1[19] = new QuestionEntry ("Q20: " + questionBegin + "DIZZINESS?" , answerYN, 3, "Dizziness",3); 
		//		msas1018sec1[20] = new QuestionEntry ("Q21: " + questionBegin + "DIFFICULTY SWALLOWING'?" , answerYN, 3, "Difficulty Swallowing",1); 
		//		msas1018sec1[21] = new QuestionEntry ("Q22: " + questionBegin + "FEELING OF BEING IRRITABLE?" , answerYN, 3); 
		//		msas1018sec1[22] = new QuestionEntry ("Q23: " + questionBegin + "HEADACHE?" , answerYN, 3, "Headache",3); 
		//setion 2 : 8 questions - how severe? how much bother or distress?
		msas1018sec2[0] = new QuestionEntry ("Q1: " + questionBegin + " MOUTH SORES?" , answerYN, 3, "Mouth sores",3); 
		//		msas1018sec2[1] = new QuestionEntry ("Q2: " + questionBegin + " CHANGE IN THE WAY FOOD TASTES?" , answerYN, 3); 
		//		msas1018sec2[2] = new QuestionEntry ("Q3: " + questionBegin + " WEIGHT LOSS?" , answerYN, 3); 
		//		msas1018sec2[3] = new QuestionEntry ("Q4: " + questionBegin + " LESS HAIR THAN USUAL?" , answerYN, 3); 
		//		msas1018sec2[4] = new QuestionEntry ("Q5: " + questionBegin + " CONSTIPATION or UNCOMFORTABLE BECAUS BOWEL MOVEMENTS ARE LESS OFTEN?" , answerYN, 3, "Constipation",3); 
		//		msas1018sec2[5] = new QuestionEntry ("Q6: " + questionBegin + " SWELLING OF ARMS or LEGS?" , answerYN, 3, "Swelling in arms of legs",5); 
		//		msas1018sec2[6] = new QuestionEntry ("Q7: " + questionBegin + " Having the thought - I DO NOT LOOK LIKE MYSELF?" , answerYN, 3); 
		//		msas1018sec2[7] = new QuestionEntry ("Q8: " + questionBegin + " CHANGES IN SKIN?" , answerYN, 3,"Changes in skin",5); 
		//followup questions
		followUp[0] = new QuestionEntry("How often did you have it?", answerOften, 1);
		followUp[1] = new QuestionEntry("How severe was it usually?", answerSev, 1);
		followUp[2] = new QuestionEntry("How much did it bother or distress you?", answerBother, 0);//"not at all" = 0
		
		
		for (int i=0; i<lenPartA; i++) {
			if(i==0) questionArr[i] = new QuestionEntry(msas1018sec1intro, clickToBegin,0);
			else if(i==len1+1) questionArr[i] = new QuestionEntry(msas1018sec2intro, clickToBegin,0); 
			else if(i<=len1){
				if(i%4==1){		
					questionArr[i]=msas1018sec1[i/4]; 
				}
				else{
				questionArr[i]=(i%4==0)?followUp[2]:followUp[i%4-2];
				}
			}else if(i<lenPartA){
				int j=i-len1-1;
				if(j%3==1){
					questionArr[i]=msas1018sec2[j/3];
				}
				else{
				questionArr[i]=(j%3==0)?followUp[2]:followUp[1];
				}
			}
			
			
		}
		return questionArr;
		
		
	}// end of PopulateQuestionArr(); 
	
	QuestionEntry[] populateMSAS0809(int len){
		//MSAS 8-9 Questions with followup
		QuestionEntry[] msas0809 = new QuestionEntry[len];
		string intro = "We want to find out how you have been feeling since your last diary.";
		string[] answerHowLong = new string[]{"A very short time","A medium amount","Almost all the time"} ;
		string[] answerTrouble = new string[]{"Not at all", "A little", "A medium amount", "Very much"} ;
		
		msas0809[0] = new QuestionEntry(intro,clickToBegin,0);
		msas0809[1] = new QuestionEntry("Q1: " + "Did you feel more tired since your last entry than you usually do?", answerYN, 0);
		msas0809[2] = new QuestionEntry("How long did it last?", answerHowLong, 0);
		msas0809[3] = new QuestionEntry("How tired did you feel?", 
		                                new string[]{"A little","A medium amount", "Very tired"} , 0);
		msas0809[4] = new QuestionEntry("Q9: " + "If you had anyting else which made you feel bad or sick since your last diary, type it in below", cont, 0);
		msas0809[5] = new QuestionEntry ("How much did this bother or trouble you?", answerTrouble, 0);
		
		
		//		msas0809[3] = new QuestionEntry ("How much did being tired bother or trouble you?", answerTrouble, 0);
		//		msas0809[4] = new QuestionEntry ("Q2: " + "Did you feel sad since your last entry?", answerYN, 0);
		//		msas0809[5] = new QuestionEntry ("How long did you feel sad?", answerHowLong, 0);
		//		msas0809[6] = new QuestionEntry ("How sad did you feel?", 
		//			                                 new string[]{"A little", "A medium amount","Very sad"}, 0);
		//		msas0809[7] = new QuestionEntry ("How much did feeling sad bother or trouble you?", answerTrouble, 0);
		//		msas0809[8] = new QuestionEntry ("Q3: " + "Were you itchy since your last diary entry", answerYN, 0);
		//		msas0809[9] = new QuestionEntry ("How long were you itchy?", answerHowLong, 0);
		//		msas0809[10] = new QuestionEntry ("How itchy were you?", 
		//			                                  new string[]{"A little", "A medium amount","Very itchy"}, 0);
		//		msas0809[11] = new QuestionEntry ("How much did being itchy bother or trouble you?", answerTrouble, 0);
		//		msas0809[12] = new QuestionEntry ("Q4: " + "Did you feel any pain  since your last entry?", answerYN, 0);
		//		msas0809[13] = new QuestionEntry ("How much of the time did you feel pain?", answerHowLong, 0);
		//		msas0809[14] = new QuestionEntry ("How much pain did you feel?",
		//			                                  new string[]{"A little", "A medium amount","A lot"}, 0);
		//		msas0809[15] = new QuestionEntry ("How much did the pain bother or trouble you?", answerTrouble, 0);
		//		msas0809[16] = new QuestionEntry ("Q5: " + "Did you feel more worried since your last entry?", answerYN, 0);
		//		msas0809[17] = new QuestionEntry ("How much of the time did you feel worried", answerHowLong, 0);
		//		msas0809[18] = new QuestionEntry ("How worried did you feel?", 
		//			                                  new string[]{"A little", "A medium amount","Very worried"}, 0);
		//		msas0809[19] = new QuestionEntry ("How much did feeling worried bother or trouble you?", answerTrouble, 0);
		//
		//		msas0809[20] = new QuestionEntry ("Q6: " + "Since your last diary entry, did you feel like eating as you normally do?", answerYN, 0);
		//		msas0809[21] = new QuestionEntry ("How long did this last?", answerHowLong, 0);
		//		msas0809[22] = new QuestionEntry ("How much did this feeling bother or trouble you?", answerTrouble, 0);
		//		msas0809[23] = new QuestionEntry ("Q7: " + "Did you feel like going to vomit (or going to throw up) since your last diary entry?", answerYN, 0);
		//		msas0809[24] = new QuestionEntry ("How much of the time did you feel like you could vomit(or could throw up)?", answerHowLong, 0);
		//		msas0809[25] = new QuestionEntry ("How much did did this feeling bother or trouble you?", answerTrouble, 0);
		//
		//		msas0809[26] = new QuestionEntry ("Q8: " + "Did you have trouble going to sleep since your last diary entry?", answerYND, 0);
		//		msas0809[27] = new QuestionEntry ("How much did not being able to sleep bother or trouble you?", answerTrouble, 0);
		//
		//		msas0809[28] = new QuestionEntry ("Q9: " + "If you had anyting else which made you feel bad or sick since your last diary, type it in below", answerYN, 0);
		//		msas0809[29] = new QuestionEntry ("How much did this bother or trouble you?", answerTrouble, 0));
		
		return msas0809;
		
		
	}
	
	QuestionEntry[] populateAPPT(int len){
		QuestionEntry[] questionArr = new QuestionEntry[len];
		string Intro = "Now I need you to describe your pain.";
		string sec1Intro = "Select the areas on these drawings to show where you have pain.";
		string sec2Intro = "Move the slider to show how much pain you have had SINCE YOUR LAST DIARY ENTRY.";
		string sec3Intro = "Select as many of these words that describe your pain.";
		string other = "Do you have any other words that you would like to add that describe your pain?";
		string typeIn = "If yes, please type the min the text boxes on the screen.";
		
		
		
		return questionArr;
		
	}
	
	string[] createAPPTSec1(){
		string[] bod = new string[42];
		return bod;
	}
	
	string[][] populateAPPTSec3(){
		string[][] pains = new string[15][];
		pains[0]  = new string[]{"annoying", "bad", "horrible", "misserable", "terrible", "uncomfortable"} ;
		pains[1]  = new string[]{"numb","stiff","swollen","tight"} ;
		pains[2]  = new string[]{"awful","deadly","dying","killing"} ;
		pains[3]  = new string[]{"crying", "frightening","screaming","terrifying"} ;
		pains[4]  = new string[]{"dizzy","sickeing", "suffocating"} ;
		pains[5]  = new string[]{"aching", "hurting", "like an ache", "like a hurt", "sore"} ;
		pains[6]  = new string[]{"beating", "hitting", "pouding", "punching", "throbbing"} ;
		pains[7]  = new string[]{"biting", "cutting", "like a pin", "like a sharp knife", "pin like", "sharp", "stabbing"} ;
		pains[8]  = new string[]{"blistering","burning","hot"} ;
		pains[9]  = new string[]{"never goes away", "uncontrollable"} ;
		pains[10]  = new string[]{"always", "comes and goes", "comes on all of a sudden", "constant","sontinuous","forever"} ;
		pains[11]  = new string[]{"cramping", "crushing", "like a pinch", "pinching", "pressure"} ;
		pains[12]  = new string[]{"Itching", "like a scratch", "like a sting", "scratching", "stinging"} ;
		pains[13]  = new string[]{"off and on", "once in a while", "sneaks up", "sometimes", "steady"} ;
		pains[14]  = new string[]{"shocking", "shooting", "splitting"} ;
		return pains;
	}
	
	QuestionEntry[] populatePMIQ(int len1, int len2, int len3){
		QuestionEntry[] typeIn = new QuestionEntry[len1];
		QuestionEntry[] fillIn = new QuestionEntry[len2];
		QuestionEntry[] slideBar = new QuestionEntry[len3];
		
		QuestionEntry[] questionArr = new QuestionEntry[4];
		string intro = "Now I'm going to ask you if you've tried anything to help decrease your pain?";
		questionArr[0] = new QuestionEntry(intro, clickToBegin, 0);
		questionArr[1] = new QuestionEntry(intro, clickToBegin, 0);
		questionArr[2] = new QuestionEntry(intro, clickToBegin, 0);
		questionArr[3] = new QuestionEntry(intro, clickToBegin, 0);
		return null;
		
		
	}
	bool symptomAlgCriteria(String symptom, double[] ratings){
		double sum = 0;
		for(int i=0;i<ratings.Length;i++){
			sum+=ratings[i];}
		switch(symptom){
		case "Pain" : 
			return onceAlert(ratings, sum, 7.5,7.5,7.5);
		case "Drowsiness" : 
			return onceAlert(ratings, sum, 7.5,7.5,7.5);
		case "Problems with urination" : 
			return onceAlert(ratings, sum, 7.5,7.5,5.0);
		case "Shortness of breath" : 
			return onceAlert(ratings,sum, 5.0,5.0,5.0 );	
		case "Difficulty Swallowing" : 
			return onceAlert(ratings, sum, 7.5,7.5,7.5);
			//3 consecutive
		case "Cough": 
			return nthAlertForSec1(ratings, sum,7.5,7.5,5.0,3,"Cough");
		case "Numbness/Tingling":
			return nthAlertForSec1(ratings, sum,10,10,7.5,3,"Numbness/Tingling");                    
		case "Vomiting": 
			return nthAlertForSec1(ratings, sum,7.5,7.5,7.5,3,"Vomiting");                    
		case "Diarrhea":
			return nthAlertForSec1(ratings, sum,7.5,7.5,7.5,3,"Diarrhea");                    
		case "Dizziness": 
			return nthAlertForSec1(ratings, sum,7.5,7.5,7.5,3,"Dizziness");                    
		case "Itching":         
			return nthAlertForSec1(ratings, sum,7.5,7.5,7.5,3,"Itching");                    
		case "Constipation":        
			return nthAlertForSec2(ratings, sum,7.5,7.5,3,"Constipation");                    
		case "Headache":
			return nthAlertForSec1(ratings, sum,5,5,5,3,"Headache");                    
		case "Mouth sores":        
			return nthAlertForSec2(ratings, sum,5,5,3,"Mouth sores");
			//5 consecutive
		case "Lack of Energy":        
			return nthAlertForSec1(ratings, sum,5,5,5,5,"Lack of energy");                    
		case "Neverousness":         
			return nthAlertForSec1(ratings, sum,7.5,10,7.5,5,"Neverousness");                    
		case "Feeling of sadness":        
			return nthAlertForSec1(ratings, sum,10,10,10,5,"Feeling of sadness");                    
		case "Sweats":
			return nthAlertForSec1(ratings, sum,10,10,7.5,5,"Sweats");                    
		case "Swelling in arms or legs": 
			return nthAlertForSec2(ratings, sum,10,10,5,"Swelling in arms or legs");                    
		case "Changes in skin":        
			return nthAlertForSec2(ratings, sum,10,7.5,5,"Changes in skin");                  
		}
		return false;
	}
	
	double rate(char response){
		double rst =0; 
		switch (response) {
		case '1':
			rst  = 2.5;
			break; 
		case '2':
			rst  = 5.0;
			break;	
		case '3':
			rst  = 7.5;
			break;	
		case '4':
			rst = 10.0;
			break;	
		defualt:
				rst = 0;
			break; 
		}
		return rst;
	}
	
	AudioClip[] loadAudio(int length, String sex){//sex: female or male 
		AudioClip[] ac = new AudioClip[length];	
		//		for (int i=0,j=1; i<audioNum; j++) {
		//			if(i<msas1018sec1.Length*4){
		//
		//				ac [i] = Resources.Load ("Audio/Survey/"+sex+"/PartA/1018/majorQuestion_" + j) as AudioClip;
		//				for (int k=1; k<=3; k++) {
		//					ac [i + k] = Resources.Load ("Audio/Survey/"+sex+"/PartA/1018/minorQuestion_" + k) as AudioClip; 
		//				}
		//				i+=4; 
		//			}
		//			else{
		//				ac[i] = Resources.Load("Audio/Survey/"+sex+"/PartA/1018/majorQuestion_" + j) as AudioClip;
		//				for(int k=1; k<=2; k++){
		//					ac[i+k] = Resources.Load ("Audio/Survey/"+sex+"/PartA/1018/minorQuestion_" + (k+1)) as AudioClip; 
		//				}
		//				i+=3; 
		//			}
		//		}
		return ac;
	}
	
	
}



