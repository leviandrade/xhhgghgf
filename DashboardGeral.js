$(window).resize(function () {
    var b = false;
    google.charts.load('current', { 'packages': ['corechart'], 'language': 'pt' });
    google.charts.setOnLoadCallback(function () {
        PopularComboDenunciantesAvulso();
        CarregarDenunciados();
        PopularEmpresaAvulsa();
        PopularTipoCategoriaAvulso();
        popularProduto();
        CarregarClassesProdutos();
        gerarGrafico(b)
    });
})

function abrirImpressao() {

    $("#imprimir").removeClass('d-none');
    //gerarGrafico();
    AtualizarDashboards(dadosDashboard);
    bootbox.dialog({
        size: 'large',
        message: observacaoGeral,
        onEscape: function () {
            $("#imprimir").addClass('d-none');
        }
    });
}

function imprimirGeral(form, event) {
    event.preventDefault();
    var objeto = montarObjeto(form);
    var elementos = $('#imprimir').find("label");

    for (var i = 0; i < elementos.length; i++) {
        $("#" + elementos[i].id).empty();
        $.each(objeto, function (prop, valor) {
            if (elementos[i].id == prop && valor.length > 0) {
                elementos[i].append("Observação: " + valor)
            }
        })
    }
    var observacao = $('#observacao').val()
    imprimir('imprimir', observacao)
}

var indice = 0;
var dadosDashboard = null;
function gerarGrafico(b, form, event) {
    if (b)
        indice++

    if (indice == 3) 
        indice = 0;

    if (event != null) {
        event.preventDefault();
    }
    if (form != null) {
        DadosFiltro = form;
    }
    bootbox.hideAll();
    var objeto = montarObjeto(DadosFiltro);
    var url = servidor.url + "Dashboards/DashboardGeral";
    var promisse = requisicao(url, 'POST', objeto);
    promisse.done(function (dados) {
        dadosDashboard = dados;
        conteudoDamage(dados[0]);
        conteudoRegistroProduto(dados[1]);
        conteudoLitrosProduto(dados[2]);
        conteudoOcorreciaPorUF(dados[3]);
        conteudoTotalOcorrencias(dados[4]);
        conteudoTipoOcorrencia(dados[5]);
    })
}

function AtualizarDashboards(dados) {
    conteudoDamage(dados[0]);
    conteudoRegistroProduto(dados[1]);
    conteudoLitrosProduto(dados[2]);
    conteudoOcorreciaPorUF(dados[3]);
    conteudoTotalOcorrencias(dados[4]);
    conteudoTipoOcorrencia(dados[5]);
}