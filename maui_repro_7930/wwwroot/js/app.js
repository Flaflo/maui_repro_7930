function onOpenPopupClick() {
    const popup = window.open("about:blank", "Popup", "location=no,menubar=no,width=800,height=600");
    popup.addEventListener("load", () => popup.focus());
}