using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UITest_ListviewScroll
{
    class Employee
    {
        public ImageSource ImageSource { get; set; }
        public string Name { get; set; }
        public string Job { get; set; }

        public Employee(string _imageSource, string _name, string _job)
        {
            ImageSource = ImageSource.FromFile(_imageSource);
            Name = _name;
            Job = _job;
        }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageDetail : ContentPage
    {

        List<Employee> employeeListFull;  // list of every employee
        string[] jobTitles; // array of possible job titles
        Random random;  // we'll use Random to pick job titles and employee images

        public MainPageDetail()
        {
            InitializeComponent();

            random = new Random();
            employeeListFull = new List<Employee>();

            // Create some sample employees
            for (int i = 0; i < 250; i++)
            {
                employeeListFull.Add(
                    new Employee(
                        GetRandomFace(),
                        "Employee " + (i + 1),
                        GetRandomJobTitle()
                        )
                    );
            }

            // Set the picker source
            TitlePicker.ItemsSource = jobTitles;

            // TitlePicker Selection Change
            TitlePicker.SelectedIndexChanged += TitlePickerSelectionChanged;

            // Set the listview source
            EmployeeListview.ItemsSource = employeeListFull;
        }

        /// <summary>
        /// Whenever a new job title is selected, update the listview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void TitlePickerSelectionChanged(object sender, EventArgs args)
        {
            // Which item was selected?
            switch (TitlePicker.SelectedIndex)
            {
                case 0: // (Show All) was selected, set the listview's source to the full employee list
                    EmployeeListview.ItemsSource = employeeListFull;
                    break;
                default:
                    // a specific job title was selected, query the full list for all employees who match
                    IEnumerable<Employee> employeeListSorted = employeeListFull.Where(employeeListFull => employeeListFull.Job == TitlePicker.Items[TitlePicker.SelectedIndex]);
                    // set the listview's source to the sorted employee list
                    EmployeeListview.ItemsSource = employeeListSorted;
                    break;
            }
        }


        /// <summary>
        /// Return the name of a random face image
        /// </summary>
        /// <returns></returns>
        string GetRandomFace() { return "face" + random.Next(1, 11) + ".png"; }

        /// <summary>
        /// Return a randomly selected job title
        /// This will also build the JobTitle array if hasn't been populated yet
        /// </summary>
        /// <returns></returns>
        string GetRandomJobTitle()
        {
            // if we don't have any titles defined yet, lets build the list
            if (jobTitles == null)
            {
                jobTitles = new string[] {
                    "(Show All)",
                    "Administrative",
                    "Banking",
                    "Construction",
                    "Consulting",
                    "Corporate",
                    "Customer Service",
                    "Director",
                    "Engineering",
                    "Event Planning",
                    "Fashion",
                    "Hospitality",
                    "Human Resources",
                    "Information Technology (IT)",
                    "Insurance",
                    "Legal",
                    "Maintenance",
                    "Manager",
                    "Manufacturing",
                    "Market Research",
                    "Marketing",
                    "Public Relations",
                    "Purchasing",
                    "QA",
                    "Sales",
                    "Social Media",
                    "Transportation",
                    "Travel"
                };
            }

            // return a random job title, skipping the first item (Show All)
            return jobTitles[random.Next(1, jobTitles.Length - 1)];

        }
    }
}