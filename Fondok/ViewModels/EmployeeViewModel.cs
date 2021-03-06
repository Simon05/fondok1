﻿
using Fondok.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Fondok.Views.Windows;
using Fondok.Context;
using System.Data.Entity;
using System.Linq;
using System.Windows.Input;
using Fondok.Commands;
using System;
using System.Windows.Controls;
using System.Globalization;
using System.Windows;

namespace Fondok.ViewModels
{
    class EmployeeViewModel : INotifyPropertyChanged 
    {

        public EmployeeViewModel() : this(null) { }
        private DateTime? _futureValidatingDate;
        public EmployeeViewModel(Employee Employee)
        {
            EditEmployee = Employee;

            Employee.EmployeeDateOfBirth = DateTime.Now.Date;
        }
        private Employee _editEmployee;
        public Employee EditEmployee
        {
            get
            {
                return _editEmployee;
            }
            set
            {
                _editEmployee = value;
                NotifyPropertyChanged("EditEmployee");
            }
        }

         private int _EmployeeUserName;
       public int EmployeeUserName
        {
          get
          {
             return _EmployeeUserName;
           }
           set
           {
                _EmployeeUserName = value;
                NotifyPropertyChanged("EmployeeUserName");
          }
       }

        public DateTime? FutureValidatingDate
        {
            get { return _futureValidatingDate; }
            set
            {
                _futureValidatingDate = EditEmployee.EmployeeDateOfBirth;
                NotifyPropertyChanged("FutureValidatingDate");
            }
        }


        public bool Run()
        {
            EmployeeWindow sw = new EmployeeWindow();
            sw.DataContext = this;
            if (sw.ShowDialog() == true)
            {
                return true;
            }
            return false;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }





    class EmployeeRepository
    {

        DatabaseContext db = new DatabaseContext();
        public EmployeeRepository(DatabaseContext _db)
        {
            db = _db;
            db.Employees.Load();
        }
        public System.ComponentModel.BindingList<Employee> GetAllEmployees()
        {
            return db.Employees.Local.ToBindingList();
        }
        public void AddEmployee(Employee Employee)
        {
            db.Employees.Add(Employee);
            db.SaveChanges();
        }
        public Employee GetEmployee(int id)
        {
            return db.Employees.Where(b => b.EmployeeID.Equals(id)).First();
        }
        public void UpdateEmployee(int EmployeeID, string EmployeeUserName, string EmployeePassWord, string EmployeeEMail
            , string EmployeeJob, string EmployeeFirstName, string EmployeeLastName, DateTime EmployeeDateOfBirth)
        {
            Employee Employee = GetEmployee(EmployeeID);
            

            Employee.EmployeeUserName = EmployeeUserName;
            Employee.EmployeePassWord = EmployeePassWord;
            Employee.EmployeeEMail = EmployeeEMail;
            Employee.EmployeeJob = EmployeeJob;
            Employee.EmployeeFirstName = EmployeeFirstName;
            Employee.EmployeeLastName = EmployeeLastName;
            Employee.EmployeeDateOfBirth = EmployeeDateOfBirth.Date;

            db.SaveChanges();
        }
        public void UpdateEmployee(Employee b)
        {
            UpdateEmployee(b.EmployeeID, b.EmployeeUserName, b.EmployeePassWord, b.EmployeeEMail
                , b.EmployeeJob, b.EmployeeFirstName, b.EmployeeLastName, b.EmployeeDateOfBirth);
        }
        public void DeleteEmployee(int id)
        {
            db.Employees.Remove(GetEmployee(id));
            db.SaveChanges();
        }
    }













    class EmployeeLibraryViewModel : INotifyPropertyChanged
    {
        private EmployeeRepository rep;
        private DatabaseContext db;
        private BindingList<Employee> _Employees;
        public BindingList<Employee> Employees
        {
            get
            {
                return _Employees;
            }
            set
            {
                _Employees = value;
                NotifyPropertyChanged("Employees");
            }
        }
        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get
            {
                return _selectedEmployee;
            }
            set
            {
                _selectedEmployee = value;
                NotifyPropertyChanged("SelectedEmployee");
            }
        }
        public EmployeeLibraryViewModel()
        {
            db = new DatabaseContext();
            rep = new EmployeeRepository(db);
            Employees = rep.GetAllEmployees();
            deleteCommand = new DelegateCommand(DeleteEmployee);
            updateCommand = new DelegateCommand(UpdateEmployee);
            createCommand = new DelegateCommand(CreateEmployee);
        }
        public bool IsSelected()
        {
            return SelectedEmployee != null;
        }
        private ICommand deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                return deleteCommand;
            }
        }
        public void DeleteEmployee()
        {
            if (!IsSelected())
            {
                return;
            }
            rep.DeleteEmployee(SelectedEmployee.EmployeeID);
        }
        private DelegateCommand updateCommand;
        public ICommand UpdateCommand
        {
            get
            {
                return updateCommand;
            }
        }
        public void UpdateEmployee()
        {
            if (!IsSelected())
            {
                return;
            }
            EmployeeViewModel bwvm = new EmployeeViewModel(SelectedEmployee);
            if (bwvm.Run())
            {
                rep.UpdateEmployee(SelectedEmployee);
            }
        }
        private DelegateCommand createCommand;
        public ICommand CreateCommand
        {
            get
            {
                return createCommand;
            }
        }
        public void CreateEmployee()
        {
            Employee bk = new Employee();

            EmployeeViewModel bwvm = new EmployeeViewModel(bk);
        
            if (bwvm.Run() && bk.EmployeeDateOfBirth < DateTime.Now && bk.EmployeeFirstName != null && bk.EmployeeLastName != null && bk.EmployeeUserName != null
                && bk.EmployeePassWord != null && bk.EmployeeJob != null && bk.EmployeeEMail != null)
            {
                //if (bk.EmployeeFirsName == null)
                //{
                //    MessageBox.Show("gggggggggggggggggg");
                //}
                rep.AddEmployee(bk);
            }
            else
            {
                MessageBox.Show("Hello");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}