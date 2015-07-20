<%@ Page Title="Student Details" Language="C#" MasterPageFile="~/Monday.Master" AutoEventWireup="true" CodeBehind="student.aspx.cs" Inherits="Error_handling1.student" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h1>Student Details</h1>
    <h5>All fields are required</h5>

    <fieldset>
        <label for="txtLastName" class="col-sm-2">Last Name:</label>
        <asp:TextBox ID="txtLastName" runat="server" required MaxLength="50" />
    </fieldset>
    <fieldset>
        <label for="txtFirstMidName" class="col-sm-2">First Name:</label>
        <asp:TextBox ID="txtFirstMidName" runat="server" required MaxLength="50" />
    </fieldset>
    <fieldset>
        <label for="txtEnrollmentDate" class="col-sm-2">Enrollment Date:</label>
        <asp:TextBox ID="txtEnrollmentDate" runat="server" required TextMode="Date" />
    </fieldset>

    <div class="col-sm-offset-2">
        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary"
            OnClick="btnSave_Click" />
    </div>
    <div>
        <h1>Courses</h1>
        <asp:GridView ID="grdCourses" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-hover sort display"
            OnRowDeleting="grdCourses_RowDeleting" DataKeyNames="EnrollmentID">
            <Columns>
                <asp:BoundField DataField="Name" HeaderText="Department" SortExpression="Name" />
                <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title" />
                <asp:BoundField DataField="Grade" HeaderText="Grade" SortExpression="Grade" />
                <asp:CommandField DeleteText="Delete" HeaderText="Delete" ShowDeleteButton="true" />
            </Columns>
        </asp:GridView>
    </div>
    <div>
        <table class="table table-striped table-hover">
            <thead>
                <th>Department</th>
                <th>Title</th>
                <th>Add</th>
            </thead>
            <tbody>
                <tr>
                    <td>
                        <asp:DropDownList ID="ddlDepartment" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged"
                            DataValueField="DepartmentID" DataTextField="Name"></asp:DropDownList>
                        <asp:RangeValidator runat="server" ControlToValidate="ddlDepartment" Type="Integer" MinimumValue="1" MaximumValue="999999999" 
                            ErrorMessage="Required" CssClass="label label-danger" />
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCourse" runat="server" 
                            DataValueField="CourseID" DataTextField="Title"></asp:DropDownList>
                        <asp:RangeValidator runat="server" ControlToValidate="ddlCourse" Type="Integer" MinimumValue="1" MaximumValue="999999999" 
                            ErrorMessage="Required" CssClass="label label-danger" />
                    </td>
                    <td>
                        <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-primary" OnClick="btnAdd_Click" />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</asp:Content>
