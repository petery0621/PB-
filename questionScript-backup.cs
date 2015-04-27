using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

//FIXME what's serializable mean, again? 
[System.Serializable]
public class QuestionEntry{
	String text; 
	String type; // FIXME considering change to Integer ("symptom", "followup")

	// constructors
	public QuestionEntry(){
	}
	public QuestionEntry(String text){
		this.text = text; 
	}

	public void setText(String text){
		// set text displayed in question box
		// e.g. "How severe was it usually?"
		this.text = text; 
	}
	
	public String getText (){
		return this.text;
	}

	public void setType(String type){
		// set type of text displayed in question box
		// e.g., "symptom", "followup", "filler"
		this.type = type; 
	}

}

public class questionScript : MonoBehaviour {
	
	// Direct-referenced Objects
	// Answer tools - obtain user input
	public GameObject yesNoButtons; 
	public GameObject howOftenButtons; 
	public GameObject howSevrButtons; 
	public GameObject howBotherButtons;
	public GameObject contButton; 
	public GameObject howStopButtons;
	public GameObject howTrblButtons; 
	public GameObject howFeelButtons;
	public GameObject howMuchTimeButtons;
	public GameObject sliderObj;
	public Slider slider;
	public Text slider_text1;
	public GameObject sliderButtonStart; 
	public GameObject sliderButtonEnd; 
	public GameObject sliderButtonMidRight; 
	public GameObject sliderButtonMidLeft; 
	public GameObject sliderButtonMid;
	public Text sliderButtonStartText; 
	public Text sliderButtonEndText;
	public Text sliderButtonMidText; 
	public Text sliderButtonMidLeftText; 
	public Text sliderButtonMidRightText; 
	public GameObject toggle1;
	public GameObject toggle2;
	public GameObject toggle3;
	public GameObject toggle4;
	public GameObject toggle5;
	public GameObject toggle6;
	public GameObject toggle7;
	public GameObject toggle8;
	public GameObject toggle9;
	public GameObject toggle10;
	public GameObject toggle11;
	public GameObject toggle12;
	public GameObject toggle13;
	public GameObject toggle14;
	public GameObject toggle15;
	public GameObject inputFieldObj;
	public InputField inputField;
	public GameObject toggleM1; 
	public GameObject toggleM2; 
	public GameObject panda; 
	// additional direct-referenced
	// public GameObject panda; 
	public GameObject canvasAppt; 
	public GameObject transparentBg;
	public GameObject menu; 
	public GameObject back_button; 
	
	// Indirect-referenced Objects
	Text thisText; // to display question text 
	Text sliderValue; // to display sliderbar value
	Animator anim;
	
	// Control GameObjects 
	GameObject prevBtn;
	GameObject[] toggle;
	GameObject prevTgl;
	
	// Control Variables
	int count;
	int winId; // not in use
	ArrayList al; 
	bool clickNo;
	// Question text arrays 
	string[] questionSymArray; //PART A symptoms
	string[] questionFollowArray; // PART B followups
	string[] apptQuestionArray;
	string[] piqTextArray1;
	string[] piqFollowArray1;
	string[] piqTextArray2;
	string[] piqFollowArray2;
	
	// Storing Variables
	string answers; 
	string response1; 
	string response1_input; 
	string response2; 
	string response2_words;
	string response2_input; 

	// Auxiliary Variables
	string slider_indicator; 
	// Auxiliary variables - part A 
	int numPartAQuestionIntro; 
	int numPartA1_sym ; 
	int numPartA1;
	int numPartA2_sym;
	int numPartA2;
	int numPartA; // 
	int numPartA_sym;
	int numberLimit; 
	string inputTest; 
	// Auxiliary Variables - part B
	int numPartB; 
	int numberPart_B2; 
	int numberPart_B3; 
	int numberPart_B; 
	int numberPart_AB; 
	int count_toggle;
	string[] apptArray; 
	string apptString; 
	bool painLvOn;
	// Auxiliary Variables - part C
	int numPartC2_sym;
	bool toggleMedOn; 
	bool toggleMed;

	//For class QuestionEntry
	static string questionEntry_type_sym; 
	static string questionEntry_type_flw; 

	
	
	// Called when the attached objected fisrt active
	// This Start () is "startDiary" FIXME in patientsceneController, don't call startDiary (), just set Diary_Canvas to be true
	void Start () {
		
		thisText = GameObject.Find (this.name).GetComponent<Text> ();// renamed the Text object so that there will be no two same names 
		al = new ArrayList ();
		anim = panda.GetComponent<Animator>(); 
		//		pandaAvatar = GameObject.Find ("Panda Placeholder"); 

		// Initialize parameters for Part A
		// FIXME these number should sync with array length automatically
		numPartAQuestionIntro = 1; // FIXME Last time stopped here
		numPartA1_sym = 23; 
		numPartA2_sym = 8; 
		numPartA_sym = numPartA1_sym + numPartA2_sym; 
		numPartA1 = numPartAQuestionIntro + numPartA1_sym * 4; //93
		numPartA2 = numPartAQuestionIntro + numPartA2_sym * 3 + 9; // 9: other symptoms 
		numPartA = numPartA1 + numPartAQuestionIntro + numPartA2_sym * 3; //108
		numberLimit = 300; // for dubugging
		
		// Initializing parameters for Part B 
		// (Move to the end of the Part A)
		numPartB = 9; // length of apptQuestionArray 
		//		numberPart_B2 = 4; 
		//		numberPart_B3 = 2;
		//		numberPart_B = numberPart_B1 + numberPart_B2 + numberPart_B3; 
		//		numberPart_AB = numberPart_A + numberPart_B;
		
		// For Part C
		numPartC2_sym = 8; 
		
		// Create needed arrays and lists
		populateQuestionSymArray (); 
		populateQuestionFollowArray (); 
		populateApptArray ();
		populatePiqArray1 ();
		populatePiqArray2 ();
		startDiary ();
		
		//===================debugging=====
		//		partA1 (); 
		//		startAppt ();

		part_a_fix();

		
	}
	public void debug (){
		count--; 
		count_toggle--;
	}
	
