<%@ Page Async="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreatePersonGroups.aspx.cs" Inherits="PersonGroupWebsite.CreatePersonGroups" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3>Create Person Group</h3>

    New Person Group ID:
    <asp:TextBox id="newPersonGroupID" runat="server" />
    <br />
    New Person Group Name:
    <asp:TextBox id="newPersonGroupName" runat="server" />
    <br />
    <asp:Button ID="btnCreateGroup" runat="server" OnClick="btnCreateGroup_Click" Text="Create Person Group"/>
    <br />
    <asp:Label ID="txtMessage" runat="server" />
    <br />
    <br /><br />

    <h3>Delete Person Group</h3>
    Select a group to delete:
    <asp:DropDownList ID="ddlAllGroups" runat="server" OnSelectedIndexChanged="ddlAllGroups_SelectedIndexChanged"></asp:DropDownList>
    <br />
    <asp:Button ID="btnDeleteGroup" runat="server" Text="Delete Group" OnClick="btnDeleteGroup_Click" />
    <br />
    <asp:Label ID="txtMessage2" runat="server" />
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
