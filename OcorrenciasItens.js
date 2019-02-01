var urlPrincipalItens = servidor.url + "OcorrenciasItens/";
var idOcorrencia = sessionStorage.getItem('idOcorrencia');

function fecharFormularioItens() {
    LimparDadosItens();
    $('#formularioItens').addClass('d-none');
}

function LimparDadosItens() {
    $("[name='lote']").val("");
    $("[name='idDenunciado']").val("");
    $("[name='idProduto']").val("");
    $("[name='litros']").val("");
    $("[name='damage']").val("");
    $("[name='id']").val("");
    $("#dataOcorrenciaItem").val("");
    $("[name='idLocalRoubo']").val("");
    $("[name='modoOperacao']").val("");
    $("[name='descricao']").val("");
    $("[name='idEmpresaProduto']").val("");
    $("[name='idCategoria']").val("");
    $("[name='idCategoria']").val("");
    $("[name='paisOrigem']").val("");
    $("[name='idLogradouro']").val("");
    $("[name='complementoLogradouro']").val("");
    $("#ListaEmpresas").val("");
    $('#formularioItens').find(".obrigatorios").css("border-color", "red");
    $("#gridAnexos tbody tr").remove();
}

$(document).on('keyup', '.telefone', function () {
    formataTelefone(this)
});

function SalvarLogradouro(form, event) {
    var enderecoAvulso = servidor.url + "Logradouros/";
    event.preventDefault();
    var objeto = montarObjeto(form);
    var promisse = requisicao(enderecoAvulso, 'POST', objeto);
    promisse.done(function () {
        successAlert("O CADASTRO FOI SALVO COM SUCESSO");
        PopularComboLogradouro();
        bootbox.hideAll();
    });
}

function enviarEmail() {
    var id = $("#idOcorrencia").val();
    var endereco = servidor.url + "Ocorrencias/" + id + "/EnviarEmail";
    var promisse = requisicao(endereco, 'PUT');
    return false;
}

function PopularComboItens() {
    var Denunciados = $("#ListaDenunciados");
    var endereco = servidor.url + "Denunciados";

    var promisse = requisicao(endereco, 'GET');

    promisse.done(function (data) {
        for (var f = 0; f < data.length; f++) {
            if (f == 0) {
                Denunciados.append("<option value=" + "" + ">" + "--SELECIONE--" + "</option>");
            }
            Denunciados.append("<option value=" + data[f].id + ">" + data[f].nome + "</option>");
        }
    });
}
function salvarRegistroItens(form, event) {
    event.preventDefault();

    form = $(form);

    var valores = form.serializeArray();
    var objeto = {};
    $.each(valores, function (i, item) {
        if (item.name == "damage") {
            var valor = item.value.replace(/\./g, '');
            valor = valor.replace(',', '.');
            valor = valor.replace('R$ ', '');
            valor = valor.trim();
            objeto[item.name] = valor;
        }
        else if (item.name == "data") {
            var dia = item.value.split("/")[0];
            var mes = item.value.split("/")[1];
            var ano = item.value.split("/")[2];
            var hora = ano.split(" ");
            var data = hora[0] + '-' + ("0" + mes).slice(-2) + '-' + ("0" + dia).slice(-2) + " " + hora[1];
            objeto[item.name] = data;
        }
        else {
            objeto[item.name] = item.value;
        }
    });

    if (!Date.parse(objeto.data)) {
        bootbox.alert("CAMPO INVÁLIDO");
        $('#dataOcorrenciaItem').css("border-color", "#e60000");
        return false;
    }

    objeto["anexos"] = listarAnexosID(objeto.id);

    var metodo = "POST";
    var endereco = urlPrincipalItens;

    if (objeto.id == "") {
        objeto.id = 0;
    }

    if (objeto.id > 0) {
        metodo = "PUT";
        endereco = endereco + objeto.id
    }

    var idOcorrencia = sessionStorage.getItem('idOcorrencia');
    var TipoOcorrencia = sessionStorage.getItem('TipoOcorrencia');
    objeto.idOcorrencia = idOcorrencia;
    objeto.TipoOcorrencia = TipoOcorrencia;

    var promisse = requisicao(endereco, metodo, objeto);

    promisse.done(function () {
        atualizarGrid();
        PreencherOcorrencia();
        LimparParcial();
        $("#gridAnexos tbody tr").remove();
        LimparAnexos();
        successAlert("O CADASTRO FOI SALVO COM SUCESSO");
    })

    promisse.fail(function (data) {
        console.log(data);
    });
}

