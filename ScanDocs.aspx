<%@ Page Language="C#"  MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ScanDocs.aspx.cs" Inherits="ScanDocs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <script type="text/javascript" src="Scripts/kissy-min.js"></script>
    <script  type="text/javascript" src="Scripts/jquery-1.10.2.js"></script>
     <style type="text/css">
        /*body {
            padding: 0px;
            margin: 0px;
            background-color: #3a3a3a;
            font-family: "verdana", "sans-serif";
            font-size: 11px;
        }*/
         .NoUnderline {
             text-decoration:none !important;
         }
        a img {
            border: none;
        }

            a img:hover {
                border: none;
            }

        li.fontstyle {
            font-size: 10px;
            color: #222222;
            line-height: 15px;
        }

        li#tallerli {
            line-height: 47px;
        }

        input.bigbutton {
            width: 120px;
            height: 37px;
            font-family: "Arial Black";
            color: #FE8E14;
            font-size: 14pt;
            font-style: italic;
            background-color:orange !Important;
        }

        ul {
            list-style: none;
            padding-left: 0px;
            margin: 0px;
        }

            ul li {
                margin-bottom: 6px;
            }

        #browsersupport ul li {
            float: left;
            padding: 0 10px;
        }

        #browsersupport img {
            vertical-align: middle;
        }

        #browsersupport {
            margin: 0;
            padding: 0;
            padding-left: 6px;
            height: 55px;
            width: 260px;
            z-index: 1;
            position: absolute;
            top: -80px;
        }

        div.divinput {
            font-size: 11px;
            color: #222222;
            padding: 10px;
            line-height: 14px;
            font-family: Arial;
            margin: 5px;
            margin-bottom: 10px;
            background-color: #f0f0f0;
            text-align: left;
        }

        div.menudiv {
            float: left;
            height: 25px;
            padding-top: 10px;
        }

        div#DWTcontainer {
            margin: 0 auto;
        }

        .divcontrol {
            width: 580px;
            height: 600px;
        }

        .divcontrolthumbnail {
            width: 90px;
            height: 560px;
        }

        div#dwtcontrolContainer {
            margin: 0px;
            margin-left: 22px;
            float: left;
            padding: 0px;
            padding-top: 10px;
            width: 600px;
            height: 600px;
        }

        div.dwtcontrolThumbnail {
            padding: 5px;
            padding-top: 10px;
            text-align: center;
            border-collapse: collapse;
            border: 3px solid #cE5E04;
            position: absolute;
            height: 580px;
            z-index: 1;
            background-color: #f0f0f0;
            width: 100px;
        }

        div#ScanWrapper {
            margin: 0px;
            float: right;
            padding-right: 18px;
            width: 320px;
            height: 800px;
            margin-top: -750px;
            *margin-top: -770px;
            *margin-top: -770\9 px;
        }

        div#Crop {
            padding: 5px;
            padding-top: 10px;
            text-align: center;
            border-collapse: collapse;
            border: 3px solid #cE5E04;
            position: absolute;
            height: 80px;
            z-index: 1;
            background-color: #f0f0f0;
            width: 250px;
        }

        div#ImgSizeEditor {
            padding: 5px;
            padding-top: 10px;
            text-align: center;
            border-collapse: collapse;
            border: 3px solid #cE5E04;
            position: absolute;
            height: 110px;
            z-index: 1;
            background-color: #f0f0f0;
            width: 300px;
        }

        div#MoreEditMethods {
            padding: 5px;
            padding-top: 10px;
            border-collapse: collapse;
            border: 3px solid #cE5E04;
            position: absolute;
            height: auto;
            z-index: 1;
            background-color: #f0f0f0;
            width: 250px;
            text-align: left;
        }

        div#divCapabilityNegotiation {
            padding: 5px;
            padding-top: 10px;
            border-collapse: collapse;
            border: 3px solid #cE5E04;
            position: absolute;
            height: auto;
            z-index: 2;
            background-color: #f0f0f0;
            width: auto;
        }

        div#divRotateConfig {
            padding: 5px;
            padding-top: 10px;
            border-collapse: collapse;
            border: 3px solid #cE5E04;
            position: absolute;
            height: 100px;
            z-index: 2;
            background-color: #f0f0f0;
            width: 200px;
        }

        div#divSetImageLayout {
            padding: 5px;
            padding-top: 10px;
            border-collapse: collapse;
            border: 3px solid #cE5E04;
            position: absolute;
            height: 75px;
            z-index: 2;
            background-color: #f0f0f0;
            width: 350px;
        }

        div#divHighlight {
            padding: 5px;
            padding-top: 10px;
            border-collapse: collapse;
            border: 3px solid #cE5E04;
            position: absolute;
            height: 110px;
            z-index: 2;
            background-color: #f0f0f0;
            width: 280px;
        }

        div#tblLoadImage {
            padding: 5px;
            padding-top: 10px;
            text-align: left;
            border-collapse: collapse;
            border: 3px solid #cE5E04;
            position: absolute;
            height: 170px;
            z-index: 1;
            background-color: #fefefe;
            width: 280px;
        }

        div#AddTextDiv {
            padding: 5px;
            padding-top: 10px;
            text-align: center;
            border-collapse: collapse;
            border: 3px solid #cE5E04;
            position: absolute;
            height: 170px;
            z-index: 1;
            background-color: #f0f0f0;
            width: 300px;
        }

        div#SetTextFontDiv {
            padding: 5px;
            padding-top: 10px;
            text-align: center;
            border-collapse: collapse;
            border: 3px solid #cE5E04;
            position: absolute;
            height: 250px;
            z-index: 1;
            background-color: #f0f0f0;
            width: 300px;
        }

        a:link {
            color: #222222;
            line-height: 18px;
            text-decoration: underline;
        }

        a:visited {
            color: #222222;
            line-height: 18px;
            text-decoration: underline;
        }

        a:active {
            color: #666666;
            line-height: 18px;
            text-decoration: underline;
        }

        a:hover {
            color: #ff3300;
            line-height: 18px;
            text-decoration: underline;
        }

        a.menu:link {
            color: #222222;
            line-height: 18px;
            text-decoration: none;
        }

        a.menu:visited {
            color: #222222;
            line-height: 18px;
            text-decoration: none;
        }

        a.menu:active {
            color: #222222;
            line-height: 18px;
            text-decoration: none;
        }

        a.menu:hover {
            color: #222222;
            line-height: 18px;
            text-decoration: none;
        }

        a.white:link {
            color: #d9d9d9;
            line-height: 18px;
            text-decoration: underline;
        }

        a.white:visited {
            color: #d9d9d9;
            line-height: 18px;
            text-decoration: underline;
        }

        a.white:active {
            color: #d9d9d9;
            line-height: 18px;
            text-decoration: underline;
        }

        a.white:hover {
            color: #d9d9d9;
            line-height: 18px;
            text-decoration: none;
        }

        a.gray:link {
            color: #222222;
            line-height: 18px;
            text-decoration: none;
        }

        a.gray:visited {
            color: #222222;
            line-height: 18px;
            text-decoration: none;
        }

        a.gray:active {
            color: #222222;
            line-height: 18px;
            text-decoration: none;
        }

        a.gray:hover {
            color: #222222;
            line-height: 18px;
            text-decoration: underline;
        }

        a.grayunder:link {
            color: #454545;
            line-height: 18px;
            text-decoration: underline;
        }

        a.grayunder:visited {
            color: #454545;
            line-height: 18px;
            text-decoration: underline;
        }

        a.grayunder:active {
            color: #454545;
            line-height: 18px;
            text-decoration: underline;
        }

        a.grayunder:hover {
            color: #454545;
            line-height: 18px;
            text-decoration: none;
        }

        .tableborder {
            border-right: #cdcdcd 1px solid;
            border-top: #cdcdcd 1px solid;
            border-left: #cdcdcd 1px solid;
            border-bottom: #cdcdcd 1px solid;
        }

        .tableborderbottom {
            border-bottom: #cdcdcd 1px solid;
        }

        .fontgray12B {
            font-weight: bold;
            color: #555555;
        }

        .fontyellow12B {
            font-weight: bold;
            color: #3a3a3a;
        }

        .titlepagetd {
            vertical-align: middle;
            height: 30px;
        }

        .subtitletd {
            vertical-align: bottom;
            height: 30px;
        }

        .titlepage {
            font-weight: bold;
            font-size: 14px;
            color: #fe8e14;
        }

        .subtitle {
            font-weight: 600;
            font-size: 11px;
            vertical-align: bottom;
            color: #fe8e14;
            FONT-FAMILY: Verdana;
            height: 20px;
            TEXT-ALIGN: left;
        }

        .menuout {
            padding-bottom: 5px;
            color: #ffffff;
            background-color: #fe8e14;
        }

        .menuover {
            color: #ffffff;
            background-color: #5f6062;
        }

        .menu_top_over {
            padding-left: 30px;
            font-size: 11px;
            background: url(../images/menutop1.jpg) no-repeat 50% bottom;
            width: 151px;
            color: #353535;
            padding-top: 9px;
            FONT-FAMILY: "verdana";
            height: 48px;
        }

        .menu_top_out {
            padding-left: 30px;
            font-size: 11px;
            background: url(../images/menutop.jpg) no-repeat 50% bottom;
            width: 151px;
            color: #353535;
            padding-top: 9px;
            FONT-FAMILY: "verdana";
            height: 48px;
        }

        .menu_over {
            padding-left: 30px;
            font-size: 11px;
            background: url(../images/menuover.jpg) no-repeat 50% bottom;
            width: 151px;
            color: #353535;
            FONT-FAMILY: "verdana";
            height: 33px;
        }

        .menu_out {
            padding-left: 30px;
            font-size: 11px;
            background: url(../images/menuout.jpg) no-repeat 50% bottom;
            width: 151px;
            color: #353535;
            FONT-FAMILY: "verdana";
            height: 33px;
        }

        .menu_blank {
            background: url(../images/menublank.jpg) no-repeat 50% bottom;
            width: 151px;
            color: #353535;
            height: 33px;
        }

        .body_Narrow_width {
            width: 964px;
        }

        .body_Broad_width {
            width: 984px;
            height:800px;/*Main Height RSS 1-4-14*/
        }

        input {
            font: normal 11px verdana;
        }

            input.invalid {
                background-color: #FF9;
                border: 2px red inset;
            }

        a.menucolor:link {
            font-weight: bold;
            font-family: Arial;
            font-size: 12px;
            margin-right: 5px;
            color: #FFFFFF;
            text-decoration: none;
        }

        a.menucolor:visited {
            font-weight: bold;
            font-family: Arial;
            font-size: 12px;
            margin-right: 5px;
            color: #FFFFFF;
            text-decoration: none;
        }

        a.menucolor:hover {
            font-weight: bold;
            font-family: Arial;
            font-size: 12px;
            margin-right: 5px;
            color: #FE8E14;
            text-decoration: none;
        }

        a.menucolor:active {
            font-weight: bold;
            font-family: Arial;
            font-size: 12px;
            margin-right: 5px;
            color: #FFFFFF;
            text-decoration: none;
        }

        a.fontcolor:link {
            color: #000000;
            text-decoration: none;
            line-height: 14px;
        }

        a.fontcolor:visited {
            color: #000000;
            text-decoration: none;
            line-height: 14px;
        }

        a.fontcolor:hover {
            color: #000000;
            text-decoration: none;
            line-height: 14px;
        }

        a.fontcolor:active {
            color: #000000;
            text-decoration: none;
            line-height: 14px;
        }

        div#tblLoadImage2 {
            padding: 5px;
            padding-top: 10px;
            text-align: left;
            border-collapse: collapse;
            border: 3px solid #cE5E04;
            position: absolute;
            height: 330px;
            z-index: 1;
            background-color: #fefefe;
            width: 300px;
        }

        #menu {
            background: url(../Images/bg_menubar_black.png) repeat-x;
            border-top: 1px solid #CCC;
            border-bottom: 1px solid #CCC;
            height: 40px;
        }

            #menu ul {
                list-style: none;
                margin: 0;
                padding: 0;
            }

            #menu a {
                text-decoration: none;
                display: block;
                white-space: nowrap;
            }

            #menu .nohref {
                cursor: default;
            }
        /* first level start */
        .menubar_split, .menubar_split_last {
            background: #000;
            width: 1px;
            height: 40px;
            position: absolute;
            top: 0;
            border-left: 1px solid #666;
            border-right: 1px solid #666;
        }

        .menubar_split {
            left: 0;
        }

        .menubar_split_last {
            right: 0;
        }

        #menu .D_menu_item {
            float: left;
            width: 110px;
            height: 40px;
            line-height: 40px;
            color: #FFF;
            font: bold 13px/16px Helvetica;
            margin: 0;
            padding: 0;
            text-align: center;
            vertical-align: middle;
        }

        #menu .D_menu_item {
            position: relative;
            z-index: 1;
        }

            #menu .D_menu_item a {
                height: 40px;
                line-height: 40px;
            }

                #menu .D_menu_item a:visited {
                    background: none;
                }

            #menu .D_menu_item:hover {
                background-color: #FFF;
                color: #555;
            }
        /* first level end */

        #menu a:link {
            color: #fff;
            line-height: 40px;
            text-decoration: none;
        }

        #menu a:visited {
            color: #fff;
            line-height: 40px;
            text-decoration: none;
        }

        #menu a:active {
            color: #666666;
            line-height: 40px;
            text-decoration: none;
        }

        #menu a:hover {
            color: #ff3300;
            line-height: 40px;
            text-decoration: none;
            cursor: pointer;
        }


        .ks-overlay {
            position: absolute;
            left: -9999px;
            top: -9999px;
        }

        .ks-ext-close {
            padding: 0 20px;
            position: absolute;
            right: -5px;
            top: 5px;
        }

            .ks-ext-close, .ks-ext-close:link {
                color: #2222AA;
                text-decoration: none;
                cursor: pointer;
            }

        .ks-ext-mask {
            background: #999;
            filter: alpha(opacity=70); /* IE */
            -moz-opacity: 0.7; /* Moz + FF */
            opacity: 0.7; /* CSS3 e.g. FF 1.5 */
        }


        .D-dailog .ks-ext-close .ks-ext-close-x {
            height: 8px;
        }

        .D-dailog .ks-ext-close, .D-dailog .ks-ext-close:hover {
            background: none;
            border: none;
        }

        .D-dailog {
            position: absolute;
            left: -9999px;
            top: -99999px;
            margin: 100px auto;
            padding: 0;
            width: 392px;
            height: 242px;
            background-color: #F1F2F2;
        }

            .D-dailog .D-dailog-body {
                width: 350px;
                height: 200px;
                position: relative;
                top: 5px;
                left: 5px;
                margin: 0;
                background-color: #ffffff;
                border: 1px solid #E7E7E7;
                padding: 15px;
            }

            .D-dailog .D-dailog-body-Mac {
                width: 350px;
                height: 255px;
                position: relative;
                top: 5px;
                left: 5px;
                margin: 0;
                background-color: #ffffff;
                border: 1px solid #E7E7E7;
                padding: 15px;
            }

            .D-dailog .D-dailog-body-Scan {
                width: 380px;
                height: 240px;
                position: relative;
                top: 5px;
                left: 5px;
                margin: 0;
                background-color: #ffffff;
                border: 1px solid #E7E7E7;
                padding: 15px;
            }


            .D-dailog .D-dailog-body-Scan-sample {
                width: 380px;
                height: 200px;
                position: relative;
                top: 5px;
                left: 5px;
                margin: 0;
                background-color: #ffffff;
                border: 1px solid #E7E7E7;
                padding: 15px;
            }

            .D-dailog a {
                text-decoration: none;
            }

        .link {
            text-decoration: underline;
        }


        .box_title {
            color: #333;
            font-family: helvetica, arial, sans-serif;
            font-size: 16px;
            font-weight: bold;
        }

        .box_title_scan {
            color: #333;
            font-family: helvetica, arial, sans-serif;
            font-size: 12px;
            font-weight: bold;
        }

        .box_scan_subtitle {
            color: #333;
            font-family: helvetica, arial, sans-serif;
            font-size: 12px;
        }

        .box_scan_detail {
            color: #777777;
            margin-top: 5px;
            margin-bottom: 8px;
            font-family: helvetica, arial, sans-serif;
            font-size: 12px;
        }

        .D-dailog ul {
            margin: 10px 0 10px 20px;
            list-style-type: disc;
        }


        .red {
            color: red;
            margin-left: 5px;
        }

        .button {
            background-image: url(../images/btn-down-install-plugin.gif);
            background-repeat: no-repeat;
            width: 212px;
            height: 34px;
            position: relative;
            margin: 20px auto 0;
            cursor: pointer;
        }

            .button:hover {
                background-position: 0 -34px;
            }

            .button:active {
                background-position: 0 -68px;
            }



        #PCollapse {
            list-style: none inside none;
            margin-left: 0;
            padding-left: 0;
        }

        .divType {
            font-weight: bold;
            font-size: 12px;
            height: 25px;
            cursor: pointer;
        }

        .mark_arrow {
            display: block;
            float: left;
            height: 12px;
            margin: 0px 2px 0 0;
            width: 12px;
        }

        .collapsed {
            background: url("../images/arrow.gif") no-repeat scroll 3px center transparent;
        }

        .expanded {
            background: url("../images/arrow-down.gif") no-repeat scroll 3px 4px transparent;
        }

        #div_SampleImage {
            padding-left: 15px;
        }

        #div_LoadLocalImage {
            padding-left: 15px;
            width: 260px;
        }

        #div_SampleImage ul {
            padding-left: 0;
            margin-left: 0;
        }

        .divTableStyle {
            border: solid 8px #ddd;
        }

        /*Upgrade*/
        #message {
            font-family: Arial,Helvetica,sans-serif;
            position: fixed;
            top: 0px;
            left: 0px;
            width: 100%;
            z-index: 105;
            text-align: center;
            font-weight: bold;
            font-size: 100%;
            padding: 10px 0px 10px 0px;
            color: #239210;
            background-color: #E0F0D6;
            border: 1px solid #54D33F;
            box-shadow: 0 0 10px #3A3A3A;
        }

        #MessageBody a {
            color: #6A9962;
            font-size: 14px;
        }

        #message span {
            text-align: center;
            width: 95%;
            float: left;
        }

        #MessageBoy a:hover {
            text-decoration: underline;
        }

        .close-notify {
            white-space: nowrap;
            float: right;
            margin-right: 10px;
            color: #fff;
            text-decoration: none;
            border: 2px #fff solid;
            padding-left: 3px;
            padding-right: 3px;
        }

            .close-notify a {
                color: #fff;
            }

        .DWTPage {
            margin: 0 auto;
        }

        .DWTHeader {
            background-color: #3a3a3a;
            border: 0;
            padding: 0;
        }

        .DWTTail {
            background-color: #ffffff;
            border: 0;
        }
    
