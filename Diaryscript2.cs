using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
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
	int count;
	int maxPerDay;
	int age;
	int[] answerArr;//not in use
	int partASec1Num;
	int partASec2Num ;
	int partALen;
	int partA0809Len;
	string answers;
	string symptoms = "";	
	String alert;
	string[] answerYN = {"Yes", "No"}  ;
	string[] answerYND = {"Yes", "No", "Did not try since last entry"}  ;
	string[] clickToBegin = {"Click to begin"}  ;
	string[] cont = {"continue"}  ;
	QuestionEntry[] partAQuestionArr; 
	QuestionEntry[] questionArr;
	bool apptWindow;
	Texture apptImage;
	int winID;
	
	
	//Audio and Animation
	AudioClip[] questionAudio; 
	bool audioSwitch;
	int audioNum;
	//for PartA
	bool textOn = false;
	string input ="";
	int alertSymptomTimes;
	string response1;
	string response1_input;
	
	//for partB
	int sldrValue ;
	bool showSlider = false;
	int painlocation = 0;
	float painLv;
	float[] bod = new float[43];
	GUIStyle apptStyle;
	string painWord="";
	string painLocation = "";
	int[][] painFlag = new int[15][];
	string response2="";
	string response2_words;
	string response2_input;
	float[] apptWgrsArr;
	
	//for partC
	string[] medNames = new string[4];
	float[] medHelp4 = new float[12]; 
	string[] medTimes= new string[12];
	float[] medHelp6 =new float[2];
	string[][] painWords;
	string response3_medications;
	string response3_activity;
	string response3_input;
	
	
	// Initialization
	void Start () {
		//		clearKeys (); 	
		count = 0;
		maxPerDay = 2;
		ApplicationController = GameObject.Find ("ApplicationController"); 
		diaryWinLeft = 30; 
		diaryWinTop = 60; 
		age = 9;
		winID = -1;
		surveyGUI = new Rect(diaryWinLeft, diaryWinTop, 
		                     Screen.width-diaryWinLeft*2, Screen.height-diaryWinTop*2);
		apptImage = Resources.Load("appt") as Texture2D;
		apptGUI = new Rect(0, 0, 
		                   Screen.width-diaryWinLeft*2, Screen.height-diaryWinTop*2);
		//partB
		apptWgrsArr = new float[4];
		//partC
		painWords = populateApptSec3 ();//15 blocks
		medNames=new string[4]{"","","",""} ;
		for(int i=0;i<12;i++) medTimes[i]="";
		
		//Audio and Animation
		//		audioNum = questionArr.Length-1;
		//		audioSwitch = false; 
		//		questionAudio = loadAudio (audioNum, "female");
		//		apptsec3 = new QuestionEntry[4]; 
		//		   
	}
	
	public void OnGUI () {
		//ArgumentException, don't know solution
		switch(winID){//if(surveyWindow)
		case 0: GUI.skin = mySkin;
			surveyGUI = GUI.Window(0, surveyGUI, msas, "");
			break;
		case 1: GUI.skin = mySkin;
			surveyGUI = GUI.Window(1,surveyGUI, apptIntro,"");
			break;
		case 2: surveyGUI = GUI.Window(2,surveyGUI, appt,"");
			break;
		case 3: GUI.skin = mySkin;
			surveyGUI = GUI.Window(3,surveyGUI,apptWgrs,"");
			break;
		case 4: GUI.skin = mySkin;
			surveyGUI = GUI.Window(4,surveyGUI, describePain,"");
			break;
		case 5: GUI.skin = mySkin;
			surveyGUI = GUI.Window(5,surveyGUI, pmiq,"");
			break;
		default: break;
		}
		
	}
	
	//part A
	void msas(int windowID){
		//questionArr: all questions for Part A
		QuestionEntry[] questionArr = (age>9)?populateMSAS1018():populateMSAS0809();
		if(count<questionArr.Length){
			GUILayout.Label(questionArr[count].questionText);
			for (int j = 0; j < questionArr[count].answerText.Length; j++) {
				if(questionArr[count].answerText==cont) input=GUILayout.TextField(input, 20);
				if (GUILayout.Button (questionArr [count].answerText [j])) {
					// save user's answer to answerArr
					if (questionArr [count].answerText == answerYN
					    ||questionArr[count].answerText == answerYND) {
						//					answerArr [count] = 1 - j;
						//separate every qusetion with a comma
						answers += (count==1)?"":",";
						answers += (1 - j).ToString ();
						//for alert
						response1 += (1-j).ToString();
					}else if(questionArr[count].answerText==cont){
					answers+=(input!="")?("/"+input+","):"/*,*";
					}  else if (questionArr[count].answerText!=clickToBegin) {
						//answerArr [count] = j + questionArr [count].answerOffset;
						answers += (j + questionArr [count].answerOffset).ToString ();
						response1 += (j + questionArr [count].answerOffset).ToString ();
					}  
					
					
					//save response locally
					PlayerPrefs.SetString ("responses", answers);
					alert = getSymptom(response1); 
					symptoms += encodeSymptom(alert);
					
					//					alert = getSymptomCode (count, answers);
					//					symptoms += alert;
					PlayerPrefs.SetString ("symptoms", symptoms);
					
					// if user answers "no" to any symptom, skip the follow up questions
					if (questionArr [count].answerText == answerYN && j == 1) {
						count++;  
						while (questionArr[count].answerText != answerYN
						       &&questionArr[count].answerText != answerYND
						       &&questionArr[count].answerText.Length!=1) {
							//						answerArr [count] = -1;
							answers += "*";
							response1 += "*";
							PlayerPrefs.SetString ("responses", answers);
							alert = getSymptom(response1); 
							count++;
							if (count >= questionArr.Length) {
								answers+="/";
								PlayerPrefs.SetString ("responses", answers);
								winID=1;
								count=0;
								//							Debug.Log("after partA"+answers);
							}
						}
						//if no user input
					}  else if(questionArr[count].answerText==cont&&input==""){
						count+=2;
					}  else {
						count++;
					}
					
					if(count>=questionArr.Length){
						answers+="/";
						PlayerPrefs.SetString ("responses", answers);
						winID=1;
						count=0;
						//					Debug.Log("after partA"+answers);
					}
				}
			}  // presented answers
			
		}
		else{
			answers+="/";
			PlayerPrefs.SetString ("responses", answers);
			winID=1;
			count=0;
			//			Debug.Log("after msas"+answers);
		}
		if(GUI.Button(new Rect(20,600,70,60),"skip")) {
			for(int i=count;i<questionArr.Length-2;i++){
				answers+="*";}
			answers+="/*,*/";
			PlayerPrefs.SetString ("responses", answers);
			winID=1;
			count=0;
		}
	}  
	void apptIntro(int windowID){
		GUILayout.Label("Now I need you to describe your pain.");
		if(GUI.Button(new Rect(200,600,200,70),"Continue")){
			winID = 2;
		}
	}
	
	//part B
	void appt(int windowID){
		float w = apptGUI.width;
		float h = apptGUI.height;
		Rect sldrPos = new Rect(w*(float)0.02, h*(float)0.9, 80, 20);
		Rect[] painPos = populateBod(43);
		
		GUI.DrawTexture(apptGUI,apptImage, ScaleMode.StretchToFill, true, 10);
		for(int i=0;i<painPos.Length;i++){
			if(GUI.Button(painPos[i],"")){
				painlocation = i;
				//show the slider bar
//				showSlider = true;
			}
		}
		if(showSlider){
			bod[painlocation] = GUI.HorizontalSlider (sldrPos, bod[painlocation], 0f, 10f);
			GUI.Label(new Rect(w*(float)0.05, h*(float)0.85, 50,20), Convert.ToInt32(bod[painlocation]).ToString());
		}
		if(GUI.Button(new Rect(w*(float)0.03,h*(float)0.95,65,20),"continue")) {
			for(int i=0;i<bod.Length;i++){
				sldrValue = Convert.ToInt32(bod[i])-1;
				answers+= (sldrValue==-1)?"*":sldrValue.ToString();
				PlayerPrefs.SetString ("responses", answers);
			}
			answers+="/";
			count=0;
			winID=3;
			//			Debug.Log("after appt:" + answers);
		}    
	}
	void apptWgrs(int windowID){
		QuestionEntry[] questionArr = populateApptWgrs ();
		for(int i=0; i<questionArr.Length;i++){
			GUILayout.Label(questionArr[i].questionText);
			apptWgrsArr[i] = GUILayout.HorizontalSlider(apptWgrsArr[i],0f,10f);
			if(GUILayout.Button(cont[0])){
				answers+=(apptWgrsArr[i]!=0)? (apptWgrsArr[i]-1).ToString():"*";
				PlayerPrefs.SetString ("responses", answers);
				if(i==4){
					answers+="/";
					winID = 4; 
					count = 0;
				}
			}
		}


	}

	void describePain(int windowID){
		//painWords is array of pre-defined descriptors, not including user input
		if(count<painWords.Length){
			GUILayout.Label("Select as many of these words that describe your pain.");
			for(int j=0; j<painWords[count].Length;j++){
				if(GUILayout.Button(painWords[count][j])){
					painFlag[count][j] =1;
				}
			}
			if(GUI.Button(new Rect(200,600,200,70),"Continue")){
				for(int j=0;j<painWords[count].Length;j++){
					answers+= painFlag[count][j].ToString();
					PlayerPrefs.SetString ("responses", answers);
				}
				count++;
			}
		}else{
			GUILayout.Label("Do you have any other words that you would like to add that describe your your pain?");
			painWord = GUILayout.TextField(painWord,255);
			if(GUILayout.Button("Continue")){
			answers+=(painWord!="")?"/"+painWord+"/":"/*/";
				PlayerPrefs.SetString ("responses", answers);
				count=0;
				winID=5;
				//				Debug.Log("after describePain:"+ answers);
			}
		}
		//testing
		if(GUI.Button(new Rect(20,700,120,60), "skip") ) {
			for(int i=count;i<painWords.Length;i++){
				answers+="*";}
			answers+="/*/";
			PlayerPrefs.SetString ("responses", answers);
			count= 0;
			winID=5;}
	}
	//part C
	void pmiq(int windowID){
		//questionArr includes all questions of Part C
		QuestionEntry[] questionArr = populatePMIQ();
		if(count<questionArr.Length){
			GUILayout.Label(questionArr[count].questionText);
			//debugging
			GUILayout.Label("count="+count.ToString());
			if(count==0){ if(GUILayout.Button(clickToBegin[0])) count++;}
			else if(count<13){
				if(count%4==1){
					if(GUILayout.Button(questionArr[count].answerText[0])){
					answers+=(count==1)?"1":",1";
						count++;
					}
					else if(GUILayout.Button(questionArr[count].answerText[1])){
						while(count!=13){
							if(count==1) answers+="0";
							else if (count%4==1) answers+=",0";
							else answers+=",*";
							count++;
						}
					}
					PlayerPrefs.SetString ("responses", answers);
				}     
				else if(count%4==2){
					medNames[count/4] = GUILayout.TextField(medNames[count/4], 20);   
				}
				else if(count%4==3) {
					medTimes[count/4] = GUILayout.TextField(medTimes[count/4], 5); 
				}
				else if(count%4==0){
					medHelp4[count/4-1] = GUILayout.HorizontalSlider(medHelp4[count/4-1],0f,3f);
				}
			}
			else if(count<37){
				if(count%3==1){
					if(GUILayout.Button(questionArr[count].answerText[0])){
						if(count==13) answers += "/1";
						else {
							answers+=",1";}
						count++;
					}else if(GUILayout.Button(questionArr[count].answerText[1])){
						if(count==13) answers += "/0,*,*";
						else{  
							answers+=",0,*,*";}
						count+=3;//jump 
					}
					PlayerPrefs.SetString ("responses", answers);
				}     
				else if(count%3==2){
					medTimes[count/3-1] = GUILayout.TextField(medTimes[count/3-1], 20);   
				}
				else if(count%3==0){
					medHelp4[count/3-2] = GUILayout.HorizontalSlider(medHelp4[count/3-2], 0f,3f);
				}
				
			}
			else if (count<43){
				if(count==37){
					if(GUILayout.Button(questionArr[count].answerText[0])){
						answers+="/1";
						PlayerPrefs.SetString ("responses", answers);
						count++;
					}if(GUILayout.Button(questionArr[count].answerText[1])){
						answers+="/0,*,*,*";
						PlayerPrefs.SetString ("responses", answers);
						count+=4;//jump 
					}
				}     
				else if(count==38){
					medNames[3] = GUILayout.TextField(medNames[3], 255);   
				}
				else if(count==39){
					medTimes[11] = GUILayout.TextField(medTimes[11], 255);   
				}
				else if(count==40){
					medHelp4[11] = GUILayout.HorizontalSlider(medHelp4[11], 0f,3f);
				}
				else if(count==41){
					medHelp6[0] = GUILayout.HorizontalSlider(medHelp6[0], 0f,6f);
				}else{
					medHelp6[1] = GUILayout.HorizontalSlider(medHelp6[1], 0f,6f);
				}
			}
			if(((count<13&&count%4!=1)||(count%3!=1&&count>=13))&&count!=0||count>37){
				if(GUI.Button(new Rect(12,600,200,60), "continue")){
					if(count<13){
						if(count%4==2) answers+=(medNames[count/4]!="")?","+medNames[count/4]:",*";
						else if(count%4==3) answers+=(medTimes[count/4]!="")?","+medTimes[count/4]:",*";
						else if(count%4==0) answers+=","+Convert.ToInt32(medHelp4[count/4-1]).ToString();
					}else if(count<37){
						if(count%3==2) answers+=(medTimes[count/3]!="")?","+medTimes[count/3]:",*";
						else if(count%3==0) answers+=","+Convert.ToInt32(medHelp4[count/3-1]).ToString();
					}	
					else if(count==38){
					answers+=(medNames[3]!="")?","+medNames[3]:",*";   
					}
					else if(count==39){
					answers+=(medTimes[11]!="")?","+medTimes[11]:",*";   
					}
					else if(count==40){
						answers+=","+Convert.ToInt32(medHelp4[11]).ToString();   
					}
					else if(count==41){
						answers+=","+Convert.ToInt32(medHelp6[0]).ToString();
					}else{
						answers+=","+Convert.ToInt32(medHelp6[1]).ToString()+"/";
					}
					PlayerPrefs.SetString ("responses", answers);
					count++;
				}
			}
		}
		//avoid this part being excuted twice(don't why twice yet)
		else if(count<questionArr.Length+1){
			count++;
			winID = -1;
			GUILayout.Label("You've you finished diary today!");
			//			Debug.Log("after part3:"+answers);
			endDiary();
		}
		
	}
	
	//******** functional methods ********//
	//******** functional methods ********//
	public void startDiary(){
		//check last diary 
		string today = System.DateTime.Now.ToShortDateString();
		int diaryDailyTimes = PlayerPrefs.GetInt("diaryDailyTimes"+today);
		//		String preResponse = PlayerPrefs.GetString("responses");
		//		String preDate = PlayerPrefs.GetString("date");
		//		String preTime = PlayerPrefs.GetString("time");
		String diaryLastState = PlayerPrefs.GetString("diaryLastState");
		
		//if last unfishished, upload last time with "#incomplete"
		//		if(diaryLastState.Equals("unfinished"))
		//upload the response of last time
		//			queueResponsesForUploading(preDate+" "+preTime+":"+preResponse+"#incomplete");
		//		    Debug.Log("Have done " +times2day+" time(s) today");
		//		    Debug.Log("Last time response: " + PlayerPrefs.GetString("responses")); 
		if(diaryDailyTimes>=maxPerDay&&!devOptions.allowUnlimitedDiaries){
			//			toast("You've taken diary"+ times2day+ " times today");
			Debug.Log("exceeded the daily maximum" );
			ApplicationController.SendMessage("endDiary");
		}
		else{
			//			Debug.Log("last time:"+ preState);
			if(diaryLastState.Equals("unfishied")){//default/"finished"
				Debug.Log("Let's resume diary from beginning"); 
				//				toast ("Let's resume diary");
			}
			else{  //"finished"
				Debug.Log("Let's start last diary");
				PlayerPrefs.SetString("diaryLastState", "unfishied");
				//				toast ("Let's start diary");
			}
			//			PlayerPrefs.SetString("date", today); 
			//			PlayerPrefs.SetString("time",System.DateTime.Now.ToShortTimeString()); 
			count = 0; 
			answers = ""; 
			response1 = "";
			response1_input = "";
			response2 ="";
			response2_words = "";
			response2_input = "";
			response3_medications = "";
			response3_activity = "";
			response3_input="";
			winID = 0;	
		}
	}
	
	public void endDiary( ){
		winID = -1;
		//		String time = System.DateTime.Now.ToShortTimeString();
		string today = System.DateTime.Now.ToShortDateString ();
		int diaryDailyTimes = PlayerPrefs.GetInt ("diaryDailyTimes" + today);//what if started yesterday, finished today?
		//		if(audio.isPlaying){
		//			audio.Stop();
		//		}
		PlayerPrefs.SetString("diaryLastState", "finished");
		PlayerPrefs.SetInt("diaryDailyTimes"+today, ++diaryDailyTimes);
		//		PlayerPrefs.SetString("diary"+today+time+"responses", 
		//		                      PlayerPrefs.GetString("responses"));
		queueResponsesForUploading (answers); 
		//		Debug.Log ("Answers stored (via PlayerPref): " + 
		//		           PlayerPrefs.GetString("responses"));
		//		Debug.Log ("Responses stored : " + answers);
		//		Debug.Log("Symptom alerts: " + symptoms);//or call player
		//quit diary
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
		if(sym==null||sym.Length==0){
			return null;}
		string[] arr=sym.ToLower().Split(new char[]{' '} );        
		int len = arr.Length;
		string first = arr[0];
		StringBuilder sb = new StringBuilder();
		
		if(first=="problem") sb.Append("pu");
		else if (first=="difficulty") sb.Append("ds");
		else if (first=="nausea") sb.Append("na");
		else if (first=="dizziness") sb.Append("dz");
		else if (first=="constipation") sb.Append("cs");
		else if (first=="swelling") sb.Append("sal");
		
		else{
			sb.Append(getFirstLetter(arr[0]));
		}
		sb.Append("/");
		sb.Append(getFirstLetter(arr[len-1]));
		
		//		for(int i=0;i<len;i++){
		//            if(i=len-1) sb.Append("/");
		//			sb.Append(getFirstLetter(arr[i]));
		//		}
		return sb.Append(",").ToString();
	}
	
	string getFirstLetter(string str){
		if(str==null||str.Length==0){
			Debug.Log("no first letter");
			return null;}
		char[] arr = str.ToCharArray();
		return arr[0].ToString();
	}
	
	string getFirst3Letter(string str){
		if(str==null||str.Length<3){
			Debug.Log("length less than 3");
			return null;}
		return str.Substring(0,3);
	}
	
	//current count and responses, return symptom string triggered
	String getSymptom(string str){
		int len = str.Length;
		QuestionEntry[] questionArr = (age>9)?populateMSAS1018():populateMSAS0809();
		int n = 0;
		if(age>9){
			if(len<4||len>117) return null;
			else {
				int sec1length = 23*4; //
				n = (len<=sec1length)?len-3:len-2;
				
			}
		}else{//age 8~9
			if(len<4||len>28) return null;
			else if(len<=20){
				n = len-3;}
			else if (len<=26){
				n = len-2;}
			else n=len-1;
		}
		if(questionArr[n].alertSymptom!=null){
			//	if(triggerAlert(str)){
			//		return questionArr[n].alertSymptom;
			//	}
			//Map responses to each question to scores
			double[] ratings = getRatings(n, str);
			if((age>9&&symptomAlgCriteria1018(questionArr[n].alertSymptom, ratings))
			   ||(age<10&&symptomAlgCriteria0809(questionArr[n].alertSymptom,ratings))){
				Debug.Log("Seems that you have: " + questionArr[n].alertSymptom); 
				return questionArr[n].alertSymptom+" "+questionArr[n].alertSymptomTimes;
			}
		}
		return null;
		
	}
	
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
	
	double[] getRatings(int n, string str){//n:index in questionArr, str current responses
		int len = str.Length;
		String curQuestionResponse = null; 
		double[] ratings = new double[0]; 
		if (age>9){
			if (len <= 93) {//section 1: 3 followUp questions
				if (len % 4 != 0) {
					Debug.Log ("something wrong w/ response (93)");
					return null; 
				}
				curQuestionResponse = str.Substring (len - 3); // "345", "***"
				char[] curQR = curQuestionResponse.ToCharArray(); 
				ratings = new double[3];
				for (int i=0; i<curQuestionResponse.Length; i++) {
					//Debug.Log ("current resonse: " + curQR[i]); 
					ratings[i] = rate1018(curQR[i]);
				}
			}   else if (len <= 117) {//section 2: 2 followUp questions
				if (len % 3 != 2) {
					Debug.Log ("something wrong w/ response (117)");
					return null; 
				}
				curQuestionResponse = str.Substring (len - 2); // "35", "**"
				char[] curQR = curQuestionResponse.ToCharArray(); 
				ratings= new double[2];
				for (int i=0; i<curQuestionResponse.Length; i++) {
					//Debug.Log ("current resonse: " + curQR[i]);
					ratings[i] = rate1018(curQR[i]);
				}
			}   else {
				Debug.Log ("length exceeds 117");
			}
		}else{//age: 8~9
			if(len<=21){
				curQuestionResponse = str.Substring(len-3);
				ratings = new double[3];
			}else if(len<=27){
				curQuestionResponse = str.Substring(len-2);
				ratings = new double[2];
			}
			else if(len<=29){
				curQuestionResponse = str.Substring(len-1);
				ratings = new double[1];
			}
			else {
				Debug.Log("length exceeds 29");
			}
			char[] curQR = curQuestionResponse.ToCharArray();
			for(int i=0;i<curQuestionResponse.Length;i++) 
				ratings[i] = rate0809(curQR[i]);
		}
		return ratings;
	}
	
	bool nthAlertFor3FollowUp(double[] arr, double sum, double a, double b, double c, int limit, String sym){
		int n = PlayerPrefs.GetInt(sym);
		if((arr[0]>=a
		    &&arr[1]>=b
		    &&arr[2]>=c)){
			n++;
			if(n==limit) {
				PlayerPrefs.SetInt(sym, 0); 
				return true;
			}
			PlayerPrefs.SetInt(sym, n);
		}
		else {
			PlayerPrefs.SetInt(sym, 0); 
		}
		return false;
	}
	
	bool nthAlertFor2FollowUp(double[] arr, double sum, double a, double b, int limit, String sym){
		int n = PlayerPrefs.GetInt(sym);
		if(arr[0]>=a&&arr[1]>=b){
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
	
	bool nthAlertFor1FollowUp(double[] arr, double sum, double a, int limit, String sym){
		int n = PlayerPrefs.GetInt(sym);
		if(arr[0]>=a){
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
		//put all of your responses into a string, values separated by underscores
		submission = date + ":" + responses+"#"+symptoms; 
		ApplicationController.SendMessage("queueDiarySubmission", submission);
		Debug.Log ("final submission:" +submission);
	}
	
	//	for debugging
	void toast(string message)
	{
		//only include in code for tests on Android Device
		//		Toast.Instance ().ToastshowMessage (message, ToastLenth.LENGTH_SHORT);
	}
	
	//********//Populate and Hard Coding Methods Start Here //********//
	//********//Populate and Hard Coding Methods Start Here //********//
	//Merged this method with drawSurvey()
	QuestionEntry[] populateMSAS1018() {  
		int num1 = 23, num2 = 8;
		QuestionEntry[] msas1018sec1 = new QuestionEntry[23]; //Section1: 23 Questions MSAS 10-18
		QuestionEntry[] msas1018sec2 = new QuestionEntry[8]; // Section2: 8 Questions MSAS 10-18
		QuestionEntry[] followUp = new QuestionEntry[3]; // 3 follow-up questions
		
		int len1 = num1*4;
		int len2 = num2*3;
		int lenPartA = len1+len2+2+2;//"2" for introductions 2 for other symptoms
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
		string[] answerOften = new string[] {"Almost Never", "Sometimes", "A lot", "Almost always"}  ;
		string[] answerSev = new string[] {"Slight", "Moderate", "Severe", "Very severe"}  ;
		string[] answerBother = new string[] {"Not at all", "A little bit", "Somewhat", "Quite a bit", "Very much"}  ;
		
		//section 1: 23 questions - how often? how severe? how much bother or distress
		msas1018sec1[0] = new QuestionEntry ("Q1: " + questionBegin + "difficulty concentrating or paying attention?", answerYN, 3);
		msas1018sec1[1] = new QuestionEntry ("Q2: " + questionBegin + "pain?", answerYN, 3, "Pain",1);
		msas1018sec1[2] = new QuestionEntry ("Q3: " + questionBegin + "lack of energy?", answerYN, 3, "Lack of Energy", 5);
		msas1018sec1[3] = new QuestionEntry ("Q4: " + questionBegin + "COUGH? ", answerYN, 3, "Cough",3); 
		msas1018sec1[4] = new QuestionEntry ("Q5: " + questionBegin + "FEELING OF BEING NERVOUS? ", answerYN, 3, "Nervousness",5); 
		msas1018sec1[5] = new QuestionEntry ("Q6: " + questionBegin + "DRY MOUTH? ", answerYN, 3); 
		msas1018sec1[6] = new QuestionEntry ("Q7: " + questionBegin + "NAUSEA or FEELING LIKE YOU COULD VOMIT? ", answerYN, 3, "Nausea",3); 
		msas1018sec1[7] = new QuestionEntry ("Q8: " + questionBegin + "A FEELING OF BEING DROWSY? ", answerYN, 3, "Drowsiness",1); 
		msas1018sec1[8] = new QuestionEntry ("Q9: " + questionBegin + "ANY NUMBNESS, TINGLING or PINS AND NEEDLES FEELING IN HANDS or FEET? ", answerYN, 3, "Numbness/Tingling",3); 
		msas1018sec1[9] = new QuestionEntry ("Q10: " + questionBegin + "DIFFICULTY SLEEPING? ", answerYN, 3); 
		msas1018sec1[10] = new QuestionEntry ("Q11: " + questionBegin + "PROBLEMS WITH URINATION or 'PEEING'?" , answerYN, 3, "Problems with Urination",1); 
		msas1018sec1[11] = new QuestionEntry ("Q12: " + questionBegin + "VOMITING or THROWING UP?" , answerYN, 3,"Vomiting",3); 
		msas1018sec1[12] = new QuestionEntry ("Q13: " + questionBegin + "SHORTNESS OF BREATH?" , answerYN, 3, "Shortness of breath",1); 
		msas1018sec1[13] = new QuestionEntry ("Q14: " + questionBegin + "DIARRHEA OR LOOSE BOWEL MOVEMENT?" , answerYN, 3, "Diarrhea",3); 
		msas1018sec1[14] = new QuestionEntry ("Q15: " + questionBegin + "FEELING OF SADNESS?" , answerYN, 3, "Feeling of sadness",5); 
		msas1018sec1[15] = new QuestionEntry ("Q16: " + questionBegin + "SWEATS?" , answerYN, 3, "Sweats",5); 
		msas1018sec1[16] = new QuestionEntry ("Q17: " + questionBegin + "WORRING?" , answerYN, 3); 
		msas1018sec1[17] = new QuestionEntry ("Q18: " + questionBegin + "ITCHING?" , answerYN, 3, "Itching",3); 
		msas1018sec1[18] = new QuestionEntry ("Q19: " + questionBegin + "LACK OF APPETITE OR NOT WANTING TO EAT?" , answerYN, 3); 
		msas1018sec1[19] = new QuestionEntry ("Q20: " + questionBegin + "DIZZINESS?" , answerYN, 3, "Dizziness",3); 
		msas1018sec1[20] = new QuestionEntry ("Q21: " + questionBegin + "DIFFICULTY SWALLOWING'?" , answerYN, 3, "Difficulty Swallowing",1); 
		msas1018sec1[21] = new QuestionEntry ("Q22: " + questionBegin + "FEELING OF BEING IRRITABLE?" , answerYN, 3); 
		msas1018sec1[22] = new QuestionEntry ("Q23: " + questionBegin + "HEADACHE?" , answerYN, 3, "Headache",3); 
		//setion 2 : 8 questions - how severe? how much bother or distress?
		msas1018sec2[0] = new QuestionEntry ("Q1: " + questionBegin + " MOUTH SORES?" , answerYN, 3, "Mouth sores",3); 
		msas1018sec2[1] = new QuestionEntry ("Q2: " + questionBegin + " CHANGE IN THE WAY FOOD TASTES?" , answerYN, 3); 
		msas1018sec2[2] = new QuestionEntry ("Q3: " + questionBegin + " WEIGHT LOSS?" , answerYN, 3); 
		msas1018sec2[3] = new QuestionEntry ("Q4: " + questionBegin + " LESS HAIR THAN USUAL?" , answerYN, 3); 
		msas1018sec2[4] = new QuestionEntry ("Q5: " + questionBegin + " CONSTIPATION or UNCOMFORTABLE BECAUS BOWEL MOVEMENTS ARE LESS OFTEN?" , answerYN, 3, "Constipation",3); 
		msas1018sec2[5] = new QuestionEntry ("Q6: " + questionBegin + " SWELLING OF ARMS or LEGS?" , answerYN, 3, "Swelling in arms of legs",5); 
		msas1018sec2[6] = new QuestionEntry ("Q7: " + questionBegin + " Having the thought - I DO NOT LOOK LIKE MYSELF?" , answerYN, 3); 
		msas1018sec2[7] = new QuestionEntry ("Q8: " + questionBegin + " CHANGES IN SKIN?" , answerYN, 3,"Changes in skin",5); 
		//followup questions
		followUp[0] = new QuestionEntry("How often did you have it?", answerOften, 1);
		followUp[1] = new QuestionEntry("How severe was it usually?", answerSev, 1);
		followUp[2] = new QuestionEntry("How much did it bother or distress you?", answerBother, 0);//"not at all" = 0
		for (int i=0; i<lenPartA-2; i++) {
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
		questionArr [lenPartA - 2] = new QuestionEntry ("If you have had other symptoms since your last diary entry, please list them below and indicate how " +
		                                                "much the symptom distressed or bothered you.", cont, 0);
		questionArr[lenPartA-1] = new QuestionEntry("How much did it bother or distress you?", answerBother,0);
		return questionArr;
		
		
	}// end of PopulateQuestionArr(); 
	
	QuestionEntry[] populateMSAS0809(){
		//MSAS 8-9 Questions with followup
		QuestionEntry[] msas0809 = new QuestionEntry[31];
		string intro = "We want to find out how you have been feeling since your last diary.";
		string[] answerHowLong = new string[]{"A very short time","A medium amount","Almost all the time"}  ;
		string[] answerTrouble = new string[]{"Not at all", "A little", "A medium amount", "Very much"}  ;
		
		msas0809[0] = new QuestionEntry(intro,clickToBegin,0);
		msas0809[1] = new QuestionEntry("Q1: " + "Did you feel more tired since your last entry than you usually do?", answerYN, 0, "Tired",3);
		msas0809[2] = new QuestionEntry("How long did it last?", answerHowLong, 1);
		msas0809[3] = new QuestionEntry("How tired did you feel?", 
		                                new string[]{"A little","A medium amount", "Very tired"}  , 1);
		msas0809[4] = new QuestionEntry ("How much did being tired bother or trouble you?", answerTrouble, 0);
		msas0809[5] = new QuestionEntry ("Q2: " + "Did you feel sad since your last entry?", answerYN, 0,"Feeling Sad", 5 );
		msas0809[6] = new QuestionEntry ("How long did you feel sad?", answerHowLong, 1);
		msas0809[7] = new QuestionEntry ("How sad did you feel?", 
		                                 new string[]{"A little", "A medium amount","Very sad"} , 1);
		msas0809[8] = new QuestionEntry ("How much did feeling sad bother or trouble you?", answerTrouble, 0);
		msas0809[9] = new QuestionEntry ("Q3: " + "Were you itchy since your last diary entry", answerYN, 0, "Itching",3);
		msas0809[10] = new QuestionEntry ("How long were you itchy?", answerHowLong, 1);
		msas0809[11] = new QuestionEntry ("How itchy were you?", 
		                                  new string[]{"A little", "A medium amount","Very itchy"} , 1);
		msas0809[12] = new QuestionEntry ("How much did being itchy bother or trouble you?", answerTrouble, 0);
		msas0809[13] = new QuestionEntry ("Q4: " + "Did you feel any pain  since your last entry?", answerYN, 0,"Pain",1);
		msas0809[14] = new QuestionEntry ("How much of the time did you feel pain?", answerHowLong, 1);
		msas0809[15] = new QuestionEntry ("How much pain did you feel?",
		                                  new string[]{"A little", "A medium amount","A lot"} , 1);
		msas0809[16] = new QuestionEntry ("How much did the pain bother or trouble you?", answerTrouble, 0);
		msas0809[17] = new QuestionEntry ("Q5: " + "Did you feel more worried since your last entry?", answerYN, 0, "Worried",5);
		msas0809[18] = new QuestionEntry ("How much of the time did you feel worried", answerHowLong, 1);
		msas0809[19] = new QuestionEntry ("How worried did you feel?", 
		                                  new string[]{"A little", "A medium amount","Very worried"} , 1);
		msas0809[20] = new QuestionEntry ("How much did feeling worried bother or trouble you?", answerTrouble, 0);
		
		msas0809[21] = new QuestionEntry ("Q6: " + "Since your last diary entry, did you feel like eating as you normally do?", answerYN, 0);
		msas0809[22] = new QuestionEntry ("How long did this last?", answerHowLong, 1);
		msas0809[23] = new QuestionEntry ("How much did this feeling bother or trouble you?", answerTrouble, 0);
		msas0809[24] = new QuestionEntry ("Q7: " + "Did you feel like going to vomit (or going to throw up) since your last diary entry?", answerYN, 0,"Vomiting",3);
		msas0809[25] = new QuestionEntry ("How much of the time did you feel like you could vomit(or could throw up)?", answerHowLong, 1);
		msas0809[26] = new QuestionEntry ("How much did did this feeling bother or trouble you?", answerTrouble, 0);
		
		msas0809[27] = new QuestionEntry ("Q8: " + "Did you have trouble going to sleep since your last diary entry?", answerYND, 0, "Sleep",5);
		msas0809[28] = new QuestionEntry ("How much did not being able to sleep bother or trouble you?", answerTrouble, 0);
		
		msas0809[29] = new QuestionEntry ("Q9: " + "If you had anyting else which made you feel bad or sick since your last diary, type it in below", cont, 0);
		msas0809[30] = new QuestionEntry ("How much did this bother or trouble you?", answerTrouble, 0);
		
		return msas0809;
		
		
	}
	
	bool symptomAlgCriteria1018(String symptom, double[] ratings){
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
			return nthAlertFor3FollowUp(ratings, sum,7.5,7.5,5.0,3,"Cough");
		case "Numbness/Tingling":
			return nthAlertFor3FollowUp(ratings, sum,10,10,7.5,3,"Numbness/Tingling");                    
		case "Vomiting": 
			return nthAlertFor3FollowUp(ratings, sum,7.5,7.5,7.5,3,"Vomiting");                    
		case "Diarrhea":
			return nthAlertFor3FollowUp(ratings, sum,7.5,7.5,7.5,3,"Diarrhea");                    
		case "Dizziness": 
			return nthAlertFor3FollowUp(ratings, sum,7.5,7.5,7.5,3,"Dizziness");                    
		case "Itching":         
			return nthAlertFor3FollowUp(ratings, sum,7.5,7.5,7.5,3,"Itching");                    
		case "Constipation":        
			return nthAlertFor2FollowUp(ratings, sum,7.5,7.5,3,"Constipation");                    
		case "Headache":
			return nthAlertFor3FollowUp(ratings, sum,5,5,5,3,"Headache");                    
		case "Mouth sores":        
			return nthAlertFor2FollowUp(ratings, sum,5,5,3,"Mouth sores");
			//5 consecutive
		case "Lack of Energy":        
			return nthAlertFor3FollowUp(ratings, sum,5,5,5,5,"Lack of energy");                    
		case "Neverousness":         
			return nthAlertFor3FollowUp(ratings, sum,7.5,10,7.5,5,"Neverousness");                    
		case "Feeling of sadness":        
			return nthAlertFor3FollowUp(ratings, sum,10,10,10,5,"Feeling of sadness");                    
		case "Sweats":
			return nthAlertFor3FollowUp(ratings, sum,10,10,7.5,5,"Sweats");                    
		case "Swelling in arms or legs": 
			return nthAlertFor2FollowUp(ratings, sum,10,10,5,"Swelling in arms or legs");                    
		case "Changes in skin":        
			return nthAlertFor2FollowUp(ratings, sum,10,7.5,5,"Changes in skin");                  
		}
		return false;
	}
	//Note you cannot be age 8~9 today and 10~18 tomorrow
	//8~9 and 10~18 use the same keywords for alert triggering, like "Itching"
	bool symptomAlgCriteria0809(String symptom, double[] ratings){
		double sum = 0;
		for(int i=0;i<ratings.Length;i++){
			sum+=ratings[i];}
		switch(symptom){
		case "Pain" : 
			return onceAlert(ratings, sum, 6.6,6.6,3.3);
			//3 consecutive
		case "Tired": 
			return nthAlertFor3FollowUp(ratings, sum,10,10,10,3,"Tired");               
		case "Itching":         
			return nthAlertFor3FollowUp(ratings, sum,10,10,10,3,"Itching");                    
		case "Vomiting":        
			return nthAlertFor2FollowUp(ratings, sum,6.6,6.6,3,"Vomiting");                    ;
			//5 consecutive
		case "Sleep":        
			return nthAlertFor1FollowUp(ratings, sum,6.6,5,"Sleep");                                    
		case "Feeling Sad":        
			return nthAlertFor3FollowUp(ratings, sum,6.6,6.6,6.6,5,"Feeling Sad");                    
		case "Worried":
			return nthAlertFor3FollowUp(ratings, sum,6.6,10,6.6,5,"Worried");                    
		}
		return false;
	}
	
	
	double rate1018(char response){
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
	
	double rate0809(char response){
		double rst = 0; 
		switch(response){
		case '1':
			rst  = 3.3;
			break; 
		case '2':
			rst  = 6.6;
			break;	
		case '3':
			rst  = 10.0;
			break;		
		defualt:
				rst = 0;
			break; 
		}
		return rst;
	}
	
	QuestionEntry[] populateAppt(int len){
		QuestionEntry[] questionArr = new QuestionEntry[len];
		string Intro = "Now I need you to describe your pain.";
		string sec1Intro = "Select the areas on these drawings to show where you have pain.";
		string sec2Intro = "Move the slider to show how much pain you have had SINCE YOUR LAST DIARY ENTRY.";
		string sec3Intro = "Select as many of these words that describe your pain.";
		string other = "Do you have any other words that you would like to add that describe your pain?";
		string typeIn = "If yes, please type the min the text boxes on the screen.";
		return questionArr;
		
	}
	
	string[][] populateApptSec3(){
		string[][] pains = new string[15][];
		//evaluative
		pains[0]  = new string[]{"annoying", "bad", "horrible", "misserable", "terrible", "uncomfortable"}  ;
		//sensory
		pains[1]  = new string[]{"aching", "hurting", "like an ache", "like a hurt", "sore"}  ;
		pains[2]  = new string[]{"beating", "hitting", "pouding", "punching", "throbbing"}  ;
		pains[3]  = new string[]{"biting", "cutting", "like a pin", "like a sharp knife", "pin like", "sharp", "stabbing"}  ;
		pains[4]  = new string[]{"blistering","burning","hot"}  ;
		pains[5]  = new string[]{"cramping", "crushing", "like a pinch", "pinching", "pressure"}  ;
		pains[6]  = new string[]{"Itching", "like a scratch", "like a sting", "scratching", "stinging"}  ;
		pains[7]  = new string[]{"shocking", "shooting", "splitting"}  ;
		pains[8]  = new string[]{"numb","stiff","swollen","tight"}  ;
		//affective
		pains[9]  = new string[]{"awful","deadly","dying","killing"}  ;
		pains[10]  = new string[]{"crying", "frightening","screaming","terrifying"}  ;
		pains[11]  = new string[]{"dizzy","sickeing", "suffocating"}  ;
		//evalucative
		pains[12]  = new string[]{"never goes away", "uncontrollable"}  ;
		//temporal
		pains[13]  = new string[]{"always", "comes and goes", "comes on all of a sudden", "constant","sontinuous","forever"}  ;
		pains[14]  = new string[]{"off and on", "once in a while", "sneaks up", "sometimes", "steady"}  ;
		for(int i=0;i<pains.Length;i++){
			painFlag[i]=new int[pains.Length];
		}
		return pains;
	}
	
	Rect[] populateBod(int len){
		Rect[] painPos = new Rect[len];
		
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
		
		return painPos;
	}

	QuestionEntry[] populateApptWgrs(){
		QuestionEntry[] rst = new QuestionEntry[4];
		rst[0] = new QuestionEntry("How much pain do you have right NOW?", cont, 0);
		rst[1] = new QuestionEntry("How much pain did you have, ON AVERAGE, since your last diary entry?", cont, 0);
		rst[2] = new QuestionEntry("What was your WORST pain since your last diary entry?", cont, 0);
		rst[3] = new QuestionEntry("What was your LEAST pain since your last diary entry?", cont, 0);
		return rst;
	}

	QuestionEntry[] populatePMIQ(){//initialize it in Start() 
		string intro = "Now I'm going to ask you if you've tried anything to help decrease your pain?";
		string q1 = "Have you taken ";
		string q2 = " pain medication since the last diary entry?";
		string typeInName = "Please type in the name of the medication: (If you do not know how to spell it, that's ok, just try your best).";
		string typeInTimes = "Please type in how many times was this medication taken since the last entry.";
		string typeInTimes2 = "How many times was this activity done since the last entry.";
		string slider = "How much did this medicine help? (Move the slider bar to show how much the medicine helped, from 'Did not help at all' to ' Helped a lot')";
		string slider2 ="How much did this activity help? (Move the slider bar to show how much the medicine helped, from 'Did not help at all' to ' Helped a lot')";
		string beginQ = "Since your last diary entry, have you tried ";       
		string ctrl = "Based on all the things you did to cope, deal with your pain TODAY how much control did you fell you had over it?" +
			"Please select the appropriate number. Remember, you can select any number along the scale.";
		string dec = "Based on all the things you did to cope, deal with your pain TODAY how much were you able to decrease it?" +
			"Please select the appropriate number. Remember, you can select any number along the scale.";
		string[] times = new string[]{"any", "a second", "a third"} ;
		string[] activity = new string[]{"DEEP BREATHING", "A RELAXATION EXERCISE","Thinking about your pain in a positive way (for example, thinking that the" + 
			" pain means that my treatment is working)", "HEAT PACKS", "MASSAGE", "IMAGERY? Or using your mind to imagine pictures, sights, sounds to relax", 
			"DISTRACTION? Like watching TV, playing video games or doing something to take your mind off of the pain", "Talking with friends/parents"} ;
		
		string[] empty = new string[]{""} ;
		int len = 43;
		QuestionEntry[] questionArr = new QuestionEntry[len];   
		for(int i=0; i<len;i++){
			if(i==0) questionArr[i] = new QuestionEntry(intro, clickToBegin, 0);
			else if(i<13){
				if(i%4==1) questionArr[i] = new QuestionEntry(q1+times[i/4]+q2,answerYN,0);
				else if(i%4==2) questionArr[i] = new QuestionEntry(typeInName, empty,0);
				else if(i%4==3) questionArr[i] = new QuestionEntry(typeInTimes,empty,0);
				else if(i%4==0) questionArr[i] = new QuestionEntry(slider,empty,0);
			}
			else if(i<37){
				//				Debug.Log("activity : " + activity.Length);
				//				Debug.Log("i = " + i); 
				if(i%3==1) questionArr[i] = (i==13)?new QuestionEntry(beginQ+activity[(i-13)/3]+"?",answerYN,0):
					new QuestionEntry(activity[(i-13)/3]+"?",answerYN,0);
				if(i%3==2) questionArr[i] = new QuestionEntry(typeInTimes2,empty,0);
				if(i%3==0) questionArr[i] = new QuestionEntry(slider2,empty,0);
			}
		}
		questionArr[37] = new QuestionEntry("Any OTHER acitivity?", answerYN, 0);
		questionArr[38] = new QuestionEntry("Please type in the name of this activity",new string[] {""} , 0);
		questionArr[39] = new QuestionEntry(typeInTimes2, new string[]{""} , 0);
		questionArr[40] = new QuestionEntry(slider2,empty, 0);
		questionArr[41] = new QuestionEntry(ctrl, new string[]{""} , 0);
		questionArr[42] = new QuestionEntry(dec,new string[] {""} , 0);
		
		
		return questionArr;
	}
	
	Dictionary<string,string> getApptStringCode() {
		Dictionary<string,string> rst = new Dictionary<string,string > ();
		string[][] labels = populateApptSec3 ();
		
		for(int i=0; i<labels.Length;i++){
			string code ;
			string [] arr = labels[i];
			if(i==0)  code = "e"; //evaluative word
			else if(i<9)  code = "s";//sensory word
			else if(i<12) code = "a";//affective word
			else if(i<13) code = "e";//evaluative word
			else code = "t";//temporal word
			
			for(int j=0;j<arr.Length;j++)
				rst.Add(arr[j],code);
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