	// Update is called once per frame
	void Update () {
		if(toggleMed){
			inputFieldObj.SetActive(toggleMedOn);
		}
		if (sliderObj.activeSelf) {
//			sliderValue.text = slider.value.ToString();
			switch (slider_indicator){
			case "control":
				//set slider range
				slider.minValue = 0; 
				slider.maxValue = 6;
				if (slider.value >= 0 &&slider.value<2){
					slider_text1.text = "No control";
				}else if(slider.value>=2&&slider.value<4){
					slider_text1.text = "Some control";
				}else if(slider.value>=4&&slider.value<=6){
					slider_text1.text = "Complete control";
				}
				break; 
			case "pain": 
				//FIXME set slider range
				slider.minValue = 0; 
				slider.maxValue = 10;
				if (slider.value >= 0 &&slider.value<3){
					slider_text1.text = "Not Hurting";
				}else if(slider.value>=3&&slider.value<5){
					slider_text1.text = "Hurting a little";
				}else if(slider.value>=5&&slider.value<7){
					slider_text1.text = "Hurting a median";
				}else if(slider.value>=7&&slider.value<=10){
					slider_text1.text = "Hurting a lot";
				}
				break; 
			case "help": 
				//FIXME the same as the above
				slider.minValue = 0; 
				slider.maxValue = 10;
				if (slider.value >= 0 &&slider.value<5){
					slider_text1.text = "Did not help at all";
//				}else if(slider.value>=3&&slider.value<5){
//					slider_text1.text = "Hurting a littel";
//				}else if(slider.value>=5&&slider.value<7){
//					slider_text1.text = "Hurting a median";
				}else if(slider.value>=5&&slider.value<=10){
					slider_text1.text = "Help a lot";
				}
				break; 
			case "decrease": 
				//FIXME the same as the above
				slider.minValue = 0; 
				slider.maxValue = 6;
				if (slider.value >= 0 &&slider.value<2){
					slider_text1.text = "Can't decrease it at all";
				}else if(slider.value>=2&&slider.value<4){
					slider_text1.text = "Can decrease it somewhat";
				}else if(slider.value>=4&&slider.value<=6){
					slider_text1.text = "Can decrease it completely";
				}
				break; 
			}
		}
	}
	//=============================================================
	
	public void onClick (int value){// proceed to change the question text

		anim.SetTrigger("hitSpace"); 
		if (count%3==0)anim.SetTrigger("wave"); 
		if (count%3==1)anim.SetTrigger("shrug"); 
		if (count%3==2)anim.SetTrigger("tummyRub"); 

		// display the current question
		// winId switches "Parts"
		switch (winId) {
			
		case 0: // Part A, age>9

			// store data 
			if (value != -1){
				// yes/no, followup
				answers += value.ToString();
				 
			}else{ 
				// continue button: input field


			}
			storeData(winId, value);
			partAtext (numPartA1);
			partAbtn ();
			break;
		case 1: // Part A, age<=9
			//			partAtext (numPartA_kid);
			//			partAbtn (); //Possible change due to question change
			break;
		case 2: // 1018 2nd half (same structure w/ 1st half)
			storeData(winId, value);
			partAtext (numPartA2);
			partAbtn ();
			break;
		case 3: 
			partBtext ();
			partBbtn ();
			break;
		case 4:
			partCtext (); 
			partCbtn (); 
			break;
		default:
			Debug.Log ( "winId = sth else; run default in onClick switch"); 
			break;
		}
		
		
		
		//		Debug.Log ("answers: " + answers);
		count ++;
		
		
		if (!(count < numberLimit)) 
			thisText.text = "You have finished the diary! Hooray!";
	}
	
	public void onClickNo(int value) {
		clickNo = true;
		onClick (value); 
		clickNo = false;
	}
	
	
	public void setApptArray (string str){
		int idx =  Convert.ToInt32(str.Substring (0, str.IndexOf (":")));
		string update = str.Substring (str.IndexOf (":") + 1); 
		apptArray [idx] = update; 
		//		Debug.Log (idx.ToString()+ ":" + apptArray [idx]);
		//		for(int i =0; i<apptArray.Length;i++)
		//			Debug.Log (apptArray[i]);
		
	}
	//======================================================================
	
