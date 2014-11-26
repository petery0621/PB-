using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Net;
using System.Collections;

[System.Serializable]
public class DevOptions
{
	public bool usingPC, allowTestUserLogin;
}
[System.Serializable]
public class AdminScreenObjects
{
	public Text summaryText;
	public Button createUser;
}
[System.Serializable]
public class CreateUserPanel
{
	public GameObject panel;
	public InputField createInput_First, createInput_Last, createInput_Age, createInput_ID; 
	public Toggle createInput_Control;
}
[System.Serializable]
public class SettingsScreenObjects
{
	public Text title;
}
[System.Serializable]
public class ScreenList
{
	public GameObject adminScreen, cbtScreen, coinBankScreen, diaryScreen, loginScreen, messagesScreen, settingsScreen, welcomeScreen;
}

[System.Serializable]
public class CbtList
{
	public GameObject bellyBreathingScreen, distraction1Screen, distraction2Screen, distraction3Screen, guidedImagery1Screen, guidedImagery2Screen, guidedImagery3Screen, mindfulness1Screen, mindfulness2Screen, progRelaxationScreen;
}

public class AppControlScript2 : MonoBehaviour 
{
	//public objects and collections
	public DevOptions devOptions;
	public AdminScreenObjects adminScreenObjects;
	public CreateUserPanel createUserPanel;
	public SettingsScreenObjects settingsScreenObjects;
	public Camera cam;
	public ScreenList Screens;
	public CbtList cbtSkills;
	public Texture[] WelcomeScreenTextures;
	public InputField loginInput;
	public GameObject diaryBackButton;

	//private application stuff
	private GameObject CurrentScreen, PreviousScreen;
	private bool loggedInPatient, loggedInAdmin, InternetOn;
	private int numDaysSinceUpload;

	//constants
	private Vector3 camHeight;
	private GameObject[] cbtSkillsArray;

	//diary script
	private GameObject diaryControl;
	private DiaryScript2 diaryWindow ; //diaryControl.GetComponent(typeof(SurveyDiary914)); 
	private int currentNumberPendingDiaryUploads;

	//user data
	private bool userExists, data_Control;//data_Control is FALSE if experimental user
	private string display_name, data_ID, data_FirstName, data_LastName;
	private int data_Age;
	private int currentAvatar, currentBackground;
	private string sesLoginTime;

	//user (total) statistics
	private int totin, totout;//total number of logins and logouts
	private int tottim;//total number of session timeouts
	private int totinc;//total number of incomplete diaries
	private int totlog;//total number of days logged on
	private int tottrig;//total number of CBT triggers
	private int totccom;//total number CBT skill trainings completed
	private int totcses;//total number of CBT sessions completed
	private int totcself;//total number of self-initiated cbt sessions
	private TimeVariable totlogtm;//total time spent in Painbuddy app (not including timeouts) (DD HH:MM:SS) format
	private TimeVariable tothour;//total time spent in Painbuddy

	//UNITY METHODS
	//###################################################################################################
	void Awake() 
	{
		if(!devOptions.usingPC)
			cam.orthographicSize = Screen.height/2;

		//initalize DiaryController object
		diaryControl = GameObject.Find ("DiaryController"); 
		diaryWindow = diaryControl.GetComponent<DiaryScript2> ();
	}
	
	void Start() 
	{
		setUpInputFields();
		createArrayCbtSkills();
		loadStatisticsFromDevice();
		assignInitialValues();//also moves camera to login screen
		checkNetwork();//checks if internet is available, carries out other methods?
		//check for new admin passwords?
	}
	
	void Update()
	{
		//use for settings things active or not
		//use bool to update things only when changed

		if(Input.GetKeyDown(KeyCode.Escape))
			back(1);
	}

	void OnApplicationPause(bool paused)
	{
		if(paused)
		{
			//saveUserData();
		}
	}

	//##################################################################################################
	//PRIVATE METHODS
	private void setUpInputFields()
	{
		loginInput.keyboardType = TouchScreenKeyboardType.NumberPad;
		createUserPanel.createInput_First.keyboardType = TouchScreenKeyboardType.Default;
		createUserPanel.createInput_Last.keyboardType = TouchScreenKeyboardType.Default;
		createUserPanel.createInput_Age.keyboardType = TouchScreenKeyboardType.NumberPad;
		createUserPanel.createInput_ID.keyboardType = TouchScreenKeyboardType.NumberPad;
	}

