var urlPrincipal = servidor.url + "Ocorrencias/"

function salvarRegistro(form) {
    event.preventDefault();

    form = $(form);

    var valores = form.serializeArray();
    var objeto = {};
    $.each(valores, function (i, item) {
        if (item.name == "damage") {
            var valor = item.value.replace(/\./g, '');
            valor = valor.replace(',', '.');
            objeto[item.name] = valor;
        }
        else {
            objeto[item.name] = item.value;
        }
    });

    var metodo = "POST";
    var endereco = urlPrincipal;

    if (objeto.id == "") {
        objeto.id = 0;
    }

    if (objeto.status == "") {
        objeto.status = 0;
    }

    if (objeto.id > 0) {
        metodo = "PUT";
        endereco = endereco + objeto.id
    }

        $.ajax({
            url: endereco,
            type: metodo,
            data: JSON.stringify(objeto),
            contentType: 'application/json'
        })
            .done(function () {
                atualizarGrid();
                LimparDados();
            })
            .fail(function (data) {
                console.log(data);
            });
    }

function LimparDados() {
    $('#formulario').find("input").val("");
    $('#formulario').find(".is-invalid").removeClass("is-invalid");
}

function Editar(id) {
    event.preventDefault();
    abrirFormulario()
    $.ajax({
        url: urlPrincipal + id,
        type: 'GET',
        contentType: 'application/json'
    })
   .done(function (data) {
        var formulario = $('#formulario').find("form");
        formulario.data("item", data);
        if (data.data != null) {
            data.data = (data.data).replace(/(\d*)-(\d*)-(\d*).*/, '$1-$2-$3');
        }
        if (data.fechamento != null) {
            data.fechamento = (data.fechamento).replace(/(\d*)-(\d*)-(\d*).*/, '$1-$2-$3');
        }

        data.damage = data.damage.toLocaleString('pt-BR')

        $.each(data, function (prop, valor) {
            formulario.find("[name=" + prop + "]").val(valor);
        });
    })
}

function Excluir(id) {
    bootbox.confirm({
        message: "Deseja realmente excluir este item?",
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
                $.ajax({
                    url: urlPrincipal + id,
                    type: 'DELETE',
                    contentType: 'application/json'
                }).done(function () {
                    atualizarGrid();
                })
            }
        }
    });
}


function atualizarGrid() {
    $.ajax({
        url: urlPrincipal,
        type: 'GET',
        contentType: 'application/json'
    })
        .done(function (data) {
            grid.clear();
            grid.rows.add(data).draw();
        });
}

function Finalizar(id) {
    bootbox.confirm({
    message: "Deseja realmente finalizar este item?",
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
                $.ajax({
                    url: urlPrincipal + id + "/FinalizarOcorrencia",
                    type: 'PUT',
                    contentType: 'application/json'
                }).done(function () {
                    atualizarGrid();
                    LimparDados();
                    $("#finalizado").addClass("finalizar")
                })
            }
    }
});
}

function PopularCombo() {
    var Denunciantes = $("#ListaDenunciantes");
    var Denunciados = $("#ListaDenunciados");
    var Produtos = $("#ListaProdutos");
    //$.ajax({
    //    url: servidor.Url + "Ocorrencias/ListaCombo",
    //    type: 'GET',
    //    contentType: 'application/json'
    //})
    $.get(servidor.urlPrincipal + "ListaCombo").done(function (data) {
            repopularComboBox(data, Denunciantes, Denunciados, Produtos)
        });
}

function repopularComboBox(data, Denunciantes, Denunciados, Produtos) {


    for (var i = 0; i < data.length; i++) {
        if (i == 0) {
            Denunciantes.append("<option value=" + "" + ">" + "--Selecione--" + "</option>");
            Denunciados.append("<option value=" + "" + ">" + "--Selecione--" + "</option>");
            Produtos.append("<option value=" + "" + ">" + "--Selecione--" + "</option>");
        }
            if (data[i].idDenunciante > 0) {

                Denunciantes.append("<option value=" + data[i].idDenunciante + ">" + data[i].descricaoDenunciante + "</option>");
            }

            if (data[i].idDenunciado > 0) {
                Denunciados.append("<option value=" + data[i].idDenunciado + ">" + data[i].descricaoDenunciado + "</option>");
            }

            if (data[i].idProduto > 0) {
                Produtos.append("<option value=" + data[i].idProduto + ">" + data[i].descricaoProduto + "</option>");
            }
    }
}

