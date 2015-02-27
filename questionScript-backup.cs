using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;


public class questionScript : MonoBehaviour {
	
	// GameObject, drag and drop 
	public GameObject yesNoButtons; 
	public GameObject howOftenButtons; 
	public GameObject howSevrButtons; 
	public GameObject howBotherButtons;
	public GameObject contButton; 

	public GameObject canvasAppt; 
	public GameObject transparentBg;
	public GameObject sliderObj;
	public Slider slider;
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

	Text thisText; // questionText object 
	Text painLvText;
	public GameObject menu; 
	GameObject pandaAvatar;
	GameObject prevBtn;
	GameObject[] toggle;
	GameObject prevTgl;
	
	int count;
	int winId; 
	ArrayList al; 
	bool clickNo;

	string answers; 
	string response1; 
	string response1_input; 
	string response2; 
	string response2_words;
	string response2_input; 



	// part A 
	int numPartAIntro; 
	int numPartA1_sym ; 
	int numPartA1;
	int numPartA2_sym;
	int numPartA2;
	int numPartA; 
	int numPartA_sym;
	int numberLimit; 
	string inputTest; 
	// part B
	int numPartB; 
	int numberPart_B2; 
	int numberPart_B3; 
	int numberPart_B; 
	int numberPart_AB; 
	int count_toggle;
	string[] apptArray; 
	string apptString; 
	bool painLvOn;
	// part C
	int numPartC2_sym;

	string[] questionSymArray; //PART A symptoms
	string[] questionFollowArray; // PART B followups
	string[] apptQuestionArray;
	string[] piqTextArray1;
	string[] piqFollowArray1;
	string[] piqTextArray2;
	string[] piqFollowArray2;

	
	// Use this for initialization
	void Start () {
		
		thisText = GameObject.Find (this.name).GetComponent<Text> ();// renamed the Text object so that there will be no two same names 
		pandaAvatar = GameObject.Find ("Panda Placeholder"); 

		al = new ArrayList ();
		
		
		// Initialize parameters for Part A
		numPartA1_sym = 1; 
		numPartA2_sym = 1;

		numPartAIntro = 1; 
		numPartA_sym = numPartA1_sym + numPartA2_sym; 
		numPartA1 = numPartAIntro + numPartA1_sym * 4; //93
		numPartA2 = numPartAIntro + numPartA2_sym * 3 + 3; // 3: other-symptom text
		numPartA = numPartA1 + numPartAIntro + numPartA2_sym * 3; //108
		numberLimit = 300;
		
		
		// For Part B 
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
		//		startDiary ();
		//		startAppt ();

	}
	public void debug (){
		count--; 
		count_toggle--;
	}
	
	// Update is called once per frame
	void Update () {
		if(painLvOn)
			painLvText.text = slider.value.ToString();
	}
	//=============================================================
	