	void storeData(int winId, int value) {
		// Store clicked response into pre-defined strings
		/* Input: 
		 * value: the value tied with the button
		 * 
		 * Output: processed string 
		 */
		
		switch (winId) {
		case 0:
			// you click "No" in symptom question, add "*" for the followups in partXtext()
			
			if(value != -1){
				answers += value.ToString();
				
			}
			break; 
		case 1:
			Debug.Log("0809 not finished"); 
			break; 
		case 2 :
			if (value != -1) {
				if (inputFieldObj.activeSelf){
					answers += (clickNo || inputField.text != "" )? 
						inputField.text+",": "*,";
					
				}
				else{
					answers += value.ToString();
				}			}
			break; 
		default: 
			Debug.Log ("to be completed");
			break;
			
		}
		
		
	}
	
	
	//======================================================================
	// "static functions"
	//====================================================================== 
	void startDiary (){
		// check last diary finished or not
		
		
		// initialize parameters: date, time, user info
		count = 0; 
		winId = 0;
		clickNo = false; 
		//		winId = (uesrAge > 9) ? 0 : 1;
		
		contButton.SetActive (true);
		prevBtn = contButton;
		
		menu.SetActive (false);
		yesNoButtons.SetActive (false);
		howBotherButtons.SetActive (false);
		howSevrButtons.SetActive (false);
		howOftenButtons.SetActive (false);
		canvasAppt.SetActive (false);
		
		apptArray = new string[43]; 
		
		toggleMedOn = false; 
		toggleMed = false;
		sliderObj.SetActive (true);
//		sliderValue = sliderObj.GetComponentInChildren<Text> ();
//		sliderValue.text = "0"; 
		sliderObj.SetActive (false);
		
		// start to LOAD and show question text 
		// 10-18 or 08-09
		switch (winId){
		case 0: 
			partA ();
			break;
		case 1:
			//partA_kid (); 
			break;
		default:
			partA (); 
			Debug.Log ("This is the default in startDiary");
			break;
		}
		thisText.text = al[count].ToString ();
		count ++;
		
	}
	
	
	void endDiary (){
		
	}
	//---------------------------------------------------------------------------
	//partA	
	//---------------------------------------------------------------------------
	// new partA for kid 
	// not in use FIXME FIXME FIXME: NEXT TIME YOU START HERE!
	void partA (){//first half of part A 
		/* Part A has three blocks ;
		 * this is the first one
		 */
		
		
		// Building structure for Part A 10-18 section 1 
		
		
		string partA1Intro = "We have 23 questions";
		
		al.Clear (); 
		al.Add (partA1Intro);
		// 3 questions for one sym question 
		for (int i=0; i<numPartA1_sym; i++) {
			al.Add(questionSymArray[i]);
			for(int j=0 ;j<questionFollowArray.Length;j++){
				al.Add (questionFollowArray [j]);
			}
		}	
		// debugging 
		//		for (int i=0;i<al.Count;i++) {
		//			Debug.Log (i.ToString() + al[i].ToString());
		//		}
	}
	// new partA
	// not in use, yet
	QuestionEntry[] part_a_fix(){
		// FIXME rename it 

		// First letter capitalized, when typed in. 
		// All letters capitalized for SYMPTOMS, when displayed.

		string intro1 = "We have 23 symptoms to ask you about in this section. Pay attention to each one carefully. " +
						"If you have had the symptom since your LAST DIARY ENTRY, select YES." +
						"\n"+"If YES, let us know how OFTEN you had it, " +
						"how SEVERE it was usually and how much it BOTHERED OR DISTRESSED by selecting the appropriate answer. " +
						"\n"+"If you did not have the symptom, select NO.";
		string intro2 = "We have 8 symptoms to ask you about in this section. " +
						"Pay attention to each one carefully. " +
						"If you have had the symptom since your LAST DIARY ENTRY, " +
						"let us know how SEVERE it was usually " +
						"and how much it BOTHERED OR DISTRESSED you by selecting the appropriate answer." +
						"\n" + "If you did not have the symptom, select NO."; 
		string typeIn = "Please type in your Symptom.";
		string other1 = "Did you have any other symptom since your last diary entry?";//FIXME: notify Tianrui for possble change for PostParameters
		string other2 = "Did you have a second other symptom since your last diary entry?";
		string other3 = "Did you have a third other symptom since your last diary entry?";
		string[] other = new string[]{other1, other2, other3};

		// head repeats itself every 3 symptom questions 
		string header1 = "Since your last diary entry, did you have any: ";
		string header2 = "Since your last diary entry, did you have "; 
		string subHeader = "Have the thought:\n";
		// symptoms shoud be upper case
		string conc = "Difficulty concentrating or paying attention";
		string pain = "Pain"; 
		string ener = "Lack of energy"; 
		string cougt = "Cough";
		string nerv = "Feeling of being nervous";
		string mott = "Dry mouth"; 
		string naust = "Nausea or feeling like you could vomit"; 
		string drowt = "A feeling of being drowsy";
		string numbt = "Any numbness,tingling, or pins and needles feeling in hands or feet";
		string slept = "Difficulty sleeping"; 
		string urint = "Problems with urination or 'peeing'"; 
		string vomit = "Vomiting or throwing up"; 
		string breat = "Shortness of breath"; 
		string diart = "Diarrhea or loose bowel movement"; 
		string sadt = "Feelings of sadness";
		string sweat = "Sweats"; 
		string worrt = "Worrying"; 
		string itcht = "Itching"; 
		string appt = "Lack of appetite or not wanting to eat"; 
		string dizzt = "Dizziness";
		string swalt = "Difficulty swallowing";
		string irrit = "Feelings of being irritable";
		string headt = "Headache"; 
		string msort = "Mouth sores"; 
		string foodt = "Change in the way food tastes"; 
		string weitt = "Weight Loss"; 
		string hairt = "Less hair then usual"; 
		string constp = "Constipation or were you uncomfortable bacause bowel movements are less often";
		string swelt = "Swelling of arms or legs"; 
		string lookt = "-\"I Don't look like myself\""; 
		string skint = "Changes in skin"; 

		QuestionEntry[] qe = new QuestionEntry[questionSymArray.Length];// FIXME make questionSymArray local

		questionSymArray = new string[]{conc,pain,ener,cougt,nerv,mott,naust,drowt,numbt,slept,
			urint,vomit,breat,diart,sadt,sweat,worrt,itcht,appt,dizzt,
			swalt,irrit,headt,
			msort,foodt,weitt,hairt,constp,swelt,lookt,skint};
		// Format symptom questions
		for (int i=0; i<questionSymArray.Length;i++){
			String str = questionSymArray[i]; 
			// to make upper cases
			char[] delim = new char[]{' '}; 
			if (!str.Contains ("or")) {
				str = str.ToUpper (); 
			}else {
				string[] strArr = str.Split(delim); 
				for (int k=0; k<strArr.Length; k++){
					if (!strArr[k].Equals ("or")){
						strArr[k] = strArr[k].ToUpper (); 
					}
				}
				str = String.Join (" ", strArr); 
			}
			questionSymArray[i] = str; 
			// to do with the header
			if ((i<23&&i%3 == 0)||(i>=23&&i%3==2)){
				if (questionSymArray[i].ToLower ().Contains ("don't look like myself")){
					// "Since..do you have" + "Have the thought" + "..look like.."
					questionSymArray[i] = header2 + subHeader + questionSymArray[i]+"?"; 
				}
				questionSymArray[i] = header1 + questionSymArray[i]+"?"; 
			}
			questionSymArray[i] += "?";
			// to create QuestionEntry
			qe[i].setText (questionSymArray[i]);

		}
//		for (int i=0;i<questionSymArray.Length;i++){
//			if ((i<23&&i%3 == 0)||(i>=23&&i%3==2)){
//				if (questionSymArray[i].ToLower ().Contains ("don't look like myself")){
//					// "Since..do you have" + "Have the thought" + "..look like.."
//					questionSymArray[i] = header2 + subHeader + questionSymArray[i]+"?"; 
//				}
//				questionSymArray[i] = header1 + questionSymArray[i]+"?"; 
//			}
//			questionSymArray[i] += "?";
//		}


		// for testing purposes
//		for (int i=0;i<questionSymArray.Length;i++){
//			Debug.Log (qe[i].getText());

//		}
	}

