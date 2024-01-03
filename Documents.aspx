<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Documents.aspx.cs" Inherits="admin_Documents" ClientIDMode="AutoID" %>

<asp:Content ID="Content1" ContentPlaceHolderID="chpHeader" runat="Server">
    <link href="assets/libs/pickadate/lib/themes/default.css" rel="stylesheet" />
    <link href="assets/libs/pickadate/lib/themes/default.date.css" rel="stylesheet" />
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
                            <h3 class="page-title text-truncate text-dark font-weight-medium mb-1">Documenti D.D.T.</h3>
                        </div>

                    </div>
                </div>
                <div class="card">
                    <div class="card-body p-3">
                        
                        <div class="d-flex">
                            <span class="font-weight-medium">Visualizza D.D.T. caricati in base al periodo selezionato</span>
                            <div class="ml-2">
                                <input type="text" runat="server" id="txtStart" class="form-control pickdate w-auto" />
                            </div>
                            <div class="ml-2">
                                <input type="text" runat="server" id="txtEnd" class="form-control pickdate w-auto" />
                            </div>
                            <div class="ml-2"><asp:LinkButton runat="server" ID="lnkApplyFilter" OnClick="lnkApplyFilter_Click" CssClass="btn btn-primary"><i class="fa fa-filter"></i> Applica</asp:LinkButton></div>
                            
                        </div>
                    </div>
                </div>
                <div class="row">
                   
                    <asp:Repeater runat="server" ID="rpDocuments">
                        <ItemTemplate>
                            <div class="col-3">
                                <div class="border text-center p-2" style="border-radius: 8px" onclick="openPreview('<%# ResolveUrl("~/media/" + ((DDT)Container.DataItem).File)  %>');">
                                    <img class="mb-2" src="<%# ResolveUrl("~/media/" + ((DDT)Container.DataItem).File)  %>" style="width: 100%; height: auto" />
                                    <span><%# ((DDT)Container.DataItem).Data.ToString("dd/MM/yyyy HH:mm")  %></span>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>

                <div id="modalPreview" class="modal" tabindex="-1" role="dialog" aria-hidden="true" style="display: none;">
                    <div class="modal-dialog modal-xl">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            </div>
                            <div class="modal-body p-2">
                                <div class="mb-2">
                                    <div class="card-body p-2">
                                        <img id="imgPreview" style="width:100%; height:auto" />
                                    </div>
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
    <script src="assets/libs/pickadate/lib/picker.js"></script>
    <script src="assets/libs/pickadate/lib/picker.date.js"></script>
    <script src="assets/libs/pickadate/lib/translations/it_IT.js"></script>

    <script type="text/javascript">
        function pageLoad() {
            $(".pickdate").pickadate({
                format: "dd/mm/yyyy",
                formatSubmit: "dd/mm/yyyy",
                editable: false
            });
        }
        function openPreview(file) {
            $("#imgPreview").attr("src", file);
            $("#modalPreview").modal("show");
        }
</script>
</asp:Content>

