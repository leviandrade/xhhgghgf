var urlPrincipal = servidor.url + "Empresas/";
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
        console.log(data);
    });
}

function Editar(id, event) {
    event.preventDefault();
    removerBorda('#formulario');
    abrirFormulario()
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
        grid
            .rows
            .add(data)
            .draw();
    });
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

function PopularCombo() {
    var ListaRamosAtividade = $("#ListaRamosAtividade");
    var endereco = servidor.url + "RamosAtividade";
    var promisse = requisicao(endereco, 'GET');
    promisse.done(function (data) {
        repopularComboBox(data, ListaRamosAtividade)
    });
}

function repopularComboBox(data, ListaRamosAtividade) {
    for (var i = 0; i < data.length; i++) {
        if (i == 0) {
            ListaRamosAtividade.append("<option value=" + "" + ">" + "--Selecione--" + "</option>");
        }
        ListaRamosAtividade.append("<option value=" + data[i].id + ">" + data[i].nome + "</option>");
    }
}

function IncluirAvulso() {
    bootbox.dialog({
        message: $('.form-RamosAtividade').html()
    });
}

function SalvarRegistroAvulso(form, event) {
    event.preventDefault();
    var enderecoAvulso = servidor.url + "RamosAtividade/";
    var objeto = montarObjeto(form);
    var promisse = requisicao(enderecoAvulso, 'POST', objeto);
    promisse.done(function () {
        successAlert("O CADASTRO FOI SALVO COM SUCESSO");
        bootbox.hideAll();
        $("#ListaRamosAtividade").empty();
        PopularCombo()
    });
}

function FecharFormularioAvulso() {
    bootbox.hideAll();
}

$(document).on('keyup', '.telefone', function () {
    formataTelefone(this)
})

$(document).ready(function () {
    //$("#cep").blur(function () {
    //    var cep = $(this).val().replace(/\D/g, '');
    //    $.getJSON("https://viacep.com.br/ws/" + cep + "/json/?callback=?", function (dados) {
    //        $("#rua").val(dados.logradouro);
    //        $("#bairro").val(dados.bairro);
    //        $("#cidade").val(dados.localidade);
    //        $("#uf").val(dados.uf);
    //        $("#ibge").val(dados.ibge);
    //    });
    //});
    PopularCombo();
    grid = $('#grid').DataTable({
        data: [],
        columns: [
            {
                data: "id",
                width: "20px",
                render: function (data, type) {
                    if (type === 'display') {

                        var editar = '<a href="#" title="Editar" onclick="Editar(' + data + ",event" + ');" class="fa fa-pencil">&nbsp;</a>';
                        var excluir = '<a href="#" title="Excluir" onclick="Excluir(' + data + ');" class="fa fa-remove">&nbsp;</a>';

                        data = editar + '&nbsp;' + excluir;
                    }

                    return data;
                }
            },
            { title: "EMPRESAS", data: "razaoSocial" },
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