	private void createArrayCbtSkills()
	{
		cbtSkillsArray = new GameObject[10];
		cbtSkillsArray[0] = cbtSkills.bellyBreathingScreen;
		cbtSkillsArray[1] = cbtSkills.distraction1Screen;
		cbtSkillsArray[2] = cbtSkills.distraction2Screen;
		cbtSkillsArray[3] = cbtSkills.distraction3Screen;
		cbtSkillsArray[4] = cbtSkills.guidedImagery1Screen;
		cbtSkillsArray[5] = cbtSkills.guidedImagery2Screen;
		cbtSkillsArray[6] = cbtSkills.guidedImagery3Screen;
		cbtSkillsArray[7] = cbtSkills.mindfulness1Screen;
		cbtSkillsArray[8] = cbtSkills.mindfulness2Screen;
		cbtSkillsArray[9] = cbtSkills.progRelaxationScreen;
	}

	private void loadStatisticsFromDevice()
	{
		toast("* load statistics *", false);
		if(PlayerPrefs.GetInt("userExists", 0) == 1)
		{
			userExists = true;

			display_name = PlayerPrefs.GetString("displayName", "<null>");
			currentAvatar = PlayerPrefs.GetInt("currentAvatar", -1);
			currentBackground = PlayerPrefs.GetInt("currentBackground", -1);
			
			data_FirstName = PlayerPrefs.GetString("FirstName", "<null>");
			data_LastName = PlayerPrefs.GetString("LastName", "<null>");
			data_Age = PlayerPrefs.GetInt("Age", -1);
			data_ID = PlayerPrefs.GetString("ID", "<null>");
			if(PlayerPrefs.GetInt("Control", 0) == 0)//use one of the above variables for checking if user exists
			{
				data_Control = false;
			}
			else
			{
				data_Control = true;
			}
		}
		else
		{
			userExists = false;
		}
	}
	
	private void assignInitialValues()
	{
		camHeight = new Vector3(0, 1000, 0);;
		cam.transform.position = Screens.loginScreen.transform.position + camHeight;
		CurrentScreen = Screens.loginScreen;
		loggedInPatient = false;
		loggedInAdmin = false;
		InternetOn = false;

		if(userExists)
		{
			if(display_name.CompareTo("<null>") == 0)
			{
				settingsScreenObjects.title.text = "Welcome!";
			}
			else//no name given
			{
				settingsScreenObjects.title.text = "Welcome, " + display_name + "!";
			}
		}
	}

	//both back buttons CURRENTLY do the same thing
	//0 means button back, 1 means soft keyboard back
	public void back(int flag)
	{
		if (CurrentScreen == Screens.loginScreen) 
		{
			Application.Quit();
		} 
		else 
		{
			if(CurrentScreen == Screens.welcomeScreen)
			{	
				logout();
			}
			else if(CurrentScreen == Screens.diaryScreen && diaryWindow.surveyWindow)
			{
				//don't allow back button
			}
			else if(PreviousScreen == Screens.cbtScreen)//currently on one of the cbt skills screens
			{
				PreviousScreen = Screens.welcomeScreen;//will result in log-out from welcome screen regardless of PreviousScreen setting. this is formality
				CurrentScreen = Screens.cbtScreen;
			}
			else
			{
				CurrentScreen = PreviousScreen;
				PreviousScreen = Screens.welcomeScreen;
			}

			cam.transform.position = CurrentScreen.transform.position + camHeight;
		}
	}

	public void login()
	{
		if(devOptions.allowTestUserLogin && loginInput.value.Equals ("1001")) 
		{
			loggedInPatient = true;
			cam.transform.position = Screens.welcomeScreen.transform.position + camHeight;
			CurrentScreen = Screens.welcomeScreen;
			PreviousScreen = Screens.loginScreen;//should they be able to log out with back button?
			loginInput.value = "Input ID";
			toast("* logged in *", false);
		}
		else if(userExists && loginInput.value.Equals(data_ID))
		{
			loggedInPatient = true;
			cam.transform.position = Screens.welcomeScreen.transform.position + camHeight;
			CurrentScreen = Screens.welcomeScreen;
			PreviousScreen = Screens.loginScreen;//should they be able to log out with back button?
			loginInput.value = "Input ID";
			toast("* logged in *", false);
		}
		else if (loginInput.value.Equals ("9999")) 
		{
			loggedInAdmin = true;
			goToAdminScreen();
			loginInput.value = "Input ID";
		}
		else
		{
			toast("Incorrect login. Please try again.", false);
		}
	}

