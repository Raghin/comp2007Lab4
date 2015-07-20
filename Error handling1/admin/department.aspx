<%@ Page Title="Department Details" Language="C#" MasterPageFile="~/monday.Master" AutoEventWireup="true" CodeBehind="department.aspx.cs" Inherits="Error_handling1.department" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Department Details</h1>

    <h5>All fields are required</h5>

    <div class="form-group">
        <label for="txtName" class="col-sm-3">Name:</label>
        <asp:TextBox ID="txtName" runat="server" required="true" MaxLength="50" />
    </div>
    <div class="form-group">
        <label for="txtBudget" class="col-sm-3">Budget:</label>
        <asp:TextBox ID="txtBudget" runat="server" required="true" MaxLength="22" />
        <asp:RangeValidator runat="server" ControlToValidate="txtBudget"
             CssClass="label label-danger" ErrorMessage="Must be between 0 and 10,000,000"
         MinimumValue="0" MaximumValue="10000000" Type="Currency" />
    </div>

    <div class="col-sm-offset-3">
        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary"
             OnClick="btnSave_Click" />
    </div>
    <div>
        <h1>Courses</h1>
        <asp:GridView ID="grdCourses" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-hover sort display"
             DataKeyNames="DepartmentID" OnRowDeleting="grdCourses_RowDeleting">
            <Columns>
                <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title" />
                <asp:CommandField DeleteText="Delete" HeaderText="Delete" ShowDeleteButton="true" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