	QuestionEntry[] part_a_fix2(){
		// FIXME rename it 
		// NO rules to follow, have to hard code
		string other1 = "Did you have anything else that made you feel bad or sick since your last diary entry?";
		string other2 = "Did you have any other thing that made you feel bad or sick since your last diary entry?";
		string typeIn = "Please type in your symptom: "; 
		string howMuch = "How much did this bother or trouble you?"; 

		string tired7 = "Since your last diary entry, did you feel more tired than you usually do?"; 
		string tiredt7 = "How long did it last?"; 
		string tiredf7 = "How tired did you feel?"; 
		string tiredb7 = "How much did being tired bother you or trouble you?";

		string sad7 = "Did you feel sad since your last diary entry?";
		string sadt7 = "How long did you feel sad?";
		string sadf7 = "How sad did you feel?"; 
		string sadb7 = "How much did feeling sad bother or trouble you?"; 

		string itchy7 = "Were you itchy since your last diary entry?"; 
		string itchyt7 = "How much of the time were you itchy?";
		string itchyf7 = "How itchy were you?";
		string itchyb7 = "How much did being itchy bother you or trouble you?";
	
		string pain7 = "Did you have pain since your last diary entry?";
		string paint7 = "How much of the time did you have pain?";
		string painf7 = "How much pain did you feel?";
		string painb7 = "How much did the pain bother you or trouble you?";

		string worry7 = "Did you feel worried since your last diary entry?";
		string worryt7 = "How much of the time did you feel worried?";
		string worryf7 = "How worried did you feel?"; 
		string worryb7 = "How much did feeling worried bother you or trouble you?";

		string eat7 = "Since your last diary, did you feel like eating as you normally do?";
		string eatt7 = "How long did this last?";
		string eatf7 = howMuch;

		string vomit7 = "Did you feel like you were going to vomit (or going to throw up) since your last diary entry?";
		string vomitt7 = "How much of the time did you feel like you could vomit (or could throw up)?";
		string vomitb7 = "How much did this feeling bother or trouble you?";

		string sleep7 = "Did you have trouble going to sleep since your last diary entry?";
		string sleepb7 = "How much did not being able to sleep bother or trouble you?";

		String[] rstArr = new String[]{
						tired7, tiredt7, tiredf7, tiredb7, 
						sad7, sadt7, sadf7, sadb7,
						itchy7, itchyt7, itchyf7, itchyb7,
						pain7, paint7, painf7, painb7, 
						worry7, worryt7, worryf7, worryb7, 
						eat7, eatf7, eatt7, 
						vomit7, vomitt7, vomitb7, 
						sleep7, sleepb7, 
						other1, typeIn, howMuch, 
						other2, typeIn, howMuch
						};

		QuestionEntry[] rst = new QuestionEntry[rstArr.Length];
		// (hard-code) popul
		for (int i=0; i<rst.Length;i++){
			rst[i] = new QuestionEntry(rstArr[i]);

			// Setting question types
				// #1-5 symptom questions each have 3 followups
			if( ( (i<20) && (i%4==0) ) || 
				// #6-7 symptom questions each have 2 followup
			    ( (i<26) && ((i-20)%3==0) ) ||
				// #8 has 1 followup
		 	    ( (i<28) && ((i-26)%2==0) ) || 
				// "other" each have 2 followup
		 	    ( (i<34) && ((i-28)%3==0) ) ){
			
				rst[i].setType (questionEntry_type_sym); 

			}else {
				rst[i].setType (questionEntry_type_flw);
			}
		}

		return rst;
	}
	void partA1() {
		// Build question structure for Part A 10-18 section 2
		string other1 = "Did you have any other symptom since your last diary entry?";//FIXME: notify Tianrui for possble change for PostParameters
		string other2 = "Did you have a second other symptom since your last diary entry?";
		string other3 = "Did you have a third other symptom since your last diary entry?";
		string intro = "We have 8 more questions."; 
		string typeIn = "Please type in your Symptom.";
		string howBother = "How much did this bother you or distress you?"; 
		string[] other = new string[]{other1, other2, other3}; 
		
		al.Clear(); 
		al.Add (intro); 
		for (int i=numPartA1_sym; i<numPartA_sym; i++) {
			al.Add(questionSymArray[i]);
			for(int j=1; j<questionFollowArray.Length; j++) {
				al.Add (questionFollowArray [j]); 
			}				
		}
		for(int i=0; i<other.Length;i++){
			al.Add (other[i]); 
			al.Add (typeIn) ;
			al.Add (howBother); 
		}

		//		debugging 
		//		for (int i=0;i<al.Count;i++) 
		//		 Debug.Log (i.ToString()+al[i].ToString());
	}
	
