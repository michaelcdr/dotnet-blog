function applyTheme(theme) {
    document.documentElement.setAttribute("data-theme", theme);
    document.body.setAttribute("data-theme", theme);
    localStorage.setItem("coding-blog-theme", theme);
}

$(function () {
    const savedTheme = localStorage.getItem("coding-blog-theme") || "dark";
    applyTheme(savedTheme);

    $(".theme-toggle").click(function () {
        const currentTheme = document.body.getAttribute("data-theme") || "dark";
        applyTheme(currentTheme === "dark" ? "light" : "dark");
    });

    $(".ir-para-topo").click(function (ev) {
        ev.preventDefault();
        $("html, body").animate({ scrollTop: "0" }, 1000);
    });

    $('input[type=search]').keyup(function (ev) {
        if (ev.which === 13) {
            document.location = '/Post/Pesquisa/' + $(this).val();
        }
    });
});
