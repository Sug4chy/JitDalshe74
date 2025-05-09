window.getElementID = (element) => {
    return element.id;
}

window.showModal = (modalID) => {
    $(modalID).modal('show')
}

window.hideModal = (modalID) => {
    $(modalID).modal('hide')
}