	// change answer button
	void partAbtn(){//count -> buttons
		/* Since the general pattern for button pattern transtion 
		 * stays the same for Part A, this part is for 
		 * count: the current position in the ArrayList al
		 * 
		 * And, add separators in the response string
		 */ 
		
		// Present answer choices according to the current question 
		string currText = al [count].ToString();
		//		Debug.Log ("(btn)current text: " +currText);
		
		// input field 
		if (
		    currText.Contains ("type in your ")){
			if (prevBtn != contButton ){
				prevBtn.SetActive (false); 
				contButton.SetActive (true); 
				prevBtn = contButton ; 
			}
			if (!inputFieldObj.activeSelf)
				inputFieldObj.SetActive (true); 
			answers += "/"; 
		}
		else {
			if (inputFieldObj.activeSelf)
				inputFieldObj.SetActive (false); 
			
			// introduction -> continue
			if (currText.Contains ("We have ")){
				prevBtn.SetActive (false);
				if(!contButton.activeSelf)
					contButton.SetActive (true);
				prevBtn = contButton ;
			}
			// "Since the last diary entry.." -> yes/no
			else if (currText.Contains ("Since your last diary entry, did you have")||
			         currText.Contains("other symptom")){
				prevBtn.SetActive (false);
				yesNoButtons.SetActive (true); 
				prevBtn = yesNoButtons; 
				
				answers += ","; 
			}
			// "how often.." 
			else if (currText.Contains ("often")){
				//FIXME delete the below after testing
//				thisText.alignment = TextAnchor.MiddleCenter;

				prevBtn.SetActive (false);
				howOftenButtons.SetActive (true); 
				prevBtn = howOftenButtons;	
			}
			// "how severe.." 
			else if (currText.Contains ("severe")){
				prevBtn.SetActive (false);
				howSevrButtons.SetActive (true);
				prevBtn = howSevrButtons;
			}
			// "how did it bother or distress.."
			else if (currText.Contains ("bother") || currText.Contains ("distress")){
				prevBtn.SetActive (false);
				howBotherButtons.SetActive (true);
				prevBtn = howBotherButtons;
			}
			
			else {
				prevBtn.SetActive (false); 
				contButton.SetActive (true); 
				prevBtn = contButton ;
				
			}	
		}
	}
	// change question text
	void partAtext(int length){
		// display part A text 
		// transition to part B
		
		if(count <length ){
			if(!clickNo){
				thisText.text = al [count].ToString();	// NOTE: present then count++
			}
			else{
				while ((count<length) && !al[count].ToString().Contains("diary")){
					// In the "if(count<XXX)", after "count++" 
					// you need to decide again
					if (al[count].ToString().Contains("type in ")){
						// THE SAME CODE
						if(winId == 0){
							winId = 2;//
							partA1 ();
							count = 0;
						}
						else if(winId == 2){
							winId = 3; 
							partB (); 
							count = 0; 
							count_toggle = 0; 
							painLvOn = false;
							prevTgl = toggle[0];
						}
						break;
					}
					count++;
					// fill answer if click No
					answers += "*"; 
				}
				
				if (!(count<length)) {
					// THE SAME CODE
					if(winId == 0){
						winId = 2;//
						partA1 ();
						count = 0;
					}
					else if(winId == 2){
						winId = 3; 
						partB (); 
						count = 0; 
						count_toggle = 0; 
						painLvOn = false;
						prevTgl = toggle[0];
					}
				}
				
				thisText.text = al [count].ToString();
			}
		}
		else {
			// THE SAME CODE
			if(winId == 0){
				winId = 2;//
				partA1 ();
				count = 0;
				thisText.text = al[count].ToString();
			}
			else if(winId == 2){
				winId = 3; 
				partB (); 
				count = 0; 
				thisText.text = al[count].ToString();
				count_toggle = 0; 
				painLvOn = false;
				prevTgl = toggle[0];
				answers += "/"; 
			}
		}
		
	}
	//---------------------------------------------------------------------------
	//partB
	//---------------------------------------------------------------------------
	void partB (){
		// Loading text for Part B
		al.Clear (); 
		for (int i=0; i<apptQuestionArray.Length; i++) {
			al.Add(apptQuestionArray[i]);}	


		// debugging 
		//for (int i=0;i<al.Count;i++) {
		//Debug.Log (i.ToString() + al[i].ToString());
		//}
	}
	
	void partBbtn (){
		
		// Present answer choices according to the current question 
		string currText = al [count].ToString();
		//		Debug.Log ("current text: " +currText);
		
		// slider bar
		if ((currText.Contains ("did you")|| currText.Contains ("do you")|| 
		    currText.Contains ("was your"))&& 
			!currText.Contains ("one.")){
			slider_indicator = "pain"; 
			if (!sliderObj.activeSelf)
				sliderObj.SetActive (true); 
			slider.value = 0; 
			sliderButtonMid.SetActive(true); 
			sliderButtonMidLeft.SetActive(true); 
			sliderButtonMidRight.SetActive(true); 
			sliderButtonEndText.text = "Hurting a whole lot";
			sliderButtonMidText.text = "Hurting a median amount";
			sliderButtonMidLeftText.text = "Hurting a little";
			sliderButtonMidRightText.text = "Hurting a lot";
			sliderButtonStartText.text = "Not hurting"; 
		}else {
			if (sliderObj.activeSelf) 
				sliderObj.SetActive (false); 
		}
		
		// toggle
		if (currText.Contains("words:")){
			prevTgl.SetActive (false); 
			//			Debug.Log ("count_toggle: "+ count_toggle);
			toggle[count_toggle].SetActive (true);
			prevTgl = toggle[count_toggle];
			
			count_toggle ++; 
		}else {
			if(prevTgl.activeSelf)
				prevTgl.SetActive( false);
		}
		
		// input field
		if (currText.Contains ("Other word")&&currText.Contains(":")){
			inputFieldObj.SetActive (true); 
			inputField.text = "Please type in.";
			
		}else {
			if (inputFieldObj.activeSelf)
				inputFieldObj.SetActive (false);
		}
		
		// proceed
		if (currText.Contains ("other words")) {
			
			prevBtn.SetActive (false); 
			yesNoButtons.SetActive (true); 
			prevBtn = yesNoButtons;
		}else if(currText.Contains("How much did you")){
			prevBtn.SetActive (false); 
			howStopButtons.SetActive (true); 
			prevBtn = howStopButtons;
		}else if (prevBtn != contButton){
			prevBtn.SetActive (false);
			contButton.SetActive (true);
			prevBtn = contButton ;
		}
	}
	
