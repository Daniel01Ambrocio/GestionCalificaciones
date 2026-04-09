<%@ Page Title="" Language="C#" MasterPageFile="~/Presentation/menu.Master" AutoEventWireup="true" CodeBehind="AsignarCalificacion.aspx.cs" Inherits="gestionescolar.Presentation.AsignarCalificacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-5">
        <div class="col-12 col-md-10 col-lg-10">
            <h2>Asignar calificaciones</h2>

            <!-- Rol -->
            <div class="mb-3">
                <asp:Label ID="lbGrupo" runat="server" class="form-label" Text="Grupo: "></asp:Label>
                <asp:DropDownList ID="ddlGrupo" runat="server" CssClass="form-select  textbox-delgado" AutoPostBack="false" onchange="toggleCampos()">
                </asp:DropDownList>
                <asp:Button ID="btngrupo" runat="server" Text="Seleccionar grupo" CssClass="btn btn-primary" OnClick="btngrupo_Click" />
                <asp:Button ID="btnAtras" runat="server" Text="Atras" CssClass="btn btn-danger" OnClick="btnAtras_Click" />
            </div>

            <div class="table-responsive">
                <asp:GridView runat="server" ID="gdvAlumnoCalificaciones" CssClass="table table-striped table-bordered mb-0"
                    AutoGenerateColumns="False" GridLines="None"
                    DataKeyNames="IDCalificacion"
                    AutoGenerateEditButton="False"
                    OnRowEditing="gdvAlumnoCalificaciones_RowEditing"
                    OnRowCancelingEdit="gdvAlumnoCalificaciones_RowCancelingEdit"
                    OnRowUpdating="gdvAlumnoCalificaciones_RowUpdating">
                    <Columns>

                        <asp:BoundField DataField="nombreAlumno" HeaderText="Nombre Alumno" ReadOnly="True" />
                        <asp:BoundField DataField="Grupo" HeaderText="Grupo" ReadOnly="True" />
                        <asp:BoundField DataField="Materia" HeaderText="Materia" ReadOnly="True" />

                        <asp:BoundField DataField="Parcial1" HeaderText="Parcial 1" />
                        <asp:BoundField DataField="Parcial2" HeaderText="Parcial 2" />
                        <asp:BoundField DataField="Parcial3" HeaderText="Parcial 3" />
                        <asp:BoundField DataField="Parcial4" HeaderText="Parcial 4" />

                        <asp:BoundField DataField="Promedio" HeaderText="Promedio" ReadOnly="True" />

                        <asp:TemplateField HeaderText="Acciones">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnAsignar"
                                    runat="server"
                                    Text="Asignar calificación"
                                    CommandName="Edit"
                                    CssClass="btn btn-primary btn-sm" />
                            </ItemTemplate>

                            <EditItemTemplate>
                                <asp:LinkButton ID="btnGuardar"
                                    runat="server"
                                    Text="Guardar"
                                    CommandName="Update"
                                    CssClass="btn btn-success btn-sm" />

                                <asp:LinkButton ID="btnCancelar"
                                    runat="server"
                                    Text="Cancelar"
                                    CommandName="Cancel"
                                    CssClass="btn btn-secondary btn-sm" />
                            </EditItemTemplate>
                        </asp:TemplateField> 
                    </Columns>
                </asp:GridView> 
            </div>
        </div> 
    </div>
</asp:Content>
