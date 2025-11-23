// Actualizar contador del carrito
function updateCartCount() {
    const cartBadge = document.getElementById('cart-count');
    if (cartBadge) {
        fetch('/Carrito/GetCantidad')
            .then(response => response.json())
            .then(data => {
                cartBadge.textContent = data.cantidad;
                if (data.cantidad > 0) {
                    cartBadge.classList.remove('d-none');
                } else {
                    cartBadge.textContent = '0';
                }
            })
            .catch(error => console.error('Error al actualizar carrito:', error));
    }
}

// Actualizar cuando carga la página
document.addEventListener('DOMContentLoaded', function() {
    updateCartCount();
});
