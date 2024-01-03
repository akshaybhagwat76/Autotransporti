<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Maintenance.aspx.cs" Inherits="admin_Maintenance" %>

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
                            <h3 class="page-title text-truncate text-dark font-weight-medium mb-1">Manutenzioni</h3>
                        </div>
                        <div class="col-2 text-right">
                            <asp:LinkButton runat="server" class="btn btn-sm btn-primary waves-effect mt-2 pull-right" ID="lnkNew" OnClick="lnkNew_Click"><i class="fa fa-plus-circle"></i> Nuovo</asp:LinkButton>
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
                                                <th>Da km</th>
                                                <th>A km</th>
                                                <th>Descrizione</th>
                                                <th data-sortable="false" style="max-width: 50px; width: auto"></th>
                                                <th data-sortable="false" style="max-width: 50px; width: auto"></th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater runat="server" ID="rpTable">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td><%# ((Manutenzione)Container.DataItem).KmDa %></td>
                                                        <td><%# ((Manutenzione)Container.DataItem).KmA %></td>
                                                        <td><%# ((Manutenzione)Container.DataItem).Descrizione %></td>
                                                        <td class="text-center" style="width: auto">
                                                            <asp:LinkButton runat="server" idc='<%# ((Manutenzione)Container.DataItem).Id  %>' class="fas fa-1-5x icon-pencil" ID="lnkEdit" OnClick="lnkEdit_Click"></asp:LinkButton>
                                                        </td>
                                                        <td class="text-center" style="width: auto">
                                                            <asp:LinkButton runat="server" ID="lnkDelete" CssClass="fa fa-1-5x icon-trash" OnClientClick="return confirm('Confermi di voler eliminare questo elemento?');" OnClick="lnkDelete_Click" idc='<%# ((Manutenzione)Container.DataItem).Id %>'></asp:LinkButton>
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

                <div id="modalEdit" class="modal" tabindex="-1" role="dialog" aria-hidden="true" style="display: none;">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h4 class="modal-title font-weight-bolder">Manutezione</h4>
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            </div>
                            <div class="modal-body p-2">
                                <div class="mb-2">
                                    <div class="card-body p-2">
                                        <div class="row" id="main" data-parsley-validate>
                                            <div class="col-3 mb-2">
                                                <label class="text-muted">Da km <small class="text-danger">*</small></label>
                                                <input type="text" runat="server" id="txtKmFrom" class="form-control input-numeric" required data-parsley-group="main" />
                                            </div>

                                            <div class="col-3 mb-2">
                                                <label class="text-muted">a Km <small class="text-danger">*</small></label>
                                                <input type="text" runat="server" id="txtKmTo" class="form-control input-numeric" required data-parsley-group="main" />
                                            </div>
                                            <div class="col-12 mb-2">
                                                <label class="text-muted">Descrizione <small class="text-danger">*</small></label>
                                                <input type="text" runat="server" id="txtDescription" required class="form-control" data-parsley-group="main" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer p-0">
                                    <asp:LinkButton runat="server" ID="lnkSave" CssClass="btn btn-primary waves-effect" OnClick="lnkSave_Click" OnClientClick=<%# string.Format("return multiformValidateConfirmDialog('{0}','{1}','{2}','{3}','{4}','{5}', '{6}')","Attenzione", "Procedere al salvataggio dei dati inseriti?", "Conferma", "Annulla", lnkSave.UniqueID, "#main", "main") %>><span class="fa fa-save"></span> Salva</asp:LinkButton>
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
    <script src="<%= ResolveUrl("~/assets/libs/inputmask/new/inputmask.js") %>"></script>
    <script src="<%= ResolveUrl("~/assets/libs/inputmask/new/inputmask.numeric.extensions.js") %>"></script>
    <script src="<%= ResolveUrl("~/assets/libs/inputmask/new/jquery.inputmask.js") %>"></script>
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
            $(document).ready(function () {
                $(".input-numeric").inputmask({
                    alias: "decimal",
                    digits: 0,
                    allowPlus: false,
                    allowMinus: false
                });
            });
        }

    </script>
</asp:Content>