	public void onClick (int value){// proceed to change the question text

		// display the current question
		switch (winId) {
			
		case 0: 
			partAtext (numPartA1);
			partAbtn ();
//			storeData(winId, value);
			break;
		case 1:
			//			partAtext (numPartA_kid);
			//			partAbtn (); //Possible change due to question change
			break;
		case 2: // 1018 2nd half (same structure w/ 1st half)
//			storeData(winId, value);
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

		Debug.Log ("answers: " + answers);
		count++; 

		
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
		Debug.Log ("current al length: " + al.Count);
		string currText = al [count].ToString (); 


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
//			if (  ) //discard input other "yes/no"  question
//				break; 
			if (value != -1) {
				answers += value.ToString();
			}
			if (inputFieldObj.activeSelf){
				answers += (inputField.text != "" )? 
					inputField.text+",": "*,";	
			}
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

		menu.SetActive (true);
		yesNoButtons.SetActive (false);
		howBotherButtons.SetActive (false);
		howSevrButtons.SetActive (false);
		howOftenButtons.SetActive (false);
		canvasAppt.SetActive (false);

		apptArray = new string[43]; 
		
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
		string other = "Did you have anything else which made you feel bad or sick since your last diary?"; 
		string typeIn = "Please type it in below.";
		string howBother = "How much did this bother you or trouble you?"; 
		
		al.Clear(); 
		al.Add (intro); 
		for (int i=numPartA1_sym; i<numPartA_sym; i++) {
			al.Add(questionSymArray[i]);
			for(int j=1; j<questionFollowArray.Length; j++) {
				al.Add (questionFollowArray [j]); 
			}				
		}
		al.Add (other);
		al.Add (typeIn);
		al.Add (howBother); 
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
		if (currText.Contains ("type it in below")){
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
			else if (currText.Contains ("Since the last diary entry, did you have")||
				currText.Contains ("anything else")){
				prevBtn.SetActive (false);
				yesNoButtons.SetActive (true); 
				prevBtn = yesNoButtons; 

				answers += (currText.Contains("anything else"))? "": ","; 
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
				contButton.SetActive (true); 
				prevBtn = contButton ;

			}	
		}
	}
	// change question text
	void partAtext(int length, Input value){
		/* display part A text, transition to part B
		 * Input: 
		 * length: to distinguish sec1 and sec2 in partA
		 * value: to store user's response
		 */ 



		if(count <length ){
			if(!clickNo){
				thisText.text = al [count].ToString();	// NOTE: present then count++
				answers += value.ToString(); 
			}
			else{
				while ((count<length) && !al[count].ToString().Contains("diary")){
					// In the "if(count<XXX)", after "count++" 
					// you need to decide again
				
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
		Debug.Log ("current text: " +currText);

		// slider bar
		if (currText.Contains ("did you")|| currText.Contains ("do you")
			|| currText.Contains ("was your")){
			if (!sliderObj.activeSelf)
				sliderObj.SetActive (true); 
			slider.value = 0; 
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
		}
		else if (prevBtn != contButton){
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


			thisText.text = al [count].ToString ();
		}
		else {
			inputFieldObj.SetActive (false); 
			if (prevBtn!=contButton)
				prevBtn.SetActive(false); 
			if(!contButton.activeSelf)
				contButton.SetActive (true); 
			winId = 4; 
			count = 0; 
			partC (); 
			thisText.text = al [count].ToString(); 

		
		}
		
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
		string ctrl = "Based on all the things you did to cope, deal with your pain TODAY how much control did you fell you had over it?" +
			"Please select the appropriate number. Remember, you can select any number along the scale. " 
				+"\n" + "(0: No control, 3: Some control, 6: Complete control)";
		string dec = "Based on all the things you did to cope, deal with your pain TODAY how much were you able to decrease it?" +
			"Please select the appropriate number. Remember, you can select any number along the scale."
				+"\n" + "(0: Can't decrease it at all, 3: Can't decrease it somewhat, 6: Can decrease it completely)";	
		string name = "Please write in the name of this actiivty"; 
		string other = "Any other Activities?";

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

	void partCbtn (){
		if (count == al.Count || count > al.Count) {
			prevBtn.SetActive (false); 
			if (sliderObj.activeSelf) sliderObj.SetActive (false); 
			endDiary();
			return;
		}
		// Present answer choices according to the current question 
		string currText = al [count].ToString();
		Debug.Log ("current text: " +currText);

		// have you taken/tried..
		if (currText.Contains("Have you")||currText.Contains("have you")){
			if (prevBtn!=yesNoButtons){
				prevBtn.SetActive (false);
				yesNoButtons.SetActive (true); 
				prevBtn = yesNoButtons;
			}
		}
		// input field: name or times -> tpye in 
		if (currText.Contains("Name")||currText.Contains("name")
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
			if (currText.Contains("control")
			    || currText.Contains("decrease")){
				slider.maxValue = 6;
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
					while (!al[count].ToString().Contains("have you")
					       &&!al[count].ToString().Contains("Based on")){
						count ++; 
					}
					thisText.text = al[count].ToString(); 
				}
			}
		}
		else{
			//do nothing for now 
			thisText.text = "You've finished the diary!";
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
		string header = "Since the last diary entry, did you have any: ";
		
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
	
	void populateApptArray (){
		string intro = "Now I need you to describe your pain.";
		string sec1Intro = "Tap on the areas on the body to show where you have pain.";
		string sec2Intro = "Move the slider to show how much pain you have had SINCE YOUR LAST DIARY ENTRY.";
		string sec3Intro = "Select as many of these words that describe your pain.";
		string other = "Would you like to add other words describe your pain?";
		string wgr = "How much pain do you have right NOW?"; 
		string wgra = "How much pain did you have, ON AVERAGE, since your last diary entry?"; 
		string wgrw = "What was your WORST pain since your last diary entry?"; 
		string wgrl = "What was your LEAST pain since your last diary entry?";	
		
		string note = "\n"+" 0: Not Hurting, 3: Huring A little, 5: Hurtring a Median amount, " +
			"7: Hurting a log, 10: Hurting a whole lot";
		
		string t =  "Select as many of these words: " +"\n";
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

		string ow1 = "Other word 1:";
		string ow2 = "Other word 2:";
		string ow3 = "Other word 3:";
		
		apptQuestionArray = new string[]{intro, sec1Intro, 
			sec2Intro, wgr+note, wgra+note, wgrw+note, wgrl+note,
//			sec3Intro, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13 ,p14,
			sec3Intro,t, t, t, t, t, t, t, t, t, t, t, t, t, t, t, 
			other, ow1, ow2, ow3};

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

		string name = "Name of medication ?";
		string times = "How many times was this medication taken since the last diary?";
		string help = "How much did this medication help? " +
			"\n" + "(Move the slider bar to show how much the medicine helped, from 'Did not help at all' to ' Helped a lot')";

		string q1 = header + first + trailer;
		string q2 = header + second + trailer;
		string q3 = header + third + trailer;


		piqTextArray1 = new string[] {q1,q2,q3};
		piqFollowArray1 = new string[]{name, times, help};
		
	}

	void populatePiqArray2 () {
		string header = "Since your last diary entry, have you tried: ";       

		string times = "How many times was this activity done since the last entry.";
		string help = "How much did this activity help? " +
			 "\n" + "(Move the slider bar to show how much the medicine helped, from 'Did not help at all' to ' Helped a lot')";
	
		string ctrl = "Based on all the things you did to cope, deal with your pain TODAY how much control did you fell you had over it?" +
			"Please select the appropriate number. Remember, you can select any number along the scale.";
		string dec = "Based on all the things you did to cope, deal with your pain TODAY how much were you able to decrease it?" +
			"Please select the appropriate number. Remember, you can select any number along the scale.";	

		string hp = "Heat Packs"; 
		string db = "Deep Breathing"; 
		string re = "Relaxation Exercise"; 
		string t = "Thought about my pain in a positive way (for example," +					
			"thought that the pain means that my treatment is working)";
		string m = "Massage"; 
		string i = "Imagery(using your mind to imagine pictures, sights, sounds to relax)"; 
		string d = "Distraction (TV, video games, something to take your mind off of pain)"; 
		string twf = "Talking with friends/parents"; 
		string other = "Any other activities"; 

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

