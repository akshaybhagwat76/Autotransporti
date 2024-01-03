<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Drivers.aspx.cs" Inherits="admin_Drivers" ClientIDMode="AutoID" %>

<asp:Content ID="Content1" ContentPlaceHolderID="chpHeader" runat="Server">
    <link href="<%= ResolveUrl(string.Format("~/assets/libs/datatables.net-bs4/css/dataTables.bootstrap4.min.css")) %>" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpContent" runat="Server">
    <div class="container-fluid">
        <asp:UpdateProgress runat="server" ID="updProgWrap" AssociatedUpdatePanelID="updContent" DisplayAfter="500">
            <ProgressTemplate>
                <div class="text-center" style="width: 100%; height: 100%; position: absolute; background-color: #343a406b; z-index: 99999; top: 0px; left: 0px">
                    <div class="p-5 text-center" style="background: #fff; width: 50%; left: 25%; margin-top: 20%; border-radius: 10px; position: absolute">
                        <h3>Attendere...</h3>
                        <span class="fa fa-4x fa-cog fa-spin"></span>
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>



        <asp:UpdatePanel runat="server" ID="updContent">
            <ContentTemplate>
                <div class="page-breadcrumb p-0 mb-3">
                    <div class="row">
                        <div class="col-10 align-self-center">
                            <h3 class="page-title text-truncate text-dark font-weight-medium mb-1">Autisti</h3>
                        </div>
                        <div class="col-2 text-right">
                            <a class="btn btn-sm btn-primary waves-effect mt-2 pull-right" href="DriversEdit.aspx"><i class="fa fa-plus-circle"></i> Nuovo</a>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-12">
                        <div class="card">

                            <div class="card-body p-2">

                                <div class="table-responsive">
                                    <table id="tabledata" class="table table-bordered no-wrap">
                                        <thead>
                                            <tr>
                                                <th>Nominativo</th>
                                                <th>Username</th>
                                                <th>Telefono</th>
                                                <th class="text-center">Abilitato</th>
                                                <th data-sortable="false" style="max-width: 50px; width: auto"></th>
                                                <th data-sortable="false" style="max-width: 50px; width: auto"></th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater runat="server" ID="rpTable">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td><%# string.Format("{0} {1}", ((Autista)Container.DataItem).Cognome, ((Autista)Container.DataItem).Nome) %></td>
                                                        <td><%# ((Autista)Container.DataItem).Username %></td>
                                                        <td><%# ((Autista)Container.DataItem).Telefono %></td>
                                                        <td class="text-center"><%# string.Format("<i class=\"fa fa-1-5x {0}\"></i>", ((Autista)Container.DataItem).Abilitato ? "fa-check" : "")  %></td>
                                                        <td class="text-center" style="width: auto">
                                                            <a href="<%# string.Format("DriversEdit.aspx?id={0}", ((Autista)Container.DataItem).Id)  %>" class="fas fa-1-5x icon-pencil"></a>
                                                        </td>
                                                        <td class="text-center" style="width: auto">
                                                            <asp:LinkButton runat="server" ID="lnkDelete" CssClass="fa fa-1-5x icon-trash" OnClientClick="return confirm('Confermi di voler eliminare questo elemento?');" OnClick="lnkDelete_Click" idc='<%# ((Autista)Container.DataItem).Id %>'></asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>

                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpScripts" runat="Server">
    <script src="assets/libs/datatables.net/js/jquery.dataTables.min.js"></script>
    <script src="<%= ResolveUrl("~/assets/libs/parsley/parsley.min.js") %>"></script>
    <script src="<%= ResolveUrl(string.Format("~/assets/libs/parsley/i18n/it.js")) %>"></script>
    <script src="<%= ResolveUrl(string.Format("~/assets/libs/parsley/i18n/it.extra.js")) %>"></script>
    <script type="text/javascript">
        
        function pageLoad() {
            $("#tabledata").DataTable({
                pageLength: 25,
                stateSave: true,
                stateDuration: -1,
                responsive: true,
                language: {
                    url: 'assets/libs/datatables.net/lang/it.json'
                }
            });
        }

    </script>
</asp:Content>