	void partBtext (){
		// display the text 
		// responsible for part-transition if any
		if (count < apptQuestionArray.Length && !clickNo){
			if(count == 1){ //appt starts
				canvasAppt.SetActive (true);
			}else {
				if(	canvasAppt.activeSelf)
					canvasAppt.SetActive (false);
			}
			
			if (count == 2) {// store apptString
				for (int i=0;i<apptArray.Length;i++){
					apptString += (apptArray[i] == null || apptArray[i] == "")? "0": apptArray[i]; 
				}
				
				answers += "/" + apptString +"/";
			} else if(count>3 ||count<8) {
				answers += slider.value.ToString(); 
			} 
			
			
		}
		else if (clickNo){
			while (count!= apptQuestionArray.Length-5)
				count++; 
		}else{
			// the transition code 
			inputFieldObj.SetActive (false); 
			if (prevBtn!=contButton)
				prevBtn.SetActive(false); 
			if(!contButton.activeSelf)
				contButton.SetActive (true); 
			winId = 4; 
			count = 0; 
			partC (); 
			
		}
		thisText.text = al [count].ToString(); 

		
	}
	
	//---------------------------------------------------------------------------
	//partC
	//---------------------------------------------------------------------------
	void partC (){
		string intro = "Now I'm going to ask you if you've tried anything to help decrease your pain.";//continue
		
		al.Clear (); 
		al.Add(intro);
		for (int i=0; i<piqTextArray1.Length; i++) {
			al.Add(piqTextArray1[i]);
			for(int j=0;j<piqFollowArray1.Length;j++){
				al.Add (piqFollowArray1[j]); 
			}
		}
		string intro2 = "Now I'm going to ask you if you've tried any activity to help decrease your pain.";//continue
		string ctrl = "Based on all the things you did to cope, or deal with your pain TODAY, how much control did you feel you had over it? Please select the appropriate number. " +
			"Remember, you can select any number along the scale. " ;
				//+"\n" + "(0: No control, 3: Some control, 6: Complete control)";
		string dec = "Based on all the things you did to cope, or deal with your pain TODAY, how much were you able to decrease it? Please select the appropriate number. " +
			"Remember, you can select any number along the scale.";
				//+"\n" ;+ "(0: Can't decrease it at all, 3: Can't decrease it somewhat, 6: Can decrease it completely)";	
		string name = "Please type in the name of this actiivty"; 
		string other = "Any OTHER activity?";
		
		al.Add(intro2);
		for (int i=0; i<numPartC2_sym;i++){
			al.Add (piqTextArray2[i]);
			for(int j=0; j<piqFollowArray2.Length;j++)
				al.Add (piqFollowArray2[j]); 
		}
		al.Add (other);
		al.Add (name); 
		for(int j=0; j<piqFollowArray2.Length;j++)
			al.Add (piqFollowArray2[j]); 
		al.Add (ctrl); 
		al.Add (dec); 
		//		//debugging 
		//		for(int i=0; i<al.Count;i++)
		//			Debug.Log(i.ToString()+al[i].ToString());
	}
	public void setToggleMed (){
		Debug.Log (toggleMedOn);
		toggleMedOn = !toggleMedOn; 
	}
	
	void partCbtn (){
		if (count == al.Count || count > al.Count) {
			prevBtn.SetActive (false); 
			if (sliderObj.activeSelf) sliderObj.SetActive (false); 
			endDiary();
			return;
		}
		// Present answer choices according to the current question 
		string currText = al [count].ToString();
		//		Debug.Log ("current text: " +currText);
		
		// toggle
		if (currText.Contains("Name of medication")){
			if (!currText.Contains("cont.")){
				if(!toggleM1.activeSelf){
					toggleM1.SetActive(true);
				}
				thisText.alignment = TextAnchor.UpperLeft;
			}else{ 
				if (toggleM1.activeSelf){
					toggleM1.SetActive(false);
				}
				if(!toggleM2.activeSelf){
					toggleM2.SetActive(true);
				}
			}
			toggleMed = true;
			prevBtn.SetActive (false); 
			contButton.SetActive(true);
		}else {
			toggleMed = false; 
			toggleM2.SetActive(false);
			thisText.alignment = TextAnchor.MiddleCenter;
		}
		
		// have you taken/tried..
		if (currText.Contains("Have you")||currText.Contains("have you")||
		    currText.Contains("OTHER activity")){
			prevBtn.SetActive (false);
			yesNoButtons.SetActive (true); 
			prevBtn = yesNoButtons;
			
		}
		// input field: name or times -> tpye in 
		if ((currText.Contains("(cont.)")&&toggleMedOn)||currText.Contains("name")
		    ||currText.Contains("times")){
			if (prevBtn!=contButton){
				prevBtn.SetActive (false); 
				contButton.SetActive (true) ;
				prevBtn = contButton; 
			}
			if (sliderObj.activeSelf){
				sliderObj.SetActive (false); 
			}
			if (!inputFieldObj.activeSelf){
				inputFieldObj.SetActive (true); 
			}
			inputField.text = "Please type in.";
			
		}
		// slider bar
		if (currText.Contains("help") || currText.Contains("how much control")
		    || currText.Contains("decrease it")){
			
			if (prevBtn!=contButton){
				prevBtn.SetActive (false); 
				contButton.SetActive (true) ;
				prevBtn = contButton; 
			}
			if (inputFieldObj.activeSelf){
				inputFieldObj.SetActive (false);
			}
			if (currText.Contains("help")){
				slider_indicator = "help"; 
				if(sliderButtonMid.activeSelf)
					sliderButtonMid.SetActive(false); 
				if(sliderButtonMidLeft.activeSelf)
					sliderButtonMidLeft.SetActive(false); 
				if(sliderButtonMidRight.activeSelf)
					sliderButtonMidRight.SetActive(false); 
				sliderButtonStartText.text = "Did not help at all";
				sliderButtonEndText.text = "Helped a lot";
			}
			else if (currText.Contains("control")){
//				slider.maxValue = 6; //this is already set in update()
				slider_indicator = "control"; 
				if(!sliderButtonMid.activeSelf)
					sliderButtonMid.SetActive(true); 
				if(sliderButtonMidLeft.activeSelf)
					sliderButtonMidLeft.SetActive(false); 
				if(sliderButtonMidRight.activeSelf)
					sliderButtonMidRight.SetActive(false); 
				sliderButtonStartText.text = "No control";
				sliderButtonMidText.text = "Some control"; 
				sliderButtonEndText.text = "Complete control";
			}else if (currText.Contains("decrease")){
				slider_indicator = "decrease"; 
				if(!sliderButtonMid.activeSelf)
					sliderButtonMid.SetActive(true); 
				if(sliderButtonMidLeft.activeSelf)
					sliderButtonMidLeft.SetActive(false); 
				if(sliderButtonMidRight.activeSelf)
					sliderButtonMidRight.SetActive(false); 
				sliderButtonStartText.text = "Can't decrease it at all";
				sliderButtonMidText.text = "Can decrease it somewhat";
				sliderButtonEndText.text = "Can decrease it completely"; 
			}
			
			if (!sliderObj.activeSelf)
				sliderObj.SetActive (true); 
			slider.value = 0; 
		}else {
			if (sliderObj.activeSelf)
				sliderObj.SetActive(false); 
		}
	}
	
