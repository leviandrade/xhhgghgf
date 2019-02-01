var DadosFiltro = null;

var indice = 0;
var a = 0;
function conteudoDamage(dados) {
    var data = new google.visualization.DataTable();
    data.addColumn('string', 'ANO');
    data.addColumn('number', 'TOTAL');
    data.addColumn({ type: 'string', role: "style" });
    data.addRows(dados);

    var view = new google.visualization.DataView(data);
    view.setColumns([0, 1,
        {
            calc: "stringify",
            sourceColumn: 1,
            type: "string",
            role: "annotation",
        }, 2]);

    var options = {
        title: "PERDA",
        titleTextStyle: {
            color: 'black',
            fontSize: 21,
        },
        backgroundColor: {
            strokeWidth: 1,
            stroke: "#FFDEAD",
            fill: '#FFFFF0'
        },
        hAxis: { format: '#. ###%' },
        legend: { position: "none" },
        animation: {
            duration: 500,
            easing: 'out',
            startup: true
        },
    };
    var chart = new google.visualization.ColumnChart(document.getElementById("DashboardDamage"));
    chart.draw(view, options);
    var impressao = document.getElementById("DashboardDamageImpressao");
    if (impressao != null) {
        var chart = new google.visualization.ColumnChart(impressao);
        chart.draw(view, options);
    }
}

function conteudoOcorreciaPorUF(dados,b) {
    var data = new google.visualization.arrayToDataTable(dados);

    if (b) {
        indice++
    }

    var a = indice;

    if (indice == 3) {
        indice = 0;
        a = 0;
    }

    if (a == 1) {
        var options = {
            title: "OCORRENCIAS POR UF",
            titleTextStyle: {
                color: 'black',
                fontSize: 22,
            },
            pieHole: 0.4,
            backgroundColor: {
                strokeWidth: 1,
                stroke: "#FFDEAD",
                fill: '#FFFFF0'
            },
            animation: {
                duration: 1000,
                easing: 'out',
                startup: true
            }
        };
    }
    else if (a == 2) {
        var options = {
            title: "OCORRENCIAS POR UF",
            titleTextStyle: {
                color: 'black',
                fontSize: 21,
            },
            is3D: true,
            backgroundColor: {
                strokeWidth: 1,
                stroke: "#FFDEAD",
                fill: '#FFFFF0'
            },
            animation: {
                duration: 1000,
                easing: 'out',
                startup: true
            }
        };
    }
    else {
        var options = {
            title: "OCORRENCIAS POR UF",
            titleTextStyle: {
                color: 'black',
                fontSize: 21,
            },
            backgroundColor: {
                strokeWidth: 1,
                stroke: "#FFDEAD",
                fill: '#FFFFF0'
            },
            animation: {
                duration: 1000,
                easing: 'out',
                startup: true
            }
        };
    }
    var chart = new google.visualization.PieChart(document.getElementById("dashboardOcorrenciasUF"));
    chart.draw(data, options);

    var impressao = document.getElementById("dashboardOcorrenciasUFImpressao");
    if (impressao != null) {
        var chart = new google.visualization.PieChart(impressao);
        chart.draw(data, options);
    }
}

function conteudoLitrosProduto(dados,b) {
    var data = new google.visualization.arrayToDataTable(dados);
    if (b) {
        indice++
    }
    var a = indice;
    if (indice == 3) {
        indice = 0;
        a = 0;
    }
    if (a == 1) {

        var options = {
            title: "QTD. LITROS POR PRODUTO",
            titleTextStyle: {
                color: 'black',
                fontSize: 21,
            },
            backgroundColor: {
                strokeWidth: 1,
                stroke: "#FFDEAD",
                fill: '#FFFFF0'
            },
            pieHole: 0.4,
            animation: {
                duration: 1000,
                startup: true
            },
        };
    }
    else if (a == 2) {
        var options = {
            title: "QTD. LITROS POR PRODUTO",
            titleTextStyle: {
                color: 'black',
                fontSize: 21,
            },
            backgroundColor: {
                strokeWidth: 1,
                stroke: "#FFDEAD",
                fill: '#FFFFF0'
            },
            is3D: true,
            animation: {
                duration: 1000,
                startup: true
            }
        };
    }
    else {
        var options = {
            title: "QTD. LITROS POR PRODUTO",
            titleTextStyle: {
                color: 'black',
                fontSize: 21,
            },
            backgroundColor: {
                strokeWidth: 1,
                stroke: "#FFDEAD",
                fill: '#FFFFF0'
            },
            animation: {
                duration: 1000,
                startup: true
            }
        };
    }
    var chart = new google.visualization.PieChart(document.getElementById("DashboardLitrosProduto"));
    chart.draw(data, options);

    var impressao = document.getElementById("DashboardLitrosProdutoImpressao");

    if (impressao != null) {
        var chart = new google.visualization.PieChart(impressao);
        chart.draw(data, options);
    }
}

