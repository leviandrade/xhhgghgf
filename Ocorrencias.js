var urlPrincipal = servidor.url + "Ocorrencias/"
var idBaseOcorrencia = 0;

function salvarRegistro(form, event) {
    event.preventDefault();

    var objeto = montarObjeto(form);

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

    var promisse = requisicao(endereco, metodo, objeto);

    promisse.done(function (data) {
        var id = data.id;
        var TipoOcorrencia = data.tipoOcorrencia
        atualizarGrid();
        LimparDados();
        sessionStorage.setItem('idOcorrencia', id);
        sessionStorage.setItem('TipoOcorrencia', TipoOcorrencia);

        if (TipoOcorrencia == 0)
            window.location.href = "Ocorrencias/FormOcorrenciasFalsificacao";

        else if (TipoOcorrencia == 1)
            window.location.href = "Ocorrencias/FormOcorrenciasRouboCarga";

        else
            window.location.href = "Ocorrencias/FormOcorrenciasContrabando";

    });

    promisse.fail(function (data) {
        console.log(data);
    });
}

function Editar(id, tipoOcorrencia) {
    sessionStorage.setItem('idOcorrencia', id);
    sessionStorage.setItem('TipoOcorrencia', TipoOcorrencia);
    if (tipoOcorrencia == 0)
        window.location.href = "Ocorrencias/FormOcorrenciasFalsificacao";

    else if (tipoOcorrencia == 1)
        window.location.href = "Ocorrencias/FormOcorrenciasRouboCarga";

    else
        window.location.href = "Ocorrencias/FormOcorrenciasContrabando";
}

function FecharSubFormularioAvulsoDenunciantes() {
    bootbox.hideAll();
    IncluirDenunciantesAvulso();
}

function FecharFormularioAvulsoOcorrencias() {
    $('#FormularioDenunciantes').find(':input').val('')
    bootbox.hideAll();
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
                })
            }
        }
    });
}


function atualizarGrid() {

    var promisse = requisicao(urlPrincipal, 'GET');

    promisse.done(function (data) {
        for (var i = 0; i < data.length; i++) {
            data[i].damage = data[i].damage.toLocaleString("pt-BR", { style: "currency", currency: "BRL" });
        }
        grid.clear();
        grid.rows.add(data).draw();
    });
}

function Finalizar(id) {
    bootbox.confirm({
        message: "DESEJA REALMENTE FINALIZAR ESTE ITEM?",
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
                var endereco = urlPrincipal + id + "/FinalizarOcorrencia";
                var promisse = requisicao(endereco, 'PUT');
                promisse.done(function () {
                    atualizarGrid();
                    LimparDados();
                    $("#finalizado").addClass("finalizar")
                })
            }
        }
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
}

function IncluirAvulso() {
    bootbox.dialog({
        size: 'large',
        message: $('.form-Locais').html()
    });
}


function format(d) {
    var html = '<table cellpadding="5" cellspacing="0" border="1" style="padding-left:50px;">';
    //html += '<tr> Denuncias </tr>' ; 
    for (var i = 0; i < d.subListaOcorrenciasItens.length; i++) {
        d.subListaOcorrenciasItens[i].damage = d.subListaOcorrenciasItens[i].damage.toLocaleString("pt-BR", { style: "currency", currency: "BRL" });

        if (i == 0) {
            html += '<tr>' +
                '<td style="background-color:#C1CDCD;font-weight:bold;text-align:center">' + "DENUNCIADOS" + '</td>' +
                '<td style="background-color:#C1CDCD;font-weight:bold;text-align:center">' + "PRODUTOS" + '</td>' +
                '<td style="background-color:#C1CDCD;font-weight:bold;text-align:center">' + "LITROS/KG" + '</td>' +
                '<td style="background-color:#C1CDCD;font-weight:bold;text-align:center">' + "PERDA" + '</td>' +
                '</tr>';
        }
        html += '<tr>' +
            '<td>' + (d.subListaOcorrenciasItens[i].denunciado != null ? d.subListaOcorrenciasItens[i].denunciado : "INDEFINIDO") + '</td>' +
            '<td>' + (d.subListaOcorrenciasItens[i].produto != null ? d.subListaOcorrenciasItens[i].produto : "INDEFINIDO") + '</td>' +
            '<td style="text-align:center">' + d.subListaOcorrenciasItens[i].litros + '</td>' +
            '<td style="text-align:right">' + 'R$ ' + d.subListaOcorrenciasItens[i].damage.toLocaleString('pt-BR') + '</td>' +
            '</tr>';
    }
    return html += '</table>';
}

