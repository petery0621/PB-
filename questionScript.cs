using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class questionScript : MonoBehaviour {

	// GameObject, drag and drop 
	public GameObject yesNoButtons; 
	public GameObject howOftenButtons; 
	public GameObject howSevrButtons; 
	public GameObject howBotherButtons;
	public GameObject contButton; 
	public GameObject sliderObj;
	public GameObject[] answerButtons;

	public GameObject canvasDiary; 
	public GameObject canvasAppt; 
	public GameObject transparentBg;
	
	public GameObject b1; 
	public GameObject b2; 
	public GameObject b3; 
	public GameObject b4; 
	public GameObject b5; 
	GameObject pandaAvatar;

	
	Text thisText; // questionText object 
	Text painLvText;
	Slider slider;
	GameObject prevBtn;

	int count;
	int winId; 
	ArrayList al; 
	string answer; 

	// part A 
	int numPartAIntro; 
	int numPartA1_sym ; 
	int numPartA1;
	int numPartA2_sym;
	int numPartA2;
	int numPartA; 
	int numPartA_sym;
	// part B
	int numPartB; 
	int numberPart_B2; 
	int numberPart_B3; 
	int numberPart_B; 
	int numberPart_AB; 
	int numberLimit; 
	bool painLvOn;

	
	string[] questionSymArray; //PART A symptoms
	string[] questionFollowArray; // PART B followups
	string[] apptQuestionArray;
	string[] pmiq;
	
	// Use this for initialization
	void Start () {
		
		thisText = GameObject.Find (this.name).GetComponent<Text> ();// renamed the Text object so that there will be no two same names 
		pandaAvatar = GameObject.Find ("Panda Placeholder"); 
		slider = sliderObj.GetComponentInChildren<Slider> ();
		al = new ArrayList ();


		// Initialize parameters for Part A
		numPartAIntro = 1; 
		numPartA1_sym = 23; 
		numPartA2_sym = 8; 
		numPartA_sym = numPartA1_sym + numPartA2_sym; 
		numPartA1 = numPartAIntro + numPartA1_sym * 4; //93
		numPartA2 = numPartAIntro + numPartA2_sym * 3; 
		numPartA = numPartA1 + numPartAIntro + numPartA2_sym * 3; //108

		
		// For Part B 
		numPartB = 9; // length of apptQuestionArray 
		//		numberPart_B2 = 4; 
		//		numberPart_B3 = 2;
		//		numberPart_B = numberPart_B1 + numberPart_B2 + numberPart_B3; 
		//		numberPart_AB = numberPart_A + numberPart_B;
		numberLimit = 250;


		// Create needed arrays and lists
		populateQuestionSymArray (); 
		populateQuestionFollowArray (); 
		populateApptQuestionArray ();

		startDiary ();
//===================debugging=====
//		startDiary ();
//		startAppt ();


		
	}
	
	// Update is called once per frame
	void Update () {
		if(painLvOn)
			painLvText.text = slider.value.ToString();
	}
	//=============================================================
	
	public void onClick (){// proceed to change the question text 

		switch (winId) {
		
		case 0: 
//			partAtext (numPartA1);
			partAtext (numPartA1);
			partAbtn ();
			break;
		case 1:
//			partAtext (numPartA_kid);
//			partAbtn (); //Possible change due to question change
			break;
		case 2: // 1018 2nd half (same structure w/ 1st half)
			partAtext (numPartA2);
			partAbtn ();
			break;
		case 3: 
			partBtext ();
			partBbtn ();
			break;
		default:
			Debug.Log ("run default in coClick switch"); 
			break;
		}







		count ++;
		if (!(count < numberLimit)) 
			thisText.text = "You have finished the diary! Hooray!";
	}
	
	public void onClickNo() {
//	
	}
	//======================================================================
	
	void startDiary (){
		// check last diary finished or not


		// initialize parameters: date, time, user info
		count = 0; 
		winId = 0;
		painLvOn = false;
//		winId = (uesrAge > 9) ? 0 : 1;

		contButton.SetActive (true);
		prevBtn = contButton;
		
		yesNoButtons.SetActive (false);
		howBotherButtons.SetActive (false);
		howSevrButtons.SetActive (false);
		howOftenButtons.SetActive (false);
		canvasAppt.SetActive (false);

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
	//partA	
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
//		for (int i=0;i<al.Count;i++) {
//			Debug.Log (i.ToString() + al[i].ToString());
//		}
	}

	void partA1() {
		// Build question structure for Part A 10-18 section 2

		string intro = "We have 8 more questions."; 

		al.Clear(); 
		al.Add (intro); 
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
		Debug.Log ("current text: " +currText);

		// introduction -> continue
		if (currText.Contains ("We have ")){
			prevBtn.SetActive (false);
			if(!contButton.activeSelf)
		    	contButton.SetActive (true);
			prevBtn = contButton ;
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
			prevBtn = yesNoButtons ;
		}



	}
	// change question text
	void partAtext(int length){
//		string[] temp = (string[]) al.ToArray( typeof( string ) );
		if(count <length ){
			thisText.text = al [count].ToString();	// NOTE: present then coutnt++
		}
		else {
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
		Debug.Log ("current text: " +currText);


		// slider bar
		if (currText.Contains ("much pain") && currText.Contains ("do you"))
				|| currText.Contains ("was your")){
			if (!sliderObj.activeSelf)
				sliderObj.SetActive (true); 
		}
		else {
			if (sliderObj.activeSelf) 
				sliderObj.SetActive (false); 
		}

		// toggle
		if (currText.Contains("as many of")){

		}


		// continue
		prevBtn.SetActive (false);
		if(!contButton.activeSelf)
			contButton.SetActive (true);
		prevBtn = contButton ;




	}

	void partBtext (){
		if(count == 1){ //appt starts
			canvasAppt.SetActive (true);
		}
		else {
			if(	canvasAppt.activeSelf)
				canvasAppt.SetActive (false);
		}
		thisText.text = al [count].ToString ();

			
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

	void populateQuestionKid(){
		string intro = "We want to find out how you have been feeling since your last diary entry." +
			"Tap on the screen to select your answers";
		string a = "a"; 
		string b = "b";
		
		
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

		string note = "\n"+" 0: Not Hurting, 3: Huring A little, 5: Hurtring a Median amount, " +
			"7: Hurting a log, 10: Hurting a whole lot";

		string t = sec3Intro + "\n";
		string p0 = t +"annoying, bad, horrible, miserable, terrible, uncomfortable";
		//senseory
		string p1 = t +"aching,hurting,like an ache,like a hurt,sore"  ;
		string p2 = t +"beating, hitting , pouding, punching, throbbing";  
		string p3  = t +"biting,cutting,like a pin,like a sharp knife,pin like,sharp,stabbing"  ;
		string p4  = t +"blistering,burning,hot";
		string p5  = t +"cramping,crushing,like a pinch,pinching,pressure"  ;
		string p6  = t +"Itching,like a scratch,like a sting,scratching,stinging";
		string p7  = t +"shocking,shooting,splitting"  ;
		string p8  = t +"numb,stiff,swollen,tight"  ;
		//affective
		string p9  = t +"awful,deadly,dying,killing"  ;
		string p10  = t +"crying,frightening,screaming,terrifying"  ;
		string p11  = t +"dizzy,sickeing,suffocating"  ;
		//evalucative
		string p12  = t +"never goes away,uncontrollable"  ;
		//temporal
		string p13  = t +"always,comes and goes,comes on all of a sudden,constant,sontinuous,forever  ";
		string p14  = t +"off and on,once in a while,sneaks up,sometimes,steady"  ;
		
		apptQuestionArray = new string[]{intro, sec1Intro, 
			sec2Intro, wgr+note, wgra+note, wgrw+note, wgrl+note,
			sec3Intro, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13 ,p14,
			other};
		
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

