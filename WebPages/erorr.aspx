﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="erorr.aspx.cs" Inherits="WebPages.erorr" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ارور </title>
    <link rel="apple-touch-icon" sizes="57x57" href="<%= ResolveUrl("_construction/favicon/apple-touch-icon-57x57.png") %>" />
    <link rel="apple-touch-icon" sizes="60x60" href="<%= ResolveUrl("_construction/favicon/apple-touch-icon-60x60.png") %>" />
    <link rel="icon" type="image/png" href="<%= ResolveUrl("_construction/favicon/favicon-16x16.png") %>" sizes="16x16" />
    <link rel="icon" type="image/png" href="<%= ResolveUrl("_construction/favicon/favicon-32x32.png") %>" sizes="32x32" />
    <meta name="msapplication-TileColor" content="#da532c" />
    <style>
        form {
            height: 100%;
        }

        body {
            background-color: #E3E4E8;
            margin: 0;
            height: 637px;
        }

        .mian {
            float: none !important;
            /*-ms-transform: rotate(53deg);
            -webkit-transform: rotate(53deg);
            transform: rotate(53deg);*/
            width: 500px;
            height: 222px;
            position: relative;
            margin: auto;
            direction: rtl;
            text-align: center;
            padding-top: 58px;
        }

        img {
            width: 400px;
            height: 400px;
            position: absolute;
            margin: auto;
            z-index: -1;
            bottom: 0;
            left: 0;
        }

        a {
            letter-spacing: 1px;
            position: relative;
            display: inline-block;
            outline: none;
            color: #000;
            text-decoration: none;
            text-transform: uppercase;
            letter-spacing: 1px;
            font-weight: 400;
            text-shadow: 0 0 1px rgba(255,255,255,0.3);
            font-size: 1.35em;
        }

        .ij-effect-3 a::after {
            position: absolute;
            top: 100%;
            left: 0;
            width: 100%;
            height: 4px;
            background: rgba(0,0,0,0.3);
            content: '';
            opacity: 0;
            -webkit-transition: opacity 0.3s, -webkit-transform 0.3s;
            -moz-transition: opacity 0.3s, -moz-transform 0.3s;
            transition: opacity 0.3s, transform 0.3s;
            -webkit-transform: translateY(10px);
            -moz-transform: translateY(10px);
            transform: translateY(10px);
        }

        .ij-effect-3 a:hover::after,
        .ij-effect-3 a:focus::after {
            opacity: 1;
            -webkit-transform: translateY(0px);
            -moz-transform: translateY(0px);
            transform: translateY(0px);
        }
    </style>

    <link href="_construction/css/style.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="row">
                <div class="mian col-sm-12 col-xs-12">
                    <h2>ارور </h2>
                    <h1>مشکلی پیش آمده است</h1>
                    <h4>دوباره امتحان کنید اگر حل نشد با پشتیبانی تماس بگیرید </h4>

                    <nav class="ij-effect-3">

                        <a href="_construction/index.aspx">خانه</a>
                    </nav>
                </div>
            </div>
            <img src="Images/ORFI0N0.png" />
        </div>
    </form>
</body>
</html>
