$(document).ready(function () {
    initCardapio();
});

let cart = JSON.parse(localStorage.getItem('padoka_cart')) || [];

function initCardapio() {
    checkAuthStatus();
    loadCategorias();
    loadCardapio();
    setupEventListeners();
    updateCartCount();
}

// ========================================
// Authentication
// ========================================

function checkAuthStatus() {
    const token = localStorage.getItem('padoka_token');
    const userData = localStorage.getItem('padoka_user');
    
    if (token && userData) {
        try {
            const user = JSON.parse(userData);
            showUserMenu(user);
        } catch (e) {
            showLoginButton();
        }
    } else {
        showLoginButton();
    }
}

function showLoginButton() {
    $('#btnLogin').removeClass('d-none');
    $('#userMenu').addClass('d-none');
}

function showUserMenu(user) {
    $('#btnLogin').addClass('d-none');
    $('#userMenu').removeClass('d-none');
    
    // Exibe o primeiro nome do usuário
    const firstName = user.nome ? user.nome.split(' ')[0] : 'Usuário';
    $('#userName').text(firstName);
}

function toggleUserDropdown() {
    $('#userMenu').toggleClass('open');
}

function logout() {
    localStorage.removeItem('padoka_token');
    localStorage.removeItem('padoka_user');
    localStorage.removeItem('padoka_cart');
    
    showLoginButton();
    cart = [];
    updateCartCount();
    
    showToast('Você saiu da sua conta', 'info');
}

// Fechar dropdown ao clicar fora
$(document).on('click', function (e) {
    if (!$(e.target).closest('.user-menu').length) {
        $('#userMenu').removeClass('open');
    }
});

function setupEventListeners() {
    const searchInput = $('#searchInput');
    let searchTimeout;
    
    searchInput.on('input', function () {
        clearTimeout(searchTimeout);
        const query = $(this).val().trim();
        
        searchTimeout = setTimeout(() => {
            if (query.length >= 2) {
                searchItems(query);
            } else if (query.length === 0) {
                loadCardapio();
            }
        }, 300);
    });
    
    $('#btnClearSearch').on('click', function () {
        searchInput.val('');
        loadCardapio();
    });
    
    $(document).on('click', '.category-tab', function () {
        const categoryId = $(this).data('category-id');
        
        $('.category-tab').removeClass('active');
        $(this).addClass('active');
        
        if (categoryId === 'all') {
            loadCardapio();
        } else {
            loadCategoryItems(categoryId);
        }
    });
    
    $('#btnOpenCart').on('click', function () {
        openCart();
    });
    
    $('#btnCloseCart, #cartOverlay').on('click', function () {
        closeCart();
    });
    
    $('#btnCheckout').on('click', function () {
        checkout();
    });
    
    $(document).on('keydown', function (e) {
        if (e.key === 'Escape' && $('#cartSidebar').hasClass('open')) {
            closeCart();
        }
    });
}

function loadCategorias() {
    $.ajax({
        url: '/api/cardapio/categorias',
        method: 'GET',
        success: function (response) {
            if (response.sucesso && response.dados) {
                renderCategorias(response.dados);
            }
        },
        error: function (xhr) {
            console.error('Erro ao carregar categorias:', xhr);
        }
    });
}

function loadCardapio() {
    showLoading();
    
    $.ajax({
        url: '/api/cardapio',
        method: 'GET',
        success: function (response) {
            if (response.sucesso) {
                renderCardapio(response.dados);
            } else {
                showError(response.mensagem);
            }
        },
        error: function (xhr) {
            console.error('Erro ao carregar cardápio:', xhr);
            showError('Não foi possível carregar o cardápio. Tente novamente.');
        }
    });
}

function loadCategoryItems(categoryId) {
    showLoading();
    
    $.ajax({
        url: `/api/cardapio/categoria/${categoryId}`,
        method: 'GET',
        success: function (response) {
            if (response.sucesso && response.dados) {
                renderSingleCategory(response.dados);
            } else {
                showEmpty();
            }
        },
        error: function (xhr) {
            console.error('Erro ao carregar categoria:', xhr);
            showError('Não foi possível carregar os itens. Tente novamente.');
        }
    });
}

