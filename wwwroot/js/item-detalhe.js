$(document).ready(function () {
    initItemDetalhe();
});

let itemData = null;
let selectedOptions = [];
let quantity = 1;

function initItemDetalhe() {
    loadItemData();
    setupEventListeners();
    updateCartCount();
}

function loadItemData() {
    const pathParts = window.location.pathname.split('/');
    const itemId = pathParts[pathParts.length - 1];
    
    if (!itemId || isNaN(itemId)) {
        showError('Item não encontrado');
        return;
    }
    
    $.ajax({
        url: `/api/cardapio/item/${itemId}`,
        method: 'GET',
        success: function (response) {
            if (response.sucesso && response.dados) {
                itemData = response.dados;
                renderItemDetails();
            } else {
                showError(response.mensagem || 'Item não encontrado');
            }
        },
        error: function (xhr) {
            console.error('Erro ao carregar item:', xhr);
            showError('Não foi possível carregar o item. Tente novamente.');
        }
    });
}

function renderItemDetails() {
    const placeholderImg = "data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='400' height='300' viewBox='0 0 400 300'%3E%3Crect fill='%23f5e6d3' width='400' height='300'/%3E%3Ctext x='50%25' y='50%25' fill='%23c4a574' font-family='Arial' font-size='80' text-anchor='middle' dy='.3em'%3E🍞%3C/text%3E%3C/svg%3E";
    $('#headerTitle').text(itemData.nome);
    
    if (itemData.imagemUrl && itemData.imagemUrl.trim() !== '') {
        $('#itemImage').attr('src', itemData.imagemUrl).attr('alt', itemData.nome);
        $('#itemImage').on('error', function() {
            $(this).attr('src', placeholderImg);
        });
    } else {
        $('#itemImage').attr('src', placeholderImg).attr('alt', itemData.nome);
    }

    if (itemData.categoria) {
        $('#itemCategory').text(itemData.categoria);
    }
    
    $('#itemName').text(itemData.nome);
    $('#itemDescription').text(itemData.descricao || '');
    
    if (itemData.ingredientes) {
        $('#itemIngredients').text(itemData.ingredientes);
        $('#itemIngredientsSection').removeClass('d-none');
    }
    
    $('#itemPrice').text(formatCurrency(itemData.preco));
    
    renderOptions();
    
    updateTotal();
    
    $('#loadingState').hide();
    $('#itemContent').removeClass('d-none');
}

function renderOptions() {
    const container = $('#itemOptionsSection');
    
    if (!itemData.opcoesAdicionais || itemData.opcoesAdicionais.length === 0) {
        return;
    }
    
    let html = '';
    
    itemData.opcoesAdicionais.forEach(opcao => {
        html += `
            <div class="option-item" data-opcao-id="${opcao.id}" onclick="toggleOption(${opcao.id})">
                <div class="option-checkbox">
                    <i class="fas fa-check"></i>
                </div>
                <div class="option-info">
                    <span class="option-name">${opcao.nome}</span>
                    ${opcao.descricao ? `<span class="option-description">${opcao.descricao}</span>` : ''}
                </div>
                <span class="option-price">+ ${formatCurrency(opcao.preco)}</span>
            </div>
        `;
    });
    
    $('#optionsList').html(html);
    container.removeClass('d-none');
}

function toggleOption(opcaoId) {
    const optionElement = $(`.option-item[data-opcao-id="${opcaoId}"]`);
    const opcao = itemData.opcoesAdicionais.find(o => o.id === opcaoId);
    
    if (!opcao) return;
    
    const index = selectedOptions.findIndex(o => o.id === opcaoId);
    
    if (index >= 0) {
        selectedOptions.splice(index, 1);
        optionElement.removeClass('selected');
    } else {
        selectedOptions.push({
            id: opcao.id,
            nome: opcao.nome,
            preco: opcao.preco
        });
        optionElement.addClass('selected');
    }
    
    updateTotal();
}

function setupEventListeners() {
    $(document).on('keydown', function (e) {
        if (e.key === 'Escape' && $('#cartSidebar').hasClass('open')) {
            fecharCarrinho();
        }
    });
}

function incrementQuantity() {
    if (quantity < 99) {
        quantity++;
        updateQuantityDisplay();
        updateTotal();
    }
}

