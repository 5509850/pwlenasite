<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sqlinstall.aspx.cs" Inherits="lenapw.test.sqlinstall" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Label_mess" runat="server" Font-Bold="True" ForeColor="#CC0000"></asp:Label>
            <br />
            </div>
        <br />
        <hr />
        <div>Access code:</div>
        <div>
        <asp:TextBox ID="TextBox_code" runat="server"></asp:TextBox>    
    </div>
        <br />
        <hr />
        <div><b>SQL:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="Button_clear" runat="server" Font-Italic="True" Text="Clear Sql" OnClick="Button_clear_Click" />
            </b></div>
        <div>
        <asp:TextBox ID="TextBox_SQL" runat="server" Height="352px" Width="1490px" TextMode="MultiLine"></asp:TextBox>    
    </div>
        <br />
        <br />
        <hr />
        <p>
            <asp:Button ID="Button_submit" runat="server" Text="Submit" Font-Bold="True" Font-Size="XX-Large" Font-Strikeout="False" Font-Underline="True" Width="1497px" ForeColor="Maroon" Height="51px" />
        </p>
        <p>
            Result:</p>
        <p>
            <asp:TextBox ID="TextBox_result" runat="server" Height="168px" Width="1502px" ReadOnly="True" TextMode="MultiLine"></asp:TextBox>
        </p>        
            <hr/>

        <asp:Button ID="Button_List_Tables" runat="server" Text="List of Tables in DB" OnClick="Button_List_Tables_Click" />
           &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="Button_list_SP" runat="server" OnClick="Button_list_SP_Click" Text="List SP in DB" Width="124px" />

    </form>
    <br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />
    <pre class="src" style="font-family: monospace; font-size: 10pt; border-width: 1px; border-style: solid; border-color: rgb(204, 182, 126) rgb(255, 254, 250) rgb(255, 254, 250) rgb(204, 182, 126); margin: 4px 0px; background-color: rgb(255, 250, 237); padding: 9px; color: rgb(34, 34, 34); font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px;">sp_helptext &lt;имя процедуры&gt;           -------код хранимой процедуры</pre>
    <pre class="src" style="font-family: monospace; font-size: 10pt; border-width: 1px; border-style: solid; border-color: rgb(204, 182, 126) rgb(255, 254, 250) rgb(255, 254, 250) rgb(204, 182, 126); margin: 4px 0px; background-color: rgb(255, 250, 237); padding: 9px; color: rgb(34, 34, 34); font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px;"><font color="blue">SELECT</font> <font color="magenta">QUOTENAME</font>( SCHEMA_NAME ( schema_id ) <span style="float: none !important; display: inline !important; margin: 0px !important; background-color: rgb(249, 180, 179);">)</span> + <font color="red">&#39;.&#39;</font> + <span style="float: none !important; display: inline !important; margin: 0px !important; background-color: rgb(249, 180, 179);"><font color="magenta">QUOTENAME</font>(</span> name )  <font color="blue">FROM</font> sys.objects <font color="blue">WHERE</font> type = <font color="red">&#39;P&#39;    -----------------список хранимых процедур</font></pre>
    <p>
        SELECT t.name AS table_name, SCHEMA_NAME(schema_id) AS schema_name, c.name AS column_name FROM sys.tables AS t INNER JOIN sys.columns c ON t.OBJECT_ID = c.OBJECT_ID WHERE c.name LIKE &#39;%FIELD%&#39; ORDER BY schema_name, table_name; ---------------- найти ХП по имени поля FIELD&nbsp;</p>
    <hr />
    SELECT distinct so.name
FROM syscomments sc
INNER JOIN sysobjects so ON sc.id=so.id
WHERE sc.TEXT LIKE '%tableName%'&nbsp;&nbsp; --------- найти ХП по имени используемой таблицы
    <hr />
    SELECT obj.Name SPName, sc.TEXT SPText
FROM sys.syscomments sc
INNER JOIN sys.objects obj ON sc.Id = obj.OBJECT_ID
WHERE sc.TEXT LIKE '%' + 'Name Your SP Here' + '%'
AND TYPE = 'P' --------------------- текст ХП по имени ХП
</body>
</html>
