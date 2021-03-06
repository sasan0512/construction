﻿using Common;
using DataAccess;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebPages.Panels.UserPanel
{
    public partial class UsersMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UsersRepository repemplo = new UsersRepository();
                User user = repemplo.getUserById(Session["userid"].ToString().ToInt());

                Name.InnerText = user.FirstName + " " + user.LastName;
                MessageRepository repmsg = new MessageRepository();
                messageCount.InnerText = repmsg.CountUserNewMessages(Session["userid"].ToString().ToInt());
                ContactUsRepository repo = new ContactUsRepository();

                ContactWay cnw = repo.Findcwy(1);
                phone.InnerHtml = "<span><i class='fa fa-phone' style='margin-right: 7px'></i>" + cnw.PhoneNumber + "</span>";
                mail.InnerHtml = "<span><i class='fa fa-envelope-o' style='margin-right: 7px'></i>" + cnw.Email + "</span>";
                AboutUs.InnerHtml = cnw.AboutUs;
                contactEmail.InnerHtml = "<i class='fa fa-envelope'></i>" + cnw.Email;
                contactPhone.InnerHtml = "<i class='fa fa-phone'></i>" + cnw.PhoneNumber;
                contactHome.InnerHtml = "<i class='fa fa-home'></i>" + cnw.Adrees;
                logo.Attributes["style"] = "background-image:url(" + cnw.logo + ")";
            }
        }

        private string setLogoImage()
        {
            string ans = "";
            using (SqlConnection cn = new SqlConnection(OnlineTools.conString))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(string.Format("select logo from ContactWay"), cn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.Default))
                    {
                        if (dr.Read())
                        {
                            byte[] fileData = (byte[])dr.GetValue(0);
                            ans = "data:image/png;base64," + Convert.ToBase64String(fileData);
                        }

                        dr.Close();
                    }
                    cn.Close();
                }
            }
            return ans;
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Session.Remove("userid");
            Response.Redirect("/");
        }
    }
}