function decrementQuantity() {
    if (quantity > 1) {
        quantity--;
        updateQuantityDisplay();
        updateTotal();
    }
}

function abrirCarrinho() {
    renderCart();
    $('#cartOverlay').removeClass('d-none');
    $('#cartSidebar').addClass('open');
    $('body').css('overflow', 'hidden');
}

function fecharCarrinho() {
    $('#cartSidebar').removeClass('open');
    $('#cartOverlay').addClass('d-none');
    $('body').css('overflow', '');
}

function adicionarAoCarrinho() {
    addItemToCart();
}

function irParaCheckout() {
    checkout();
}

function updateQuantityDisplay() {
    $('#quantityValue').text(quantity);
    $('#btnDecreaseQty').prop('disabled', quantity <= 1);
}

function updateTotal() {
    if (!itemData) return;
    
    let total = itemData.preco * quantity;
    
    selectedOptions.forEach(opcao => {
        total += opcao.preco * quantity;
    });
    
    $('#totalPrice').text(formatCurrency(total));
}

function addItemToCart() {
    const token = localStorage.getItem('padoka_token');
    if (!token) {
        showLoginModal();
        return;
    }
    
    const observacao = $('#itemNotes').val().trim();
    
    const cartItem = {
        id: itemData.id,
        nome: itemData.nome,
        preco: itemData.preco,
        imagemUrl: itemData.imagemUrl,
        quantidade: quantity,
        opcoes: selectedOptions.length > 0 ? [...selectedOptions] : null,
        observacao: observacao || null
    };
    
    let cart = JSON.parse(localStorage.getItem('padoka_cart')) || [];
    
    const itemKey = JSON.stringify({
        id: cartItem.id,
        opcoes: cartItem.opcoes
    });
    
    const existingIndex = cart.findIndex(i => 
        JSON.stringify({ id: i.id, opcoes: i.opcoes }) === itemKey
    );
    
    if (existingIndex >= 0) {
        cart[existingIndex].quantidade += cartItem.quantidade;
        if (cartItem.observacao) {
            cart[existingIndex].observacao = cartItem.observacao;
        }
    } else {
        cart.push(cartItem);
    }
    
    localStorage.setItem('padoka_cart', JSON.stringify(cart));
    
    updateCartCount();
    showToast('Item adicionado ao carrinho!', 'success');
    
    setTimeout(() => {
        abrirCarrinho();
    }, 500);
}

function updateCartCount() {
    const cart = JSON.parse(localStorage.getItem('padoka_cart')) || [];
    const count = cart.reduce((total, item) => total + item.quantidade, 0);
    const badge = $('#cartCount');
    
    if (count > 0) {
        badge.text(count > 99 ? '99+' : count).show();
    } else {
        badge.hide();
    }
}

function renderCart() {
    const cart = JSON.parse(localStorage.getItem('padoka_cart')) || [];
    const cartEmpty = $('#cartEmpty');
    const cartItems = $('#cartItems');
    const cartFooter = $('#cartFooter');
    
    if (cart.length === 0) {
        cartEmpty.removeClass('d-none');
        cartItems.addClass('d-none').empty();
        cartFooter.addClass('d-none');
        return;
    }
    
    let html = '';
    
    cart.forEach((item, index) => {
        let optionsText = '';
        if (item.opcoes && item.opcoes.length > 0) {
            optionsText = item.opcoes.map(o => o.nome).join(', ');
        }
        
        let itemTotalPrice = item.preco;
        if (item.opcoes) {
            item.opcoes.forEach(o => itemTotalPrice += o.preco);
        }
        
        const placeholderImg = "data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='80' height='80' viewBox='0 0 80 80'%3E%3Crect fill='%23f5e6d3' width='80' height='80'/%3E%3Ctext x='50%25' y='50%25' fill='%23c4a574' font-family='Arial' font-size='30' text-anchor='middle' dy='.3em'%3E🍞%3C/text%3E%3C/svg%3E";
        const imgUrl = item.imagemUrl && item.imagemUrl.trim() !== '' ? item.imagemUrl : placeholderImg;
        
        html += `
            <div class="cart-item">
                <img src="${imgUrl}" 
                     alt="${item.nome}" 
                     class="cart-item-image"
                     onerror="this.src='${placeholderImg}'">
                <div class="cart-item-info">
                    <div class="cart-item-name">${item.nome}</div>
                    ${optionsText ? `<div class="cart-item-options">${optionsText}</div>` : ''}
                    <div class="cart-item-footer">
                        <span class="cart-item-price">${formatCurrency(itemTotalPrice * item.quantidade)}</span>
                        <div class="cart-item-quantity">
                            <button onclick="updateCartItemQuantity(${index}, -1)">
                                ${item.quantidade === 1 ? '<i class="fas fa-trash"></i>' : '<i class="fas fa-minus"></i>'}
                            </button>
                            <span>${item.quantidade}</span>
                            <button onclick="updateCartItemQuantity(${index}, 1)">
                                <i class="fas fa-plus"></i>
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        `;
    });
    
    cartEmpty.addClass('d-none');
    cartItems.html(html).removeClass('d-none');
    
    $('#cartTotal').text(formatCurrency(getCartTotal()));
    cartFooter.removeClass('d-none');
}