function LimparParcial() {
    $("[name='lote']").val("");
    //$("[name='idProduto']").val("").css("border-color", "red");
    $("[name='litros']").val("");
    $("[name='damage']").val("");
    $("[name='id']").val("");
    //$("[name='idEmpresaProduto']").val("").css("border-color", "red");
    //$("[name='idCategoria']").val("").css("border-color", "red");
    //limparGridAnexos();
}

function ExcluirItem(id) {
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
                var endereco = urlPrincipalItens + id;
                var promisse = requisicao(endereco, 'DELETE');

                promisse.done(function () {
                    atualizarGrid();
                    PreencherOcorrencia();
                });
            }
        }
    });
}

function EditarOcorrenciasItens(id, event) {
    LimparAnexos();
    event.preventDefault();
    abrirFormularioItens()
    removerBorda('#formularioItens');
    var promisse = requisicao(urlPrincipalItens + id, 'GET');
    promisse.done(function (data) {
        var idProduto = data.idProduto;
        $("#ListaProdutos").empty();
        var ListaProdutos = $("#ListaProdutos");
        var promisse = requisicao(servidor.url + "Produtos", 'GET');
        promisse.done(function (dados) {
            ListaProdutos.append("<option value=" + "" + ">" + "--SELECIONE--" + "</option>");
            for (var i = 0; i < dados.length; i++) {
                if (dados[i].id == idProduto) {
                    var idEmpresa = dados[i].idEmpresaProduto;
                    $('#ListaEmpresas').val(idEmpresa);
                }
            }
            for (var j = 0; j < dados.length; j++) {
                if (dados[j].idEmpresaProduto == idEmpresa) {
                    ListaProdutos.append("<option value=" + dados[j].id + ">" + dados[j].nome + "</option>");
                }
            }
            $("#ListaProdutos").val(idProduto);
        });
        if (data.damage != null) {
            data.damage = data.damage.toLocaleString("pt-BR", { style: "currency", currency: "BRL" });
            data.damage = data.damage.replace('R$', '');
        }

        data.data = formataDataHora(data.data);

        var formulario = $('#formularioItens').find("form");
        formulario.data("item", data);

        $.each(data, function (prop, valor) {
            formulario.find("[name=" + prop + "]").val(valor);
        });

        for (var i = 0; i < data.anexos.length; i++) {
            var nomeArquivo = data.anexos[i].nome;
            var index = listaAnexos.findIndex(x => x.nome == nomeArquivo);

            if (index >= 0) {
                excluirAnexo(index);
            }

            listaAnexos.push(data.anexos[i]);
           
        }
    });
}


function abrirFormularioItens() {
    var form = $('#formularioItens').find("form");
    LimparDadosItens();
    $('#valor').money('[###.]###,##');
    $('#formularioItens').removeClass('d-none');

    return form;
}
function incluirFormularioItens(event) {
    event.preventDefault();
    abrirFormularioItens();
}

function IncluirLogradouro() {
    bootbox.dialog({
        message: Logradouro
    });
}

function PreencherOcorrencia() {
    var urlOcorrencia = servidor.url + "Ocorrencias/";

    var promisse = requisicao(urlOcorrencia + sessionStorage.getItem('idOcorrencia'), 'GET');

    promisse.done(function (data) {
        var formulario = $('#formularioOcorrencias').find("form");
        $('#CampoResponsavel').addClass('d-none');
        formulario.data("item", data);

        if (data.data != null) {
            data.data = (data.data).replace(/(\d*)-(\d*)-(\d*).*/, '$1-$2-$3');
        }

        if (data.fechamento != null) {
            $('#DataFechamento').val((data.fechamento).replace(/(\d*)-(\d*)-(\d*).*/, '$3/$2/$1'));
        }
        $.each(data, function (prop, valor) {
            formulario.find("[name=" + prop + "]").val(valor);
        });
        if (data.status == 1) {
            $('#CampoResponsavel').removeClass('d-none');
        }
        popularDamage(data);
    });
}


