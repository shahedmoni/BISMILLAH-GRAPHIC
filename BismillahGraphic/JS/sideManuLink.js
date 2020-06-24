
//jQuery(function ($) {
//    const url = "/Basic/GetSideMenu";

//    $.get(url, function (response) {
//        appendLinks(JSON.parse(response));
//    });

//    function appendLinks(data) {
//        const menuItem = $("#menuItem");
//        let menu = `<li><a href="/Dashboard/Index"><strong> <span class="fas fa-tachometer-alt"></span> Dashboard</strong></a></li><li><a href="/Dashboard/UserProfile"><strong> <span class="fas fa-user-circle"></span> Profile</strong></a></li>`;

//        $.each(data, (i, item) => {
//            menu += `<strong><span class="${item.IconClass}"></span> ${item.Category} <i class="fas fa-caret-right"></i></strong><ul class="sub-manu">${menuLink(item.links)}</ul>`;
//        });

//        menuItem.html(menu);
//    }

//    function menuLink(data) {
//        let link = "";
//        $.each(data, (i, item) => {
//            link += `<li><a class="links" href="/${item.Controller}/${item.Action}">${item.Title}</a></li>`;
//        });
//        return link;
//    }
//});


//selectors
const menuItem = document.getElementById("menuItem")

//functions

//get data from server
const getMenuData = function () {
    const url = '/Basic/GetSideMenu'

    $.get(url, function(response) {
        appendMenuDOM(JSON.parse(response))
        setNavigation()
    });
}

//create links
const createLinks = function (links) {
    let fragment = document.createDocumentFragment()
    links.forEach(link => {
        let anchor = document.createElement('a')
        anchor.classList.add('links')
        anchor.href = `/${link.Controller}/${link.Action}`
        anchor.textContent = link.Title

        let li = document.createElement('li')
        li.appendChild(anchor)

        fragment.appendChild(li)
    });

    return fragment
}

//create link li
const linkCategory = function (category, iconCss, links) {
    let span = document.createElement('span')
    span.className = iconCss

    let ico = document.createElement('i')
    ico.classList.add('fas', 'fa-caret-right')

    let strong = document.createElement('strong')
    strong.appendChild(span)
    strong.appendChild(ico)
    strong.appendChild(document.createTextNode(category))

    let ul = document.createElement('ul')
    ul.classList.add('sub-menu')
    ul.appendChild(links)

    let li = document.createElement('li')
    li.appendChild(strong)
    li.appendChild(ul)

    return li
}

//append link to DOM
const appendMenuDOM = function (linkData) {
    let fragment = document.createDocumentFragment()

    let span = document.createElement('span')
    span.className = 'fas fa-tachometer-alt'

    let anchor = document.createElement('a')
    anchor.classList.add('links')
    anchor.href = '/Dashboard/Index'
    anchor.textContent = 'Dashboard'

    let strong = document.createElement('strong')
    strong.appendChild(span)
    strong.appendChild(anchor)

    let li = document.createElement('li')
    li.appendChild(strong)

    fragment.appendChild(li)

    linkData.forEach(item => {
        const li = linkCategory(item.Category, item.IconClass, createLinks(item.links))
        fragment.appendChild(li)
    });

    menuItem.appendChild(fragment)
}

//active current url
function setNavigation() {
    const links = menuItem.querySelectorAll(".links")
    let path = window.location.pathname

    path = path.replace(/\/$/, "")
    path = decodeURIComponent(path)

    links.forEach(link => {
        const href = link.getAttribute('href')

        if (path === href) {
            if (link.parentElement.nodeName !== "STRONG") {
                const prentElement = link.parentElement.parentElement
                prentElement.previousElementSibling.classList.add("open")
                prentElement.classList.add("active")
            }

            link.classList.add('link-active')
        }
    })
}

//click on link
const linkCategoryClicked = function (evt) {
    const element = evt.target;
    if (element.nodeName === "STRONG") {
        if (element.nextElementSibling) {
            element.nextElementSibling.classList.toggle("active")
            element.classList.toggle("open")
        }
    }
}

//event listener
menuItem.addEventListener("click", linkCategoryClicked)

//on load
getMenuData()