	public void logout()
	{
		if(loggedInPatient == true)
		{
			loggedInPatient = false;
			cam.transform.position = Screens.loginScreen.transform.position + camHeight;
			CurrentScreen = Screens.loginScreen;
			PreviousScreen = Screens.loginScreen;//so back button can't be used to log in
			toast("* logged out *", false);
		}
		else if(loggedInAdmin == true)
		{
			if(createUserPanel.panel.activeSelf)
			{
				//add dialog box to ask if wants to cancel current user creation
				createUserToggle(false);
			}
			loggedInAdmin = false;
			cam.transform.position = Screens.loginScreen.transform.position + camHeight;
			CurrentScreen = Screens.loginScreen;
			PreviousScreen = Screens.loginScreen;//so back button can't be used to log in
			toast("* logged out admin *", false);
		}
	}

	public void createUserToggle(bool active)
	{
		createUserPanel.panel.SetActive(active);
		adminScreenObjects.summaryText.gameObject.SetActive(!active);
		adminScreenObjects.createUser.interactable = !active;

		if(!active)//clear inputs when logged out
		{
			createUserPanel.createInput_First.text.text = "";
			createUserPanel.createInput_Last.text.text = "";
			createUserPanel.createInput_Age.text.text = "";
			createUserPanel.createInput_ID.text.text = "";
			createUserPanel.createInput_Control.isOn = false;
		}
	}

	public void createUser()
	{
		string url, first, last, age, id;
		bool control_bool;
		int control_int;
		WWWForm form;
		WWW www;

		url = "http://128.195.185.108/pb_dev/UnityAndroid_Access/create_user.php";
		first = createUserPanel.createInput_First.text.text;
		last = createUserPanel.createInput_Last.text.text;
		age = createUserPanel.createInput_Age.text.text;
		id = createUserPanel.createInput_ID.text.text;
		control_bool = createUserPanel.createInput_Control;
		form = new WWWForm();

		if(control_bool)
			control_int = 1;
		else
			control_int = 0;

		if(!InternetOn)
		{
			toast("This device currently has no Internet access.", true);
		}
		else if(first.CompareTo("") == 0 || last.CompareTo("") == 0 || age.CompareTo("") == 0 || id.CompareTo("") == 0)
		{
			toast ("All fields are required. Please try again.", true);
		}
		else if(userExists)
		{
			toast("A user already exists on this device.", true);
		}
		else
		{
			form.AddField("firstName", first);
			form.AddField("lastName", last);
			form.AddField("age", age);
			form.AddField("id", id);
			form.AddField("control", control_int);

			www = new WWW(url, form);

			StartCoroutine(UploadCoroutine(www, "create_user", -1));//flag not needed in this case

			data_FirstName = first;
			data_LastName = last;
			data_Age = Int32.Parse(age);
			data_ID = id;
			data_Control = control_bool;
			userExists = true;
			//if not successful, variable names will be reset to default values
		}
	}

	//diary methods
	public void startDiary()
	{
		PreviousScreen = CurrentScreen;
		CurrentScreen = Screens.diaryScreen;
		cam.transform.position = Screens.diaryScreen.transform.position + camHeight;
		//toast ("* diary started *", false);
		//diaryControl.GetComponent<SurveyDiary914>().startDiary ();
		diaryWindow.startDiary(); 
		//diaryWindow.surveyWindow = true; 
		diaryBackButton.SetActive(false);
	}
	
	public void endDiary()
	{
		diaryBackButton.SetActive(true);
		//toast("You have completed this session's diary!", true);
	}

