var urlPrincipal = servidor.url + "Denunciantes/";
function salvarRegistro(form,event) {
    event.preventDefault();

    form = $(form);

    var valores = form.serializeArray();

    var objeto = {};
    $.each(valores, function (i, item) {
        objeto[item.name] = item.value;
    });

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
        })
        .fail(function (data) {
            console.log(data);
        });
}

function IncluirAvulso(a) {
    if (a == 0) {
        bootbox.dialog({
            message: $('.form-TiposDenunciantes').html()
        })
    };
    if (a == 1) {
        bootbox.dialog({
            size: 'large',
            message: $('.form-Locais').html()
        })
    }
}

function FecharFormularioAvulso() {
    bootbox.hideAll();
}

function SalvarRegistroAvulso(form,event) {
        var EnderecoAvulso = servidor.url + "TiposDenunciantes/";

    event.preventDefault();

    form = $(form);

    var valores = form.serializeArray();
    var objeto = {};

    $.each(valores, function (i, item) {
        objeto[item.name] = item.value;
    });

    var promisse = requisicao(EnderecoAvulso, "POST", objeto);
    promisse.done(function () {
        successAlert("O CADASTRO FOI SALVO COM SUCESSO");
            bootbox.hideAll();
            $("#ListaTiposDenunciante").empty();
            PopularCombo()
        })
}


function Editar(id,event) {
    event.preventDefault();
    abrirFormulario();
    removerBorda('#formulario');
    var promisse = requisicao(urlPrincipal + id, "GET");
    promisse.done(function (data) {
        var formulario = $('#formulario').find("form");
        formulario.data("item", data);

        $.each(data, function (prop, valor) {
            formulario.find("[name=" + prop + "]").val(valor);
        });
    })
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
            if (!result) {
                return
            }
            else {
                var promisse = requisicao(urlPrincipal + id, "DELETE");
                promisse.done(function () {
                    atualizarGrid();
                })
            }
        }
    });
}


function atualizarGrid() {
    var promisse = requisicao(urlPrincipal, "GET");
    promisse.done(function (data) {
            grid.clear();
            grid.rows.add(data).draw();
        });
}

function PopularCombo() {
    var TiposDenunciantes = $("#ListaTiposDenunciante");

    var promisse = requisicao(servidor.url + "TiposDenunciantes", "GET");
    promisse.done(function (data) {
        repopularComboBoxDenunciantes(data, TiposDenunciantes)
        });
}

function repopularComboBoxDenunciantes(data, TiposDenunciantes) {

    for (var i = 0; i < data.length; i++) {

        for (var i = 0; i < data.length; i++) {
            if (i == 0) {
                TiposDenunciantes.append("<option value=" + "" + ">" + "--SELECIONE--" + "</option>");
            }
            TiposDenunciantes.append("<option value=" + data[i].id + ">" + data[i].descricao + "</option>");
        }
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
                width: "30px",
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