function popularDamage(data) {
    var dadosOcorrencia = data;
    var damageTotal = 0;
    var promisse = requisicao(urlPrincipalItens, 'GET');
    promisse.done(function (data) {
        for (var i = 0; i < data.length; i++) {
            if (data[i].idOcorrencia == dadosOcorrencia.id) {
                damageTotal = damageTotal + data[i].damage;
            }
        }
        $('#DamageOcorrencia').val(damageTotal.toLocaleString("pt-BR", { style: "currency", currency: "BRL" }));
    });
}

function PopularComboLogradouro() {
    var listaLogradouros = $('#ListaLogradouros');
    var endereco = servidor.url + "Logradouros";
    var promisse = requisicao(endereco, "GET");

    promisse.done(function (data) {
        listaLogradouros.find("option").remove();
        for (var i = 0; i < data.length; i++) {
            if (i == 0)
                listaLogradouros.append("<option value=" + "" + ">" + "--SELECIONE--" + "</option>");
            listaLogradouros.append("<option value=" + data[i].id + ">" + data[i].nome + "</option>");
        }
    })
}

function PopularComboProdutos() {
    var ListaProdutos = $("#ListaProdutos");
    ListaProdutos.append("<option value=" + "" + ">" + "--SELECIONE--" + "</option>");
    $('#ListaEmpresas').change(function () {
        ListaProdutos.empty();
        ListaProdutos.append("<option value=" + "" + ">" + "--SELECIONE--" + "</option>");
        var idEmpresa = $(this).val();
        var endereco = servidor.url + "Produtos";
        var promisse = requisicao(endereco, 'GET');
        promisse.done(function (data) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].idEmpresaProduto == idEmpresa) {
                    ListaProdutos.append("<option value=" + data[i].id + ">" + data[i].nome + "</option>");
                }
            }
        });
    });
}

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
    //$('#dataOcorrenciaItem').mask("99/99/9999 99:99");
    //document.getElementById("ListaEmpresas").onkeyup = function() { alert("ok")}
    var TipoOcorrencia = sessionStorage.getItem('TipoOcorrencia');
    if (TipoOcorrencia != 0) 
        $('#FormOcorrenciasFalsificacao').addClass('d-none');

    if (TipoOcorrencia != 1) 
        $('#FormOcorrenciasRouboCarga').addClass('d-none');

    if (TipoOcorrencia != 2 && TipoOcorrencia != 3) 
        $('#FormOcorrenciasContrabando').addClass('d-none');


    PopularComboProdutos();
    PopularComboItens();
    PopularComboOcorrencia();
    PopularTipoCategoriaAvulso();
    PopularEmpresaAvulsa();
    PopularComboLogradouro();
    grid = $('#gridOcorrenciasItens').DataTable({
        data: [],
        columns: [
            {
                data: "id",
                width: "20px",
                render: function (data, type) {
                    if (type === 'display') {

                        var editar = '<a href="#" title="Editar" onclick="EditarOcorrenciasItens(' + data + ",event" + ');" class="fa fa-pencil">&nbsp;</a>';
                        var excluir = '<a href="#" title="Excluir" onclick="ExcluirItem(' + data + ');" class="fa fa-remove">&nbsp;</a>';

                        data = editar + '&nbsp;' + excluir;
                    }

                    return data;
                }
            },
            {
                data: null,
                title: "DENUNCIADO",
                render: function (data) {
                    var valor = data.denunciado != null ? data.denunciado : "INDEFINIDO" ;
                    data = valor
                    return data
                }
            },
            {
                data: null,
                title: "PRODUTOS",
                render: function (data) {
                    var valor = data.produto != null ? data.produto : "INDEFINIDO";
                    data = valor
                    return data
                }
            },
            { title: "LITROS", data: "litros", "className": "text-center", width: "20px", },
            {
                data: null,
                title: "PERDA",
                width: "20px",
                "className": "text-right",
                render: function (data) {
                    var valor = data.damage;
                    data = valor.toLocaleString("pt-BR", { style: "currency", currency: "BRL" });
                    return data
                }
            },
        ],
        language: {
            "lengthMenu": "_MENU_ REGISTROS POR PAGINA",
            "zeroRecords": "NENHUM REGISTRO ENCONTRADO",
            "info": "EXIBINDO _PAGE_ DE _PAGES_",
            "infoEmpty": "Nenhum registro encontrado",
            "infoFiltered": "(Filtrado de _MAX_ registros)",
            "search": "Pesquisar:",
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
    CarregarPaises();
});

var ItensCombo = [];

function PopularComboOcorrencia() {
    var endereco = servidor.url + "Locais";

    var promisse = requisicao(endereco, 'GET');
    promisse.done(function (data) {
        ItensCombo.push(data);
        popularListaDenunciantes();
    });
}

function popularListaDenunciantes() {
    var ListaLocais = $(".ListaLocais");
    var ListaDenunciantes = $("#ListaDenunciantes");
    var endereco = servidor.url + "Denunciantes";
    var promisse = requisicao(endereco, 'GET');
    promisse.done(function (data) {
        ItensCombo.push(data);
        repopularComboBoxOcorrencia(ItensCombo, ListaLocais, ListaDenunciantes);
        PreencherOcorrencia();
    });
}

function repopularComboBoxOcorrencia(ItensCombo, ListaLocais, ListaDenunciantes) {

    for (var i = 0; i < ItensCombo.length; i++) {
        if (i == 0) {
            for (var j = 0; j < ItensCombo[0].length; j++) {
                if (j == 0) {
                    ListaLocais.append("<option value=" + "" + ">" + "--SELECIONE--" + "</option>");
                }
                ListaLocais.append("<option value=" + ItensCombo[i][j].id + ">" + ItensCombo[i][j].cidade + " - " + ItensCombo[i][j].estado + "</option>");
            }
        }
        if (i == 1) {
            for (var f = 0; f < ItensCombo[1].length; f++) {
                if (f == 0) {
                    ListaDenunciantes.append("<option value=" + "" + ">" + "--SELECIONE--" + "</option>");
                }
                ListaDenunciantes.append("<option value=" + ItensCombo[i][f].id + ">" + ItensCombo[i][f].nome + "</option>");
            }
        }
    }
}

function atualizarGrid() {
    var promisse = requisicao(urlPrincipalItens, 'GET');
    promisse.done(function (data) {
        var Lista = [];
        for (var i = 0; i < data.length; i++) {
            if (data[i].idOcorrencia == idOcorrencia) {
                Lista.push(data[i]);
            }
        }
        grid.clear();
        grid.rows.add(Lista).draw();
    });
}


function SalvarOcorrencia(event) {
    event.preventDefault();
    var formularioOcorrencia = $('#formularioOcorrencias').find("form");

    var objeto = montarObjeto();

    var valores = formularioOcorrencia.serializeArray();
    var objeto = {};
    $.each(valores, function (i, item) {
        objeto[item.name] = item.value;
    });

    objeto.id = sessionStorage.getItem('idOcorrencia');

    var endereco = servidor.url + "Ocorrencias/" + sessionStorage.getItem('idOcorrencia');;

    var promisse = requisicao(endereco, 'PUT', objeto);

    promisse.done(function () {
        window.location.href = "/Ocorrencias";
    });
    promisse.fail(function (data) {
        console.log(data);
    });
}

function FecharFormularioAvulsoOcorrencias() {
    $('#FormularioDenunciantes').find(":input").val("");
    bootbox.hideAll();
}

function FecharSubFormularioAvulsoDenunciados() {
    bootbox.hideAll();
    IncluirDenunciadosAvulso();
}
function FecharSubFormularioAvulsoDenunciantes() {
    bootbox.hideAll();
    IncluirDenunciantesAvulso();
}

function FecharSubFormularioAvulsoProdutos() {
    $('#FormProdutoEmpresa').find(':input').val('')
    bootbox.hideAll();
    IncluirProdutoAvulso();
}

function IncluirAvulso() {
    bootbox.dialog({
        size: 'large',
        message: $('.form-Locais').html()
    });
}

function SalvarRegistroAvulso(form, b, event) {

    var enderecoAvulso = servidor.url + "Locais/"

    event.preventDefault();

    var objeto = montarObjeto(form);
    var promisse = requisicao(enderecoAvulso, 'POST', objeto);

    promisse.done(function () {
        successAlert("O CADASTRO FOI SALVO COM SUCESSO");
        bootbox.hideAll();
        $(".ListaLocais").empty();
        $("#ListaDenunciantes").empty();
        PopularComboOcorrencia()
    })
}

function FecharFormularioAvulso() {
    $('#FormDenunciados').find(':input').val('');
    bootbox.hideAll();
}

function IncluirSubDenunciantesAvulso(a) {
    //bootbox.hideAll();
    BootboxProduto.modal('hide');
    if (a == 0) {
        bootbox.dialog({
            size: 'large',
            message: $('.form-SubDenunciantesLocais').html()
        });
    }
    if (a == 1) {
        bootbox.dialog({
            message: $('.form-SubTiposDenunciantes').html()
        });
    }
}

function SalvarSubDenunciantesAvulso(form, a, event) {
    var enderecoAvulso = "";

    if (a == 1) {
        enderecoAvulso = servidor.url + "TiposDenunciantes/";
    }
    if (a == 0) {
        enderecoAvulso = servidor.url + "Locais/";
    }
    event.preventDefault();

    var objeto = montarObjeto(form);

    var promisse = requisicao(enderecoAvulso, 'POST', objeto);

    promisse.done(function () {
        successAlert("O CADASTRO FOI SALVO COM SUCESSO");
        bootbox.hideAll();
        IncluirDenunciantesAvulso();
    });
}
var boxDenunciados = null;
function IncluirDenunciadosAvulso() {
    if (boxDenunciados == null) {
        boxDenunciados = bootbox.dialog({
            closeButton: false,
            size: 'large',
            message: Denunciado,
            show: false
        });
        CarregarTiposDenunciados();
        CarregarLocaisDenunciados();
        boxDenunciados.modal('show');
    }
    else {
        boxDenunciados.modal('show');
    }
}


$(document).on('focus', '#ListaTiposDenunciados', function () {
    CarregarTiposDenunciados()
});

$(document).on('focus', '#ListaLocaisDenunciados', function () {
    CarregarLocaisDenunciados();
});

function SalvarDenunciadosAvulso(form, event) {

    var enderecoAvulso = servidor.url + "Denunciados/";
    event.preventDefault();

    var objeto = montarObjeto(form);

    var promisse = requisicao(enderecoAvulso, 'POST', objeto);

    promisse.done(function () {
        successAlert("O CADASTRO FOI SALVO COM SUCESSO");
        $('#FormDenunciados').find(':input').val('');
        bootbox.hideAll();
        adicionarBorda('#FormDenunciados');
        $("#ListaDenunciados").empty();
        PopularComboItens();
    });
}

function IncluirTiposDenunciados() {
    boxDenunciados.modal('hide');
    bootbox.dialog({
        message: TipoDenunciado
    });
}
function IncluirSubLocaisDenunciados() {
    boxDenunciados.modal('hide');
    bootbox.dialog({
        size: 'large',
        message: LocaisDenunciado
    });

}

function SalvarTiposDenunciados(form, event) {
    var enderecoAvulso = servidor.url + "TiposDenunciados/";

    event.preventDefault();

    var objeto = montarObjeto(form);

    var promisse = requisicao(enderecoAvulso, 'POST', objeto);
    promisse.done(function () {
        successAlert("O CADASTRO FOI SALVO COM SUCESSO");
        popularTiposDenunciados();
        bootbox.hideAll();
        IncluirDenunciadosAvulso();
    });
}

function popularTiposDenunciados() {

    $("#ListaTiposDenunciados").empty();
    var ListaDenunciados = $("#ListaTiposDenunciados");
    var endereco = servidor.url + "TiposDenunciados";
    var promisse = requisicao(endereco, 'GET');
    promisse.done(function (dados) {
        for (var i = 0; i < dados.length; i++) {
            if (i == 0) {
                ListaDenunciados.append("<option value=" + "" + ">" + "--SELECIONE--" + "</option>");
            }
            ListaDenunciados.append("<option value=" + dados[i].id + ">" + dados[i].descricao + "</option>");
        }
    });
}

function SalvarSubLocaisDenunciados(form, event) {
    var enderecoAvulso = servidor.url + "Locais/";
    event.preventDefault();
    var objeto = montarObjeto(form);
    var promisse = requisicao(enderecoAvulso, 'POST', objeto);
    promisse.done(function () {
        successAlert("O CADASTRO FOI SALVO COM SUCESSO");
        popularLocaisDenunciados();
    });
}

function popularLocaisDenunciados() {
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
        bootbox.hideAll();
        IncluirDenunciadosAvulso();
    });
}

