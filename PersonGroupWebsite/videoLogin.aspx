<%@ Page Async="true" Title="Video Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="videoLogin.aspx.cs" Inherits="PersonGroupWebsite.videoLogin" %>

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
            <td><img id="imgCapture" src="" runat="server" width="320" height="240"/></td>
        </tr>
    </table>
    <br/>
    <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click"/>
    <br/>
    <script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script src="/Scripts/jquery.webcam.js"></script>
    <script type="text/javascript">
        $(function () {
            jQuery("#webcam").webcam({
                width: 320,
                height: 240,
                mode: "save",
                swffile: '/Scripts/jscam.swf'
            });
        });
        function Login() {
            webcam.capture();
        }
        </script>

    <div class="row">
    </div>

</asp:Content>
