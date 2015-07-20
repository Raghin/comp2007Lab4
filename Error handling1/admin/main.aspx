<%@ Page Title="Contoso University - Main Menu" Language="C#" MasterPageFile="~/monday.Master" AutoEventWireup="true" CodeBehind="main.aspx.cs" Inherits="Error_handling1.Main" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container">
        <div class="well">
            <h3>Departments</h3>
            <ul class="list-group">
                <li class="list-group-item"><a href="admin/departments.aspx">Departments</a></li>
                <li class="list-group-item"><a href="admin/department.aspx">Add Departments</a></li>
            </ul>
        </div>

        <div class="well">
            <h3>Courses</h3>
            <ul class="list-group">
                <li class="list-group-item"><a href="admin/courses.aspx">List Courses</a></li>
                <li class="list-group-item"><a href="admin/course.aspx">Add Course</a></li>
            </ul>
        </div>

        <div class="well">
            <h3>Students</h3>
            <ul class="list-group">
                <li class="list-group-item"><a href="admin/students.aspx">List Students</a></li>
                <li class="list-group-item"><a href="admin/student.aspx">Add Student</a></li>
            </ul>
        </div>
    </div>

</asp:Content>
