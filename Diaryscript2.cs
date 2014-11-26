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
	public GUISkin mySkin; //for diary window style 
    public bool surveyWindow ;
	public QuestionEntry[] questionArr;
	public GameObject ApplicationController; //This is for Tyler to use this script outside it

	//Private Objects and Variables
    //Diary process
	Rect surveyGUI ;
	QuestionEntry[] msas1018sec1, msas1018sec2, followUp;
	int[] answerArr;
	string answers;
	string symptoms = "";
	int count = 0;
	string questionBegin = "Since the last entry, have you had any ";
	string[] answerYN = new string[] {"Yes", "No"};
	string[] answerYND = new string[] {"Yes", "No", "Did not try since last entry"};
	string[] answerOften = new string[] {"Almost Never", "Sometimes", "A lot", "Almost always"};
	string[] answerSev = new string[] {"Slight", "Moderate", "Severe", "Very severe"};
	string[] answerBother = new string[] {"Not at all", "A little bit", "Somewhat", "Quite a bit", "Very much"};
	int diaryWinLeft; 
	int diaryWinTop; 
    bool messageTrigger;
	int maxPerDay;
	String today ;
	String alert; 
	QuestionEntry[] apptsec3;

    //Audio and Animation
	AudioClip[] questionAudio; 
	bool audioSwitch;
	int audioNum;
    

	// Initialization
	void Start () {
		//QuestionEnties

//		clearKeys (); 
		drawSurvey();
		maxPerDay = 20;
		ApplicationController = GameObject.Find ("ApplicationController"); 
        diaryWinLeft = 30; 
		diaryWinTop = 60; 
        messageTrigger = false; 
//		today = System.DateTime. Now.ToShortDateString();//This time is initialization time not diary starting or finishing time
        
        //Audio and Animation
		audioNum = questionArr.Length-1;
		audioSwitch = false; 
		questionAudio = loadAudio (audioNum, "female");
		apptsec3 = new QuestionEntry[4]; 
//		   
	}
	
	public void OnGUI () {
		GUI.skin = mySkin; 
		if(surveyWindow) {
			Rect surveyGUI = GUI.Window(0, new Rect(diaryWinLeft, diaryWinTop, 
			                 Screen.width-diaryWinLeft*2, Screen.height-diaryWinTop*2), 
			                 beginSurvey, "");
		}
	}
	void beginSurvey(int windowID) {
		if (count < questionArr.Length) {
			// print question text in box
			GUILayout.Label (questionArr [count].questionText);

			// loop through answer text
			for (int j = 0; j < questionArr[count].answerText.Length; j++) {
				if (GUILayout.Button (questionArr [count].answerText [j])) {
					// save user's answer to answerArr
					if (questionArr [count].answerText == answerYN) {
							answerArr [count] = 1 - j;
							answers += (1 - j).ToString ();
					} else if (count != 0) {
							answerArr [count] = j + questionArr [count].answerOffset;
							answers += (j + questionArr [count].answerOffset).ToString ();
					}

					PlayerPrefs.SetString ("responses", answers);
//					alert = getSymptom(count, answers); 
//					symptoms += encodeSymptom(alert);
					alert = getSymptomCode (count, answers);
					symptoms += alert;
					PlayerPrefs.SetString ("symptoms", symptoms);

//					toast("It looks like you have had"+ questionArr[Convert.ToInt32(alert)].alertSymptom+", your health care provider has been notified. Your Pain Buddy will now start a skills practice that can help you feel better");
//					ApplicationController.SendMessage(alert);

					// if user answers "no" to any symptom, skip the follow up questions
					if (questionArr [count].answerText == answerYN && j == 1) {
							count++;
							while (questionArr[count].answerText != answerYN) {
									answerArr [count] = -1;
									answers += "*";
									PlayerPrefs.SetString ("responses", answers);
//							alert = getSymptom(count, answers);
									//toast("It looks like you have had"+ alert+", your health care provider has been notified. 
									//Your Pain Buddy will now start a skills practice that can help you feel better")
									//ApplicationController.SendMessage(alert);
									count++;
									if (count >= questionArr.Length) {
//								Debug.Log("End of survey questions - 0"); 
											endDiary (answerArr);
											return;
									}
							}
					} else {
							count++;
					}

					if (count >= questionArr.Length) {
//						Debug.Log("End of survey questions - 1 ");
							endDiary (answerArr);
							return;
					}
					//Click to play audio
					audio.Stop ();
					audioSwitch = true; 
				}

			} // end of answerText loop

			//Audio and Animation 
			if (audioSwitch) {
					if (count <= audioNum && count >= 1) {
							audio.clip = questionAudio [count - 1]; 
							audio.Play (); 
							audioSwitch = false; 
					} else {
							audio.Stop ();
					}
			}
		} // end of question if statement
		else { 
////		answers = encodeToString(answerArr); 
            GUILayout.Label("You have already taken the survey today!");
//			Debug.Log("count has exceeded question number" ); 
			ApplicationController.SendMessage("endDiary");
			audio.Stop();
			return;
		}
	}

