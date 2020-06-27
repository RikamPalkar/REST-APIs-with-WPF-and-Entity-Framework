using A.Utilities;
using DataAccessLayer;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;

namespace A
{
    class MainWindowViewModel : BindableBase, INotifyPropertyChanged
    {
        #region Properties

        private List<Employee> _employees;

        public List<Employee> Employees
        {
            get { return _employees; }
            set {SetProperty(ref _employees, value); }
        }

        private Employee _selectedEmployee;

        public Employee SelectedEmployee
        {
            get { return _selectedEmployee; }
            set { SetProperty(ref _selectedEmployee, value); }
        }

        private bool _isLoadData;

        public bool IsLoadData
        {
            get { return _isLoadData; }
            set { SetProperty(ref _isLoadData, value); }
        }

        private string _responseMessage = "Welcome to REST API Tutorials";

        public string ResponseMessage
        {
            get { return _responseMessage; }
            set { SetProperty(ref _responseMessage , value); }
        }

        #region [Create Employee Properties]

        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set { SetProperty(ref _firstName, value); }
        }


        private string _lastName;

        public string LastName
        {
            get { return _lastName; }
            set { SetProperty(ref _lastName, value); }
        }

        private string _gender;

        public string Gender
        {
            get { return _gender; }
            set { SetProperty(ref _gender, value); }
        }

        private int _salary;

        public int Salary
        {
            get { return _salary; }
            set { SetProperty(ref _salary, value); }
        }
        #endregion
        private bool _isShowForm;

        public bool IsShowForm
        {
            get { return _isShowForm; }
            set { SetProperty(ref _isShowForm, value); }
        }

        private string _showPostMessage = "Fill the form to register an employee!";

        public string ShowPostMessage
        {
            get { return _showPostMessage; }
            set { SetProperty(ref _showPostMessage, value); }
        }
        #endregion

        #region ICommands
        public DelegateCommand GetButtonClicked { get; set; }
        public DelegateCommand ShowRegistrationForm { get; set; }
        public DelegateCommand PostButtonClick { get; set; }
        public DelegateCommand<Employee> PutButtonClicked { get; set; }
        public DelegateCommand<Employee> DeleteButtonClicked { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initalize perperies & delegate commands
        /// </summary>
        public MainWindowViewModel()
        {
            GetButtonClicked = new DelegateCommand(GetEmployeeDetails);
            PutButtonClicked = new DelegateCommand<Employee>(UpdateEmployeeDetails);
            DeleteButtonClicked = new DelegateCommand<Employee>(DeleteEmployeeDetails);
            PostButtonClick = new DelegateCommand(CreateNewEmployee);
            ShowRegistrationForm = new DelegateCommand(RegisterEmployee);
         }
        #endregion

        #region CRUD
        /// <summary>
        /// Make visible Regiter user form
        /// </summary>
        private void RegisterEmployee()
        {
            IsShowForm = true;
        }

        /// <summary>
        /// Fetches employee details
        /// </summary>
        private void GetEmployeeDetails()
        {
            var employeeDetails = WebAPI.GetCall(API_URIs.employees);
            if (employeeDetails.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Employees = employeeDetails.Result.Content.ReadAsAsync<List<Employee>>().Result;
                IsLoadData = true;
            }
        }

        /// <summary>
        /// Adds new employee
        /// </summary>
        private void CreateNewEmployee()
        {
            Employee newEmployee = new Employee()
            {
                FirstName = FirstName,
                LastName = LastName,
                Gender = Gender,
                Salary = Salary
            };
            var employeeDetails = WebAPI.PostCall(API_URIs.employees, newEmployee);
            if (employeeDetails.Result.StatusCode == System.Net.HttpStatusCode.Created)
            {
                ShowPostMessage = newEmployee.FirstName + "'s details has successfully been added!";
            }
            else
            {
                ShowPostMessage = "Failed to update" + newEmployee.FirstName + "'s details.";
            }
        }


        /// <summary>
        /// Updates employee's record
        /// </summary>
        /// <param name="employee"></param>
        private void UpdateEmployeeDetails(Employee employee)
        {
            var employeeDetails = WebAPI.PutCall(API_URIs.employees+ "?id="+employee.ID, employee);
            if (employeeDetails.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                ResponseMessage = employee.FirstName + "'s details has successfully been updated!";
            }
            else
            {
                ResponseMessage = "Failed to update"+ employee.FirstName + "'s details.";
            }
        }

        /// <summary>
        /// Deletes employee's record
        /// </summary>
        /// <param name="employee"></param>
        private void DeleteEmployeeDetails(Employee employee)
        {
            var employeeDetails = WebAPI.DeleteCall(API_URIs.employees + "?id=" + employee.ID);
            if (employeeDetails.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                ResponseMessage = employee.FirstName + "'s details has successfully been deleted!";
            }
            else
            {
                ResponseMessage = "Failed to delete" + employee.FirstName + "'s details.";
            }
        }
        #endregion
    }
}
