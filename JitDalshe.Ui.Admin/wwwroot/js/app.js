window.getElementID = (element) => {
    console.log(`Modal ID from JS: ${element.id}`)
    console.log(element)
    return element.id;
}

window.showModal = (modalID) => {
    $(modalID).modal()
}