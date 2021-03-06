﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Panels/Admin/NewAdminMaster.Master" AutoEventWireup="true" CodeBehind="Scores.aspx.cs" Inherits="WebPages.Panels.Admin.Scores" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>امتیازات</title>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageStyle" runat="server">
    <link href="../../_Styles/GridView.css" rel="stylesheet" />
    <style>
        .myContainer {
            padding: 30px 100px;
        }

        .title {
            text-align: right;
            margin-bottom: 15px;
        }

        #Content_btnSearch {
            height: 38px;
        }

        @media (max-width: 414px) {
            .myContainer {
                padding: 30px 5%;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Content" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <style>
        div#container {
            min-height: 500px;
        }

        .InnerContainer {
            min-height: 300px;
            width: 100%;
            box-shadow: 5px 4px 10px #858585;
            padding: 1px 5px;
        }
    </style>
    <div id="container">

        <div class="InnerContainer">
            <div class="myContainer">
                <div class="row">
                    <div class="col-md-12 title">
                        <h3>لیست کارمندان</h3>
                    </div>
                </div>
                <div style="direction: rtl">
                    <div class="form-group" style="clear: right;">
                        <label style="display: block" for="DDLJobGroup">گروه کاری : </label>
                        <asp:DropDownList ID="DDLJobGroup" OnSelectedIndexChanged="DDLJobGroup_SelectedIndexChanged" AutoPostBack="true" CssClass="DDLClass" runat="server"></asp:DropDownList>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <div class="form-group" style="">
                                <label style="display: block" for="DDLJob">کار : </label>
                                <asp:DropDownList ID="DDLJob" OnSelectedIndexChanged="DDLJob_SelectedIndexChanged" AutoPostBack="true" CssClass="DDLClass" runat="server"></asp:DropDownList>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="DDLJobGroup" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="col-md-4 col-xs-12 text-righ" style="float: right; height: 50px">
                    <div class="input-group">
                        <span class="input-group-btn">

                            <asp:Button ID="btnSearch" OnClick="btnSearch_ServerClick" CssClass="btn btn-primary" runat="server" Text="جستجو" />
                        </span>

                        <div id="ContentPlaceHolder1_upSearch">

                            <input name="ctl00$ContentPlaceHolder1$tbxnameSearch" style="height: 38px" runat="server" placeholder="متن جستجو"
                                type="text" maxlength="50" id="tbxSearch" class="form-control text-right dirRight pull-right" />
                        </div>
                    </div>
                </div>
                <div class="col-md-2 col-xs-12 text-right ">
                    <asp:Button ID="btnSabt" CssClass="btn btn-success" OnClick="btnSabt_ServerClick" runat="server" Text="ثبت" />
                </div>
                <div class="row">
                    <div class="col-md-12 col-xs-12" style="overflow-x: auto">

                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvEmployees" runat="server" BackColor="White" BorderColor="#CCCCCC"
                                    BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black"
                                    GridLines="Horizontal" AutoGenerateColumns="False" CssClass="dirRight table"
                                    HorizontalAlign="Center" AllowPaging="True" PageSize="20" OnRowDataBound="gvEmployees_RowDataBound">
                                    <Columns>

                                        <asp:BoundField DataField="EmployeeID" HeaderText="شناسه" />
                                        <asp:BoundField DataField="UserName" HeaderText="نام کاربری" />

                                        <asp:BoundField DataField="FullName" HeaderText="نام" />
                                        <asp:BoundField DataField="StateCity" HeaderText="استان و شهر" />

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                امتیاز
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <input type="number" id="Score" name="quantity" runat="server" min="0" max="100" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                    <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle HorizontalAlign="center" CssClass="GridPager" />
                                    <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                    <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                    <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                    <SortedDescendingHeaderStyle BackColor="#242121" />
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSabt" EventName="click" />
                                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="click" />
                                <asp:AsyncPostBackTrigger ControlID="btnViewAll" EventName="click" />
                                <asp:AsyncPostBackTrigger ControlID="DDLJob" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                        <asp:UpdateProgress ID="updateProgress1" runat="server" DisplayAfter="0">
                            <ProgressTemplate>
                                <div style="position: fixed; text-align: center; height: 100%; padding-top: 100px; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #ffffff; opacity: 0.8;">
                                    <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/_construction/images/44frgm.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; top: 45%; left: 50%;" />
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-5 col-md-push-7 col-xs-6 col-xs-push-6">

                        <asp:Button ID="btnViewAll" OnClick="btnViewAll_Click" CssClass="btn btn-auto-h btn-info goRight" runat="server" Text="مشاهده تمام کاربران" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Script" runat="server">
    <script>
        $(document).ready(function () {
            var score = document.getElementById("Content_gvEmployees_Score_0");

            score.addEventListener("input", function (event) {
                if (score.validity.rangeUnderflow) {
                    score.setCustomValidity("عدد وارد شده باید بین 0 تا 100 باشد!");
                } else {
                    score.setCustomValidity("");
                }
                if (score.validity.rangeOverflow) {
                    score.setCustomValidity("عدد وارد شده باید بین 0 تا 100 باشد!");
                } else {
                    score.setCustomValidity("");
                }
            });
        });
    </script>
</asp:Content>