function SalvarRegistroAvulso(form, b) {
    if (b == 0) {
        var EnderecoAvulso = servidor.url + "Denunciados/"
    }
    if (b == 1) {
        var EnderecoAvulso = servidor.url + "Denunciantes/"
    }
    if (b == 2) {
        var EnderecoAvulso = servidor.url + "Produtos/"
    }

    event.preventDefault();

    form = $(form);

    var valores = form.serializeArray();

    var objeto = {};

    $.each(valores, function (i, item) {
        objeto[item.name] = item.value;
    });

    $.ajax({
        url: EnderecoAvulso,
        type: 'POST',
        data: JSON.stringify(objeto),
        contentType: 'application/json'
    })
        .done(function () {
            bootbox.hideAll();
            $("#ListaProdutos").empty();
            $("#ListaDenunciantes").empty();
            $("#ListaDenunciados").empty();
            PopularCombo()
        })
}

function incluir() {
    event.preventDefault();
    $("#TituloFormulario").html("Incluir");

    abrirFormulario();
}

function abrirFormulario() {
    var form = $('#formulario').find("form");
    LimparDados();
    $('#valor').money('[###.]###,##');
    $('#formulario').removeClass('d-none');

    return form;
}
function fecharFormulario() {
    LimparDados();
    $('#formulario').addClass('d-none');
}

function FecharFormularioAvulso() {
    bootbox.hideAll();
    LimparFormularioAvulso();
}

function LimparFormularioAvulso(){
    $("#ListaLocaisDenunciantes").empty();
    $("#ListaLocaisDenunciados").empty();
    $("#ListaTiposDenunciantes").empty();
    $("#ListaTiposDenunciados").empty();
    $("#ListaCategoriasProdutos").empty();
    $("#ListaClassesProdutos").empty();
}

function IncluirAvulso(a) {
    if (a == 0) {
        $.get(servidor.url + "Denunciados/ListaCombo/").done(function (data) {
            repopularSubComboBox(data,a)
            bootbox.dialog({
                size: 'large',
                message: $('.form-Denunciados').html()
            });
        });
    }

    if (a == 1) {
        $.get(servidor.url + "Denunciantes/ListaCombo").done(function (data) {
            repopularSubComboBox(data,a)
            bootbox.dialog({
                size: 'large',
                message: $('.form-Denunciantes').html()
            });
        });
    }

    if (a == 2) {
        $.get(servidor.url + "Produtos/ListaCombo").done(function (data) {
            repopularSubComboBox(data, a)
            console.log(data)
            bootbox.dialog({
                size: 'large',
                message: $('.form-Produtos').html()
            });
        });
    }
}
function repopularSubComboBox(data, a) {
    LimparFormularioAvulso();

    var ListaLocaisDenunciantes = $("#ListaLocaisDenunciantes");
    var ListaLocaisDenunciados = $("#ListaLocaisDenunciados");
    var TiposDenunciantes = $("#ListaTiposDenunciantes");
    var TiposDenunciados = $("#ListaTiposDenunciados");
    var CategoriasProdutos = $("#ListaCategoriasProdutos");
    var ClassesProdutos = $("#ListaClassesProdutos");

    for (var i = 0; i < data.length; i++) {
        if (i == 0) {
            ListaLocaisDenunciantes.append("<option value=" + "" + ">" + "--Selecione--" + "</option>");
            ListaLocaisDenunciados.append("<option value=" + "" + ">" + "--Selecione--" + "</option>");
            TiposDenunciantes.append("<option value=" + "" + ">" + "--Selecione--" + "</option>");
            TiposDenunciados.append("<option value=" + "" + ">" + "--Selecione--" + "</option>");
            CategoriasProdutos.append("<option value=" + "" + ">" + "--Selecione--" + "</option>");
            ClassesProdutos.append("<option value=" + "" + ">" + "--Selecione--" + "</option>");
        }
        if (a == 0) {
            if (data[i].idTipoDenunciado > 0) {

                TiposDenunciados.append("<option value=" + data[i].idTipoDenunciado + ">" + data[i].descricaoTipoDenunciado + "</option>");
            }
            if (data[i].idLocal > 0) {
                ListaLocaisDenunciados.append("<option value=" + data[i].idLocal + ">" + data[i].descricaoLocal + "</option>");
            }
        }

        if (a == 1) {
            if (data[i].idTipoDenunciante > 0) {

                TiposDenunciantes.append("<option value=" + data[i].idTipoDenunciante + ">" + data[i].descricaoTipoDenunciante + "</option>");
            }
            if (data[i].idLocal > 0) {
                ListaLocaisDenunciantes.append("<option value=" + data[i].idLocal + ">" + data[i].descricaoLocal + "</option>");
            }
        }

        if (a == 2) {
            if (data[i].idCategoriaProduto > 0) {
                CategoriasProdutos.append("<option value=" + data[i].idCategoriaProduto + ">" + data[i].descricaoCategoriaProduto + "</option>");
            }

            if (data[i].idClasseProduto > 0) {
                ClassesProdutos.append("<option value=" + data[i].idClasseProduto + ">" + data[i].descricaoClasseProduto + "</option>");
            }
        }
    }
}

