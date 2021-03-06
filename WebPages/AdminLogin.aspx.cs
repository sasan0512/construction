﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess.Repository;

namespace WebPages
{
    public partial class AdminLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillImageText();
            }
            while (Session["ImgValue"] == null)
            {
                FillImageText();
            }
        }

        private void FillImageText()
        {
            try
            {
                Random rdm = new Random();
                string combination = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                StringBuilder ImgValue = new StringBuilder();
                for (int i = 0; i < 5; i++)
                {
                    ImgValue.Append(combination[rdm.Next(combination.Length)]);
                    Session.Add("ImgValue", ImgValue.ToString());
                    btnImg.ImageUrl = "catchimage.aspx?";
                }
            }
            catch
            {
                throw;
            }
        }

        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Value) || string.IsNullOrEmpty(txtPassword.Value))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('نام کاربری یا رمز عبور را وارد نکردید ! ');", true);
                return;
            }

            if (Session["ImgValue"].ToString() != null)
                if (Session["ImgValue"].ToString() == txtImage.Value.ToUpper())
                {
                    //lblWarning.Text = "کد وارد شده صحیح می باشد";
                    //FillImageText();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('کد وارد شده صحیح نمی باشد ');", true);
                    txtImage.Value = "";
                    FillImageText();
                    return;
                }
            ///////////////////////////////////////////////////////////////////////////////////

            AdminsRepository ar = new AdminsRepository();
            int eid = ar.getAdminIDByUsername_Password(txtName.Value, txtPassword.Value);
            if (eid == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('نام کاربری یا رمز ورود اشتباه است ');", true);
                txtImage.Value = "";
                FillImageText();
                return;
            }
            else
            {
                Session.Add("adminid", 1);
                Session.Remove("userid");
                Session.Remove("employeeid");

                Response.Redirect("/Admin/Inbox");
            }
        }
    }
}