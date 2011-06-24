<%@ Page Title="About Us" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="About.aspx.cs" Inherits="EchoLibWebConsole.About" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
<div style="margin:10px">
    <h2>
        About
    </h2>
    <p>
        This is a very basic web console for testing the EchoLib .Net library. This library includes Echo API methods, OAuth authentication 
        and is a translation from PHP into C# of the Echo Platform Library which can be found here <a href="http://github.com/kga245/echo-php-library">http://github.com/kga245/echo-php-library</a>.
    </p>
    <p>
        Charlie Brook <a href="http://www.discussit.com">http://www.discussit.com</a>
    </p>
</div>
</asp:Content>