	public void queueDiarySubmission(string diarySubmission)
	{
		string url = "http://buddy.calplug.uci.edu/app/submit_responses.php";
		int d;
		//string cachedSubmission;

		//check network
		//checkNetwork();

		//store submission just in case upload fails
		/*currentNumberPendingDiaryUploads = PlayerPrefs.GetInt("numPendingDiary", 0);
		PlayerPrefs.SetInt("numPendingDiary", currentNumberPendingDiaryUploads + 1);//add one becaue we save it
		PlayerPrefs.SetString("diary" + currentNumberPendingDiaryUploads + 1, diarySubmission);

		d = currentNumberPendingDiaryUploads + 1;//index of last saved string*/

		//if we have internet, cycle through uploads
		if(InternetOn)
		{
			WWWForm form = new WWWForm();
			form.AddField("response_string", diarySubmission);
			WWW www = new WWW(url, form);
			UploadCoroutine(www, "diary", 0);//flag doesn't matter for testing

			/*for(int x = d; x > 0; x--)
			{
				cachedSubmission = PlayerPrefs.GetString("diary" + x);
				WWWForm form = new WWWForm();
				form.AddField("response_string", cachedSubmission);
				WWW www = new WWW(url, form);
				UploadCoroutine(www, "diary", x);
				//rembember to save this to playerprefs in saveData function
			}*/
		}
		else
		{
			toast("Data saved to device. Please connect to the Interet as soon as possible.", true);
			//put another submit option? or check internet every few minutes?
		}
	}

	//network methods	
	private IEnumerator UploadCoroutine(WWW www, string type, int flag)
	{
		yield return www;

		if(type.CompareTo("create_user") == 0)
		{
			// check for errors
			if (www.error == null)
			{
				toast("User created!", true);
			} 
			else 
			{
				Debug.Log("WWW Error: "+ www.error);
				toast("WWW Error: " + www.error, false);
				toast("User not created.", true);
				clearUserData();//just clears the fields assigned in createUser()
			} 
		}
		if(type.CompareTo("diary") == 0)
		{
			if (www.error == null)
			{
				//PlayerPrefs.DeleteKey("diary" + flag);//commented out for testing
				//numDaysSinceUpload = 0;//change name to 'numDaysSinceDiaryUpload'?
				//currentNumberPendingDiaryUploads--; //just in case uploads are interrupted, we store the value of where to start next time
				toast ("Diary upload successful.", true);//THIS ISN'T BEING CALLED???????
			}
			else
			{
				toast ("Diary upload error.", true);
			}
		}
	}

	//checks network connection
	public void checkNetwork()
	{
		string url, date_string;
		WWWForm form;
		WWW www;

		url = "http://buddy.calplug.uci.edu/app/ping.php";
		date_string = DateTime.Now.Date.ToShortDateString();
		form = new WWWForm();
			
		//for keeping track of pings by device, if CHOC is curious
		//requires loading stuff first!!!
		//form.AddField("patientID", data_ID);
		form.AddField("date", date_string);

		www = new WWW(url, form);
		StartCoroutine(checkNetworkCoroutine(www));
	}

	private IEnumerator checkNetworkCoroutine(WWW www)
	{
		yield return www;

		if (www.error == null)
		{
			toast("Full connection available.", true);
			InternetOn = true;
		} 
		else
		{
			int result = pingGoogle();
			if(result == -1)
			{
				toast("No internet connection available.", true);
				InternetOn = false;
			}
			else if(result == 0)
			{
				toast("Please login to Internet provider by opening your browser or checking your password.", true);
				InternetOn = false;
			}
			else if(result == 1)
			{
				toast("Unable to connect to Painbuddy servers at this time. Please contact an administrator for assistance.", true);
				InternetOn = false;
			}
			else
			{
				toast("Unknown internet issue.", true);
				InternetOn = false;
			}
		}
	}

	private int pingGoogle()//if Painbuddy servers are not accessible, tests if device has Internet at all
	{
		string html = string.Empty;
		int result;

		HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://google.com");
		try
		{
			using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
			{
				bool isSuccess = (int)resp.StatusCode < 299 && (int)resp.StatusCode >= 200;
				if (isSuccess)
				{
					using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
					{
						//We are limiting the array to 80 so we don't have
						//to parse the entire html document feel free to 
						//adjust (probably stay under 300)
						char[] cs = new char[80];
						reader.Read(cs, 0, cs.Length);
						foreach(char ch in cs)
						{
							html +=ch;
						}
					}
				}
			}

			if(!html.Contains("schema.org/WebPage"))
			{
				result = 0;//redirect
			}
			else
			{
				result = 1;
			}
		}
		catch
		{
			result = -1;
		}

		return result;
	}