var BootboxProduto = null
function IncluirProdutoAvulso() {
    if (BootboxProduto == null) {
        BootboxProduto = bootbox.dialog({
            closeButton: false,
            size: 'large',
            message: htmlProduto,
            show:false
        })
        CarregarEmpresas();
        CarregarClassesProdutos();
        BootboxProduto.modal('show');
    }
    else {
        BootboxProduto.modal('show');
    }
}

$(document).on('focus', '.ListaEmpresas', function () {
    CarregarEmpresas()
});

$(document).on('focus', '#ListaClassesProdutos', function () {
    CarregarClassesProdutos()
});

function CarregarEmpresas() {
    var ListaEmpresas = $(".ListaEmpresas");
    var endereco = servidor.url + "Empresas";
    var promisse = requisicao(endereco, 'GET');
    promisse.done(function (data) {
        $(ListaEmpresas).find("option").remove();
        for (var i = 0; i < data.length; i++) {
            if (i == 0) {
                ListaEmpresas.append("<option value=''>" + "--SELECIONE--" + "</option>");
            }
            ListaEmpresas.append("<option value=" + data[i].id + ">" + data[i].razaoSocial + "</option>");
        }
    });
}

function SalvarProdutosAvulso(form, event) {

    var enderecoAvulso = servidor.url + "Produtos/";
    event.preventDefault();

    var objeto = montarObjeto(form);
    var promisse = requisicao(enderecoAvulso, 'POST', objeto);
    promisse.done(function () {
        successAlert("O CADASTRO FOI SALVO COM SUCESSO");
        $('#FormProdutos').find(':input').val('');
        adicionarBorda('#FormProdutos');
        bootbox.hideAll();
    });
}