//***functional methods (sorted alphebatically)***//
	void clearKeys(){
		PlayerPrefs.DeleteKey ("diary" + today);//"finished", "unfinishied"
		PlayerPrefs.DeleteKey ("diary" + today+ "times");//1,2,..,maxPerDay
		PlayerPrefs.DeleteKey ("responses");// the string: "0***0***..."
		PlayerPrefs.DeleteAll (); 
	}
	void decodeSymptomCode(string str){
		int len = str.Length;
		Debug.Log ("str  :" + str);
		int n = Convert.ToInt32(str);
		Debug.Log("Seems you have symptom" + questionArr[n].alertSymptom+" "+questionArr[n].alertSymptomTimes+"consecutive times");

	}
	void drawSurvey() {
		msas1018sec1 = new QuestionEntry[23]; //Section1: 23 Questions MSAS 10-18
		msas1018sec2 = new QuestionEntry[8]; // Section2: 8 Questions MSAS 10-18
		followUp = new QuestionEntry[3]; // 3 follow-up questions
		apptsec3 = new QuestionEntry[4];

		int length = msas1018sec1.Length*4+msas1018sec2.Length*3;
		questionArr = new QuestionEntry[length+1];//"1" for "click to begin" 
		answerArr = new int[questionArr.Length];//the same length as above
		PopulateQuestionArr();
	}
	void proceedToNext(){
	}
	public void endDiary(int[] answerArr ){
		String time = System.DateTime.Now.ToShortTimeString();
		int times = PlayerPrefs.GetInt ("diary" + today + "times");
		surveyWindow = false; 
		
		if(audio.isPlaying){
			audio.Stop();
		}
		PlayerPrefs.SetString("diaryLastState", "finished");
		PlayerPrefs.SetInt("diary"+today+"times", ++times);
		PlayerPrefs.SetString("diary"+today+time+"responses", 
		                      PlayerPrefs.GetString("responses"));
		queueResponsesForUploading (answers); 
		//		Debug.Log ("Answers stored (via PlayerPref): " + 
		//		           PlayerPrefs.GetString("responses"));
		Debug.Log ("Responses stored : " + answers);
		Debug.Log("Symptom alerts: " + symptoms);//or call player
		//testing 
//		string[] arr2 = symptoms.Split(new char[]{','});
//		for(int i=0;i<arr2.Length;i++){
//			decodeSymptomCode(arr2[i]);
//		}
		//quit diary
		ApplicationController.SendMessage ("endDiary");
	}
	//not-in-use
	String encodeResponses(int[] input){
		if(input==null||input.Length==0) return "no input"; 
		string s = ""; 
		for (int i=1; i<answerArr.Length; i++) {//ignore i=0; corresponding button is "click to begin"
			int a = new int();
			a = input[i];
			s += (a==-1)? "*": a.ToString(); 
		}
		return s; 
	}
	//encode symptom string to concise string, "lack of energy" to "loe"
	String encodeSymptom(string sym){
		if(sym==null||sym.Length==0){
			return null;}
		string[] arr=sym.ToLower().Split(new char[]{' '});
		int len = arr.Length;
		StringBuilder sb = new StringBuilder();
		for(int i=0;i<len;i++){
			sb.Append(getFirstLetter(arr[i]));
		}return sb.Append(",").ToString();
	}
	string getFirstLetter(string str){
		if(str==null||str.Length==0){
			Debug.Log("no first letter");
			return null;}
		char[] arr = str.ToCharArray();
		return arr[0].ToString();
		
	}
	//current count and responses, return symptom string triggered
	String getSymptom(int idx, string str){
		if(idx<3||idx>116) return null;
		else {
			int sec1length = msas1018sec1.Length*4; 
			int n = (idx<=sec1length)?idx-3:idx-2;
			//Debug.Log ("n: " + n); 
			if(questionArr[n].alertSymptom!=null){
				//				if(triggerAlert(str)){
				//				 return questionArr[n].alertSymptom;
				//				}
				double[] ratings = getRatings(n, str);
				if(symptomAlgCriteria(questionArr[n].alertSymptom, ratings))
					Debug.Log("Seems that you have: " + questionArr[n].alertSymptom); 
				return questionArr[n].alertSymptom;
			}
			return null;
		}
	}
	//current count and responses string, return symptom code string. "21" for "Pain"
	string getSymptomCode(int idx, string str){
		if(idx<3||idx>116) return null;
		else {
			int sec1length = msas1018sec1.Length*4; 
			int n = (idx<=sec1length)?idx-3:idx-2;
			//Debug.Log ("n: " + n); 
			if(questionArr[n].alertSymptom!=null){
				double[] ratings = getRatings(n, str);
				if(symptomAlgCriteria(questionArr[n].alertSymptom, ratings))
					return n.ToString();
			}
			return null;
		}
	}
	double[] getRatings(int n, string str){//n symptom idx, str current responses
		int len = str.Length;
		String curQuestionResponse = null; 
		double[] ratings = new double[0]; 
		if (len <= 93) {//section 1: 3 followUp questions
			if (len % 4 != 0) {
				Debug.Log ("something wrong w/ response (93)");
				return null; 
			}
			curQuestionResponse = str.Substring (len - 3); // "345", "***"
			char[] curQR = curQuestionResponse.ToCharArray(); 
			ratings = new double[3];
			for (int i=0; i<curQuestionResponse.Length; i++) {
				//				Debug.Log ("current resonse: " + curQR[i]); 
				ratings[i] = rate(curQR[i]);
			}
		} else if (len <= 117) {//section 2: 2 followUp questions
			if (len % 3 != 2) {
				Debug.Log ("something wrong w/ response (117)");
				return null; 
			}
			curQuestionResponse = str.Substring (len - 2); // "35", "**"
			char[] curQR = curQuestionResponse.ToCharArray(); 
			ratings= new double[2];
			for (int i=0; i<curQuestionResponse.Length; i++) {
				//				Debug.Log ("current resonse: " + curQR[i]);
				ratings[i] = rate(curQR[i]);
			}
		} else {
			Debug.Log ("length exceeds 117");
		}
		return ratings;
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

	public void startDiary(){
		//check last diary 
		int times2day = PlayerPrefs.GetInt("diary"+today+"times");//if date changes clean this key
		String preResponse = PlayerPrefs.GetString("responses");
		String preDate = PlayerPrefs.GetString("date");
		String preTime = PlayerPrefs.GetString("time");
		String preState = PlayerPrefs.GetString("diaryLastState");

		//if last unfishished, upload last time with "#incomplete"
		if(preState.Equals("unfinished"))
			queueResponsesForUploading(preDate+" "+preTime+":"+preResponse+"#incomplete");

		Debug.Log("Have done " +times2day+" time(s) today");
//		Debug.Log("Last time response: " + PlayerPrefs.GetString("responses")); 
		if(times2day>=maxPerDay&&!devOptions.allowUnlimitedDiaries){
//			toast("You've taken diary"+ times2day+ " times today");
			Debug.Log("exceeded the daily maximum" );
			ApplicationController.SendMessage("endDiary");
		}
		else{
			Debug.Log("last time:"+ preState);
			if(preState.Equals("unfishied")){//default/"finished"

//				}
				Debug.Log("Let's resume diary from beginning"); 
				toast ("Let's resume diary");
			}
			else{ //"finished"
				Debug.Log("Let's start last diary");
				PlayerPrefs.SetString("diaryLastState", "unfishied");
				toast ("Let's start diary");
			}
			PlayerPrefs.SetString("date", today); 
			PlayerPrefs.SetString("time",System.DateTime.Now.ToShortTimeString()); 
			count = 0; 
			answers = ""; 
			surveyWindow = true;
//			Debug.Log ("this time: "+ PlayerPrefs.GetString("diary"+today));
		}
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
	void PopulateQuestionArr() {
		string[] beginQ = new string[] {"Click to Begin"};
		questionArr[0] = new QuestionEntry("Welcome!", beginQ, 0);
		followUp[0] = new QuestionEntry("How often did you have it?", answerOften, 1);
		followUp[1] = new QuestionEntry("How severe was it usually?", answerSev, 1);
		followUp[2] = new QuestionEntry("How much did it bother or distress you?", answerBother, 0);//"not at all" = 0
        //section 1: 23 questions - how often? how severe? how much bother or distress
		msas1018sec1[0] = new QuestionEntry("Q1: " + questionBegin + "difficulty concentrating or paying attention?", answerYN, 3);
		msas1018sec1[1] = new QuestionEntry("Q2: " + questionBegin + "pain?", answerYN, 3, "Pain",1);
		msas1018sec1[2] = new QuestionEntry("Q3: " + questionBegin + "lack of energy?", answerYN, 3, "Lack of Energy", 5);
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
		msas1018sec2[0] = new QuestionEntry ("Q24: " + questionBegin + " MOUTH SORES?" , answerYN, 3, "Mouth sores",3); 
		msas1018sec2[1] = new QuestionEntry ("Q25: " + questionBegin + " CHANGE IN THE WAY FOOD TASTES?" , answerYN, 3); 
		msas1018sec2[2] = new QuestionEntry ("Q26: " + questionBegin + " WEIGHT LOSS?" , answerYN, 3); 
		msas1018sec2[3] = new QuestionEntry ("Q27: " + questionBegin + " LESS HAIR THAN USUAL?" , answerYN, 3); 
		msas1018sec2[4] = new QuestionEntry ("Q28: " + questionBegin + " CONSTIPATION or UNCOMFORTABLE BECAUS BOWEL MOVEMENTS ARE LESS OFTEN?" , answerYN, 3, "Constipation",3); 
		msas1018sec2[5] = new QuestionEntry ("Q29: " + questionBegin + " SWELLING OF ARMS or LEGS?" , answerYN, 3, "Swelling in arms of legs",5); 
		msas1018sec2[6] = new QuestionEntry ("Q30: " + questionBegin + " Having the thought - I DO NOT LOOK LIKE MYSELF?" , answerYN, 3); 
		msas1018sec2[7] = new QuestionEntry ("Q31: " + questionBegin + " CHANGES IN SKIN?" , answerYN, 3,"Changes in skin",5); 

		//

		for (int i=1; i<questionArr.Length; i++) {
			if(i<=msas1018sec1.Length*4){
				if(i%4==1){		
					questionArr[i]=msas1018sec1[i/4]; 
				}
				else{
					questionArr[i]=(i%4==0)?followUp[2]:followUp[i%4-2];
				}
			}else{
				int j=i-msas1018sec1.Length*4;
				if(j%3==1){
					questionArr[i]=msas1018sec2[j/3];
				}
				else{
					questionArr[i]=(j%3==0)?followUp[2]:followUp[1];
				}
			}
		}

	}// end of PopulateQuestionArr(); 

	void populateQuestionMSAS0809(){
//		string[] 

	}
	void populatePartBSec3(){
		String q = "Section 3: Select as many of of these words that describe your pain.";
		string[] pain0 = {"annoying", "bad", "horrible", "misserable", "terrible", "uncomfortable"};
		string[] pain1 = {"aching", "hurting", "like an ache", "like a hurt", "sore"};
		string[] pain2 = {"beating", "hitting", "pouding", "punching", "throbbing"};
		string[] pain3 = {"bitting", "cutting", "like a pin", "like a sharp knife", "pin like", "sharp", "stabbing"};
		
		apptsec3[0] = new QuestionEntry(q, pain0,0);
		apptsec3[1] = new QuestionEntry(q, pain1,0);
		apptsec3[2] = new QuestionEntry(q, pain2,0);
		apptsec3[3] = new QuestionEntry(q, pain3,0);

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
		for (int i=0,j=1; i<audioNum; j++) {
			if(i<msas1018sec1.Length*4){

				ac [i] = Resources.Load ("Audio/Survey/"+sex+"/PartA/1018/majorQuestion_" + j) as AudioClip;
				for (int k=1; k<=3; k++) {
					ac [i + k] = Resources.Load ("Audio/Survey/"+sex+"/PartA/1018/minorQuestion_" + k) as AudioClip; 
				}
				i+=4; 
			}
			else{
				ac[i] = Resources.Load("Audio/Survey/"+sex+"/PartA/1018/majorQuestion_" + j) as AudioClip;
				for(int k=1; k<=2; k++){
					ac[i+k] = Resources.Load ("Audio/Survey/"+sex+"/PartA/1018/minorQuestion_" + (k+1)) as AudioClip; 
				}
				i+=3; 
			}
		}
		return ac;
	}


}