	//user deletion
	private void clearUserData()
	{
		PlayerPrefs.DeleteAll();//please be really, really careful

		loadStatisticsFromDevice();//resets everything to defaults
	}

	private void saveUserData()//saves current values of user data if it exists
	{
		if(userExists)
		{
			PlayerPrefs.SetString("displayName", display_name);
			PlayerPrefs.SetString("FirstName", data_FirstName);
			PlayerPrefs.SetString("LastName", data_LastName);
			PlayerPrefs.SetInt("Age", data_Age);
			PlayerPrefs.SetString("ID", data_ID);

			if(data_Control)
			{
				PlayerPrefs.SetInt("Control", 1);
			}
			else
			{
				PlayerPrefs.SetInt("Control", 0);
			}

			PlayerPrefs.SetInt("currentAvatar", currentAvatar);
			PlayerPrefs.SetInt("currentBackground", currentBackground);
		}
	}

	//goTo Methods
	public void goToAdminScreen()
	{
		PreviousScreen = CurrentScreen;
		CurrentScreen = Screens.adminScreen;
		cam.transform.position = CurrentScreen.transform.position + camHeight;
		createUserToggle(false);
		toast ("* admin tools *", false);
		
		//totalNumSelfCbtSessions++;
	}

	public void goToCbtMenu()
	{
		PreviousScreen = CurrentScreen;
		CurrentScreen = Screens.cbtScreen;
		cam.transform.position = CurrentScreen.transform.position + camHeight;

		toast ("* cbt menu *", false);

		//totalNumSelfCbtSessions++;
	}

	public void goToCbtSkill(int sel)
	{
		PreviousScreen = CurrentScreen;
		CurrentScreen = (GameObject) cbtSkillsArray[sel];
		cam.transform.position = CurrentScreen.transform.position + camHeight;

		toast ("* " + CurrentScreen.name + " *", false);
	}

	public void goToCoinBank()
	{
		PreviousScreen = CurrentScreen;
		CurrentScreen = Screens.coinBankScreen;
		cam.transform.position = CurrentScreen.transform.position + camHeight;

		toast ("* coin bank *", false);

		//totalNumCoinBank++;
	}

	public void goToSettings()
	{
		PreviousScreen = CurrentScreen;
		CurrentScreen = Screens.settingsScreen;
		cam.transform.position = CurrentScreen.transform.position + camHeight;

		toast ("* settings *", false);
		//totalNumSettings++;
	}

	public void goToMessages()//in settings screen
	{
		PreviousScreen = CurrentScreen;
		CurrentScreen = Screens.messagesScreen;
		cam.transform.position = CurrentScreen.transform.position + camHeight;
		//totalNumMessages++;
	}

	private void toast(string message, bool LongMessage)
	{
		if (!devOptions.usingPC) 
		{
			if(LongMessage)
			{
				Toast.Instance ().ToastshowMessage (message, ToastLenth.LENGTH_LONG);
			}
			else 
			{
				Toast.Instance ().ToastshowMessage (message, ToastLenth.LENGTH_SHORT);
			}
		}
	}
}

class TimeVariable
{
	int days, hours, minutes, seconds;

	public TimeVariable()
	{
		days = 0;
		hours = 0;
		minutes = 0;
		seconds = 0;
	}

	public TimeVariable(int t)
	{
		days = 0;
		hours = 0;
		minutes = 0;
		seconds = 0;

		addTime(t);
	}
	
	public TimeVariable(int h, int m, int s)
	{
		hours = h;
		minutes = m;
		seconds = s;
	}
	
	public bool addTime(int t)
	{
		int s = t % 60;
		t = (int) t / 60;
		int m = t % 60;
		t = (int) t / 60;
		int h = t % 24;
		t = (int) t / 24;
		int d = t;
		
		seconds += s;
		if(seconds >= 60)
		{
			seconds -= 60;
			minutes++;
		}
		minutes += m;
		if(minutes >= 60)
		{
			minutes -= 60;
			hours++;
		}
		hours += h;
		if(hours >= 24)
		{
			hours -= 24;
		}
		days += d;

		return true;
	}

