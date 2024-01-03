<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>
<html dir="ltr">

<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <!-- Tell the browser to be responsive to screen width -->
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="description" content="">
    <meta name="author" content="">
    <!-- Favicon icon -->
    <link rel="icon" type="image/png" sizes="16x16" href="assets/images/favicon.ico">
    <title>PP Autotrasporti | Admin dashboard</title>
    <!-- Custom CSS -->
    <link href="assets/css/style.css" rel="stylesheet">
    <link href="assets/libs/toastr/dist/build/toastr.min.css" rel="stylesheet" />
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
    <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
    <script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
<![endif]-->
</head>

<body>
    <form runat="server" id="frm">
        <asp:ScriptManager runat="server" ID="scrMgr" ScriptMode="Release" EnableScriptLocalization="true" EnableScriptGlobalization="true"></asp:ScriptManager>

        <div class="main-wrapper">

            <div class="preloader">
                <div class="lds-ripple">
                    <div class="lds-pos"></div>
                    <div class="lds-pos"></div>
                </div>
            </div>

            <div class="auth-wrapper d-flex no-block justify-content-center align-items-center position-relative"
                style="background: linear-gradient(rgba(0, 0, 0, 0.73), rgba(2, 42, 104, 0.67)), url(assets/images/bg.jpg) no-repeat; background-size: cover">
                <div class="auth-box row text-center" style="background-color: #fff">
                    <div class="col-lg-7 col-md-5 modal-bg-img custom-red-bg">
                        <img src="assets/images/logo_bianco.png" style="margin: 30px; margin-top: 7em;" />
                    </div>
                    <div class="col-lg-5 col-md-7">
                        <div class="p-3">
                            <h2 class="mt-3 text-center font-weight-bolder">Accesso area riservata</h2>
                            <div class="mt-4">
                                <div class="row">
                                    <div class="col-lg-12">
                                        <div class="form-group">
                                            <input runat="server" id="txtUser" class="form-control" data-kioskboard-type="all" data-kioskboard-placement="bottom" data-kioskboard-specialcharacters="true" type="text" placeholder="username" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12">
                                        <div class="form-group">
                                            <input runat="server" id="txtPassword" class="form-control" data-kioskboard-type="all" data-kioskboard-placement="bottom" data-kioskboard-specialcharacters="true" type="password" placeholder="password" />
                                        </div>
                                    </div>
                                    <div class="col-lg-12 text-center">
                                        <a class="btn btn-block btn-dark" onclick="verifyCaptcha();" href="#">Accedi</a>
                                        <asp:LinkButton runat="server" ID="lnkLogin" CssClass="at-btn" OnClick="lnkLogin_Click"></asp:LinkButton>
                                    </div>
                                    <%--<div runat="server" id="divCaptcha" class="g-recaptcha" data-callback="sendFormAction" data-size="invisible"></div>--%>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>

        <script src="assets/libs/jquery/dist/jquery.min.js "></script>
        <script src="assets/libs/popper.js/dist/umd/popper.min.js "></script>
        <script src="assets/libs/bootstrap/dist/js/bootstrap.min.js "></script>
        <script src="assets/libs/customScripts.js"></script>
        <script src="assets/libs/toastr/dist/toastr.js"></script>
        <script src="https://www.google.com/recaptcha/api.js" async defer></script>
        <script>
            function verifyCaptcha() {
                //grecaptcha.execute();
                document.getElementById('<%= lnkLogin.ClientID %>').click();
            }

            function sendFormAction(token) {
                document.getElementById('<%= lnkLogin.ClientID %>').click();
                grecaptcha.reset();
            }

            $(".preloader ").fadeOut();

            $(document).keypress(function (event) {
                var keycode = (event.keyCode ? event.keyCode : event.which);
                if (keycode == '13') {
                    verifyCaptcha();
                }
            });
        </script>
    </form>
</body>

</html>