	void partCtext(){
		if (count<al.Count){
			if (!clickNo){
				thisText.text = al [count].ToString (); 
			}else {
				if (al[count].ToString().Contains("Have you")){
					while (!al[count].ToString().Contains(" any activity")){
						count++; 
					}
					thisText.text = al[count].ToString();
				}else{
					while (!al[count].ToString().Contains("have you")&&
					       !al[count].ToString().Contains("Based on")&&
					       !al[count].ToString().Contains("OTHER activity")){
						count ++; 
					}
					thisText.text = al[count].ToString(); 
				}
			}
		}
		else{
			//do nothing for now 
			thisText.text = "You've finished the diary!";
			back_button.SetActive(true);
		}
	}
	
	
	//---------------------------------------------------------------------------
	
	//-------------------------------------------------------------------
	void populateQuestionFollowArray (){
		string howOften = "How often did you have it?";  
		string howSevere = "How severe was it usually?";
		string howBother = "How much did it bother or distress you?"; 
		
		questionFollowArray = new string[]{howOften, howSevere, howBother};
		return;
	}
	
	void populateQuestionSymArray (){
		// head repeats itself every 3 symptom questions 
		string header = "Since your last diary entry, did you have any: ";
		// symptoms shoud be upper case
		string conc = "Difficulty concentrating or paying attention";
		string pain = "Pain"; 
		string ener = "Lack of energy"; 
		string cougt = "Cough";
		string nerv = "Feeling of being nervous";
		string mott = "Dry mouth"; 
		string naust = "Nausea or feeling like you could vomit"; 
		string drowt = "A feeling of being drowsy";
		string numbt = "Any numbness,tingling, or pins and needles feeling in hands or feet";
		string slept = "Difficulty sleeping"; 
		string urint = "Problems with urination or 'peeing'"; 
		string vomit = "Vomiting or throwing up"; 
		string breat = "Shortness of breath"; 
		string diart = "Diarrhea or loose bowel movement"; 
		string sadt = "Feelings of sadness";
		string sweat = "Sweats"; 
		string worrt = "Worrying"; 
		string itcht = "Itching"; 
		string appt = "Lack of appetite of not wanting to eat"; 
		string dizzt = "Dizziness";
		string swalt = "Difficulty swallowing";
		string irrit = "Feelings of being irritable";
		string headt = "Headache"; 
		string msort = "Mouth sores"; 
		string foodt = "Change in the way food tastes"; 
		string weitt = "Weight Loss"; 
		string hairt = "Less hair then usual"; 
		string constp = "Constipation or were you uncomfortable because bowel movements are less often";
		string swelt = "Swelling of arms or legs"; 
		string lookt = "Have the thought:" +
						"\n"+"-\"I Don't look like myself\""; 
		string skint = "Changes in skin"; 
		
		questionSymArray = new string[]{conc,pain,ener,cougt,nerv,mott,naust,drowt,numbt,slept,
			urint,vomit,breat,diart,sadt,sweat,worrt,itcht,appt,dizzt,
			swalt,irrit,headt,
			msort,foodt,weitt,hairt,constp,swelt,lookt,skint};
		for(int i=0;i<questionSymArray.Length;i++){
			questionSymArray[i]=header + " \n" + questionSymArray[i] + "?";
		}
		//debugging
		//	for(int i=0;i<questionSymArray.Length;i++){
		//		Debug.Log ("Syptoms: ");
		return; 
	}
	
	void populateQuestionKid(){
		//		string intro = "We want to find out how you have been feeling since your last diary entry." +
		//			"Tap on the screen to select your answers";
		//		string a = "a"; 
		//		string b = "b";


	}
	
