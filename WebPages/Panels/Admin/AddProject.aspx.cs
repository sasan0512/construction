﻿using Common;
using DataAccess;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Web.UI.HtmlControls;

namespace WebPages.Panels.Admin
{
    public partial class AddProject : System.Web.UI.Page
    {
        public static int[] Ids { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["adminid"] != null)
            {
                if (!IsPostBack)
                {
                    btnSave.Enabled = false;
                    diverror.InnerText = "هیچ گروهی انتخاب نشده!";
                    //GroupsRepository repo = new GroupsRepository();
                    //DDLGroups.DataSource = repo.LoadAllGroups();
                    //DDLGroups.DataTextField = "Title";
                    //DDLGroups.DataValueField = "GroupID";
                    //DDLGroups.DataBind();
                    //DDLGroups.Items.Insert(0, new ListItem("یک گروه انتخاب کنید", "-2"));
                }
            }
            else
            {
                Response.Redirect("/AdminLogin");
            }
        }

        protected void gvSelected_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "view")
            {
            }
        }

        [WebMethod]
        public static List<Group> getGroups()
        {
            DataTable dt = new DataTable();
            List<Group> objDept = new List<Group>();
            GroupsRepository jg = new GroupsRepository();
            dt = jg.LoadAllGroups();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objDept.Add(new Group
                    {
                        GroupID = Convert.ToInt32(dt.Rows[i][0]),
                        Title = dt.Rows[i][1].ToString(),
                        FatherID = Convert.ToInt32(dt.Rows[i][2])
                    });
                }
            }
            return objDept;
        }

        [WebMethod]
        //[ScriptMethod(UseHttpGet = true)]
        public static List<Group> getSubgroups(string Id)
        {
            DataTable dt = new DataTable();
            List<Group> objDept = new List<Group>();
            GroupsRepository jg = new GroupsRepository();

            try
            {
                //Page page = (Page)HttpContext.Current.Handler;
                //DropDownList DDLGroups = (DropDownList)page.FindControl("DDLGroups");

                dt = jg.LoadSubGroup(Id.ToInt());

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        objDept.Add(new Group
                        {
                            GroupID = Convert.ToInt32(dt.Rows[i][0]),
                            Title = dt.Rows[i][1].ToString(),
                        });
                    }
                }
                else
                {
                    Group gg = jg.FindGroup(Id.ToInt());
                    objDept.Add(new Group
                    {
                        GroupID = gg.GroupID,
                        Title = gg.Title
                    }
                        );
                }
                return objDept;
            }
            catch (Exception e)

            {
                Debug.Unindent();

                Debug.Print(e.ToString());
                Debug.Indent();
                Debug.WriteLine(e.ToString());
                Debug.Flush();

                Show(e.ToString());
            }

            return objDept;
        }

        public static void Show(string message)
        {
            Debug.Print(message);
            Debug.Indent();
            Debug.WriteLine(message);
            // Cleans the message to allow single quotation marks
            string cleanMessage = message.Replace("'", "\\'");
            string script = "<script type=\"text/javascript\">alert('" + cleanMessage + "');</script>";

            // Gets the executing web page
            Page page = HttpContext.Current.CurrentHandler as Page;

            // Checks if the handler is a Page and that the script isn't allready on the Page
            if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
            {
                page.ClientScript.RegisterClientScriptBlock(typeof(Page), "alert", script);
            }
        }

        //protected void DDLGroups_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallMyFunction", "ddlGroups()", true);
        //    //GroupsRepository repo = new GroupsRepository();
        //    //if (DDLGroups.SelectedValue != "-2")
        //    //{
        //    //    DataTable DT = new DataTable();

        //    //    DT = repo.LoadSubGroup(DDLGroups.SelectedValue.ToInt());

        //    //    if ((DT.Rows.Count > 0))
        //    //    {
        //    //        SubGroups.DataSource = DT;
        //    //        SubGroups.DataTextField = "Title";
        //    //        SubGroups.DataValueField = "GroupID";
        //    //        SubGroups.DataBind();
        //    //        NoItemDiv.InnerText = "";
        //    //    }
        //    //    else
        //    //    {
        //    //        SubGroups.Items.Clear();
        //    //        SubGroups.Items.Insert(0, new ListItem(DDLGroups.SelectedItem.ToString(), DDLGroups.SelectedValue.ToString()));
        //    //        NoItemDiv.InnerText = "این گروه هیچ زیر گروهی ندارد،میتوانید نام گروه را اضافه کنید";
        //    //        NoItemDiv.Attributes["class"] = "textok";
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    SubGroups.Items.Clear();
        //    //    NoItemDiv.InnerText = "";
        //    //}
        //}

        /// /////////////////////////////////////////////////////////////////////

        //protected void AddToSub_Click(object sender, EventArgs e)
        //{
        //    if (SubGroups.SelectedIndex != -1)
        //    {
        //        bool isadd = false;
        //        string text = SubGroups.SelectedItem.Text;
        //        for (int i = 0; i < SelectedSubGroups.Items.Count; i++)
        //        {
        //            if (SelectedSubGroups.Items[i].Text == text)
        //            {
        //                isadd = true;
        //            }
        //        }
        //        if (!isadd)
        //        {
        //            SelectedSubGroups.Items.Add(text);
        //            SelectedSubGroups.Items[SelectedSubGroups.Items.Count - 1].Value = SubGroups.SelectedItem.Value;
        //            btnSave.Enabled = true;
        //            diverror.InnerText = "";
        //            NoItemDiv.InnerText = "";
        //        }
        //        else
        //        {
        //            NoItemDiv.InnerText = "این مورد قبلا اضافه شده است!";
        //            NoItemDiv.Attributes["class"] = "error";
        //        }
        //    }
        //    else
        //    {
        //        NoItemDiv.InnerText = "شما هیچ موردی را انتخاب نکرده اید!";
        //        NoItemDiv.Attributes["class"] = "error";
        //    }
        //}
        ////////////////////////////////////////////////////////////////////////////////////////////////
        //protected void RemoveFromSub_Click(object sender, EventArgs e)
        //{
        //    if (SelectedSubGroups.SelectedIndex != -1)
        //    {
        //        SelectedSubGroups.Items.RemoveAt(SelectedSubGroups.SelectedIndex);
        //        NoItemDiv.InnerText = "";
        //        if (SelectedSubGroups.Items.Count == 0)
        //        {
        //            btnSave.Enabled = false;
        //            diverror.InnerText = "هیچ گروهی انتخاب نشده!";
        //        }
        //    }
        //    else
        //    {
        //        NoItemDiv.InnerText = "شما هیچ موردی را انتخاب نکرده اید!";
        //        NoItemDiv.Attributes["class"] = "error";
        //    }
        //}

        [WebMethod]
        public static void selectedGroups(int[] list)
        {
            Ids = list;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if ((String.IsNullOrEmpty(editor1.Text) == false) &&
                (Abstract.Text.Length >= 130) &&
                (FileUpload1.HasFile) &&
                (String.IsNullOrEmpty(title.Text) == false) &&
                (String.IsNullOrEmpty(Abstract.Text) == false) &&
                (String.IsNullOrEmpty(Tags.Text) == false) &&
                (String.IsNullOrEmpty(KeyWords.Text) == false))
            {
                if (FileUpload1.FileBytes.Length > 1024 * 1024)
                {
                    diverror.InnerHtml = "حجم فایل بارگذاری شده بیشتر از 1 مگابایت است!";
                    return;
                }
                string ext = Path.GetExtension(FileUpload1.FileName).ToLower();
                if ((ext != ".jpg") && (ext != ".png"))
                {
                    diverror.InnerHtml = "فرمت فایل بارگذاری شده باید .jpg  یا .png  باشد!";
                    return;
                }

                Project ART = new Project();
                ART.Title = title.Text;
                ART.Content = editor1.Text;

                string filename = Path.GetFileName(FileUpload1.FileName);
                string rand = DBManager.CurrentTimeWithoutColons() + DBManager.CurrentPersianDateWithoutSlash();
                filename = rand + filename;
                string ps = Server.MapPath(@"~\img\") + filename;
                FileUpload1.SaveAs(ps);
                FileStream fStream = File.OpenRead(ps);
                byte[] contents = new byte[fStream.Length];
                fStream.Read(contents, 0, (int)fStream.Length);
                fStream.Close();

                ART.Image = "/img/" + filename;
                System.Drawing.Image img = imgResize.ToImage(contents);
                System.Drawing.Image image = imgResize.Resize(img, 466, 466);
                string stream = Server.MapPath(@"~\img\") + "s" + filename;
                switch (FileUpload1.FileName.Substring(FileUpload1.FileName.IndexOf('.') + 1).ToLower())
                {
                    case "jpg":
                        image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;

                    case "jpeg":
                        image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;

                    case "png":
                        image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                }

                ART.ImgFisrtPage = "/img/" + "s" + filename;

                ART.Abstract = Abstract.Text;
                ART.PostDateTime = OnlineTools.persianFormatedDate();

                ART.Tags = Tags.Text;
                ART.KeyWords = KeyWords.Text;
                ProjectsRepository ARTRep = new ProjectsRepository();
                if (ARTRep.SaveProject(ART))
                {
                    bool result = true;
                    ProjectConRepository GRConRepo = new ProjectConRepository();
                    List<int> SelectedSubGroupsList = new List<int>();

                    int c = Ids.Count();

                    int lastid = ARTRep.GetLastProjectID();
                    Session.Add("ProjectLastIDForEmployeeFilter", lastid);

                    if (c > 0)
                    {
                        for (int i = 0; i < c; i++)
                        {
                            ProjectConnection GC = new ProjectConnection();
                            GC.ProjectID = lastid;
                            GC.GroupID = Ids[i];
                            if (!GRConRepo.SaveProjectCon(GC))
                            {
                                result = false;
                            }
                        }
                    }
                    else
                    {
                        diverror.InnerText = "هیچ زیر گروهی انتخاب نشده است!";
                    }

                    if (!result)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('مشکلی در زمان ثبت به وجود آمد،لطفا دوباره سعی کنید یا با پشتیبانی تماس بگیرید ! ');window.location ='/Admin/ManageProjects'", true);
                    }
                    else
                    {
                        Response.Redirect("/Admin/AddProject/AddEmployeeToProject");
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('مشکلی در زمان ثبت به وجود آمد،لطفا دوباره سعی کنید یا با پشتیبانی تماس بگیرید ! ');window.location ='/Admin/ManageProjects'", true);
                }
            }
        }
    }
}