function searchItems(query) {
    showLoading();
    
    $.ajax({
        url: `/api/cardapio/buscar?termo=${encodeURIComponent(query)}`,
        method: 'GET',
        success: function (response) {
            if (response.sucesso && response.dados && response.dados.length > 0) {
                renderSearchResults(response.dados, query);
            } else {
                showSearchEmpty(query);
            }
        },
        error: function (xhr) {
            console.error('Erro na busca:', xhr);
            showError('Não foi possível realizar a busca. Tente novamente.');
        }
    });
}

function renderCategorias(categorias) {
    const container = $('#categoriesScroll');
    
    let html = '<button class="category-tab active" data-category-id="all">Todos</button>';
    
    categorias.forEach(cat => {
        html += `<button class="category-tab" data-category-id="${cat.id}">${cat.nome}</button>`;
    });
    
    container.html(html);
}

function renderCardapio(categorias) {
    const container = $('#cardapioContent');
    
    if (!categorias || categorias.length === 0) {
        showEmpty();
        return;
    }
    
    let html = '';
    
    categorias.forEach(categoria => {
        if (categoria.itens && categoria.itens.length > 0) {
            html += `
                <section class="category-section" id="category-${categoria.id}">
                    <h2 class="section-title">
                        <i class="fas fa-bread-slice me-2"></i>${categoria.nome}
                    </h2>
                    <div class="items-grid">
                        ${renderItems(categoria.itens)}
                    </div>
                </section>
            `;
        }
    });
    
    if (html === '') {
        showEmpty();
    } else {
        $('#loadingState').addClass('d-none');
        $('#emptyState').addClass('d-none');
        $('#searchResults').addClass('d-none');
        container.html(html).removeClass('d-none');
    }
}

function renderSingleCategory(categoria) {
    const container = $('#cardapioContent');
    
    if (!categoria.itens || categoria.itens.length === 0) {
        showEmpty();
        return;
    }
    
    let html = `
        <section class="category-section">
            <h2 class="section-title">
                <i class="fas fa-bread-slice me-2"></i>${categoria.nome}
            </h2>
            <div class="items-grid">
                ${renderItems(categoria.itens)}
            </div>
        </section>
    `;
    
    $('#loadingState').addClass('d-none');
    $('#emptyState').addClass('d-none');
    $('#searchResults').addClass('d-none');
    container.html(html).removeClass('d-none');
}

function renderSearchResults(itens, query) {
    $('#loadingState').addClass('d-none');
    $('#emptyState').addClass('d-none');
    $('#cardapioContent').addClass('d-none');
    
    $('#searchResultsGrid').html(renderItems(itens));
    $('#searchResults').removeClass('d-none');
}

function renderItems(itens) {
    return itens.map(item => `
        <a href="/Cardapio/Item/${item.id}" class="item-card">
            <img src="${item.imagemUrl || '/img/placeholder-item.png'}" 
                 alt="${item.nome}" 
                 class="item-card-image"
                 onerror="this.src='/img/placeholder-item.png'">
            <div class="item-card-body">
                <h3 class="item-card-name">${item.nome}</h3>
                <p class="item-card-description">${item.descricaoResumida || item.descricao || ''}</p>
                <div class="item-card-footer">
                    <span class="item-card-price">${formatCurrency(item.preco)}</span>
                    <button type="button" class="item-card-btn" onclick="event.preventDefault(); quickAddToCart(${item.id}, '${escapeHtml(item.nome)}', ${item.preco}, '${item.imagemUrl || ''}')">
                        <i class="fas fa-plus"></i>
                    </button>
                </div>
            </div>
        </a>
    `).join('');
}

function showLoading() {
    $('#cardapioContent').addClass('d-none');
    $('#searchResults').addClass('d-none');
    $('#emptyState').addClass('d-none');
    $('#loadingState').removeClass('d-none');
}

function showEmpty() {
    $('#loadingState').addClass('d-none');
    $('#cardapioContent').addClass('d-none');
    $('#searchResults').addClass('d-none');
    $('#emptyState').removeClass('d-none');
}