var ModalProdutoEmpresa = null;
function IncluirProdutoEmpresa() {
    BootboxProduto.modal('hide');
    if (ModalProdutoEmpresa == null) {
        ModalProdutoEmpresa = bootbox.dialog({
            closeButton: false,
            size: 'large',
            message: ProdutoEmpresa,
            show: false
        });
        CarregarRamosAtividadeEmpresa();
        ModalProdutoEmpresa.modal('show');
    }
    else {
        ModalProdutoEmpresa.modal('show');
    }
    formatarCep();
}

$(document).on('focus', '.ListaRamosAtividade', function () {
    CarregarRamosAtividade();
});

function IncluirRamosAtividadeProdutoEmpresa() {
    ModalProdutoEmpresa.modal('hide');
    bootbox.dialog({
        backdrop: false,
        message: RamosAtividadeProdutoEmpresa
    });
}
function salvarRamosAtividadeProdutoEmpresa(form, event) {
    var enderecoAvulso = servidor.url + "RamosAtividade/";
    event.preventDefault();

    var objeto = montarObjeto(form);
    var promisse = requisicao(enderecoAvulso, 'POST', objeto);
    promisse.done(function () {
        successAlert("O CADASTRO FOI SALVO COM SUCESSO");
        bootbox.hideAll();
        ModalProdutoEmpresa.modal('show');
    });
}
function FecharRamosAtividadeProdutoEmpresa() {
    bootbox.hideAll();
    ModalProdutoEmpresa.modal('show')
}


