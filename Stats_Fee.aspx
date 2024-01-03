<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Stats_Fee.aspx.cs" Inherits="admin_Stats_Fee" ClientIDMode="AutoID" %>

<asp:Content ID="Content1" ContentPlaceHolderID="chpHeader" runat="Server">
    <link href="<%= ResolveUrl(string.Format("~/assets/libs/datatables.net-bs4/css/dataTables.bootstrap4.min.css")) %>" rel="stylesheet" />
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
                            <h3 class="page-title text-truncate text-dark font-weight-medium mb-1">Tariffe dovute</h3>
                        </div>

                    </div>
                </div>
                <div class="card">
                    <div class="card-body p-3">

                        <div class="d-flex">
                            <span class="font-weight-medium">Visualizza statistiche in base al periodo selezionato</span>
                            <div class="ml-2">
                                <asp:DropDownList runat="server" ID="dropMonth" CssClass="form-control w-auto"></asp:DropDownList>
                            </div>
                            <div class="ml-2">
                                <asp:DropDownList runat="server" ID="dropYear" CssClass="form-control w-auto"></asp:DropDownList>
                            </div>
                            <div class="ml-2">
                                <asp:LinkButton runat="server" ID="lnkApplyFilter" OnClick="lnkApplyFilter_Click" CssClass="btn btn-primary"><i class="fa fa-filter"></i> Applica</asp:LinkButton>
                            </div>

                        </div>
                    </div>
                </div>
                <div class="table-responsive">

                    <table class="table table-bordered" id="tabledata">
                        <thead>
                            <tr>
                                <th>Autista</th>
                                <th class="text-right">Totale dovuto</th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater runat="server" ID="rpStats">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("driver")  %></td>
                                        <td class="text-right"><%# Convert.ToDecimal(Eval("totale")).ToString("c")  %></td>
                                        <td class="text-center">
                                            <asp:LinkButton runat="server" ID="lnkDetail" CssClass="fa fa-1-5x fa-search" dname='<%# Eval("driver") %>' did='<%# Eval("driverId") %>' OnClick="lnkDetail_Click"></asp:LinkButton></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>

                <div id="modalDetail" class="modal" tabindex="-1" role="dialog" aria-hidden="true" style="display: none;">
                    <div class="modal-dialog modal-xl">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h4>Dettaglio tariffe <strong class="custom-red-color font-weight-bolder">
                                    <asp:Literal runat="server" ID="litDetailDriver"></asp:Literal></strong></h4>
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            </div>
                            <div class="modal-body p-0">
                                <table class="table">
                                    <thead>
                                        <th>Data</th>
                                        <th>Tipologia</th>
                                        <th class="text-right">Tariffa</th>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater runat="server" ID="rpFee">
                                            <ItemTemplate>
                                                <tr>
                                                    <td><%# ((GiornataLavorativa)Container.DataItem).Data.ToShortDateString()  %></td>
                                                    <td><%# ((GiornataLavorativa)Container.DataItem).Tipo  %></td>
                                                    <td class="text-right"><%# ((GiornataLavorativa)Container.DataItem).Tariffa.ToString("c")  %></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                                <div class="bg-light-danger text-right p-3 custom-red-color">
                                    <h4> Totale dovuto <span class="font-weight-bolder"><asp:Literal runat="server" ID="litFeeTotalDetail" Text="0,00 €"></asp:Literal></span></h4>
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

