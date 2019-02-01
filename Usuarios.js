var urlPrincipal = servidor.url + "Usuarios/";

$(document).ready(function () {
    permissaoUsuario(window.location.href, sessionStorage.getItem('menus'));
    grid = $('#gridUsuarios').DataTable({
        data: [],
        columns: [
            {
                data: "id",
                width: "20px",
                render: function (data, type) {
                    if (type === 'display') {

                        var editar = '<a href="#" title="Editar" onclick="editar(' + data + ",event" + ');" class="fa fa-pencil">&nbsp;</a>';
                        var excluir = '<a href="#" title="Excluir" onclick="excluir(' + data + ');" class="fa fa-remove">&nbsp;</a>';

                        data = editar + '&nbsp;' + excluir;
                    }

                    return data;
                }
            },
            { title: "Nome", data: "nome" },
            { title: "Login", data: "login" },
        ],
        language: {
            "lengthMenu": "_MENU_ REGISTROS POR PAGINA",
            "zeroRecords": "NENHUM REGISTRO ENCONTRADO",
            "info": "EXIBINDO _PAGE_ DE _PAGES_",
            "infoEmpty": "NENHUM REGISTRO ENCONTRADO",
            "infoFiltered": "(FILTRADO DE _MAX_ REGISTROS)",
            "search": "PESQUISAR:",
            "sPaginationType": "pagination",
            "paginate": {
                "first": "PRIMEIRO",
                "last": "ULTIMO",
                "next": "PROXIMO",
                "previous": "ANTERIOR"
            }
        },
        "pagingType": "full_numbers"
    });

    atualizarGridUsuarios();
    atualizarEmpresas();
});

function limparDados() {
    $("#fUsuarios")[0].reset();
    $("#txtId").val("");
    $("#formularios").find(".obrigatorios").css("border-color", "red");
}

function abrirFormulario() {
    limparDados();
    $("#txtSenha").prop('required', true);
    $("#txtConfirmaSenha").prop('required', true);

    $('#formularios').removeClass('d-none');
    atualizarEmpresas();
}

function fecharFormulario() {
    limparDados();
    $('#formularios').addClass('d-none');
}

function salvarRegistro(form, event) {

    event.preventDefault();

    var senha = $("#txtSenha").val();
    var confirmaSenha = $("#txtConfirmaSenha").val();

    var objeto = montarObjeto(form);

    if (!validarEmail(objeto.email)) {
        bootbox.alert("E-mail inválido.");
        return false;
    }

    var permissaoEmpresa = permissoesEmpresas();

    if (permissaoEmpresa == "")
        permissaoEmpresa = objeto.idEmpresa;

    objeto["PermissaoEmpresas"] = permissaoEmpresa;
    objeto["PermissaoTiposOcorrencias"] = permissaoTipoOcorrencias();
    objeto["PermissaoMenus"] = PermissaoMenus();

    var metodo = "POST";
    var endereco = urlPrincipal;

    if ($("#Sindiveg").is(':checked')) {
        objeto.Sindiveg = true;
    }

    objeto.senha = btoa(objeto.senha);

    if (objeto.status == "")
        objeto.status = 0;

    if (objeto.id == "") {
        objeto.id = 0;
        if (senha != confirmaSenha) {
            bootbox.alert("O campo Senha não está igual ao campo Confirmar Senha.");
            $("#txtConfirmaSenha").css("border-color", "red");
            return false;
        }
    }

    if (objeto.id > 0) {
        metodo = "PUT";
        endereco = endereco + objeto.id
        if (senha != "") {
            if (senha != confirmaSenha) {
                bootbox.alert("O campo Senha não está igual ao campo Confirmar Senha.");
                $("#txtConfirmaSenha").css("border-color", "red");
                return false;
            }
        }
    }

    var promisse = requisicao(endereco, metodo, objeto);

    promisse.done(function () {
        atualizarGridUsuarios();
        limparDados();
        successAlert("O CADASTRO FOI SALVO COM SUCESSO");
    });

    promisse.fail(function (data) {
        bootbox.alert(data.responseJSON.join(", "));
    });
}

function PermissaoMenus() {
    var permissoes = $("#gridPermissoesMenus").find("tbody tr td input[type='checkbox']");

    var listaValor = [];
    for (var i = 0; i < permissoes.length; i++) {
        var permissao = permissoes.eq(i);
        if (permissao.is(':checked')) {
            listaValor.push(permissao.attr("data-id"));
        }
    }
    return listaValor.join(",");
}