function SalvarProdutoEmpresa(form, event) {
    event.preventDefault();
    var enderecoAvulso = servidor.url + "Empresas/";
    var objeto = montarObjeto(form);
    var promisse = requisicao(enderecoAvulso, 'POST', objeto);
    promisse.done(function () {
        successAlert("O CADASTRO FOI SALVO COM SUCESSO");
        $('#FormProdutoEmpresa').find(':input').val('');
        bootbox.hideAll();
        adicionarBorda('#FormProdutoEmpresa');
        BootboxProduto.modal('show')
    });
}

function IncluirProdutoClasse() {
    BootboxProduto.modal('hide');
    bootbox.dialog({
        message: ProdutoClasse
    });
}
function SalvarProdutoClasse(form, event) {
    var enderecoAvulso = servidor.url + "ClassesProdutos/";
    event.preventDefault();
    var objeto = montarObjeto(form);
    var promisse = requisicao(enderecoAvulso, 'POST', objeto);
    promisse.done(function () {
        successAlert("O CADASTRO FOI SALVO COM SUCESSO");
        bootbox.hideAll();
        BootboxProduto.modal('show');
    });
}

function IncluirTipoCategoriaAvulso() {
    bootbox.dialog({
        message: $('.form-SubCategoriaProduto').html()
    });
}