function showSearchEmpty(query) {
    $('#loadingState').addClass('d-none');
    $('#cardapioContent').addClass('d-none');
    $('#searchResults').addClass('d-none');
    
    $('#emptyState').html(`
        <i class="fas fa-search"></i>
        <h3>Nenhum resultado</h3>
        <p>Não encontramos itens para "${query}".</p>
        <button class="btn btn-primary mt-3" onclick="$('#searchInput').val(''); loadCardapio();">
            Ver todo o cardápio
        </button>
    `).removeClass('d-none');
}

function showError(message) {
    $('#loadingState').addClass('d-none');
    $('#cardapioContent').addClass('d-none');
    $('#searchResults').addClass('d-none');
    
    $('#emptyState').html(`
        <i class="fas fa-exclamation-triangle"></i>
        <h3>Ops! Algo deu errado</h3>
        <p>${message}</p>
        <button class="btn btn-primary mt-3" onclick="loadCardapio();">
            Tentar novamente
        </button>
    `).removeClass('d-none');
}

function quickAddToCart(itemId, nome, preco, imagemUrl) {
    const token = localStorage.getItem('padoka_token');
    if (!token) {
        showLoginModal();
        return;
    }
    
    const existingItem = cart.find(i => i.id === itemId && !i.opcoes);
    
    if (existingItem) {
        existingItem.quantidade++;
    } else {
        cart.push({
            id: itemId,
            nome: nome,
            preco: preco,
            imagemUrl: imagemUrl,
            quantidade: 1,
            opcoes: null,
            observacao: null
        });
    }
    
    saveCart();
    updateCartCount();
    showToast('Item adicionado ao carrinho!', 'success');
}

function addToCart(item) {
    const token = localStorage.getItem('padoka_token');
    if (!token) {
        showLoginModal();
        return false;
    }
    
    const itemKey = JSON.stringify({
        id: item.id,
        opcoes: item.opcoes
    });
    
    const existingIndex = cart.findIndex(i => 
        JSON.stringify({ id: i.id, opcoes: i.opcoes }) === itemKey
    );
    
    if (existingIndex >= 0) {
        cart[existingIndex].quantidade += item.quantidade;
    } else {
        cart.push(item);
    }
    
    saveCart();
    updateCartCount();
    showToast('Item adicionado ao carrinho!', 'success');
    return true;
}

function updateCartItemQuantity(index, change) {
    if (cart[index]) {
        cart[index].quantidade += change;
        
        if (cart[index].quantidade <= 0) {
            cart.splice(index, 1);
        }
        
        saveCart();
        renderCart();
        updateCartCount();
    }
}

function removeCartItem(index) {
    if (cart[index]) {
        cart.splice(index, 1);
        saveCart();
        renderCart();
        updateCartCount();
    }
}

function clearCart() {
    cart = [];
    saveCart();
    renderCart();
    updateCartCount();
}

function saveCart() {
    localStorage.setItem('padoka_cart', JSON.stringify(cart));
}

function getCartTotal() {
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

function updateCartCount() {
    const count = cart.reduce((total, item) => total + item.quantidade, 0);
    const badge = $('#cartCount');
    
    if (count > 0) {
        badge.text(count > 99 ? '99+' : count).show();
    } else {
        badge.hide();
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

function irParaCheckout() {
    checkout();
}

function openCart() {
    abrirCarrinho();
}

function closeCart() {
    fecharCarrinho();
}

function renderCart() {
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
        
        html += `
            <div class="cart-item">
                <img src="${item.imagemUrl || '/img/placeholder-item.png'}" 
                     alt="${item.nome}" 
                     class="cart-item-image"
                     onerror="this.src='/img/placeholder-item.png'">
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

function checkout() {
    const token = localStorage.getItem('padoka_token');
    if (!token) {
        closeCart();
        showLoginModal();
        return;
    }
    
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

function formatCurrency(value) {
    return new Intl.NumberFormat('pt-BR', {
        style: 'currency',
        currency: 'BRL'
    }).format(value);
}

function escapeHtml(text) {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML.replace(/'/g, "\\'").replace(/"/g, '\\"');
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