function IncluirSubAvulso(a) {
    if (a == 0) {
            var teste = bootbox.dialog({
                message: $('.form-SubTiposDenunciantes').html(),
                closeButton: false
        });
        teste.modal('hide');
    }
}

function FecharSubFormularioAvulso() {
    teste.hideAll();
}

$(document).ready(function () {
    PopularCombo();
    grid = $('#grid').DataTable({
        data: [],
        columns: [
            {
                data: "status",
                width: "10px",
                render: function (data) {
                    if (data == 1) {
                        var status = '<a href="#" title="Status" class="fa fa-circle" id="statusfinalizado">&nbsp;</a>';
                    }
                    else {
                        var status = '<a href="#" title="Status" class="fa fa-circle" id="status">&nbsp;</a>';
                    }
                    data = status + '&nbsp;'
                    return data;
                }
            },
            {
                data: null,
                width: "45px",
                render: function (data, type) {
                    if (type === 'display') {
                        var editar = '<a href="#" title="Editar" onclick="Editar(' + data.id + ');" class="fa fa-pencil">&nbsp;</a>';
                        var excluir = '<a href="#" title="Excluir" onclick="Excluir(' + data.id + ');" class="fa fa-remove">&nbsp;</a>';
                        if (data.status == 1) {
                            var finalizar = '<a href="#" title="Finalizar" onclick="" class="fa fa-check finalizado"; id="finalizado">&nbsp;</a>';
                        }
                        else {
                            var finalizar = '<a href="#" title="Finalizar" onclick="Finalizar(' + data.id + ');" class="fa fa-check"; id="finalizado">&nbsp;</a>';
                        }

                        data = editar + '&nbsp;' + excluir + '&nbsp;' + finalizar ;
                    }

                    return data;
                }
            },
            { title: "Data", data: "dataOcorrencia", width: "20px", },
            { title: "Boletim de Ocorrência", data: "boletimOcorrencia"  },
            { title: "Denunciante", data: "denunciante" },
            { title: "Denunciado", data: "denunciado" },
            { title: "Produto", data: "nomeProduto" },
            {
                data: null,
                title: "Damage",
                width: "20px",
                render: function (data) {
                    var valor = data.damage;
                    data = 'R$' + ' ' + valor.toLocaleString('pt-BR');
                    return data
                }
            },
        ],
        language: {
            "lengthMenu": "_MENU_ registros por página",
            "zeroRecords": "Nenhum registro encontrado",
            "info": "Exibindo _PAGE_ de _PAGES_",
            "infoEmpty": "Nenhum registro encontrado",
            "infoFiltered": "(Filtrado de _MAX_ registros)",
            "search": "Pesquisar:",
            "paginate": {
                "first": "Primeiro",
                "last": "Último",
                "next": "Próximo",
                "previous": "Anterior"
            }
        },
        "pagingType": "full_numbers"
    });

    atualizarGrid();
});