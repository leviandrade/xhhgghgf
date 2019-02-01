var urlPrincipal = servidor.url + "Denunciados/";
function salvarRegistro(form, event) {
    event.preventDefault();
    var objeto = montarObjeto(form);

    var metodo = "POST";
    var endereco = urlPrincipal;

    if (objeto.id == "") {
        objeto.id = 0;
    }

    if (objeto.id > 0) {
        metodo = "PUT";
        endereco = endereco + objeto.id
    }

    var promisse = requisicao(endereco, metodo, objeto);
    promisse.done(function () {
        atualizarGrid();
        LimparDados();
        successAlert("O CADASTRO FOI SALVO COM SUCESSO");
    });

    promisse.fail(function (data) {
        console.log(data.responseJSON[0]);
        bootbox.alert(data.responseJSON[0]);
    });
}

function IncluirAvulso(a) {
    if (a == 0) {
        bootbox.dialog({
            message: $('.form-TiposDenunciados').html()
        });
    }
    if (a == 1) {
        bootbox.dialog({
            size: "large",
            message: $('.form-Locais').html()
        })
    }
}

function FecharFormularioAvulso() {
    bootbox.hideAll();
}

function SalvarRegistroAvulso(form,event) {
    var enderecoAvulso = servidor.url + "TiposDenunciados/";
     event.preventDefault();

    var objeto = montarObjeto(form);
    var promisse = requisicao(enderecoAvulso, 'POST', objeto);
    promisse.done(function () {
        successAlert("O CADASTRO FOI SALVO COM SUCESSO");
        bootbox.hideAll();
        $("#ListaTiposDenunciados").empty();
        PopularCombo()
    });
}

function Editar(id, event) {
    event.preventDefault();
    abrirFormulario();
    removerBorda('#formulario');
    var endereco = urlPrincipal + id;
    var promisse = requisicao(endereco, 'GET');
    promisse.done(function (data) {
        var formulario = $('#formulario').find("form");
        formulario.data("item", data);
        $.each(data, function (prop, valor) {
            formulario.find("[name=" + prop + "]").val(valor);
        });
    });
}

function Excluir(id) {
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
                    atualizarGrid();
                });
            }
        }
    });
}


function atualizarGrid() {
    var promisse = requisicao(urlPrincipal, 'GET');
    promisse.done(function (data) {
        grid.clear();
        grid.rows.add(data).draw();
    });
}

function PopularCombo() {
    var TiposDenunciados = $("#ListaTiposDenunciados");
    var endereco = servidor.url + "TiposDenunciados";
    var promisse = requisicao(endereco, 'GET');
    promisse.done(function (data) {
        repopularComboTiposDenunciados(data, TiposDenunciados)
    });
}

function repopularComboTiposDenunciados(data, TiposDenunciados) {
    for (var i = 0; i < data.length; i++) {
        if (i == 0) {
            TiposDenunciados.append("<option value=" + "" + ">" + "--SELECIONE--" + "</option>");
        }
        TiposDenunciados.append("<option value=" + data[i].id + ">" + data[i].descricao + "</option>");
    }
}

function incluir(event) {
    event.preventDefault();
    $("#TituloFormulario").html("Incluir");
    abrirFormulario();
}


function abrirFormulario() {
    var form = $('#formulario').find("form");
    LimparDados();
    $('#formulario').removeClass('d-none');
    return form;
}
function fecharFormulario() {
    LimparDados();
    $('#formulario').addClass('d-none');
}

$(document).ready(function () {
    PopularCombo();
    PopularComboLocaisAvulso();
    grid = $('#grid').DataTable({
        data: [],
        columns: [
            {
                data: "id",
                width: "40px",
                render: function (data, type) {
                    if (type === 'display') {

                        var editar = '<a href="#" title="Editar" onclick="Editar(' + data + ",event" + ');" class="fa fa-pencil">&nbsp;</a>';
                        var excluir = '<a href="#" title="Excluir" onclick="Excluir(' + data + ');" class="fa fa-remove">&nbsp;</a>';

                        data = editar + '&nbsp;' + excluir;
                    }

                    return data;
                }
            },
            { title: "NOME", data: "nome" },
            { title: "TIPO", data: "documento" },
            { title: "NUMERO", data: "numeroDocumento" },
            { title: "DESCRICAO", data: "descricaoLocal" },
            { title: "CIDADE", data: "cidade" },
            { title: "ESTADO", data: "estado" },
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

    atualizarGrid();
});