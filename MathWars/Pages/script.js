function togglePanel() {
    const panel = document.getElementById('panelBoczny');
    if (panel.style.right === '0px') {
        panel.style.right = '-250px';
    } else {
        panel.style.right = '0px';
    }
}
