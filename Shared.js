function SalvarEmpresa(form, event) {
    var enderecoAvulso = servidor.url + "Empresas/"
    event.preventDefault();
    var objeto = montarObjeto(form);
    var promisse = requisicao(enderecoAvulso, 'POST', objeto);
    promisse.done(function () {
        bootbox.hideAll();
        $('#FormEmpresa').find(':input').val('');
        $(".ListaEmpresas").empty();
        adicionarBorda('#FormEmpresa')
        successAlert("O CADASTRO FOI SALVO COM SUCESSO");
        PopularEmpresaAvulsa()
    });
}
//function IncluirEmpresaAvulsa() {
//    ListaRamosAtividade = $(".ListaRamosAtividade");
//    $.get(servidor.url + "RamosAtividade")
//        .done(function (data) {
//            $(".ListaRamosAtividade").empty();
//            for (var i = 0; i < data.length; i++) {
//                if (i == 0) {
//                    ListaRamosAtividade.append("<option value=" + "" + ">" + "--Selecione--" + "</option>");
//                }
//                ListaRamosAtividade.append("<option value=" + data[i].id + ">" + data[i].nome + "</option>");
//            }
//            bootbox.dialog({
//                size: 'large',
//                message: $(".form-Empresa").html()
//            });
//        });
//}

var boxEmpresa = null;
function IncluirEmpresa() {
    if (boxEmpresa == null) {
        boxEmpresa = bootbox.dialog({
            closeButton: false,
            size: 'large',
            message: htmlEmpresa,
            show: false
        })
        CarregarRamosAtividade();
        boxEmpresa.modal('show');
    }
    else {
        boxEmpresa.modal('show');
    }
    formatarCep()
}
$(document).on('focus', '#RamosAtividade', function () {
    CarregarRamosAtividade()
});
function CarregarRamosAtividade() {
    ListaRamosAtividade = $(".ListaRamosAtividade");
    var endereco = servidor.url + "RamosAtividade";
    var promisse = requisicao(endereco, 'GET');
    promisse.done(function (data) {
        $(ListaRamosAtividade).find("option").remove();
        for (var i = 0; i < data.length; i++) {
            if (i == 0) {
                ListaRamosAtividade.append("<option value=''>" + "--SELECIONE--" + "</option>");
            }
            $(ListaRamosAtividade).append("<option value=" + data[i].id + ">" + data[i].nome + "</option>");
        }
    })
}

var ModalRamosAtividade = null;
function IncluirRamosAtividade() {
    boxEmpresa.modal('hide');
    ModalRamosAtividade = bootbox.dialog({
        backdrop: false,
        message: RamosAtividade
    });
}

function salvarRamosAtividade(form, event) {
    var enderecoAvulso = servidor.url + "RamosAtividade/";
    event.preventDefault();
    var objeto = montarObjeto(form);
    var promisse = requisicao(enderecoAvulso, 'POST', objeto);
    promisse.done(function () {
        successAlert("O CADASTRO FOI SALVO COM SUCESSO");
        bootbox.hideAll();
        IncluirEmpresa();
    });
}

function FecharRamosAtividadeAvulso() {
    bootbox.hideAll();
    boxEmpresa.modal('show');
}

$(document).on('focus', '#RamoAtividadeEmpresa', function () {
    CarregarRamosAtividadeEmpresa();
});

$(document).on('focus', '.Cnpj', function () {
    CarregarRamosAtividadeEmpresa();
    FormataCNPJ();
});

function CarregarRamosAtividadeEmpresa() {
    ListaRamosAtividade = $("#RamoAtividadeEmpresa");
    var endereco = servidor.url + "RamosAtividade";
    var promisse = requisicao(endereco, 'GET');
    promisse.done(function (data) {
        $(ListaRamosAtividade).find("option").remove();
        for (var i = 0; i < data.length; i++) {
            if (i == 0) {
                ListaRamosAtividade.append("<option value=''>" + "--SELECIONE--" + "</option>");
            }
            ListaRamosAtividade.append("<option value=" + data[i].id + ">" + data[i].nome + "</option>");
        }
    });
}

function salvarRamosAtividadeAvulso(form, event) {
    event.preventDefault();
    var enderecoAvulso = servidor.url + "RamosAtividade/";
    var objeto = montarObjeto(form);
    var promisse = requisicao(enderecoAvulso, 'POST', objeto);
    promisse.done(function () {
        successAlert("O CADASTRO FOI SALVO COM SUCESSO");
        bootbox.hideAll();
        IncluirEmpresaAvulsa();
    });
}

function PopularEmpresaAvulsa() {
    var ListaEmpresas = $(".ListaEmpresas");
    var endereco = servidor.url + "Empresas";
    var promisse = requisicao(endereco, 'GET');
    promisse.done(function (data) {
        for (var i = 0; i < data.length; i++) {
            if (i == 0) {
                ListaEmpresas.append("<option value=" + "" + ">" + "--SELECIONE--" + "</option>");
            }
            ListaEmpresas.append("<option value=" + data[i].id + ">" + data[i].razaoSocial + "</option>");
        }
    });
}

