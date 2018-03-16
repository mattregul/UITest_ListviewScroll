using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace UITest
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class Tests
    {
        // Learn more about UI Test Timeouts https://developer.xamarin.com/guides/testcloud/uitest/working-with/timeouts/
        private readonly TimeSpan myDefaultTimeout = TimeSpan.FromSeconds(20);

        IApp app;
        Platform platform;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            
            if (platform == Platform.Android)
            {
                //  How to get your device's Serial
                //  WINDOWS:
                //  Open a command prompt
                //  Run ADB.exe with the devices parameter
                //      EX: C:\Program Files(x86)\Android\android-sdk\platform-tools> adb devices
                //          List of devices attached
                //          LGLS9944y5221810 device  (my phone)
                //          emulator - 5554   device (my simulator)
                //
                app = ConfigureApp.Android
                    // Your device serial
                    .DeviceSerial("LGLS9974d328180")
                    // path to your android apk
                    //  (ensure you've built your app in RELEASE mode)
                    .ApkFile("C:\\Users\\mareg\\source\\repos\\UITest_ListviewScroll\\UITest_ListviewScroll\\UITest_ListviewScroll.Android\\bin\\Release\\com.companyname.UITest_ListviewScroll-Signed.apk")
                    //.EnableLocalScreenshots()
                    .StartApp();


            } else if (platform == Platform.iOS)
            {
                app = ConfigureApp
                    .iOS
                    /*
                     * How to find your Apple Device ID:
                     * =================================
                     * 1. Load XCode
                     * 2. Window
                     * 3. Devices & Simulators
                     * 4. Select your Device/Simulator
                     * 5. Copy "Identifier" string
                    */
                    .DeviceIdentifier("8683787a805dd2b363b4ea9114b6d0bb42258c93") // ipad
                    .InstalledApp("com.UITestListviewScroll")
                    .StartApp();
        
            }
            else
            {
                //app = AppInitializer.StartApp(platform);
                throw new System.Exception("Only Android and iOS are supported. For now?");
            }


        }

        [Test]
        public void AppLaunches()
        {
            app.Screenshot("First screen.");
        }


        /// <summary>
        /// Select 15th employee, we'll need to scroll down...
        /// </summary>
        [Test]
        public void SelectFifteenthEmployee()
        {
            // Load App
            // Attempt to click on 15th employee
            //  - Scroll down listview, looking for "Employee 15"
            //  - Repeat until employee is seen, or hit 20 second timeout (myDefaultTimeout)
            // Click on the 15th employee

            app.Screenshot("Before SelectFifteenthEmployee");

            // Keep scrolling down until we see the 15th employee, select them
            ScrollListviewToEmloyee("myList", "Employee 15");

            app.Screenshot("After SelectFifteenthEmployee");
        }
        

        /// <summary>
        /// Open the picker, select "Banking", select 4th Banking employee
        /// </summary>
        [Test]
        public void SelectFourthBankerEmployee()
        {
            // Load App
            // Click on Picker
            // Click Banker (if you can see it)
            //  - Scroll down picker list if can't be seen
            //  - Repeat until Banker is seen, or hit 20 second timeout (myDefaultTimeout)
            // *Listview updates to display only Banker employees*
            // Click on the 4th Banker (index #3)

            app.Screenshot("Before SelectFourthBankerEmployee");

            // Select the Banking job title
            ScrollPickerToJobTitle("myPicker", "Banking");
            // Tab on the 4th employee

            if (platform == Platform.iOS)
            {
                // ios
                app.Tap(d => d
                        .Class("Xamarin_Forms_Platform_iOS_CellTableViewCell")
                        .Class("UITableViewLabel")
                        .Index(3)
                       );
            }
            else
            {
                // Android
                app.Tap(x => x.Class("TextCellRenderer_TextCellView").Index(3));
            }


            app.Screenshot("After SelectFourthBankerEmployee");
        }



        /// <summary>
        /// Open the MasterDetail Menu
        /// </summary>
        [Test]
        public void MasterDetailMenu_Show()
        {
            if (platform == Platform.iOS)
            {
                app.Tap(d => d.Class("UINavigationBar").Class("UIButtonLabel"));
            } else
            {
                // TODO - android version
            }
        }

        /// <summary>
        /// Close the MasterDetail Menu
        /// </summary>
        [Test]
        public void MasterDetailMenu_Close()
        {
            if (platform == Platform.iOS)
            {
                app.Tap(x => x.Class("UIDimmingView"));
            } else {
                // TODO - android version
            }
        }

        /// <summary>
        /// Close the MasterDetail Menu
        /// </summary>
        [Test]
        public void MasterDetailMenu_SelectItem(string LabelText)
        {
            if (platform == Platform.iOS)
            {
                app.Tap(d => d.Class("UIDimmingView").Class("UILabel").Marked(LabelText));
            }else
            {
                // TODO - android version
            }
        }

        [Test]
        public void OpenPicker()
        {
            
            app.Screenshot("Before SelectFifthHREmployee");

            // Select the Banking job title
            ScrollPickerToJobTitle("myPicker", "Human Resources");
            // Tab on the 4th employee

            app.Tap(x => x.Class("TextCellRenderer_TextCellView").Index(4));

            app.Screenshot("After SelectFifthHREmployee");
        }

        /// <summary>
        /// Open the picker, select "Human Resources", select 4th Human Resources employee
        /// </summary>
        [Test]
        public void SelectFifthHREmployee()
        {
            // Load App
            // Click on Picker
            // Click Human Resources (if you can see it)
            //  - Scroll down picker list if can't be seen
            //  - Repeat until Human Resources is seen, or hit 20 second timeout (myDefaultTimeout)
            // *Listview updates to display only Human Resources employees*
            // Click on the 5thth Human Resources employee (index #4)

            app.Screenshot("Before SelectFifthHREmployee");

            // Select the Banking job title
            ScrollPickerToJobTitle("myPicker", "Human Resources");
            // Tab on the 4th employee


            if (platform == Platform.iOS)
            {
                // ios
                app.Tap(d => d
                        .Class("Xamarin_Forms_Platform_iOS_CellTableViewCell")
                        .Class("UITableViewLabel")
                        .Index(4)
                       );
            } else {
                // Android
                app.Tap(x => x.Class("TextCellRenderer_TextCellView").Index(4));
            }


            app.Screenshot("After SelectFifthHREmployee");
        }


        /// <summary>
        /// Open the Repl window, for manual query & tree
        /// Learn More Here - https://developer.xamarin.com/guides/testcloud/uitest/working-with/repl/
        /// </summary>
        [Test]
        public void OpenAppRepl()
        {
            app.Repl();
        }


        /// <summary>
        /// Look for a specifc item in a (OS specific) listview, try scrolling up or down if you can't see it
        /// </summary>
        /// <param name="listviewName">Name of listview we're working with</param>
        /// <param name="itemName">Name of item we're looking for</param>
        public void ScrollListviewToEmloyee(string listviewName, string itemName)
        {
            // Xamarin.UITest.IApp: Method Members - https://developer.xamarin.com/api/type/Xamarin.UITest.IApp/
            // - IApp.ScrollDownTo
            // - IApp.ScrollUpTo
            // UI Test Timeouts - https://developer.xamarin.com/guides/testcloud/uitest/working-with/timeouts/

            // Try clicking on the item right away, if we see it
            AppResult[] results = app.Query(c => c.Marked(itemName));
            if (results.Any())
            {
                app.Tap(e => e.Marked(itemName));
            }
            else
            {

                // Lets start by scrolling down the list, if we can't find it we'll try going back up the list
                //  (maybe the item we want is higher then our current pose)
                try
                {
                    // Look for the item by scrolling down
                    app.ScrollDownTo(z => z.Marked(itemName), x => x.Class("TextCellRenderer_TextCellView").Index(0), timeout: myDefaultTimeout);
                }
                catch
                {
                    // Didn't find it going down, lets try going up now...
                    try
                    {
                        // Look for the item by scrolling up
                        app.ScrollUpTo(z => z.Marked(itemName), x => x.Class("TextCellRenderer_TextCellView").Index(0), timeout: myDefaultTimeout);
                    }
                    catch (Exception exTriedUp)
                    {
                        // Tried going both directions... lets give up :(
                        throw exTriedUp;
                    }
                }

                // Click on the item
                app.Tap(x => x.Text(itemName));
            }

            // Some phones have a 'Done' button in their picker UI, search for and click on it
            CheckForDoneButton();

        }


        /// <summary>
		/// Look for a specifc item in a (OS specific) picker/dropdown, try scrolling up or down if you can't see it
		/// </summary>
		/// <param name="pickerName">Name of picker we're working with</param>
        /// <param name="itemName">Name of item we're looking for</param>
		public void ScrollPickerToJobTitle(string pickerName, string itemName)
        {
            // Xamarin.UITest.IApp: Method Members - https://developer.xamarin.com/api/type/Xamarin.UITest.IApp/
            // - IApp.ScrollDownTo()
            // - IApp.ScrollUpTo()
            // - IApp.Tap()
            // UI Test Timeouts - https://developer.xamarin.com/guides/testcloud/uitest/working-with/timeouts/


            // Click on the picker
            app.Tap(x => x.Marked(pickerName));

            // Try clicking on the item right away, if we see it
            AppResult[] results;
            if (platform == Platform.iOS)
            {
                //AppResult[] results = app.Query(c => c.Marked(itemName));
                results = app.Query(x => x.Class("UIPickerTableViewTitledCell").Child("UILabel").Marked(itemName).Index(0));
            }
            else
                results = app.Query(c => c.Marked(itemName));

            if (results.Any())
            {
                if (platform == Platform.iOS)
                {
                    //AppResult[] results = app.Query(c => c.Marked(itemName));
                    app.Tap(x => x.Class("UIPickerTableViewTitledCell").Child("UILabel").Marked(itemName).Index(0));
                }
                else
                    app.Tap(e => e.Marked(itemName));
            }
            else
            {

                // Lets start by scrolling down the list, if we can't find it we'll try going back up the list
                //  (maybe the item we want is higher then our current pose)
                try
                {
                    // Look for the item by scrolling down
                    if (platform == Platform.iOS)
                        //app.ScrollDownTo(z => z.Marked(itemName), x => x.Class("UIPickerTableView").Index(0), timeout: myDefaultTimeout);
                        app.ScrollDownTo(x => x.Class("UIPickerTableViewTitledCell").Child("UILabel").Marked(itemName).Index(0), x => x.Class("UIPickerTableView").Index(0), timeout: myDefaultTimeout);
                    // app.Tap(x => x.Class("UIPickerTableViewTitledCell").Child("UILabel").Marked("Banking").Index(0))  

                    else
                        app.ScrollDownTo(itemName, withinMarked: "select_dialog_listview");
                }
                catch
                {
                    // Didn't find it going down, lets try going up now...
                    try
                    {
                        // Look for the item by scrolling up
                        if (platform == Platform.iOS)
                            app.ScrollUpTo(z => z.Marked(itemName), x => x.Class("UIPickerTableView").Index(0), timeout: myDefaultTimeout);
                        else
                            app.ScrollUpTo(itemName, withinMarked: "select_dialog_listview");
                    }
                    catch (Exception exTriedUp)
                    {
                        // Tried going both directions... lets give up :(
                        throw exTriedUp;
                    }
                }

                // Click on the item
                app.Tap(x => x.Text(itemName));
            }

            // Some phones have a 'Done' button in their picker UI, search for and click on it
            CheckForDoneButton();

        }


        /// <summary>
        /// Some phones have a 'Done' button in their picker UI, search for and click on it
        /// </summary>
        public void CheckForDoneButton()
        {
            // Xamarin.UITest.IApp: Method Members - https://developer.xamarin.com/api/type/Xamarin.UITest.IApp/
            // - IApp.Tap()

            // Look for "Done"
            AppResult[] results = app.Query(c => c.Marked("Done"));
            if (results.Any()) // Did we find any?
                app.Tap(e => e.Marked("Done")); // click on it
        }

    }
}

