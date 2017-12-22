<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="HeroMonsterChallenge.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lblResult" runat="server"></asp:Label>
            <br />
            <br />
            <asp:Button ID="btnAttack" runat="server" OnClick="btnAttack_Click" Text="Attack" />
        </div>
    </form>
</body>
</html>