function getCartTotal() {
    const cart = JSON.parse(localStorage.getItem('padoka_cart')) || [];
    return cart.reduce((total, item) => {
        let itemTotal = item.preco * item.quantidade;
        
        if (item.opcoes && item.opcoes.length > 0) {
            item.opcoes.forEach(opcao => {
                itemTotal += opcao.preco * item.quantidade;
            });
        }
        
        return total + itemTotal;
    }, 0);
}

function updateCartItemQuantity(index, change) {
    let cart = JSON.parse(localStorage.getItem('padoka_cart')) || [];
    
    if (cart[index]) {
        cart[index].quantidade += change;
        
        if (cart[index].quantidade <= 0) {
            cart.splice(index, 1);
        }
        
        localStorage.setItem('padoka_cart', JSON.stringify(cart));
        renderCart();
        updateCartCount();
    }
}

function checkout() {
    const token = localStorage.getItem('padoka_token');
    if (!token) {
        closeCart();
        showLoginModal();
        return;
    }
    
    const cart = JSON.parse(localStorage.getItem('padoka_cart')) || [];
    if (cart.length === 0) {
        showToast('Seu carrinho está vazio!', 'warning');
        return;
    }
    
    window.location.href = '/Pedido/Checkout';
}

function showLoginModal() {
    const modal = new bootstrap.Modal(document.getElementById('loginModal'));
    modal.show();
}

function goToLogin() {
    window.location.href = '/Auth/Login?returnUrl=' + encodeURIComponent(window.location.pathname);
}

function goToRegister() {
    window.location.href = '/Auth/Registro?returnUrl=' + encodeURIComponent(window.location.pathname);
}

function showError(message) {
    $('#loadingState').hide();
    $('#itemContent').html(`
        <div class="error-state text-center py-5">
            <i class="fas fa-exclamation-triangle fa-4x text-muted mb-4"></i>
            <h3>Ops! Algo deu errado</h3>
            <p class="text-muted">${message}</p>
            <a href="/Cardapio" class="btn btn-primary mt-3">
                <i class="fas fa-arrow-left me-2"></i>Voltar ao cardápio
            </a>
        </div>
    `).show();
}

function formatCurrency(value) {
    return new Intl.NumberFormat('pt-BR', {
        style: 'currency',
        currency: 'BRL'
    }).format(value);
}

function showToast(message, type = 'info') {
    $('.toast-container').remove();
    
    const bgClass = {
        'success': 'bg-success',
        'error': 'bg-danger',
        'warning': 'bg-warning',
        'info': 'bg-info'
    }[type] || 'bg-info';
    
    const iconClass = {
        'success': 'fa-check-circle',
        'error': 'fa-times-circle',
        'warning': 'fa-exclamation-triangle',
        'info': 'fa-info-circle'
    }[type] || 'fa-info-circle';
    
    const toastHtml = `
        <div class="toast-container position-fixed bottom-0 end-0 p-3" style="z-index: 1100;">
            <div class="toast show" role="alert">
                <div class="toast-header ${bgClass} text-white">
                    <i class="fas ${iconClass} me-2"></i>
                    <strong class="me-auto">Padoka</strong>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="toast"></button>
                </div>
                <div class="toast-body">
                    ${message}
                </div>
            </div>
        </div>
    `;
    
    $('body').append(toastHtml);
    
    setTimeout(() => {
        $('.toast-container').fadeOut(300, function () {
            $(this).remove();
        });
    }, 3000);
}
