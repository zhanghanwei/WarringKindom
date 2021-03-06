﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web.Security;

namespace Warring_Kingdom
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void RegBtn_Click(object sender, EventArgs e)
        {
            Form regForm = new RegForm();
            regForm.ShowDialog();
        }

        private void LoginBtn_Click(object sender, EventArgs e)
        {
            if (checkInput())
            {
                if (checkLogin())
                {
                    Form gameForm = new GameForm(this.usrBox.Text);
                    gameForm.ShowDialog();
                    this.Close();
                }
            }
        }

        private bool checkInput()
        {
            String error = "";
            if (this.usrBox.Text.Equals(""))
            {
                error = "Username is required";
            }
            else if (this.pwdBox.Text.Equals(""))
            {
                error = "Password is required";
            }
            if (error.Equals(""))
            {
                return true;
            }
            else
            {
                MessageBox.Show(error);
                return false;
            }
        }

        private bool checkLogin()
        {
            try
            {
                // connect to the server
                String connectStr = "server=titan.csse.rose-hulman.edu; uid=wkuser; pwd=wkuser; database=WarKing";
                SqlConnection conn = new SqlConnection(connectStr);
                conn.Open();
                // insert the user information into the database
                try
                {
                    // check if username already exist
                    SqlCommand checkComm = new SqlCommand("EXEC CheckExistingUser @username='" + this.usrBox.Text + "', @userpassword='" + FormsAuthentication.HashPasswordForStoringInConfigFile(this.pwdBox.Text, "MD5") + "'", conn);
                    SqlDataReader reader = checkComm.ExecuteReader();
                    if (reader.Read())
                    {
                        if ((int)reader.GetValue(0)!=0)
                        {
                            MessageBox.Show("Incorrect password");
                            conn.Close();
                            return false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Username does not exist");
                        conn.Close();
                        return false;
                    }
                    reader.Close();

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                    conn.Close();
                    return false;
                }
                // close the connection
                conn.Close();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return false;
            }
        }
    }
}