*,
*:before,
*:after {
  -webkit-box-sizing: border-box;
     -moz-box-sizing: border-box;
          box-sizing: border-box;
}

  * {
    color: #000 !important;
    text-shadow: none !important;
    background: transparent !important;
    box-shadow: none !important;
  }
  /* AutoComplete highlighted item */

.autocomplete_highlightedListItem {
    background-color: lemonchiffon  !important;
    color: black !important;
    padding: 1px !important;
    font-family: Arial !important;
    font-size:12pt !important;
}

/* AutoComplete item */

.autocomplete_listItem {
    background-color: white !important;
    color: black !important;
    padding: 1px !important;
    font-family: Arial !important;
    font-size:12pt !important;
}
    </style>
 <script type="text/javascript">
     window.onload = (function () {

         if ('<%=Session["DocID"] %>' != null) {
             var sDocID = '<%=Session["DocID"] %>';
             document.getElementById('txt_fileName').value = sDocID;
         }
         else {
             alert("No Document name is the session variable!");
         }

              
               //document.forms[0].elements[1].value = "another way";
               /* note elements refers to only input children */
           });
        </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="D-dailog" id="J_waiting">
        <div id="InstallBody">
        </div>
    </div>


    <div id="container" class="body_Broad_width" style="margin: 0 auto; background-color: whitesmoke !Important">


        <div id="DWTcontainer" class="body_Broad_width" style="background-color: #CCCCCC; height: <%=Common.DW_ContainerHeight%>px; border: 0;">

            <div id="dwtcontrolContainer" style="height: 600px;"></div><br />
            <div id="DWTNonInstallContainerID" style="width: 580px;"></div><br />
            <div id="DWTemessageContainer" style="margin-left: 50px; width: 580px;"></div><br />

            <div id="ScanWrapper">
                <div id="divScanner" class="divinput">
                    <ul class="PCollapse">                        
                        <li>
                            <table style="width: 300px">
                              
                                <tr>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt">Document Name</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                       
                                <input type="text" size="20" id="txt_fileName"  disabled="disabled" class="form-control" style="width: 250px; background-color: lemonchiffon !important;" readonly="readonly"  /></td>
                                </tr>                                 
                            </table>
                        </li>
                        <li>
                            <div class="divType">
                                <div class="mark_arrow expanded"></div>
                                Scan (Step 1)</div>
                            <div id="div_ScanImage" class="divTableStyle">
                                <ul id="ulScaneImageHIDE">
                                    <li style="padding-left: 15px;">
                                        <label for="source">Select Source:</label>
                                        <select size="1" id="source" style="position: relative; width: 220px;" onchange="source_onchange()">
                                            <option value=""></option>
                                        </select></li>
                                    <li style="display: none;" id="pNoScanner">
                                        <a href="javascript: void(0)" class="ShowtblLoadImage" style="font-size: 11px; background-color: #f0f0f0; position: relative" id="aNoScanner"><b>What if I don't have a scanner/webcam connected?</b>
                                        </a></li>
                                    <li id="divProductDetail"></li>
                                    <li style="text-align: center;"><br />
                                        <input id="btnScan" class="bigbutton" style="color: #C0C0C0;" disabled="disabled" type="button" value="Scan" onclick="acquireImage();" /></li>
                                </ul>
                            </div>
                        </li>
                            <%--                        <li>
                            <div class="divType">
                                <div class="mark_arrow collapsed"></div>
                                Load the Sample Images</div>
                            <div id="div_SampleImage" style="display: none" class="divTableStyle">
                                <ul>
                                    <li><b>Samples:</b></li>
                                    <li style="text-align: center;">
                                        <table style="border-spacing: 2px; width: 100%;">
                                            <tr>
                                                <td style="width: 33%">
                                                    <input name="SampleImage3" type="image" src="Images/icon_associate3.png" style="width: 50px; height: 50px"
                                                        onclick="loadSampleImage(3);" onmouseover="Over_Out_DemoImage(this,'Images/icon_associate3.png');"
                                                        onmouseout="Over_Out_DemoImage(this,'Images/icon_associate3.png');" />
                                                </td>
                                                <td style="width: 33%">
                                                    <input name="SampleImage2" type="image" src="Images/icon_associate2.png" style="width: 50px; height: 50px"
                                                        onclick="loadSampleImage(2);" onmouseover="Over_Out_DemoImage(this,'Images/icon_associate2.png');"
                                                        onmouseout="Over_Out_DemoImage(this,'Images/icon_associate2.png');" />
                                                </td>
                                                <td style="width: 33%">
                                                    <input name="SampleImage1" type="image" src="Images/icon_associate1.png" style="width: 50px; height: 50px"
                                                        onclick="loadSampleImage(1);" onmouseover="Over_Out_DemoImage(this,'Images/icon_associate1.png');"
                                                        onmouseout="Over_Out_DemoImage(this,'Images/icon_associate1.png');" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>B&W Image
                                                </td>
                                                <td>Grey Image
                                                </td>
                                                <td>Color Image
                                                </td>

                                            </tr>
                                        </table>
                                    </li>
                                </ul>
                            </div>
                        </li>--%>
                        <li>
                            <div class="divType">
                                <div class="mark_arrow collapsed"></div>
                                Or Load a Local Image</div>
                            <div id="div_LoadLocalImage" style="display: none" class="divTableStyle">
                                <ul>
                                    <li style="text-align: center; height: 35px; padding-top: 8px;">
                                        <input type="button" value="Load Image" style="width: 130px; height: 30px; font-size: medium;" onclick="return btnLoad_onclick()" />
                                    </li>
                                </ul>
                            </div>
                        </li>

                    </ul>

                </div>

                <div id="divBlank" style="height: 20px">
                    <ul>
                        <li></li>
                    </ul>
                </div>

                <div id="tblLoadImage" style="visibility: hidden; height: 80px">
                    <ul>
                        <li><b>You can:</b><a href="javascript: void(0)" style="text-decoration: none; padding-left: 200px" class="ClosetblLoadImage">X</a></li>
                    </ul>
                    <div id="notformac1" style="background-color: #f0f0f0; padding: 5px;">
                        <ul>
                            <li>
                                <img alt="arrow" src="Images/arrow.gif" width="9" height="12" /><b>Install a Virtual Scanner:</b></li>
                            <li style="text-align: center;"><a id="samplesource32bit" href="https://www.dynamsoft.com/demo/DWT/Sources/twainds.win32.installer.2.1.3.msi">32-bit Sample Source</a>
                                <a id="samplesource64bit" style="display: none;" href="https://www.dynamsoft.com/demo/DWT/Sources/twainds.win64.installer.2.1.3.msi">64-bit Sample Source</a>
                                from <a href="http://www.twain.org">TWG</a></li>
                        </ul>
                    </div>
                </div>

                <div id="divEdit" class="divinput" style="position: relative">
                    <ul>
                        <li>
                            <img alt="arrow" src="Images/arrow.gif" width="9" height="12" /><b>Edit Image</b></li>
                        <li style="padding-left: 9px;">
                            <img src="Images/ShowEditor.png" title="Show Image Editor" alt="Show Image Editor" id="btnEditor" onclick="btnShowImageEditor_onclick()" />
                            <img src="Images/RotateLeft.png" title="Rotate Left" alt="Rotate Left" id="btnRotateL" onclick="btnRotateLeft_onclick()" />
                            <img src="Images/RotateRight.png" title="Rotate Right" alt="Rotate Right" id="btnRotateR" onclick="btnRotateRight_onclick()" />
                            <img src="Images/Mirror.png" title="Mirror" alt="Mirror" id="btnMirror" onclick="btnMirror_onclick()" />
                            <img src="Images/Flip.png" title="Flip" alt="Flip" id="btnFlip" onclick="btnFlip_onclick()" />
                            <img src="Images/Crop.png" title="Crop" alt="Crop" id="btnCrop" onclick="btnCrop_onclick();" />
                            <img src="Images/ChangeSize.png" title="Change Image Size" alt="Change Image Size" id="btnChangeImageSize" onclick="btnChangeImageSize_onclick();" /></li>
                    </ul>

                </div>

                <div id="divSave" class="divinput" style="position: relative">
                    <ul>
                        <li>
                            <img alt="arrow" src="Images/arrow.gif" width="9" height="12" /><b>Save Image/Scan Locally (Optional)</b></li>
                        <li style="padding-left: 15px;">
                            <label for="txt_fileNameforSave">File Name:
                                <input type="text" size="20" id="txt_fileNameforSave" /></label></li>
                        <li style="padding-left: 12px;">
                            <label for="imgTypebmp">
                                <input type="radio" value="bmp" name="imgType_save" id="imgTypebmp" onclick="rdsave_onclick();" />BMP</label>
                            <label for="imgTypejpeg">
                                <input type="radio" value="jpg" name="imgType_save" id="imgTypejpeg" onclick="rdsave_onclick();" />JPEG</label>
                            <label for="imgTypetiff">
                                <input type="radio" value="tif" name="imgType_save" id="imgTypetiff" onclick="rdTIFFsave_onclick();" />TIFF</label>
                            <label for="imgTypepng">
                                <input type="radio" value="png" name="imgType_save" id="imgTypepng" onclick="rdsave_onclick();" />PNG</label>
                            <label for="imgTypepdf">
                                <input type="radio" value="pdf" name="imgType_save" id="imgTypepdf" onclick="rdPDFsave_onclick();" />PDF</label></li>
                        <li style="padding-left: 12px;">
                            <label for="MultiPageTIFF_save">
                                <input type="checkbox" id="MultiPageTIFF_save" />Multi-Page TIFF</label>
                            <label for="MultiPagePDF_save">
                                <input type="checkbox" id="MultiPagePDF_save" />Multi-Page PDF </label>
                        </li>
                        <li style="text-align: center">
                            <input id="btnSave" type="button" value="Save Image" onclick="btnSave_onclick()" style="background-color: orange !Important; font-weight: bold;" /></li>
                    </ul>
                </div>

                <div id="divUpload" class="divinput" style="position: relative">
                    <ul>
                        <li>
                            <img alt="arrow" src="Images/arrow.gif" width="9" height="12" /><b>Upload Image/Scan(Step 2)</b></li>
                       
                        <li style="padding-left: 9px;">
                            <label for="imgTypejpeg2">
                                <input type="radio" value="jpg" name="ImageType" id="imgTypejpeg2" onclick="rd_onclick();" />JPEG</label>
                            <label for="imgTypetiff2">
                                <input type="radio" value="tif" name="ImageType" id="imgTypetiff2" onclick="rdTIFF_onclick();" />TIFF</label>
                            <label for="imgTypepng2">
                                <input type="radio" value="png" name="ImageType" id="imgTypepng2" onclick="rd_onclick();" />PNG</label>
                            <label for="imgTypepdf2">
                                <input type="radio" value="pdf" name="ImageType" id="imgTypepdf2" onclick="rdPDF_onclick();" />PDF</label></li>
                        <li style="padding-left: 9px;">
                            <label for="MultiPageTIFF">
                                <input type="checkbox" id="MultiPageTIFF" />Multi-Page TIFF</label>
                            <label for="MultiPagePDF">
                                <input type="checkbox" id="MultiPagePDF" />Multi-Page PDF </label>
                                                
                        </li>
                        <li> </li>
                        <li style="text-align: center">
                            <input id="btnUpload" type="button" value="Upload Image" onclick="btnUpload_onclick()" style="font-weight: bold; background-color: orange !Important;" />

                        </li>
                         
                          
                    </ul>
                </div>

                <div id="divUpgrade">
                </div>

            </div>

        </div>
    </div>

    <div id="ImgSizeEditor" style="visibility: hidden; text-align: left;">
        <ul>
            <li>
                <label for="img_height">
                    <b>New Height :</b>
                    <input type="text" id="img_height" style="width: 50%;" size="10" />pixel</label></li>
            <li>
                <label for="img_width">
                    <b>New Width :</b>&nbsp;
        <input type="text" id="img_width" style="width: 50%;" size="10" />pixel</label></li>
            <li>Interpolation method:
        <select size="1" id="InterpolationMethod">
            <option value=""></option>
        </select></li>
            <li style="text-align: center;">
                <input type="button" value="   OK   " id="btnChangeImageSizeOK" onclick="btnChangeImageSizeOK_onclick();" />
                <input type="button" value=" Cancel " id="btnCancelChange" onclick="btnCancelChange_onclick();" /></li>
        </ul>
    </div>
    <div id="Crop" style="visibility: hidden;">
        <div style="width: 50%; height: 100%; float: left; text-align: left;">
            <ul>
                <li>
                    <label for="img_left">
                        <b>left: </b>
                        <input type="text" id="img_left" style="width: 50%;" size="4" /></label></li>
                <li>
                    <label for="img_top">
                        <b>top: </b>
                        <input type="text" id="img_top" style="width: 50%;" size="4" /></label></li>
                <li style="text-align: center;">
                    <input type="button" value="  OK  " id="btnCropOK" onclick="btnCropOK_onclick()" /></li>
            </ul>
        </div>
        <div style="width: 50%; height: 100%; float: left; text-align: right;">
            <ul>
                <li>
                    <label for="img_right">
                        <b>right : </b>
                        <input type="text" id="img_right" style="width: 50%;" size="4" /></label></li>
                <li>
                    <label for="img_bottom">
                        <b>bottom:</b>
                        <input type="text" id="img_bottom" style="width: 50%;" size="4" /></label></li>
                <li style="text-align: center;">
                    <input type="button" value="Cancel" id="cancelcrop" onclick="btnCropCancel_onclick()" /></li>
            </ul>
        </div>
    </div>
  
    <script type="text/javascript" language="javascript" src="Scripts/dynamsoft.webtwain.initiate.js"></script>
    <script type="text/javascript" language="javascript" src="Scripts/online_demo_operation.js"></script>
    <script type="text/javascript" language="javascript" src="Scripts/online_demo_initpage.js"></script>
    <script type="text/javascript" language="javascript" src="Scripts/jquery.js"></script>
    <script type="text/javascript">
        $("ul.PCollapse li>div").click(function () {
            if ($(this).next().css("display") == "none") {
                $(".divType").next().hide("normal");
                $(".divType").children(".mark_arrow").removeClass("expanded");
                $(".divType").children(".mark_arrow").addClass("collapsed");
                $(this).next().show("normal");
                $(this).children(".mark_arrow").removeClass("collapsed");
                $(this).children(".mark_arrow").addClass("expanded");
            }
        });
    </script>
    <script type="text/javascript" language="javascript">
        // Assign the page onload fucntion.

        S.ready(function () {
            pageonload();
        });

    </script>
</asp:Content>