function CarregarTiposDenunciantes() {

    var ListaDenunciantes = $("#ListaTiposDenunciantes");
    var endereco = servidor.url + "TiposDenunciantes";
    var promisse = requisicao(endereco, 'GET');
    promisse.done(function (dados) {
        $(ListaDenunciantes).find("option").remove();
        for (var i = 0; i < dados.length; i++) {
            if (i == 0) {
                ListaDenunciantes.append("<option value=''>" + "--SELECIONE--" + "</option>");
            }
            ListaDenunciantes.append("<option value=" + dados[i].id + ">" + dados[i].descricao + "</option>");
        }
    });
}

function CarregarLocais() {
    var listaLocaisDenunciantes = $("#ListaLocaisDenunciantes");
    var endereco = servidor.url + "Locais";

    var promisse = requisicao(endereco, 'GET');
    promisse.done(function (data) {
        $(listaLocaisDenunciantes).find("option").remove();
        for (var i = 0; i < data.length; i++) {
            if (i == 0) {
                listaLocaisDenunciantes.append("<option value=''>" + "--SELECIONE--" + "</option>");
            }
            listaLocaisDenunciantes.append("<option value=" + data[i].id + ">" + data[i].cidade + " - " + data[i].estado + "</option>");
        }
    })
}

var box = null;
function IncluirDenunciantesAvulso() {
    if (box == null) {
        box = bootbox.dialog({
            closeButton: false,
            size: 'large',
            message: htmlDenunciantes,
            show: false
        })
        CarregarTiposDenunciantes();
        CarregarLocais();
        box.modal('show');
    }
    else {
        box.modal('show');
        //setTimeout(function () {
        //    CarregarTiposDenunciantes();
        //    CarregarLocais()
        //}, 10000);

        //box.modal('show').CarregarTiposDenunciantes();
        //$('.form-Denunciantes').on("show", function () {
        //    console.log("Janela Criada");
        //    CarregarTiposDenunciantes();
        //})
        //setTimeout(function () {
        //    console.log("Fim delay")
        //    CarregarTiposDenunciantes();
        //}, 10000);

    }
}


function PopularComboDenunciantesAvulso() {
    var ListaDenunciantes = $("#ListaDenunciantes");
    var endereco = servidor.url + "Denunciantes";
    var promisse = requisicao(endereco, "GET");

    promisse.done(function (data) {
        repopularComboBox(data, ListaDenunciantes);
    });
}

function repopularComboBox(data, ListaDenunciantes) {
    ListaDenunciantes.find("option").remove();
    for (var i = 0; i < data.length; i++) {
        if (i == 0) {
            ListaDenunciantes.append("<option value=" + "" + ">" + "--SELECIONE--" + "</option>");
        }
        ListaDenunciantes.append("<option value=" + data[i].id + ">" + data[i].nome + "</option>");
    }
}

$(document).on('focus', '#ListaTiposDenunciantes', function () {
    CarregarTiposDenunciantes();
});

$(document).on('focus', '#ListaLocaisDenunciantes', function () {
    CarregarLocais();
});

function PopularComboLocaisAvulso() {
    var idLocal = $('#LocalEdicao').val();
    var ListaLocais = $(".ListaLocais");
    $(".ListaLocais").empty();
    var endereco = servidor.url + "Locais";
    var promisse = requisicao(endereco, 'GET');
    promisse.done(function (data) {
        repopularComboLocaisAvulso(data, ListaLocais);
        $('#LocalEdicao').val(idLocal)
    });
}

function repopularComboLocaisAvulso(data, ListaLocais) {
    for (var i = 0; i < data.length; i++) {
        if (i == 0) {
            ListaLocais.append("<option value=" + "" + ">" + "--SELECIONE--" + "</option>");
        }
        ListaLocais.append("<option value=" + data[i].id + ">" + data[i].cidade + " - " + data[i].estado + "</option>");
    }
}


function SalvarLocaisAvulso(form, event) {
    event.preventDefault();
    var enderecoAvulso = servidor.url + "Locais/";
    var objeto = montarObjeto(form);
    var promisse = requisicao(enderecoAvulso, 'POST', objeto);
    promisse.done(function () {
        successAlert("O CADASTRO FOI SALVO COM SUCESSO");
        bootbox.hideAll();
        $(".ListaLocais").empty();
        PopularComboLocaisAvulso();
    });
}

var ModalTipoDenunciante = null;
function IncluirTiposDenunciantes() {
    box.modal('hide');
    ModalTipoDenunciante = bootbox.dialog({
        backdrop: false,
        message: TiposDenunciantes
    });
}
var ModalSubLocais = null;
function IncluirSubLocais() {
    box.modal('hide');
    ModalSubLocais = bootbox.dialog({
        size: 'large',
        message: DenunciantesLocais
    });

}

