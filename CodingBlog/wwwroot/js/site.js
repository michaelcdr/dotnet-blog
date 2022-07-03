$(".ir-para-topo").click(function (ev) {
    ev.preventDefault();
    $("html, body").animate({ scrollTop: "0" }, 1000);
});

$('input[type=search]').keyup(function (ev) {
    if (ev.which === 13) {
        
        document.location = '/Post/Pesquisa/' + $(this).val();
    }
});