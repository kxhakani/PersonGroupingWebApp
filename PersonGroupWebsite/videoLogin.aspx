﻿<%@ Page Async="true" Title="Video Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="videoLogin.aspx.cs" Inherits="PersonGroupWebsite.videoLogin" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

        <table border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td align="center"><u>Live Camera</u></td>
            <td></td>
            <td align="center"><u>Captured Picture</u></td>
        </tr>
        <tr>
            <td><div id="webcam"></div></td>
            <td>&nbsp;</td>
            <td><img id="imgCapture" style="visibility: hidden; width: 320px;height: 240px"/></td>
        </tr>
    </table>
    <br/>
    <input type="button" value="Capture" onclick="Capture();"/>
    <br/>
    <span id="camStatus"></span>
    <script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script src="~/Webcam_Plugin/jquery.webcam.js"></script>
    <script type="text/javascript">
        $(function () {
            jQuery("#webcam").webcam({
                width: 320,
                height: 240,
                mode: "save",
                swffile: '/Webcam_Plugin/jscam.swf',
                debug: function (type, status) {
                    $('#camStatus').append(type + ": " + status + '<br /><br />');
                },
                onSave: function (data, ab) {
                    $.ajax({
                        type: "POST",
                        url: '/Home/GetCapture',
                        data: '',
                        contentType: "application/json; charset=utf-8",
                        dataType: "text",
                        success: function (r) {
                            $("#imgCapture").css("visibility", "visible");
                            $("#imgCapture").attr("src", r);
                        },
                        failure: function (response) {
                            alert(response.d);
                        }
                    });
                },
                onCapture: function () {
                    webcam.save('/Home/Capture');
                }
            });
        });
        function Capture() {
            webcam.capture();
        }
    </script>

    <div class="row">
    </div>

</asp:Content>