	void populateApptArray (){
		string intro = "Now I need you to describe your pain.";
		string sec1Intro = "\n" +"Section 1: Select the areas on these drawings to show where you have pain. ";
		string sec2Intro = "\n" +"Section 2: For the following questions, move the slider to select your answer.";
		string sec3Intro = "Select as many of these words that describe your pain.";
		string other = "Would you like to add other words describe your pain?";
		string wgr = "How much pain do you have right NOW?"; 
		string wgra = "How much pain did you have, ON AVERAGE, since your last diary entry?"; 
		string wgrw = "What was your WORST pain since your last diary entry?"; 
		string wgrl = "What was your LEAST pain since your last diary entry?";	
		
		string note = "\n"+" 0: Not Hurting, 3: Huring A little, 5: Hurtring a Median amount, " +
			"7: Hurting a lot, 10: Hurting a whole lot";
		
		string t =  "Select as many of these words: " +"\n";
		//		string p0 = t +"annoying, bad, horrible, miserable, terrible, uncomfortable";
		//		//senseory
		//		string p1 = t +"aching,hurting,like an ache,like a hurt,sore"  ;
		//		string p2 = t +"beating, hitting , pouding, punching, throbbing";  
		//		string p3  = t +"biting,cutting,like a pin,like a sharp knife,pin like,sharp,stabbing"  ;
		//		string p4  = t +"blistering,burning,hot";
		//		string p5  = t +"cramping,crushing,like a pinch,pinching,pressure"  ;
		//		string p6  = t +"Itching,like a scratch,like a sting,scratching,stinging";
		//		string p7  = t +"shocking,shooting,splitting"  ;
		//		string p8  = t +"numb,stiff,swollen,tight"  ;
		//		//affective
		//		string p9  = t +"awful,deadly,dying,killing"  ;
		//		string p10  = t +"crying,frightening,screaming,terrifying"  ;
		//		string p11  = t +"dizzy,sickeing,suffocating"  ;
		//		//evalucative
		//		string p12  = t +"never goes away,uncontrollable"  ;
		//		//temporal
		//		string p13  = t +"always,comes and goes,comes on all of a sudden,constant,sontinuous,forever  ";
		//		string p14  = t +"off and on,once in a while,sneaks up,sometimes,steady"  ;
		
		string ow1 = "Other word 1:";
		string ow2 = "Other word 2:";
		string ow3 = "Other word 3:";
		// section 4 (never mentioned before)
		string add1 = "How much did you stop doing school activities – like going to school or doing homework - today because of your pain? Select one.";
		string add2 = "How much did you stop doing your social activities - like spending time with friends - today because of your pain? Select one.";
		string add3 = "How much did you stop doing your extracurricular (like boy/girl scouts, art or music classes, band) and/or sports activities today because of your pain? Select one.";
		string add4 = "How much did you stop doing chores today because of your pain? Select one.";
		string add5 = "​How much did your pain mess up or get in the way of your sleep last night? Select one."; 
//		string[] newAdded = new string[] {;

		
		apptQuestionArray = new string[]{intro, sec1Intro, 
//			sec2Intro, wgr+note, wgra+note, wgrw+note, wgrl+note,
			sec2Intro, wgr, wgra, wgrw, wgrl,
			//			sec3Intro, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13 ,p14,
			sec3Intro,t, t, t, t, t, t, t, t, t, t, t, t, t, t, t, 
			other, ow1, ow2, ow3, 
			add1, add2, add3, add4, add5};		

		toggle = new GameObject[]{	toggle1,toggle2,toggle3,toggle4,toggle5,
			toggle6,toggle7,toggle8,toggle9,toggle10,
			toggle11,toggle12,toggle13,toggle14,toggle15};


	}
	
	void populatePiqArray1 (){// text: intro + question 
		string header = "Have you taken ";
		string trailer = "since the last entry?";
		string first = "any medications "; 
		string second = "a second medication ";
		string third = "a third medication ";
		
		string name = "Name of medication?";
		string times = "How many times was this medication taken since the last diary?";
		string help = "How much did this medication help? " +
			"\n" + "(Move the slider bar to show how much the medicine helped, from 'Did not help at all' to ' Helped a lot')";
		
		string q1 = header + first + trailer;
		string q2 = header + second + trailer;
		string q3 = header + third + trailer;
		
		
		piqTextArray1 = new string[] {q1,q2,q3};
		piqFollowArray1 = new string[]{name, name+"(cont.)", times, help};
		
	}
	
	void populatePiqArray2 () {
		string header = "Since your last diary entry, have you tried: ";       
		
		string times = "How many times was this activity done since the last entry.";
		string help = "How much did this activity help? " +
			"\n" + "(Move the slider bar to show how much the medicine helped, from 'Did not help at all' to ' Helped a lot')";
		
		//		string ctrl = "Based on all the things you did to cope, deal with your pain TODAY how much control did you fell you had over it?" +
		//			"Please select the appropriate number. Remember, you can select any number along the scale.";
		//		string dec = "Based on all the things you did to cope, deal with your pain TODAY how much were you able to decrease it?" +
		//			"Please select the appropriate number. Remember, you can select any number along the scale.";	
		
		string hp = "Heat Packs"; 
		string db = "Deep Breathing"; 
		string re = "A Relaxation Exercise"; 
		string t = "Thinking about your pain in a positive way " +
			"(for example, thinking that the pain means that my treatment is working)";
		string m = "Massage"; 
		string i = "ImIMAGERY? Or using your mind to imagine pictures, sights, sounds to relax?"; 
		string d = "DISTRACTION? Like watching TV, playing video games, or doing something to take your mind off of the pain?"; 
		string twf = "Talking with friends/parents"; 
		//		string other = "Any other activities"; 
		
		piqTextArray2 = new string[] { 
			header + db + "?", 
			header + re + "?", 
			header + t + "?", 
			header + hp + "?", 
			header + m + "?", 
			header + i + "?", 
			header + d + "?", 
			header + twf + "?"} ;
		piqFollowArray2 = new string[]{times, help};
	}

	
	
}



