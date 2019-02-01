
$(window).resize(function () {
    google.charts.load('current', { 'packages': ['corechart'], 'language': 'pt' });
    google.charts.setOnLoadCallback(function () {
        PopularComboDenunciantesAvulso();
        CarregarDenunciados();
        PopularEmpresaAvulsa();
        PopularTipoCategoriaAvulso();
        popularProduto();
        CarregarClassesProdutos();
        gerarGrafico();
    });
});

function abrirImpressao() {
    bootbox.dialog({
        size: 'large',
        message: observacao
    });
}

function imprimirDamage() {
    var observacao = $('#observacao').val()
    imprimir('imprimir', observacao)
}

var DadosFiltro = null;
function gerarGrafico(b, form, event) {
    if (event != null) {
        event.preventDefault();
    }
    if (form != null) {
        DadosFiltro = form;
    }
    var objeto = montarObjeto(DadosFiltro);
    var url = servidor.url + "Dashboards/DashboardTipoOcorrencia";
    var promisse = requisicao(url, 'POST', objeto);
    promisse.done(function (dados) {
        console.log(dados)
        bootbox.hideAll();
        conteudoTipoOcorrencia(dados);
    });
}