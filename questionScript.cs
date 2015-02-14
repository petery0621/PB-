using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class questionScript : MonoBehaviour {
	
	Text thisText; // questionText object 

	// GameObject, drag and drop 
	public GameObject yesNoButtons; 
	public GameObject howOftenButtons; 
	public GameObject howSevrButtons; 
	public GameObject howBotherButtons;
	public GameObject contButton; 
	public GameObject[] answerButtons;

	public GameObject canvasDiary; 
	public GameObject canvasAppt; 
	public GameObject apptCont;
	public GameObject transparentBg;
	
	public GameObject b1; 
	public GameObject b2; 
	public GameObject b3; 
	public GameObject b4; 
	public GameObject b5; 
	GameObject pandaAvatar;
	
	GameObject prevBtn;

	string[] questionSymArray; //PART A symptoms
	string[] questionFollowArray; // PART B followups
	string[] apptQuestionArray;
	string[] pmiq;

	int count;
	ArrayList al; 

	// part A 
	int numPartAIntro; 
	int numPartA1_sym ; 
	int numPartA1;
	int numPartA2_sym;
	int numPartA; 
	int numPartA_sym;

	int numberPart_B1; 
	int numberPart_B2; 
	int numberPart_B3; 
	int numberPart_B; 
	int numberPart_AB; 
	int numberLimit; 
	
	// Use this for initialization
	void Start () {
		
		thisText = GameObject.Find (this.name).GetComponent<Text> ();// renamed the Text object so that there will be no two same names 
		pandaAvatar = GameObject.Find ("Panda Placeholder"); 

		// Initialize parameters for Part A
		numPartAIntro = 1; 
		numPartA1_sym = 23; 
		numPartA2_sym = 8; 
		numPartA_sym = numPartA1_sym + numPartA2_sym; 
		numPartA1 = numPartAIntro + numPartA1_sym * 4; //93
		numPartA = numPartA1 + numPartAIntro + numPartA2_sym * 3; //108

		
		// For Part B 
		//		numberPart_B1 = 1; 
		//		numberPart_B2 = 4; 
		//		numberPart_B3 = 2;
		//		numberPart_B = numberPart_B1 + numberPart_B2 + numberPart_B3; 
		//		numberPart_AB = numberPart_A + numberPart_B;
		numberLimit = 250;


		// Create needed arrays and lists
		populateQuestionSymArray (); 
		populateQuestionFollowArray (); 
		populateApptQuestionArray ();

		al = new ArrayList ();

//===================debugging=====
		partA ();
//		partA1 ();
		startDiary ();
//		startAppt ();


		
	}
	
	// Update is called once per frame
	void Update () {
	}
	//=============================================================
	
	public void onClick (){// proceed to change the question text 

		partAtext ();  
		partAbtn (); // NEED To be fixed
		count++;
		//		Debug.Log ("count = :" + count); 
		//		// change the text of the button text 
		//
		//		// Part A 
		//		if (count < numberPart_A1){
		//			thisText.text = (count%4 == 0)?	
		//				questionSymArray[count/4]:
		//				questionFollowArray[count%4-1];
		//
		//		}
		//		else if (count < numberPart_A){
		//
		//			if (count == numberPart_A1)
		//				yesNoButtons.SendMessage ("endSecA1");//only concerns "yesNoButtons"
		//
		//			int count2 = count - numberPart_A1; //
		//
		//			thisText.text = (count2%3 == 0)? 
		//				questionSymArray [count2/3 + numberPart_A1_sym]:
		//				questionFollowArray [count2%3 ];
		//
		//		}
		//		// Part B
		//		else if (count < numberPart_AB) {
		//			int count2 = count - numberPart_A;
		//
		//			if(count2 == 0){
		////				yesNoButtons.SendMessage ("endSecA"); 
		//				yesNoButtons.SetActive (false);
		//				contButton.SetActive (true); //go to part B 
		//				Debug.Log("senction A ends when count2 = 0");
		//			}
		//			if(count2 == numberPart_B1){
		//				//show appt figure with transparent background
		//				startAppt ();
		//
		//
		//				Debug.Log("appt starts as count2 = 1");
		//			}
		//			thisText.text = apptQuestionArray [count2];
		//		}
		//
		//
		//		count ++; 
		// if count exceeds the limit, deactivate this object or "You have finished"
		if (!(count < numberLimit)) 
			thisText.text = "You have finished the diary! Hooray!";
	}
	
	public void onClickNo() {
//		if(count < numberPart_A){
//			// skip the follow up questions 
//			
//			// skip 4
//			if(count < numberPart_A1){
//				if (count %4 != 0) 
//					count += (4 - count%4) ;
//				else count += 4; 
//			}
//			// skip 3
//			else { 
//				int count2 = count - numberPart_A1; 
//				if (count2 %3 != 0)
//					count += (3 - count%3) ; 
//				else count +=3;
//			}
//		}
//		
//		if(count == numberPart_A)
//			yesNoButtons.SetActive(false);
//		//show the question text
//		onClick (); 
	}
	//======================================================================
	
	void startDiary (){
		// check last diary finished or not


		// initialize parameters: date, time, user info
		count = 0; 
		thisText.text = al[count].ToString ();
		count ++;

		contButton.SetActive (true);
		prevBtn = contButton;
		
		yesNoButtons.SetActive (false);
		howBotherButtons.SetActive (false);
		howSevrButtons.SetActive (false);
		howOftenButtons.SetActive (false);
		canvasAppt.SetActive (false);
//		answerButtons = new GameObject[]{
//			yesNoButtons, howOftenButtons, howSevrButtons, howSevrButtons};

	
	}

	
	void startPartA_kid(){
	}
	void startAppt (){
		
		//		pandaAvatar.SetActive (false);
		b1.SetActive (false);
		b2.SetActive (false);
		b3.SetActive (false);
		b4.SetActive (false);
		b5.SetActive (false);
		
		//		canvasDiary.SetActive (false);
		//		transparentBg.SetActive (true);
		if(!canvasAppt.activeSelf){
			canvasAppt.SetActive (true);
		}
		
		if(!contButton.activeSelf){
			//			contButton.SetActive(true); 
			Debug.Log("continue button not active." );
		}
		
	}

	//---------------------------------------------------------------------------
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
		for (int i=0;i<al.Count;i++) {
//			string t = al[i].ToString();
			Debug.Log (al[i].ToString());
		}
	}

	void partA1() {
		// Build question structure for Part A 10-18 section 2

		string partIntro = "We have 8 more questions."; 

		al.Clear(); 
		al.Add (partIntro); 
		for (int i=numPartA1_sym; i<numPartA_sym; i++) {
			al.Add(questionSymArray[i]);
			for(int j=1; j<questionFollowArray.Length; j++) {
				al.Add (questionFollowArray [j]); 
			}				
		}
		//debugging 
		//for (int i=0;i<24;i++) 
		// Debug.Log (i.ToString()+al[i].ToString());
	}

	// change answer button
	void partAbtn(){//count -> buttons
		/* Since the general pattern for button transition 
		 * stays the same for Part A, this part is for 
		 * the transition 
		 */ 

		// Present answer choices according to the current question 
		string currText = al [count].ToString();
		Debug.Log (currText);

		// introduction -> continue
		if (currText.Contains ("We have ")){
		    contButton.SetActive (true);
		}
		// "Since the last diary entry.." -> yes/no
		else if (currText.Contains ("diary")){
			prevBtn.SetActive (false);
			yesNoButtons.SetActive (true); 
			prevBtn = yesNoButtons; 
		}
		// "how often.." 
		else if (currText.Contains ("often")){
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
			yesNoButtons.SetActive (true); 
			prevBtn.SetActive (false); 
		}



	}
	// change question text
	void partAtext(){
//		string[] temp = (string[]) al.ToArray( typeof( string ) );
		thisText.text = al [count].ToString();	;// NOTE: present then coutnt++

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
		string header = "Since the last diary entry, do you have: ";

		string conc = "Difficulty concentration or paying attention";
		string pain = "pain"; 
		string ener = "Lack of energy"; 
		string cougt = "Cough";
		string nerv = "Nervousness";
		string mott = "Dry mouth"; 
		string naust = "Nausea"; 
		string drowt = "Drowsy";
		string numbt = "Numbness/tingling or pins and needles feeling in hands or geet";
		string slept = "Difficulty sleeping"; 
		string urint = "Problems with urination"; 
		string vomit = "Vomiting"; 
		string breat = "Shortness of breath"; 
		string diart = "Diarrhea"; 
		string sadt = "Feelings of sadness";
		string sweat = "Sweats"; 
		string worrt = "Worring"; 
		string itcht = "itcht"; 
		string appt = "Appetite"; 
		string dizzt = "Dizziness";
		string swalt = "Difficulty swallowing"; 
		string irrit = "Feelings of being irritable";
		string headt = "Headache"; 
		string msort = "Mouth sores"; 
		string foodt = "Change in the way food tastes"; 
		string weitt = "Weight Loss"; 
		string hairt = "Less hair then usual"; 
		string constp = "Constipation ";
		string swelt = "Swelling of arms or legs"; 
		string lookt = "Don't look like myself"; 
		string skint = "Changes in skin"; 

		questionSymArray = new string[]{conc,pain,ener,cougt,nerv,mott,naust,drowt,numbt,slept,
			urint,vomit,breat,diart,sadt,sweat,worrt,itcht,appt,dizzt,
			swalt,irrit,headt,
			msort,foodt,weitt,hairt,constp,swelt,lookt,skint};
		for(int i=0;i<questionSymArray.Length;i++){
			questionSymArray[i]=header + questionSymArray[i] + " ?";
		}
		//debugging
		//	for(int i=0;i<questionSymArray.Length;i++){
		//		Debug.Log ("Syptoms: ");
		return; 
	}
	
	void populateApptQuestionArray (){
		string intro = "Now I need you to describe your pain.";
		string sec1Intro = "Tap on the areas on the body to show where you have pain.";
		string sec2Intro = "Move the slider to show how much pain you have had SINCE YOUR LAST DIARY ENTRY.";
		string sec3Intro = "Select as many of these words that describe your pain.";
		string other = "Woudl you like to add other words describe your pain?";
		string wgr = "How much pain do you have right NOW?"; 
		string wgra = "How much pain did you have, ON AVERAGE, since your last diary entry?"; 
		string wgrw = "What was your WORST pain since your last diary entry?"; 
		string wgrl = "What was your LEAST pain since your last diary entry?";	
		
		apptQuestionArray = new string[]{intro, sec1Intro, 
			sec2Intro, wgr, wgra, wgrw, wgrl,
			sec3Intro, other};
		
	}
	
	void populateApptSec3Array (){// Probably useless
		
	}
	
	void populatePmiqTextArray (){// text: intro + question 
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
		
		
	}
	
	
}

