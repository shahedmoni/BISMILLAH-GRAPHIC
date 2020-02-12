
jQuery(function ($) {
    //let linkData = [];
    //let storage = localStorage.getItem("linkData");

    //if (storage) {
    //    linkData = JSON.parse(storage);
    //    appendLinks(linkData);
    //} else {
    //    $.get("/Basic/GetSideManu", function (response) {
    //        localStorage.setItem("linkData", response);
    //        appendLinks(JSON.parse(response));
    //    });
    //};


    const url = "/Basic/GetSideMenu";
    $.get(url, function (response) {
        appendLinks(JSON.parse(response));
    });

    function appendLinks(data) {
        const menuItem = $("#menuItem");
        let menu = `<li><a href="/Dashboard/Index"><strong> <span class="fas fa-tachometer-alt"></span> Dashboard</strong></a></li><li><a href="/Dashboard/UserProfile"><strong> <span class="fas fa-user-circle"></span> Profile</strong></a></li>`;

        $.each(data, (i, item) => {
            menu += `<strong><span class="${item.IconClass}"></span> ${item.Category} <i class="fas fa-caret-right"></i></strong><ul class="sub-manu">${menuLink(item.links)}</ul>`;
        });

        menuItem.html(menu);
    }

    function menuLink(data) {
        let link = "";
        $.each(data, (i, item) => {
            link += `<li><a class="links" href="/${item.Controller}/${item.Action}">${item.Title}</a></li>`;
        });
        return link;
    }
});