$(document).on('focus', '#ListaTiposDenunciantes', function () {
    CarregarTiposDenunciantes();
});

$(document).on('focus', '#ListaLocaisDenunciantes', function () {
    CarregarLocais();
});

$(document).ready(function () {

    PopularComboDenunciantesAvulso();
    PopularComboLocaisAvulso();
    grid = $('#grid').DataTable({
        data: [],
        columns: [
            {
                "className": 'details-control',
                "orderable": false,
                "data": null,
                "defaultContent": '',
            },
            {
                data: "status",
                width: "10px",
                render: function (data) {
                    if (data == 0) {
                        var status = '<a href="#" title="Status" class="fa fa-circle" id="semParecer">&nbsp;</a>';
                    }
                    else if (data == 1)
                    {
                        var status = '<a href="#" title="Status" class="fa fa-circle" id="andamento">&nbsp;</a>';
                    }
                    else if (data == 2) {
                        var status = '<a href="#" title="Status" class="fa fa-circle" id="concluido">&nbsp;</a>';
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
                        var editar = '<a href="#" title="Editar" onclick="Editar(' + data.id + "," + data.tipoOcorrencia + ');" class="fa fa-pencil">&nbsp;</a>';
                        var excluir = '<a href="#" title="Excluir" onclick="Excluir(' + data.id + ');" class="fa fa-remove">&nbsp;</a>';
                        if (data.status == 2) {
                            var finalizar = '<a href="#" title="Finalizar" onclick="" class="fa fa-check finalizado"; id="finalizado">&nbsp;</a>';
                        }
                        else {
                            var finalizar = '<a href="#" title="Finalizar" onclick="Finalizar(' + data.id + ');" class="fa fa-check"; id="finalizado">&nbsp;</a>';
                        }

                        data = editar + '&nbsp;' + excluir + '&nbsp;' + finalizar;


                    }

                    return data;
                }
            },
            { title: "ID", data: "id" },
            { title: "FONTE GERADORA", data: "denunciante" },
            {
                data: null,
                title: "TIPO DE OCORRENCIA",
                width: "45px",
                render: function (data) {
                    var tipo = data.tipoOcorrencia;
                    if (tipo == 0)
                        data = "FALSIFICACAO";
                    if (tipo == 1)
                        data = "ROUBO DE CARGA";
                    if (tipo == 2)
                        data = "CONTRABANDO";
                    if (tipo == 3)
                        data = "DESCAMINHO";

                    return data;

                },
            },
            { title: "DATA", data: "dataOcorrencia", width: "20px", "className": "text-center", },
            { title: "BOLETIM DE OCORRENCIA", data: "boletimOcorrencia" },
            { title: "LOCAL", data: "localOcorrencia" },
            { title: "FECHAMENTO", data: "dataFechamento", "className": "text-center", },
            {
                data: null,
                title: "PERDA",
                "className": "text-right",
                width: "20px",
                render: function (data) {
                    var valor = data.damage;
                    data = valor.toLocaleString('pt-BR');
                    return data
                }
            },
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
        "pagingType": "full_numbers",
    });

    atualizarGrid();

    $('#grid tbody').on('click', 'td.details-control', function () {
        var tr = $(this).closest('tr');
        var row = grid.row(tr);

        if (row.child.isShown()) {
            row.child.hide();
            tr.removeClass('shown');
        }
        else {
            row.child(format(row.data())).show();
            tr.addClass('shown');
        }
    });
});

function SalvarDenuncianteAvulso(form, event) {

    var enderecoAvulso = servidor.url + "Denunciantes/";
    event.preventDefault();

    var objeto = montarObjeto(form);
    var promisse = requisicao(enderecoAvulso, 'POST', objeto);
    promisse.done(function () {
        successAlert("O CADASTRO FOI SALVO COM SUCESSO");
        $('#FormularioDenunciantes').find(":input").val("");
        bootbox.hideAll();
        adicionarBorda('#FormularioDenunciantes')
        PopularComboDenunciantesAvulso();
    })
}


