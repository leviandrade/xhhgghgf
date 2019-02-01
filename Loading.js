function loadingGif() {

    var container;

    this.abrir = function () {
        var img = $("<img />",
            {
                src: location.origin + '/images/loading.gif'
            })
            .css({
                left: '50%',
                margin: '-99px 0px 0px -99px',
                position: 'fixed',
                top: '50%'
            });
        container = $("<div />")
            .css({
                height: '100%',
                left: 0,
                position: 'fixed',
                top: 0,
                width: '100%'
            }).appendTo('body');

        img.appendTo(container);
    }

    this.fechar = function () {
        if(container)
            container.remove();

        container = null;
    }

}