﻿using Common;
using DataAccess;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebPages.Panels.Admin
{
    public partial class EditPost : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["adminid"] != null)
            {
                if (!IsPostBack)
                {
                    if (Session["PostIDForEdit"] != null)
                    {
                        int id = Session["PostIDForEdit"].ToString().ToInt();
                        Session.Add("newPostIDForEdit", id);
                        Session.Remove("PostIDForEdit");
                        ArticleRepository repArt = new ArticleRepository();
                        GroupsRepository repo = new GroupsRepository();
                        Article art = repArt.FindeArticleByID(id);
                        title.Text = art.Title;
                        Abstract.Text = art.Abstract;
                        editor1.Text = art.Content;
                        KeyWords.Text = art.KeyWords;
                        Tags.Text = art.Tags;
                        SelectedSubGroups.DataSource = repo.FindSubGroupsOfAnArticle(id);
                        SelectedSubGroups.DataTextField = "Title";
                        SelectedSubGroups.DataValueField = "GroupID";
                        SelectedSubGroups.DataBind();
                        for (int i = 0; i < SelectedSubGroups.Items.Count; i++)
                        {
                            if (SelectedSubGroups.Items[i].Value == "-1")
                            {
                                SelectedSubGroups.Items[i].Text = "گروه : " + SelectedSubGroups.Items[i].Text;
                            }
                        }

                        DDLGroups.DataSource = repo.LoadAllGroups();
                        DDLGroups.DataTextField = "Title";
                        DDLGroups.DataValueField = "GroupID";
                        DDLGroups.DataBind();
                        DDLGroups.Items.Insert(0, new ListItem("یک گروه انتخاب کنید", "-2"));
                        oldPhoto.ImageUrl = art.ImgFirstPage;
                    }
                    else
                    {
                        Response.Redirect("/Admin/ManageBlogs");
                    }
                }
            }
            else
            {
                Response.Redirect("/AdminLogin");
            }
        }

        protected void DDLGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            GroupsRepository repo = new GroupsRepository();
            DataTable DT = new DataTable();
            if (DDLGroups.SelectedValue != "-2")
            {
                DT = repo.LoadSubGroup(DDLGroups.SelectedValue.ToInt());

                if ((DT.Rows.Count > 0))
                {
                    SubGroups.DataSource = DT;
                    SubGroups.DataTextField = "Title";
                    SubGroups.DataValueField = "GroupID";
                    SubGroups.DataBind();
                    NoItemDiv.InnerText = "";
                }
                else
                {
                    SubGroups.Items.Clear();
                    SubGroups.Items.Insert(0, new ListItem(DDLGroups.SelectedItem.ToString(), DDLGroups.SelectedValue.ToString()));
                    NoItemDiv.InnerText = "این گروه هیچ زیر گروهی ندارد،میتوانید نام گروه را اضافه کنید";
                    NoItemDiv.Attributes["class"] = "textok";
                }
            }
            else
            {
                SubGroups.Items.Clear();
                NoItemDiv.InnerText = "";
            }

        }

        protected void AddToSub_Click(object sender, EventArgs e)
        {
            if (SubGroups.SelectedIndex != -1)
            {
                bool isadd = false;
                string text = SubGroups.SelectedItem.Value;
                for (int i = 0; i < SelectedSubGroups.Items.Count; i++)
                {
                    if (SelectedSubGroups.Items[i].Value == text)
                    {
                        isadd = true;
                    }
                }
                if (!isadd)
                {
                    SelectedSubGroups.Items.Add(SubGroups.SelectedItem.Text);
                    SelectedSubGroups.Items[SelectedSubGroups.Items.Count - 1].Value = SubGroups.SelectedItem.Value;
                    btnSave.Enabled = true;
                    diverror.InnerText = "";
                    NoItemDiv.InnerText = "";
                }
                else
                {
                    NoItemDiv.InnerText = "این مورد قبلا اضافه شده است!";
                    NoItemDiv.Attributes["class"] = "error";
                }
            }
            else
            {
                NoItemDiv.InnerText = "شما هیچ موردی را انتخاب نکرده اید!";
                NoItemDiv.Attributes["class"] = "error";
            }
        }

        protected void RemoveFromSub_Click(object sender, EventArgs e)
        {
            if (SelectedSubGroups.SelectedIndex != -1)
            {
                SelectedSubGroups.Items.RemoveAt(SelectedSubGroups.SelectedIndex);
                NoItemDiv.InnerText = "";
                if (SelectedSubGroups.Items.Count == 0)
                {
                    btnSave.Enabled = false;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('هیچ گروهی انتخاب نشده!')", true);
                    diverror.InnerText = "هیچ گروهی انتخاب نشده!";
                }
            }
            else
            {
                NoItemDiv.InnerText = "شما هیچ موردی را انتخاب نکرده اید!";
                NoItemDiv.Attributes["class"] = "error";
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!(String.IsNullOrEmpty(editor1.Text) ||
                String.IsNullOrEmpty(title.Text) ||
                String.IsNullOrEmpty(Abstract.Text) ||
                String.IsNullOrEmpty(Tags.Text) ||
                String.IsNullOrEmpty(KeyWords.Text) || SelectedSubGroups.Items.Count == 0 || Abstract.Text.Count() < 130))
            {
                if (Session["newPostIDForEdit"] != null)
                {


                    int id = Session["newPostIDForEdit"].ToString().ToInt();
                    Session.Remove("newPostIDForEdit");
                    ArticleRepository repArt = new ArticleRepository();
                    GroupsRepository repo = new GroupsRepository();
                    Article art = repArt.FindeArticleByID(id);

                    art.Title = title.Text;
                    art.Content = editor1.Text;

                    if (FileUpload1.HasFile)
                    {
                        if (FileUpload1.FileBytes.Length > 1024 * 1024)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('حجم فایل بارگذاری شده بیشتر از 1 مگابایت است!')", true);

                            return;
                        }
                        string ext = Path.GetExtension(FileUpload1.FileName).ToLower();
                        if ((ext != ".jpg") && (ext != ".png"))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('فرمت فایل بارگذاری شده باید .jpg  یا .png  باشد!')", true);

                            return;
                        }
                        string filename = Path.GetFileName(FileUpload1.FileName);
                        string rand = DBManager.CurrentTimeWithoutColons() + DBManager.CurrentPersianDateWithoutSlash();
                        filename = rand + filename;
                        string ps = Server.MapPath(@"~\img\") + filename;
                        FileUpload1.SaveAs(ps);
                        FileStream fStream = File.OpenRead(ps);
                        byte[] contents = new byte[fStream.Length];
                        fStream.Read(contents, 0, (int)fStream.Length);
                        fStream.Close();
                        FileInfo fi = new FileInfo(Server.MapPath(@"~\img\") + art.Image.Substring(5));
                        fi.Delete();
                        FileInfo fil = new FileInfo(Server.MapPath(@"~\img\") + art.ImgFirstPage.Substring(5));
                        fil.Delete();
                        art.Image = "/img/" + filename;
                        System.Drawing.Image img = imgResize.ToImage(contents);
                        System.Drawing.Image image = imgResize.Resize(img, 358, 358);



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

                        art.ImgFirstPage = "/img/" + "s" + filename;
                    }


                    art.Abstract = Abstract.Text;
                    art.Visits = 0;
                    art.Tags = Tags.Text;
                    art.KeyWords = KeyWords.Text;
                    ArticleRepository ARTRep = new ArticleRepository();
                    if (ARTRep.SaveArticle(art))
                    {
                        bool result = true;
                        GroupsConRepository GRConRepo = new GroupsConRepository();
                        GRConRepo.DeletArticleConnections(id);
                        List<int> SelectedSubGroupsList = new List<int>();

                        int lastid = id;
                        int count = SelectedSubGroups.Items.Count;
                        if (count > 0)
                        {
                            for (int i = 0; i < count; i++)
                            {
                                GroupConnection GC = new GroupConnection();
                                GC.ArticleID = lastid;
                                GC.GroupID = SelectedSubGroups.Items[i].Value.ToInt();
                                if (!GRConRepo.SaveGroupCon(GC))
                                {
                                    result = false;
                                }
                                else
                                {
                                    Response.Redirect("/Admin/ManageBlogs");
                                }
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('هیچ زیر گروهی انتخاب نشده است!')", true);
                        }

                        if (!result)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('مشکلی در زمان ثبت به وجود آمد،لطفا دوباره سعی کنید یا با پشتیبانی تماس بگیرید ! ');window.location ='/Admin/ManageBlogs'", true);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('ثبت با موفقیت انجام شد!');window.location ='/Admin/ManageBlogs'", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('مشکلی در زمان ثبت به وجود آمد،لطفا دوباره سعی کنید یا با پشتیبانی تماس بگیرید ! ');window.location ='/Admin/ManageBlogs'", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert(' مشکلی در زمان لود کردن به وجود آمد دوباره سعی کنید ! ');window.location ='/Admin/ManageBlogs'", true);
                }
            }
        }
    }
}