function editar(id, event) {

    event.preventDefault();
    abrirFormulario();
    removerBorda('#fUsuarios');
    $("#txtSenha").prop('required', false);
    $("#txtConfirmaSenha").prop('required', false);

    var endereco = urlPrincipal + id;

    var promise = requisicao(endereco, 'GET');
    promise.done(function (data) {
        if (data.sindiveg == true) {
            $('#Sindiveg').prop("checked", true);
        }
        var formulario = $('#fUsuarios');
        formulario.data("item", data);

        data.senha = '';

        $.each(data, function (prop, valor) {
            formulario.find("[name=" + prop + "]").val(valor);
        });

        populaPermissoes(data.permissaoEmpresas, "#gridPermissoesEmpresas");
        populaPermissoes(data.permissaoTiposOcorrencias, "#gridPermissoesTipoOcorrencias");

    });
}

function excluir(id) {
    bootbox.confirm({
        message: "DESEJA REALMENTE EXCLUIR ESTE ITEM?",
        buttons: {
            cancel: {
                label: 'NÃO',
                className: 'btn-danger'
            },
            confirm: {
                label: 'SIM',
                className: 'btn-success'
            },
        },
        callback: function (result) {
            if (result == false) {
                return
            }
            else {

                var endereco = urlPrincipal + id;
                var promisse = requisicao(endereco, 'DELETE');

                promisse.done(function () {
                    atualizarGridUsuarios();
                    successAlert("Usuário excluido com sucesso.");
                });
            }
        }
    });
}

function permissoesEmpresas() {
    return permissao("#gridPermissoesEmpresas");
}


function permissaoTipoOcorrencias() {
    return permissao("#gridPermissoesTipoOcorrencias");
}

function permissao(cbb) {
    var permissoes = $(cbb).find("tbody tr td input[type='checkbox']");

    var listaValorPermissao = [];
    for (var i = 0; i < permissoes.length; i++) {
        var permissao = permissoes.eq(i);
        if (permissao.is(':checked')) {
            listaValorPermissao.push(permissao.attr("data-id"));
        }
    }
    return listaValorPermissao.join(",");
}


function populaPermissoesEmpresa(data) {
    populaPermissoes(data, "#gridPermissoesEmpresas");
}

function populaPermissoes(data, cb) {

    if (data && data != "") {
        var listaNumeroEmpresa = data.split(",");
        var empresas = $(cb).find("tbody tr td input[type='checkbox']");

        for (var i = 0; i < listaNumeroEmpresa.length; i++) {
            for (var y = 0; y < empresas.length; y++) {
                var empresa = empresas.eq(y);
                var numeroEmpresa = empresa.attr("data-id");
                if (listaNumeroEmpresa[i] == numeroEmpresa) {
                    empresa.prop("checked", true);
                    break;
                }
            }
        }
    }
}

function atualizarGridUsuarios() {

    var promisse = requisicao(urlPrincipal, 'GET');

    promisse.done(function (data) {
        grid.clear();
        grid
            .rows
            .add(data)
            .draw();
    });
}

function atualizarEmpresas() {

    var endereco = servidor.url + "Empresas/";
    var promisse = requisicao(endereco, 'GET');

    promisse.done(function (data) {
        populaGridPermissoes(data);
        populaComboBoxEmpresas(data);
    });
}

function populaGridPermissoes(data) {

    var linhas = [];

    data.forEach(function (empresa) {
        var nomeExibicao = empresa.nomeFantasia == "" ? empresa.razaoSocial : empresa.nomeFantasia;
        var linha = '<tr><td class="col-1"> <input type="checkbox" data-id="' + empresa.id + '"></td><td class="col-11">' + nomeExibicao + '</td></tr>';
        linhas.push(linha);
    });

    $("#gridPermissoesEmpresas").find("tbody").empty();
    $("#gridPermissoesEmpresas").find("tbody").append(linhas);
}


function populaComboBoxEmpresas(data) {
    $("#ddlEmpresa").find("option").remove()
    $("#ddlEmpresa").append("<option value=" + "" + ">" + "--SELECIONE--" + "</option>");
    data.forEach(function (empresa) {
        var nomeExibicao = empresa.nomeFantasia == "" ? empresa.razaoSocial : empresa.nomeFantasia;
        var option = new Option(nomeExibicao, empresa.id);
        $("#ddlEmpresa").append(option);
    });

}


