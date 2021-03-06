﻿using Fondok.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Fondok.Models;

namespace Fondok.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
        }




        private void LoginClick(object sender, RoutedEventArgs e)


        {
            
            using (var context = new DatabaseContext())
            {
                var EmployeesList = (from s in context.Employees where s.EmployeeUserName == this.userNameField.Text select s).ToList<Employee>();


                if (EmployeesList[0].EmployeeUserName == this.userNameField.Text && EmployeesList[0].EmployeePassWord == this.userPasswordField.Password)
                {
                    MessageBox.Show(" Welcome: "  + EmployeesList[0].EmployeeJob + ", " + EmployeesList[0].EmployeeFirstName + " " + EmployeesList[0].EmployeeLastName);
                    Application.Current.MainWindow.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Error");
                }

                //MessageBox.Show(EmployeesList[0].EmployeeUserName + EmployeesList[0].EmployeeEMail + EmployeesList.Count());



            }


           
            //MessageBox.Show(a.ToString());
            /*
            SQLiteDatabase DB = new SQLiteDatabase();

            DataTable Login;

            String Query = "select * from Employees where EmployeeUserName='" + this.userNameField.Text + "' and EmployeePassWord='" + this.userPasswordField.Password + "'";

            Login = DB.GetDataTable(Query);

            string Grade = "select EmployeeJob from Employees where EmployeeUserName='" + this.userNameField.Text + "' ";
            string User = "select EmployeeUserName from Employees where EmployeeUserName='" + this.userNameField.Text + "' ";
            string Pass = "select EmployeePassWord from Employees where EmployeePassWord='" + this.userPasswordField.Password + "' ";

            string _Grade = DB.ExecuteScalar(Grade);
            string _UserName = DB.ExecuteScalar(User);
            string _Password = DB.ExecuteScalar(Pass);

            if (this.userNameField.Text != "" || this.userPasswordField.Password != "")
            {
                if (this.userNameField.Text == _UserName || this.userPasswordField.Password == _Password)
                {
                    foreach (DataRow r in Login.Rows)
                    {
                        MessageBox.Show(String.Format("Welcome {0} : {1}", _Grade, _UserName));
                        Application.Current.MainWindow.Show();
                        this.Hide();
                    }
                }
                else
                {
                    MessageBox.Show("Please Chek Your Auth Informations, Or Contact Your Developper!");

                }
            }
            else
            {
                MessageBox.Show("Please Fill The Required Fields! \n Username \n Password");

            }
            */

            //App.Current.MainWindow.Show();
            //MainWindow _MainWindow = new MainWindow();
            //Application.Current.MainWindow = _MainWindow;
            //this.Hide();
            

        }

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            //MessageBox.Show("Good Bye From LoginWindow :)!");
            MessageBoxResult result = MessageBox.Show("Are you sure to Exit? LoginWindow", "Exit from Fondok", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    MessageBox.Show("Good Bye :) Login", "Fondok");

                    Application.Current.Shutdown();
                    Process.GetCurrentProcess().Kill();

                    break;
                case MessageBoxResult.No:
                    e.Cancel = true;
                    break;
            }

            

        }
    }
}
