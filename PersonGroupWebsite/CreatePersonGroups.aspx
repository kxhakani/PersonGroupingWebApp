<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreatePersonGroups.aspx.cs" Inherits="PersonGroupWebsite.CreatePersonGroups" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3>Create Person Group</h3>
    <p>New Person Group ID:</p>
    <textbox></textbox>
</asp:Content>

<%--<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Create Group</h1>
            <label>New Person Group ID:</label>
            <textbox id="txtNewPersonGroupID"></textbox>
        </div>
    </form>
</body>
</html>--%>