	public int ToSeconds()
	{
		int result = days * 86400 + hours * 3600 + minutes * 60 + seconds;
		return result;
	}
	
	public override string ToString()
	{
		string result, s, m, h, d;
		
		s = seconds.ToString("D2");
		m = minutes.ToString("D2");
		h = hours.ToString("D2");
		d = days.ToString();

		result = d + " " + h + ":" + m + ":" + s;
		return result;
	}
}

public class UnusedStuff
{
	//private void loadStatisticsFromDevice()
	//{
		/*if(PlayerPrefs.GetInt("totalStatsRecorded") == 0)
		{
			toast("No statistics found.", false);
		}
		else
		{
			totalNumLogins = PlayerPrefs.GetInt("totalNumLogins", 0);
			totalNumDaysIn = PlayerPrefs.GetInt("totalNumDaysIn", 0);
			totalNumSelfCbtSessions = PlayerPrefs.GetInt("totalNumSelfCbtSessions", 0);
			totalNumCoinBank = PlayerPrefs.GetInt("totalNumCoinBank", 0);
			totalNumMessages = PlayerPrefs.GetInt("totalNumMessages", 0);
			totalNumSettings = PlayerPrefs.GetInt("totalNumSettings", 0);
			totalTime = new TimeVariable(PlayerPrefs.GetInt("totalTime", 0));
		}*/
	//}

	//private void saveStatisticsToDevice()
	//{		
		/*if(PlayerPrefs.GetInt("totalStatsRecorded") == 0)
		{
			PlayerPrefs.SetInt("totalStatsRecorded", 1);
		}

		PlayerPrefs.SetInt("totalNumLogins", totalNumLogins);
		//PlayerPrefs.SetInt("totalNumDaysIn", totalNumDaysIn);
		PlayerPrefs.SetInt("totalNumSelfCbtSessions", totalNumSelfCbtSessions);
		PlayerPrefs.SetInt("totalNumCoinBank", totalNumCoinBank);
		PlayerPrefs.SetInt("totalNumMessages", totalNumMessages);
		PlayerPrefs.SetInt("totalNumSettings", totalNumSettings);
		PlayerPrefs.SetInt("totalTime", totalTime.ToSeconds());*/
	//}

	//private void UploadTest(string message)
	//{
		/*//string timestamp = System.DateTime.Now.Date.ToShortDateString() + "_" + System.DateTime.Now.TimeOfDay.Hours + ":" + System.DateTime.Now.TimeOfDay.Minutes + ":" + System.DateTime.Now.TimeOfDay.Seconds;
		string timestamp = System.DateTime.Now.Date.ToShortDateString() + "_" + System.DateTime.Now.TimeOfDay.ToString().Substring(0, 11);
		string url = "http://128.195.185.108/pb_dev/UnityAndroid_Access/test_upload.php";
		
		WWWForm form = new WWWForm();
		form.AddField("timestamp", timestamp);
		form.AddField("message", message);
		WWW www = new WWW(url, form);
		StartCoroutine(UploadTestCoroutine(www));*/
	//}
	
	//private IEnumerator UploadTestCoroutine(WWW www)
	//{
		/*yield return www;
		
		// check for errors
		if (www.error == null)
		{
			Debug.Log("Update worked, thank goodness.");
			Toast.Instance().ToastshowMessage("Upload successful.", ToastLenth.LENGTH_SHORT);
		} 
		else 
		{
			Debug.Log("WWW Error: "+ www.error);
			Toast.Instance().ToastshowMessage("WWW Error: " + www.error, ToastLenth.LENGTH_SHORT);
		}*/
	//}  

	//private void saveStatisticsToDevice()
	//{
		//toast("* save statistics *", false);
		
		//make sure to account for stopping in middle of user creation process
	//}
	
	//private void uploadStatistics()//in order of cbt, coin bank, messages, settings
	//{
		/*TimeVariable sessionTime = new TimeVariable((int) Time.realtimeSinceStartup);
		string stats = totalNumLogins + "_" + totalNumSelfCbtSessions + "_" + totalNumCoinBank + "_";
		stats += totalNumMessages + "_" + totalNumSettings + "_" + sessionTime.ToString();
		Toast.Instance().ToastshowMessage(stats, ToastLenth.LENGTH_SHORT);
		UploadTest(stats);*/
	//}
}