function conteudoTotalOcorrencias(dados) {
    var data = new google.visualization.DataTable();

    data.addColumn('string', 'ANO');
    data.addColumn('number', 'TOTAL');
    data.addColumn({ type: 'string', role: "style" });
    data.addRows(dados)

    var view = new google.visualization.DataView(data);
    view.setColumns([0, 1,
        {
            calc: "stringify",
            sourceColumn: 1,
            type: "string",
            role: "annotation"
        }, 2]);

    var options = {
        title: "QUANTIDADE DE OCORRENCIAS",
        titleTextStyle: {
            color: 'black',
            fontSize: 21,
        },
        backgroundColor: {
            strokeWidth: 1,
            stroke: "#FFDEAD",
            fill: '#FFFFF0'
        },
        animation: {
            duration: 1000,
            easing: 'out',
            startup: true
        },
        legend: { position: "none" },
    };
    var chart = new google.visualization.ColumnChart(document.getElementById("DashboardQuantidadeOcorrencias"));
    chart.draw(view, options);

    var impressao = document.getElementById("DashboardQuantidadeOcorrenciasImpressao");
    if (impressao != null) {
        var chart = new google.visualization.ColumnChart(impressao);
        chart.draw(view, options);
    }
}

function conteudoRegistroProduto(dados) {
    var data = new google.visualization.DataTable();

    data.addColumn('string', 'PRODUTO');
    data.addColumn('number', 'TOTAL');
    data.addColumn({ type: 'string', role: "style" });
    data.addRows(dados)

    var view = new google.visualization.DataView(data);
    view.setColumns([0, 1,
        {
            calc: "stringify",
            sourceColumn: 1,
            type: "string",
            role: "annotation",
        }, 2]);

    var options = {
        title: "REGISTRO POR PRODUTO",
        backgroundColor: {
            strokeWidth: 1,
            stroke: "#FFDEAD",
            fill: '#FFFFF0'
        },
        titleTextStyle: {
            color: 'black',
            fontSize: 21,
        },
        animation: {
            duration: 1000,
            easing: 'out',
            startup: true
        },
        legend: { position: "none" },
    };
    var chart = new google.visualization.ColumnChart(document.getElementById("dashboardRegistroProduto"));
    chart.draw(view, options);

    var impressao = document.getElementById("dashboardRegistroProdutoImpressao");
    if (impressao != null) {
        var chart = new google.visualization.ColumnChart(impressao);
        chart.draw(view, options);
    }
}

function conteudoTipoOcorrencia(dados) {
    var data = new google.visualization.DataTable();
    data.addColumn('string', 'TIPO OCORRENCIA');
    data.addColumn('number', 'TOTAL');
    data.addColumn({ type: 'string', role: "style" });
    data.addRows(dados);

    var view = new google.visualization.DataView(data);
    view.setColumns([0, 1,
        {
            calc: "stringify",
            sourceColumn: 1,
            type: "string",
            role: "annotation",
        }, 2]);

    var options = {
        title: "TIPO DE OCORRENCIA",
        titleTextStyle: {
            color: 'black',
            fontSize: 21,
        },
        backgroundColor: {
            strokeWidth: 1,
            stroke: "#FFDEAD",
            fill: '#FFFFF0'
        },
        hAxis: { format: '#. ###%' },
        legend: { position: "none" },
        animation: {
            duration: 500,
            easing: 'out',
            startup: true
        },
    };
    var chart = new google.visualization.ColumnChart(document.getElementById("dashboardTipoOcorrencia"));
    chart.draw(view, options);
    var impressao = document.getElementById("dashboardTipoOcorrenciaImpressao");
    if (impressao != null) {
        var chart = new google.visualization.ColumnChart(impressao);
        chart.draw(view, options);
    }
}