function SalvarTiposDenunciantes(form, event) {
    event.preventDefault();
    var enderecoAvulso = servidor.url + "TiposDenunciantes/";
    var objeto = montarObjeto(form);
    var promisse = requisicao(enderecoAvulso, 'POST', objeto);
    promisse.done(function () {
        successAlert("O CADASTRO FOI SALVO COM SUCESSO");
        adicionarBorda('#form-SubTiposDenunciantes');
        ModalTipoDenunciante.modal('hide')
        IncluirDenunciantesAvulso();
    });
}

function SalvarSubLocais(form, event) {
    event.preventDefault();
    var enderecoAvulso = servidor.url + "Locais/";
    var objeto = montarObjeto(form);
    var promisse = requisicao(enderecoAvulso, 'POST', objeto);

    promisse.done(function () {
        successAlert("O CADASTRO FOI SALVO COM SUCESSO");
        PopularComboLocaisAvulso()
        ModalSubLocais.modal('hide');
        IncluirDenunciantesAvulso();
    });
}

function CarregarTiposDenunciados() {
    //$("#ListaTiposDenunciados option").not("[value='']").remove();
    var ListaDenunciados = $("#ListaTiposDenunciados");
    var endereco = servidor.url + "TiposDenunciados";
    var promisse = requisicao(endereco, 'GET');
    promisse.done(function (dados) {
        repopularTiposDenunciados(dados, ListaDenunciados)
    })
}
function repopularTiposDenunciados(dados, combobox) {
    $(combobox).find("option").remove();

    for (var i = 0; i < dados.length; i++) {
        if (i == 0) {
            $(combobox).append("<option value=''>" + "--SELECIONE--" + "</option>");
        }
        $(combobox).append('<option value="' + dados[i].id + '">' + dados[i].descricao + '</option>');
    }
}

function CarregarLocaisDenunciados() {
    var ListaLocaisDenunciados = $("#ListaLocaisDenunciados");
    var endereco = servidor.url + "Locais";
    var promisse = requisicao(endereco, 'GET');
    promisse.done(function (data) {
        $(ListaLocaisDenunciados).find("option").remove();
        for (var i = 0; i < data.length; i++) {
            if (i == 0) {
                ListaLocaisDenunciados.append("<option value=''>" + "--SELECIONE--" + "</option>");
            }
            ListaLocaisDenunciados.append("<option value=" + data[i].id + ">" + data[i].cidade + " - " + data[i].estado + "</option>");
        }
    });
}

function FecharFormularioAvulsoEmpresas() {
    $('#FormEmpresa').find(':input').val('');
    bootbox.hideAll();
}

function CarregarDenunciados() {
    var ListaDenunciados = $("#ListaDenunciados");
    var endereco = servidor.url + "Denunciados";
    var promisse = requisicao(endereco, 'GET');
    promisse.done(function (data) {
        for (var i = 0; i < data.length; i++) {
            if (i == 0) {
                ListaDenunciados.append("<option value=" + "" + ">" + "--SELECIONE--" + "</option>");
            }
            ListaDenunciados.append("<option value=" + data[i].id + ">" + data[i].nome + "</option>");
        }
    });
}

var filtroDashboard = null;
function incluirFiltro() {
    if (filtroDashboard == null) {
        filtroDashboard = bootbox.dialog({
            size: 'large',
            message: $('.Filtro-TotalOcorrencias').html()
        });
    }
    else {
        filtroDashboard.modal('show');
    }
}

function PopularTipoCategoriaAvulso() {
    var ListaCategorias = $("#ListaCategorias");
    var endereco = servidor.url + "CategoriasProdutos";
    var promisse = requisicao(endereco, 'GET');
    promisse.done(function (data) {
        for (var i = 0; i < data.length; i++) {
            if (i == 0) {
                ListaCategorias.append("<option value=" + "" + ">" + "--SELECIONE--" + "</option>");
            }
            ListaCategorias.append("<option value=" + data[i].id + ">" + data[i].descricao + "</option>");
        }
    });
}

function popularProduto() {
    var ListaProdutos = $("#ListaProdutos");
    var endereco = servidor.url + "Produtos";
    var promisse = requisicao(endereco, 'GET');
    promisse.done(function (data) {
        for (var i = 0; i < data.length; i++) {
            if (i == 0) {
                ListaProdutos.append("<option value=" + "" + ">" + "--SELECIONE--" + "</option>");
            }
            ListaProdutos.append("<option value=" + data[i].id + ">" + data[i].nome + "</option>");
        }
    });
}

function CarregarClassesProdutos() {
    var ListaClasses = $("#ListaClassesProdutos");
    var endereco = servidor.url + "ClassesProdutos";
    var promisse = requisicao(endereco, 'GET');
    promisse.done(function (data) {
        $(ListaClasses).find("option").remove();
        for (var i = 0; i < data.length; i++) {
            if (i == 0) {
                ListaClasses.append("<option value=''>" + "--SELECIONE--" + "</option>");
            }
            ListaClasses.append("<option value=" + data[i].id + ">" + data[i].descricao + "</option>");
        }
    });
}