function SalvarTipoCategoriaAvulso(form, event) {
    var enderecoAvulso = servidor.url + "CategoriasProdutos/";
    event.preventDefault();
    var objeto = montarObjeto(form);
    var promisse = requisicao(enderecoAvulso, 'POST', objeto);
    promisse.done(function () {
        bootbox.hideAll();
        $("#ListaCategorias").empty();
        successAlert("O CADASTRO FOI SALVO COM SUCESSO");
        PopularTipoCategoriaAvulso();
    });
}

function FacharAvulso() {
    bootbox.hideAll();
}

function IncluirEmpresasRamosAtividade() {
    bootbox.hideAll();
    bootbox.dialog({
        message: $(".form-EmpresasRamosAtividade").html()
    });
}


function FecharEmpresasRamosAtividade() {
    bootbox.hideAll();
    IncluirEmpresaAvulsa();
}

//function salvarEmpresasRamosAtividade(form, event) {
//    var EnderecoAvulso = servidor.url + "RamosAtividade/";

//    event.preventDefault();

//    form = $(form);

//    var valores = form.serializeArray();

//    var objeto = {};
//    $.each(valores, function (i, item) {
//        objeto[item.name] = item.value;
//    });

//    $.ajax({
//        url: EnderecoAvulso,
//        type: "POST",
//        data: JSON.stringify(objeto),
//        contentType: 'application/json'
//    })
//        .done(function () {
//            bootbox.hideAll();
//            IncluirSubProdutoAvulso(0)
//        });
//}

function CarregarPaises() {
    $.ajax({
        type: 'GET',
        url: 'http://api.londrinaweb.com.br/PUC/Paisesv2/0/1000',
        contentType: "application/json; charset=utf-8",
        dataType: "jsonp",
        async: false
    }).done(function (data) {
        var ListaPaises = $('#ListaPaises');
        for (var i = 0; i < data.length; i++) {
            if (i == 0) {
                ListaPaises.append("<option value=" + "" + ">" + "--SELECIONE--" + "</option>");
            }
            ListaPaises.append("<option value=" + data[i].Pais + ">" + data[i].Pais + "</option>");
        }
    });
}

function SalvarDenuncianteAvulso(form, event) {
    var enderecoAvulso = servidor.url + "Denunciantes/";
    event.preventDefault();
    var objeto = montarObjeto(form);
    var promisse = requisicao(enderecoAvulso, 'POST', objeto);
    promisse.done(function () {
        successAlert("O CADASTRO FOI SALVO COM SUCESSO");
        $('#FormularioDenunciantes').find(":input").val("");
        PopularComboDenunciantesAvulso();
        bootbox.hideAll();
    });
}

function FecharFormularioProdutos() {
    $('#FormProdutos').find(':input').val('');
    bootbox.hideAll();
}