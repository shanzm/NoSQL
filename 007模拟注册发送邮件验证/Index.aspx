<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="_007模拟注册发送邮件验证.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        邮箱：<asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
        <br />
        密码：<asp:TextBox ID="txtPwd" runat="server"></asp:TextBox>
        <br />
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="注册" />
        <asp:Label ID="labSuc" runat="server"></asp:Label>
    
    </div>
    </form>
</body>
</html>
